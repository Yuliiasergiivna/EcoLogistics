using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EcoLogistics.Data;
using EcoLogistics.Models.Catalogues;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
namespace EcoLogistics.ExcelImportService
{
    public class CatalogueImportService
    {
        private readonly ApplicationDbContext _context;

        public CatalogueImportService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int>ImportCatalogFromExcelAsync(string catalogType, Stream fileStream)
        {
            if(string.IsNullOrWhiteSpace(catalogType))  throw new ArgumentException("Le type de catalogue est obligatoire.", nameof(catalogType));

            using var workbook = new XLWorkbook(fileStream);
            var workheet = workbook.Worksheets.FirstOrDefault(w => w.Visibility == XLWorksheetVisibility.Visible) ?? workbook.Worksheet(1);

            return catalogType.Trim() switch
            {
                var t when t.Equals("CodeEURAL", StringComparison.OrdinalIgnoreCase) => await ImportCodeEuralAsync(workheet),
                var t when t.Equals("CodeDR", StringComparison.OrdinalIgnoreCase) => await ImportCodeDrAsync(workheet),
                var t when t.Equals("Marques", StringComparison.OrdinalIgnoreCase) => await ImportMarquesAsync(workheet),
                var t when t.Equals("Poids", StringComparison.OrdinalIgnoreCase) => await ImportPoidsAsync(workheet),
                var t when t.Equals("Divers", StringComparison.OrdinalIgnoreCase) => await ImportDiversAsync(workheet),
                _ => throw new ArgumentException($"Type de catalogue non supporté : {catalogType}")
            };
        }
        // 1. CodeEURAL
        private async Task<int> ImportCodeEuralAsync(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 2) return 0;

            var existingCodes = await _context.CodeEURALs
                .AsNoTracking()
                .Where(x => x.Code_EURAL != null)
                .Select(x => x.Code_EURAL!.Trim())
                .ToListAsync();

            var seenCodes = new HashSet<string>(existingCodes, StringComparer.OrdinalIgnoreCase);
            int importedCount = 0;

            for (int row = 2; row <= lastRow; row++)
            {
                var raw = GetCellValue(ws, row, 2); // Kolonka B
                if (string.IsNullOrWhiteSpace(raw) || raw == ":" || raw == "/")
                    continue;

                string rawCodePart;
                string? descriptionPart = null;

                int dashIndex = raw.IndexOf('-');
                if (dashIndex >= 0)
                {
                    rawCodePart = raw.Substring(0, dashIndex).Trim();
                    descriptionPart = raw.Substring(dashIndex + 1).Trim();
                }
                else
                {
                    rawCodePart = raw.Trim();
                }

                if (string.IsNullOrWhiteSpace(rawCodePart))
                    continue;

                // Ubirayem lishnie probely vnutri koda IBGE i delaem Code_EURAL bez probelov
                string codeIbge = string.Join(" ", rawCodePart.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                string codeEural = codeIbge.Replace(" ", "");

                if (string.IsNullOrWhiteSpace(codeEural) || seenCodes.Contains(codeEural))
                    continue;

                var entity = new CodeEURAL
                {
                    Code_EURAL = Truncate(codeEural, 36),
                    Code_IBGE = Truncate(codeIbge, 36),
                    Description = Truncate(string.IsNullOrWhiteSpace(descriptionPart) ? null : descriptionPart, 255),
                    Directive_ref = null,
                    IsActive = false
                };

                _context.CodeEURALs.Add(entity);
                seenCodes.Add(codeEural);
                importedCount++;
            }

            if (importedCount > 0)
                await _context.SaveChangesAsync();

            return importedCount;
        }
        // 2. CodeDR (CodeTraitementDesDechets): so 3-y stroki, kolonka A = Code_DR, kolonka B = Description
        private async Task<int> ImportCodeDrAsync(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 3) return 0;

            var existingCodes = await _context.CodeDRs
                .AsNoTracking()
                .Where(x => x.Code_DR != null)
                .Select(x => x.Code_DR!.Trim())
                .ToListAsync();

            var seenCodes = new HashSet<string>(existingCodes, StringComparer.OrdinalIgnoreCase);
            int importedCount = 0;

