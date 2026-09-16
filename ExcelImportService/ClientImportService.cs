using ClosedXML.Excel;
using EcoLogistics.Data;
using EcoLogistics.Models.ClientBlock;
using EcoLogistics.Models.Geo;
using Microsoft.AspNetCore.Authorization;
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

            int startRow = 20; // Commençons par la ligne 20
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            int importedCount = 0;

            for (int row = startRow; row <= lastRow; row++)
            {
                //Secteurs d'activité de l'entreprise(Client / Producteur)
                var nomEntreprise = GetCellValue(worksheet, row, 2 ); // Producteur
                var beNumero = GetCellValue(worksheet, row, 3);     // BE N° d'entreprise
                var numEntreprise = GetCellValue(worksheet, row, 4); // N° d'entreprise

                // Si le nom de l'entreprise est vide, passez à la ligne suivante.
                if (string.IsNullOrWhiteSpace(nomEntreprise)) continue;

                // 2. Création d'un client (Client)
                var newClientId = Guid.NewGuid();
                var prodCp = GetCellValue(worksheet, row, 11);
                int? prodLocalitedId = FindLocaliteId(localites, prodCp);

                var client = new Client
                {
                    Id_client = newClientId,
                    Nom_entreprise = nomEntreprise,
                    BE_entreprise = beNumero,
                    Numero_entreprise = numEntreprise,
                    Adresse = $"{GetCellValue(worksheet, row,9)} {GetCellValue(worksheet, row, 10)}".Trim(), // Production Rue + N°
                    Telephone = GetCellValue(worksheet, row, 6),
                    Email =GetCellValue(worksheet, row,8),
                    Remarques = GetCellValue(worksheet, row, 21), // Remarques 
                    Is_deleted = false,
                    Created_at = DateTime.Now,
                    Id_localite = prodLocalitedId
                };

                //// Ищем локацию площадки по CP (Production CP)
                //var prodCp = worksheet.Cell(row, 11).GetValue<string>()?.Trim();
                //if (!string.IsNullOrWhiteSpace(prodCp))
                //{
                //    client.Id_localite = FindLocaliteId(localites, prodCp);
                //}

                _context.Clients.Add(client);

                // 3. Créer une personne de contact (PersonneContact)
                var contactName = GetCellValue(worksheet, row, 5); // Personne de contact
                if (!string.IsNullOrWhiteSpace(contactName))
                {
                    var contact = new PersonneContact
                    {
                        //Id_contact = Guid.NewGuid(),
                        Id_client = newClientId,
                        Nom = contactName,
                        Telephone = GetCellValue(worksheet, row, 6), // Téléphones
                        Gsm = GetCellValue(worksheet, row, 7),       // Gsm2
                        Email =GetCellValue(worksheet, row,8),
                        Id_localite = prodLocalitedId
                    };
                    _context.PersonneContacts.Add(contact);
                }

                // 4.Création (AdresseExploitation)
                var prodRue =GetCellValue(worksheet,row, 9); // Production Rue
                if (!string.IsNullOrWhiteSpace(prodRue))
                {
                    var adresseExp = new AdresseExploitation
                    {
                        //Id_adresse_exp = Guid.NewGuid(),
                        Id_client = newClientId,
                        Rue = prodRue,
                        Numero =GetCellValue(worksheet, row, 10), // Production N°
                        Nom_site = "Site Principal",
                        Id_localite = prodLocalitedId
                    };
                    _context.AdressesExploitation.Add(adresseExp);
                }

                // 5. Créer une adresse légale (SiegeSocial)
                var siegeRaison = GetCellValue(worksheet, row, 13); // Raison Sociale
                var siegeRue = GetCellValue(worksheet, row, 14);    // Siège Social Rue
                var siegeCp = GetCellValue(worksheet, row, 16);     // Siège Social CP

                if (!string.IsNullOrWhiteSpace(siegeRaison) || !string.IsNullOrWhiteSpace(siegeRue))
                {
                    var siege = new SiegeSociale
                    {
                        //Id_siege = Guid.NewGuid(),
                        Id_client = newClientId,
                        Raison_sociale = string.IsNullOrWhiteSpace(siegeRaison) ? nomEntreprise : siegeRaison,
                        Adresse = $"{siegeRue} {GetCellValue(worksheet,row, 15)}".Trim(), // Rue + N°
                        Site_internet = GetCellValue(worksheet, row, 19), // Site internet
                        Secteur_activite = GetCellValue(worksheet, row, 20), // Secteur d'Activité
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
