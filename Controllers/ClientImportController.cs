using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcoLogistics.ExcelImportService;
using Microsoft.AspNetCore.Authorization;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class ClientImportController : Controller
    {
        private readonly ClientImportService _clientmportService;

        public ClientImportController(ClientImportService clientmportService)
        {
            _clientmportService = clientmportService;
        }

        // GET: Affichage d'une page contenant un formulaire de téléchargement
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        // POST: Traitement du fichier Excel téléchargé
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>Upload(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Error = "Veuillez sélectionner un fichier Excel.";
                return View("Index");
            }
            //Vérification de l'extension du fichier
            var extension = Path.GetExtension(excelFile.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls" && extension != ".xlsm")
            {
                ViewBag.Error = "Le format du fichier doit être .xlsx, .xls ou .xlsm";
                return View("Index");
            }
            try
            {
                using var stream = excelFile.OpenReadStream();
                int importedCount = await _clientmportService.ImportClientsFromExcelAsync(stream);
                
                ViewBag.Success = $"Clients importés avec succès: {importedCount}";
            }
            catch (Exception ex) 
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Error = $"Erreur DB: {innerMessage}";
            }
                return View("Index");
        }

        
    }
}