            for (int row = 3; row <= lastRow; row++)
            {
                var raw = GetCellValue(ws, row, 2); 
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                string codeDr;
                string? description = null;
                int dashIndex = raw.IndexOf('-');
                if (dashIndex >= 0)
                {
                    codeDr = raw.Substring(0, dashIndex).Trim();
                    description = raw.Substring(dashIndex + 1).Trim();
                }
                else
                {
                    codeDr = raw.Trim();
                }

                if (string.IsNullOrWhiteSpace(codeDr) || seenCodes.Contains(codeDr))
                    continue;

                var entity = new CodeDR
                {
                    Code_DR = Truncate( codeDr,16),
                    Description = Truncate(string.IsNullOrWhiteSpace(description) ? null : description, 255)
                };

                _context.CodeDRs.Add(entity);
                seenCodes.Add(codeDr);
                importedCount++;
            }

            if (importedCount > 0)
                await _context.SaveChangesAsync();

            return importedCount;
        }
        // 3. Marques: so 2-y stroki, kolonka A = Nom_marque
        private async Task<int> ImportMarquesAsync(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 2) return 0;

            var existingNames = await _context.Marques
                .AsNoTracking()
                .Where(x => x.Nom_marque != null)
                .Select(x => x.Nom_marque!.Trim())
                .ToListAsync();

            var seenNames = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);
            int importedCount = 0;

            for (int row = 2; row <= lastRow; row++)
            {
                var name = GetCellValue(ws, row, 1); // Kolonka A
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                name = name.Trim();
                if (seenNames.Contains(name))
                    continue;

                _context.Marques.Add(new Marques
                {
                    Nom_marque = name
                });

                seenNames.Add(name);
                importedCount++;
            }

            if (importedCount > 0)
                await _context.SaveChangesAsync();

            return importedCount;
        }
        // 4. Poids: so 2-y stroki, tolko 2 pervyye kolonki: A = Designation, B = Poids_valeur
        private async Task<int> ImportPoidsAsync(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 2) return 0;

            var existingDesignations = await _context.Poids
                .AsNoTracking()
                .Where(x => x.Designation != null)
                .Select(x => x.Designation!.Trim())
                .ToListAsync();

            var seenDesignations = new HashSet<string>(existingDesignations, StringComparer.OrdinalIgnoreCase);
            int importedCount = 0;

            for (int row = 2; row <= lastRow; row++)
            {
                var designation = GetCellValue(ws, row, 1); // Kolonka A
                if (string.IsNullOrWhiteSpace(designation))
                    continue;

                designation = designation.Trim();
                if (seenDesignations.Contains(designation))
                    continue;

                var rawWeight = GetCellValue(ws, row, 2); // Kolonka B
                decimal? weightValue = null;

                if (!string.IsNullOrWhiteSpace(rawWeight))
                {
                    var normalized = rawWeight.Replace(',', '.').Trim();
                    if (decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                    {
                        weightValue = parsed;
                    }
                }

                _context.Poids.Add(new Poids
                {
                    Designation = designation,
                    Poids_valeur = weightValue
                });

                seenDesignations.Add(designation);
                importedCount++;
            }

            if (importedCount > 0)
                await _context.SaveChangesAsync();

            return importedCount;
        }

        // 5. Divers: so 2-y stroki, kolonka A = Nom
        private async Task<int> ImportDiversAsync(IXLWorksheet ws)
        {
            int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
            if (lastRow < 2) return 0;

            var existingNames = await _context.Divers
                .AsNoTracking()
                .Where(x => x.Nom != null)
                .Select(x => x.Nom!.Trim())
                .ToListAsync();

            var seenNames = new HashSet<string>(existingNames, StringComparer.OrdinalIgnoreCase);
            int importedCount = 0;

            for (int row = 2; row <= lastRow; row++)
            {
                var nom = GetCellValue(ws, row, 1); // Kolonka A
                if (string.IsNullOrWhiteSpace(nom))
                    continue;

                nom = nom.Trim();
                if (seenNames.Contains(nom))
                    continue;

                _context.Divers.Add(new Divers
                {
                    Nom = nom
                });

                seenNames.Add(nom);
                importedCount++;
            }

            if (importedCount > 0)
                await _context.SaveChangesAsync();

            return importedCount;
        }

        private static string? GetCellValue(IXLWorksheet ws, int row, int col, int maxLength = 0)
        {
            var cell = ws.Cell(row, col);
            if (cell.IsEmpty()) return null;

            var val = cell.GetFormattedString()?.Trim();
            if (string.IsNullOrWhiteSpace(val)) return null;

            if (maxLength > 0 && val.Length > maxLength)
                return val.Substring(0, maxLength);

            return val;
        }

        private static string? Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var trimmed = value.Trim();
            return trimmed.Length > maxLength ? trimmed.Substring(0, maxLength) : trimmed;
        }
    }
}
