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

            //Ignorez les commentaires pour éviter l'erreur « Impossible de charger le fichier de commentaires ».
            var loadOptions = new LoadOptions
            {
                GraphicEngine = null
            };

            using var workbook = new XLWorkbook(fileStream);

            //workbook.CalculateMode = CalculateMode.Manual;
            var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Visibility == XLWorksheetVisibility.Visible)?? workbook.Worksheet(1); // 1 feuille

            int startRow = 24; // Commençons par la ligne 20
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            int importedCount = 0;

            for (int row = startRow; row <= lastRow; row++)
            {
                //Secteurs d'activité de l'entreprise(Client / Producteur)
                var nomEntreprise = GetCellValue(worksheet, row, 2 ); // Producteur
                // Si le nom de l'entreprise est vide, passez à la ligne suivante.
                if (string.IsNullOrWhiteSpace(nomEntreprise)) continue;
                var beNumero = GetCellValue(worksheet, row, 12);     // BE N° d'entreprise
                var numEntreprise = GetCellValue(worksheet, row, 13); // N° d'entreprise
                var prodCp = GetCellValue(worksheet, row, 9);
                int? prodLocalitedId = FindLocaliteId(localites, prodCp);
                var prodRue = GetCellValue(worksheet, row, 7);
                var prodNum = GetCellValue(worksheet, row, 8);
                var remarques = GetCellValue(worksheet, row, 22);
                var presentation = GetCellValue(worksheet, row, 23);


                // 2. Création d'un client (Client)
                var newClientId = Guid.NewGuid();
                var client = new Client
                {
                    Id_client = newClientId,
                    Nom_entreprise = nomEntreprise,
                    BE_entreprise = beNumero,
                    Numero_entreprise = numEntreprise,
                    Adresse = $"{prodRue} {prodNum}".Trim(), // Production Rue + N°
                    Telephone = GetCellValue(worksheet, row, 4),
                    Email =GetCellValue(worksheet, row,6),
                    Remarques = remarques, // Remarques 
                    Presentation = presentation,
                    Is_deleted = false,
                    Created_at = DateTime.Now,
                    Id_localite = prodLocalitedId
                };

                _context.Clients.Add(client);

                // 3. Créer une personne de contact (PersonneContact)
                var contactName = GetCellValue(worksheet, row, 3); // Personne de contact
                if (!string.IsNullOrWhiteSpace(contactName))
                {
                    var contact = new PersonneContact
                    {
                        Id_client = newClientId,
                        Nom = contactName,
                        Telephone = GetCellValue(worksheet, row, 4), // Téléphones
                        Gsm = GetCellValue(worksheet, row, 5),       // Gsm2
                        Email =GetCellValue(worksheet, row,6),
                        Id_localite = prodLocalitedId
                    };
                    _context.PersonneContacts.Add(contact);
                }

                // 4.Création (AdresseExploitation)
                if (!string.IsNullOrWhiteSpace(prodRue))
                {
                    var adresseExp = new AdresseExploitation
                    {
                        Id_client = newClientId,
                        Rue = prodRue,
                        Numero =prodNum, // Production N°
                        Nom_site = "Site Principal",
                        Id_localite = prodLocalitedId
                    };
                    _context.AdressesExploitation.Add(adresseExp);
                }

                // 5. Créer une adresse légale (SiegeSocial)
                var siegeRaison = GetCellValue(worksheet, row, 14); // N (14) Raison Sociale
                var siegeRue = GetCellValue(worksheet, row, 15);    // O (15) Siège Social Rue
                var siegeNum = GetCellValue(worksheet, row, 16);    // P (16) Siège Social Numero
                var siegeCp = GetCellValue(worksheet, row, 17);     // Q (17) Siège Social CP

                if (!string.IsNullOrWhiteSpace(siegeRaison) || !string.IsNullOrWhiteSpace(siegeRue))
                {
                    var siege = new SiegeSociale
                    {
                        Id_client = newClientId,
                        Raison_sociale = string.IsNullOrWhiteSpace(siegeRaison) ? nomEntreprise : siegeRaison,
                        Adresse = $"{siegeRue} {siegeNum}".Trim(),           // Rue + N°
                        Site_internet = GetCellValue(worksheet, row, 20),    // T (20) Site internet
                        Secteur_activite = GetCellValue(worksheet, row, 21), // U (21) Secteur d'Activité
                        Id_localite = FindLocaliteId(localites, siegeCp)
                    };
                    _context.SiegeSociales.Add(siege);
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
        private int? FindLocaliteId(List<Localite> localites, string? codePostal)
        {
            if (string.IsNullOrWhiteSpace(codePostal)) return null;

            var loc = localites.FirstOrDefault(l => l.Code_postal == codePostal.Trim());
            return loc?.Id_localite;
        }
    }
}
