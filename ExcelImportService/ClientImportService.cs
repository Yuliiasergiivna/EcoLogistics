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
            // 1. Загружаем все локации в память для быстрого поиска по почтовому индексу
            var localites = await _context.Localites.ToListAsync();

            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1); // Первый лист

            int startRow = 20; // Начинаем со 20-й строки
            int lastRow = worksheet.LastRowUsed().RowNumber();
            int importedCount = 0;

            for (int row = startRow; row <= lastRow; row++)
            {
                // Поля компании (Client / Producteur)
                var nomEntreprise = worksheet.Cell(row, 2).GetValue<string>()?.Trim(); // Producteur
                var beNumero = worksheet.Cell(row, 3).GetValue<string>()?.Trim();     // BE N° d'entreprise
                var numEntreprise = worksheet.Cell(row, 4).GetValue<string>()?.Trim(); // N° d'entreprise

                // Если имя компании пустое — пропускаем строку
                if (string.IsNullOrWhiteSpace(nomEntreprise)) continue;

                // 2. Создаем Клиента (Client)
                var newClientId = Guid.NewGuid();
                var prodCp = worksheet.Cell(row, 11).GetValue<string>()?.Trim();
                int? prodLocalitedId = FindLocaliteId(localites, prodCp);

                var client = new Client
                {
                    Id_client = newClientId,
                    Nom_entreprise = nomEntreprise,
                    BE_entreprise = beNumero,
                    Numero_entreprise = numEntreprise,
                    Adresse = $"{worksheet.Cell(row, 9).GetValue<string>()?.Trim()} {worksheet.Cell(row, 10).GetValue<string>()?.Trim()}".Trim(), // Production Rue + N°
                    Telephone = worksheet.Cell(row, 6).GetValue<string>()?.Trim(),
                    Email = worksheet.Cell(row, 8).GetValue<string>()?.Trim(),
                    Remarques = worksheet.Cell(row, 21).GetValue<string>()?.Trim(), // Remarques из колонки U
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

                // 3. Создаем Контактное лицо (PersonneContact)
                var contactName = worksheet.Cell(row, 5).GetValue<string>()?.Trim(); // Personne de contact
                if (!string.IsNullOrWhiteSpace(contactName))
                {
                    var contact = new PersonneContact
                    {
                        //Id_contact = Guid.NewGuid(),
                        Id_client = newClientId,
                        Nom = contactName,
                        Telephone = worksheet.Cell(row, 6).GetValue<string>()?.Trim(), // Téléphones
                        Gsm = worksheet.Cell(row, 7).GetValue<string>()?.Trim(),       // Gsm2
                        Email = worksheet.Cell(row, 8).GetValue<string>()?.Trim(),
                        Id_localite = prodLocalitedId
                    };
                    _context.PersonneContacts.Add(contact);
                }

                // 4. Создаем Площадку (AdresseExploitation)
                var prodRue = worksheet.Cell(row, 9).GetValue<string>()?.Trim(); // Production Rue
                if (!string.IsNullOrWhiteSpace(prodRue))
                {
                    var adresseExp = new AdresseExploitation
                    {
                        //Id_adresse_exp = Guid.NewGuid(),
                        Id_client = newClientId,
                        Rue = prodRue,
                        Numero = worksheet.Cell(row, 10).GetValue<string>()?.Trim(), // Production N°
                        Nom_site = "Site Principal",
                        Id_localite = prodLocalitedId
                    };
                    _context.AdressesExploitation.Add(adresseExp);
                }

                // 5. Создаем Юридический адрес (SiegeSocial)
                var siegeRaison = worksheet.Cell(row, 13).GetValue<string>()?.Trim(); // Raison Sociale
                var siegeRue = worksheet.Cell(row, 14).GetValue<string>()?.Trim();    // Siège Social Rue
                var siegeCp = worksheet.Cell(row, 16).GetValue<string>()?.Trim();     // Siège Social CP

                if (!string.IsNullOrWhiteSpace(siegeRaison) || !string.IsNullOrWhiteSpace(siegeRue))
                {
                    var siege = new SiegeSociale
                    {
                        //Id_siege = Guid.NewGuid(),
                        Id_client = newClientId,
                        Raison_sociale = string.IsNullOrWhiteSpace(siegeRaison) ? nomEntreprise : siegeRaison,
                        Adresse = $"{siegeRue} {worksheet.Cell(row, 15).GetValue<string>()?.Trim()}".Trim(), // Rue + N°
                        Site_internet = worksheet.Cell(row, 19).GetValue<string>()?.Trim(), // Site internet
                        Secteur_activite = worksheet.Cell(row, 20).GetValue<string>()?.Trim(), // Secteur d'Activité
                        Id_localite = FindLocaliteId(localites, siegeCp)
                    };
                    _context.SiegeSociales.Add(siege);
                }

                importedCount++;
            }

            // Сохраняем все данные за одну транзакцию
            await _context.SaveChangesAsync();
            return importedCount;
        }

        // Метод поиска Id_localite по почтовому индексу
        private int? FindLocaliteId(List<Localite> localites, string? codePostal)
        {
            if (string.IsNullOrWhiteSpace(codePostal)) return null;

            var loc = localites.FirstOrDefault(l => l.Code_postal == codePostal.Trim());
            return loc?.Id_localite;
        }
    }
}
