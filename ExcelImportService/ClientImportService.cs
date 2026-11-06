using ClosedXML.Excel;
using EcoLogistics.Data;
using EcoLogistics.Models.ClientBlock;
using EcoLogistics.Models.Geo;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.ExcelImportService
{
  
    public class ClientImportService
    {
        private readonly ApplicationDbContext _context;

        public ClientImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ImportClientsFromExcelAsync(Stream fileStream)
        {
            // 1. Chargement de toutes les adresses en mémoire pour une recherche rapide par code postal
            var localites = await _context.Localites.ToListAsync();
            var paysList = await _context.Pays.ToListAsync();

            //Ignorez les commentaires pour éviter l'erreur « Impossible de charger le fichier de commentaires ».
            var loadOptions = new LoadOptions
            {
                GraphicEngine = null
            };
            var existingClients = await _context.Clients
                .Include(c => c.PersonnesContact)
                .Include(c => c.AdressesExploitation)
                .Include(c => c.SiegeSociale)
                .ToListAsync();

            using var workbook = new XLWorkbook(fileStream);

            //workbook.CalculateMode = CalculateMode.Manual;
            var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Visibility == XLWorksheetVisibility.Visible)?? workbook.Worksheet(1); // 1 feuille

            int startRow = 24; // Commençons par la ligne 24
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            int importedCount = 0;

            for (int row = startRow; row <= lastRow; row++)
            {
                //Secteurs d'activité de l'entreprise(Client / Producteur)
                var numClientVal = GetCellValue(worksheet, row, 1);
                var producteurName = GetCellValue(worksheet, row, 2 ); // Producteur
                var siegeRaison = GetCellValue(worksheet, row, 14);       // Col N: Raison Sociale
                var beNumero = GetCellValue(worksheet, row, 12);     // BE N° d'entreprise
                var numEntreprise = GetCellValue(worksheet, row, 13); // N° d'entreprise
                // Si le nom de l'entreprise est vide, passez à la ligne suivante.
                if (string.IsNullOrWhiteSpace(producteurName) && string.IsNullOrWhiteSpace(siegeRaison)) continue;
                string clientName = !string.IsNullOrWhiteSpace(siegeRaison) ? siegeRaison : producteurName!;

                var prodCp = GetCellValue(worksheet, row, 9);
                var prodVille = GetCellValue(worksheet, row, 10);
                var prodPaysExcel = GetCellValue(worksheet, row, 11);

                int? prodLocalitedId = await GetOrCreateLocaliteIdAsync(localites, paysList,  prodCp, prodVille, prodPaysExcel);

                var prodRue = GetCellValue(worksheet, row, 7);
                var prodNum = GetCellValue(worksheet, row, 8);
                var remarques = GetCellValue(worksheet, row, 22);
                var presentation = GetCellValue(worksheet, row, 23);



                // 2. Création d'un client (Client)
                Client? client = null;

                if (!string.IsNullOrWhiteSpace(numClientVal) && !string.IsNullOrWhiteSpace(producteurName))
                {
                    client = existingClients
                        .FirstOrDefault(c => c.Numero_client == numClientVal &&
                        c.Numero_entreprise != null &&
                        c.Numero_entreprise.Equals(clientName, StringComparison.OrdinalIgnoreCase));
                }
                if (client == null && !string.IsNullOrWhiteSpace(numClientVal))
                {
                    client = existingClients.FirstOrDefault(c => c.Numero_client == numClientVal);
                }
                if (client == null)
                {
                    client = existingClients.FirstOrDefault(c => 
                    c.Nom_entreprise != null &&
                    c.Nom_entreprise.Equals(clientName, StringComparison.OrdinalIgnoreCase));
                }
                bool isNew = false;
                if (client == null)
                {
                    isNew = true;
           
                    client = new Client
                    {
                        Id_client = Guid.NewGuid(),
                        Created_at = DateTime.Now,
                    };

                }

                client.Numero_client = numClientVal ?? client.Numero_client;
                client.Nom_entreprise = !string.IsNullOrWhiteSpace(producteurName) ? producteurName : clientName;
                client.BE_entreprise = beNumero;
                client.Numero_entreprise = numEntreprise;
                client.Adresse = $"{prodRue} {prodNum}".Trim(); // Production Rue + N°
                client.Telephone = GetCellValue(worksheet, row, 4);
                client.Email = GetCellValue(worksheet, row, 6);
                client.Remarques = remarques; // Remarques 
                client.Presentation = presentation;
                client.Is_deleted = false;
                client.Created_at = DateTime.Now;
                client.Id_localite = prodLocalitedId;

                if (isNew)
                {
                    //client.Created_at = DateTime.Now;
                    _context.Clients.Add(client);
                    existingClients.Add(client);
                    await _context.SaveChangesAsync();
                };

                // 3. Créer une personne de contact (PersonneContact)
                var contactName = GetCellValue(worksheet, row, 3); // Personne de contact
                if (!string.IsNullOrWhiteSpace(contactName))
                {
                    var individualNames = contactName.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var rawName in individualNames)
                    {
                        var contactRawName = rawName.Trim();
                        if (string.IsNullOrWhiteSpace(contactRawName)) continue;

                        var contact = client.PersonnesContact
                            .FirstOrDefault(p => p.Id_client == client.Id_client && p.Nom.ToLower() == contactRawName.ToLower());

                        if(contact ==  null)
                        {
                            contact = new PersonneContact
                            {

                                Id_client = client.Id_client,
                                Nom = contactName
                            };

                            client.PersonnesContact.Add(contact);
                             _context.PersonneContacts.Add(contact);
  
                        }
                        contact.Telephone = GetCellValue(worksheet, row, 4); // Téléphones
                        contact.Gsm = GetCellValue(worksheet, row, 5);       // Gsm2
                        contact.Email = GetCellValue(worksheet, row, 6);
                        contact.Id_localite = prodLocalitedId;

                        if (!_context.PersonneContacts.Local.Contains(contact) && contact.Id_contact == 0)
                        {
                        _context.PersonneContacts.Add(contact);
                        }
                    }
                }

                // 4.Création (AdresseExploitation)
                if (!string.IsNullOrWhiteSpace(prodRue))
                {
                    var siteName = producteurName ?? "Site Principal";
                    var adresseExp = client.AdressesExploitation?
                        .FirstOrDefault(a => a.Rue == prodRue && a.Numero == prodNum && a.Nom_site == siteName);

                    if (adresseExp == null)
                    {
                        adresseExp = new AdresseExploitation
                        {

                            Id_client = client.Id_client,
                            Nom_site = siteName,
                            Rue = prodRue,
                            Numero = prodNum,
                            Id_localite = prodLocalitedId
                        };
                        client.AdressesExploitation ??= new List<AdresseExploitation>();
                        client.AdressesExploitation.Add(adresseExp);
                        _context.AdressesExploitation.Add(adresseExp);
                    }
                    else { 

                        if ((adresseExp.Nom_site == "Site Principal" || string.IsNullOrEmpty(adresseExp.Nom_site)) && !string.IsNullOrWhiteSpace(producteurName))
                        {
                            adresseExp.Nom_site = producteurName;
                        }
                        adresseExp.Id_localite = prodLocalitedId;
                    }

                    if (!_context.AdressesExploitation.Local.Contains(adresseExp) && adresseExp.Id_adresse_exp == 0)
                    {

                         _context.AdressesExploitation.Add(adresseExp);
                    }
                }

                // 5. Créer une adresse légale (SiegeSocial)
                var siegeRue = GetCellValue(worksheet, row, 15);    // O (15) Siège Social Rue
                var siegeNum = GetCellValue(worksheet, row, 16);    // P (16) Siège Social Numero
                var siegeCpOrVille = GetCellValue(worksheet, row, 17);     // Q (17) Siège Social CP
                var siegePays = GetCellValue(worksheet, row, 18);
                

                if (!string.IsNullOrWhiteSpace(siegeRaison) || !string.IsNullOrWhiteSpace(siegeRue))
                {
                    var siege = client.SiegeSociale ?? new SiegeSociale();
         
                    siege.Raison_sociale = string.IsNullOrWhiteSpace(siegeRaison) ? clientName : siegeRaison;
                    siege.Rue = siegeRue;           // Rue
                    siege.Numero = siegeNum;                                // N°
                    siege.Site_internet = GetCellValue(worksheet, row, 20);    // T (20) Site internet
                    siege.Secteur_activite = GetCellValue(worksheet, row, 21); // U (21) Secteur d'Activité

                    siege.Id_localite = await GetOrCreateLocaliteIdAsync(localites, paysList, siegeCpOrVille, siegeCpOrVille, siegePays);

                    if(client.SiegeSociale == null) //siege.Id_siege == 0 && 
                    {
                        siege.Id_client = client.Id_client;

                        client.SiegeSociale = siege;
                        _context.SiegeSociales.Add(siege);
                    }

                }

                importedCount++;
            }

            // Enregistration toutes les données en une seule transaction.
            await _context.SaveChangesAsync();
            return importedCount;
        }

        private string? GetCellValue(IXLWorksheet ws, int row, int col, int maxLength =0)
        {
            var cell = ws.Cell(row, col);
            if (cell.IsEmpty()) return null;

            var val = cell.GetFormattedString()?.Trim();
            if (string.IsNullOrWhiteSpace(val)) return null;
            if (maxLength > 0 && val.Length > maxLength) 
            { 
                return val.Substring(0, maxLength);
            }

            return val;
        }

        // Méthode de recherche Id_localite par code postal
        //private int? FindLocaliteId(List<Localite> localites, string? codePostal)
        //{
        //    if (string.IsNullOrWhiteSpace(codePostal)) return null;

        //    var loc = localites.FirstOrDefault(l => l.Code_postal == codePostal.Trim());
        //    return loc?.Id_localite;
        //}
        private async Task<int?> GetOrCreateLocaliteIdAsync(
            List<Localite> localites,
            List<Pays> paysList,
            string? codePostal,
            string? nomVille,
            string? nomPaysExcel
            )
        {
            if (string.IsNullOrWhiteSpace(codePostal) && string.IsNullOrWhiteSpace(nomVille)) return null;

            codePostal = codePostal?.Trim();
            nomVille = nomVille?.Trim();

            var existinLoc = localites.FirstOrDefault(l =>
                (!string.IsNullOrEmpty(codePostal) && l.Code_postal == codePostal) ||
                (!string.IsNullOrEmpty(nomVille) && l.Nom_localite != null && l.Nom_localite.Equals(nomVille, StringComparison.OrdinalIgnoreCase)));

            if (existinLoc != null)
            {
                return existinLoc.Id_localite;
            }

            int? paysId = null;
            if (!string.IsNullOrWhiteSpace(nomPaysExcel))
            {
                var pays = paysList.FirstOrDefault(p =>
                p.Nom_pays.Equals(nomPaysExcel.Trim(), StringComparison.OrdinalIgnoreCase) ||
                p.Code_ISO.Equals(nomPaysExcel.Trim(), StringComparison.OrdinalIgnoreCase)
                );
                paysId = pays?.Id_pays;
            }

            if (paysId == null)
            {
                var defaultPays = paysList.FirstOrDefault(p => p.Nom_pays == "Belgique" || p.Code_ISO == "BEL");
                paysId = defaultPays?.Id_pays ?? 1;
            }
            var newLocalite = new Localite
            {
                Code_postal = codePostal ?? "0000",
                Nom_localite = nomVille ?? "Inconnu",
                Province = nomPaysExcel ?? "International",
                Id_pays = paysId.Value,
                Id_commune = null

            };

            _context.Localites.Add(newLocalite);
            await _context.SaveChangesAsync();
            localites.Add(newLocalite);
            return newLocalite.Id_localite;
        }
    }
}
