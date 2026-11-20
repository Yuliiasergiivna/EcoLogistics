using EcoLogistics.ExcelImportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class CatalogueImportController : Controller
    {
        private readonly CatalogueImportService _importService;

        private static readonly HashSet<string> _supportedCatalogues = new(StringComparer.OrdinalIgnoreCase)
        {
            "CodeEURAL", "CodeDR", "Marques", "Poids", "Divers"
        };

        private static readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".xlsx", ".xls",".xlsm"
        };
        public CatalogueImportController(CatalogueImportService importService)
        {
            _importService = importService;
        }
        //GET : 
        [HttpGet]
        public IActionResult Index(string type = "CodeEURAL")
        {
            if (!_supportedCatalogues.Contains(type))
            {
                type = "CodeEURAL";
            }

            ViewData["CatalogType"] = type;
            return View();
        }

        // POST: /CatalogImport/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Upload(string type, IFormFile? excelFile)
        {
            if (!_supportedCatalogues.Contains(type))
            {
                ViewData["CatalogType"] = "CodeEURAL";
                ViewBag.Error = $"Type de catalogue non supporté : {type}";
                return View("Index");
            }

            ViewData["CatalogType"] = type;

            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Error = "Veuillez sélectionner un fichier Excel.";
                return View("Index");
            }

            var extension = Path.GetExtension(excelFile.FileName);
            if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension))
            {
                ViewBag.Error = "Le format du fichier doit être .xlsx, .xls ou .xlsm";
                return View("Index");
            }

            try
            {
                using var stream = excelFile.OpenReadStream();
                int importedCount = await _importService.ImportCatalogFromExcelAsync(type, stream);

                ViewBag.Success = $"Importation réussie ({type}) : {importedCount} élément(s) ajouté(s).";
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Error = $"Erreur lors de l'importation : {innerMessage}";
            }

            return View("Index");
        }
    }
}
