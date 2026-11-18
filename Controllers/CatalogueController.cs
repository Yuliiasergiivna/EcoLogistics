using EcoLogistics.Data;
using EcoLogistics.Models.Catalogues;
using EcoLogistics.ViewModels.Catalogues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoLogistics.Models;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class CatalogueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogueController(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly HashSet<string> _editableCatalogs = new(StringComparer.OrdinalIgnoreCase)
        {
            "Marques", "Divers", "Poids", "CodeEURAL"
        };
        [HttpGet]
        public async Task<IActionResult> Index(string type = "Marques", string? searchString = null)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                type = "Marques";
            }
            var items = new List<CatalogueItemViewModel>();
            List<CodeEURAL> activeCodes = new();
            List<CodeEURAL> allEuralCodes = new();

            switch (type)
            {
                case "CodeEURAL":
                    var euralQuery = _context.CodeEURALs.AsNoTracking().AsQueryable();

                        if (!string.IsNullOrWhiteSpace(searchString))
                        {
                            euralQuery = euralQuery.Where(x =>
                                x.Code_EURAL != null && x.Code_EURAL.Contains(searchString) ||
                                (x.Code_IBGE != null && x.Code_IBGE.Contains(searchString)) ||
                                (x.Description != null && x.Description.Contains(searchString)));
                        }

                        allEuralCodes = await euralQuery.OrderBy(c => c.Code_EURAL).ToListAsync();

                       
                        activeCodes = allEuralCodes
                            .Where(c => c.IsActive)
                            .ToList();
                    var officialCode = allEuralCodes.Where(c => !c.IsActive).ToList();

                    ViewData["ActiveCodes"] = activeCodes;
                    break;

                case "CodeDR":
                    var drQuery = _context.CodeDRs.AsNoTracking().AsQueryable();
                    if (!string.IsNullOrWhiteSpace(searchString))
                        drQuery = drQuery.Where(x => (x.Code_DR != null && x.Code_DR.Contains(searchString)) || (x.Description != null && x.Description.Contains(searchString)));

                    items = await drQuery.Select(x => new CatalogueItemViewModel
                    {
                        Id = x.Id_code_DR ?? 0,
                        CatalogType = "CodeDR",
                        Name = x.Code_DR ?? "",
                        Description = x.Description
                    }).ToListAsync();
                    break;

                case "Divers":
                    var diversQuery = _context.Divers.AsNoTracking().AsQueryable();
                    if (!string.IsNullOrWhiteSpace(searchString))
                        diversQuery = diversQuery.Where(x => x.Nom != null && x.Nom.Contains(searchString));

                    items = await diversQuery.Select(x => new CatalogueItemViewModel
                    {
                        Id = x.Id_divers ?? 0,
                        CatalogType = "Divers",
                        Name = x.Nom ?? ""
                    }).ToListAsync();
                    break;

                case "Poids":
                    var poidsQuery = _context.Poids.AsNoTracking().AsQueryable();
                    if (!string.IsNullOrWhiteSpace(searchString))
                        poidsQuery = poidsQuery.Where(x => x.Designation != null && x.Designation.Contains(searchString));

                    items = await poidsQuery.Select(x => new CatalogueItemViewModel
                    {
                        Id = x.Id_poids ?? 0,
                        CatalogType = "Poids",
                        Name = x.Designation ?? "",
                        PoidsValeur = x.Poids_valeur,
                        Description = x.Poids_valeur.HasValue ? $"{x.Poids_valeur} kg" : "—"
                    }).ToListAsync();
                    break;

                case "Marques":
                default:
                    type = "Marques";
                    var marquesQuery = _context.Marques.AsNoTracking().AsQueryable();
                    if (!string.IsNullOrWhiteSpace(searchString))
                        marquesQuery = marquesQuery.Where(x => x.Nom_marque != null && x.Nom_marque.Contains(searchString));

                    items = await marquesQuery.Select(x => new CatalogueItemViewModel
                    {
                        Id = x.Id_marque ?? 0,
                        CatalogType = "Marques",
                        Name = x.Nom_marque ?? ""
                    }).ToListAsync();
                    break;
            }
            string catalogDisplayName = type switch
            {
                "CodeEURAL" => "Codes EURAL",
                "CodeDR" => "Codes DR",
                "Divers" => "Divers",
                "Poids" => "Poids",
                "Marques" or _ => "Marques"
            };

            ViewData["CurrentType"] = type;
            ViewData["CatalogDisplayName"] = catalogDisplayName;
            ViewData["IsEditable"] = _editableCatalogs.Contains(type);
            ViewData["CurrentFilter"] = searchString;

            if (type == "CodeEURAL")
            {
                return View("CodeEural", allEuralCodes);
            }

            return View(items);
        }
        //POST CreateEURAL
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateEural(string code_EURAL, string? code_IBGE, string? description, string? directive_ref)
        {
            if (string.IsNullOrWhiteSpace(code_EURAL))
            {
                TempData["Error"] = "Le code EURAL est obligatoire.";
                return RedirectToAction(nameof(Index), new { type = "CodeEURAL" });
            }

            var newCode = new CodeEURAL
            {
                Code_EURAL = code_EURAL.Trim(),
                Code_IBGE = code_IBGE?.Trim(),
                Description = description?.Trim(),
                Directive_ref = directive_ref?.Trim(),
                IsActive = true
            };

            _context.CodeEURALs.Add(newCode);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { type = "CodeEURAL" });
        }
        //GET EditEURAL
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> EditEural(int id)
        {
            var item = await _context.CodeEURALs.FindAsync(id);
            if (item == null) return NotFound();
            return View(item); // Sozdaem otdelnyy View EditEural.cshtml ili ispolzuem modalnuyu formu
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEural(int id, CodeEURAL model)
        {
            if (id != model.Id_code_EURAL) return BadRequest();

            var item = await _context.CodeEURALs.FindAsync(id);
            if (item == null) return NotFound();

            item.Code_EURAL = model.Code_EURAL?.Trim();
            item.Code_IBGE = model.Code_IBGE?.Trim();
            item.Description = model.Description?.Trim();
            item.Directive_ref = model.Directive_ref?.Trim();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { type = "CodeEURAL" });
        }
        // 2. CREATE (POST)
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CatalogueItemViewModel model)
        {
            if (!_editableCatalogs.Contains(model.CatalogType))
                return BadRequest("Ce catalogue est en lecture seule.");

            if (!ModelState.IsValid)
                return View(model);

            switch (model.CatalogType)
            {
                case "Marques":
                    _context.Marques.Add(new Marques { Nom_marque = model.Name.Trim() });
                    break;
                case "Divers":
                    _context.Divers.Add(new Divers { Nom = model.Name.Trim() });
                    break;
                case "Poids":
                    _context.Poids.Add(new Poids { Designation = model.Name.Trim(), Poids_valeur = model.PoidsValeur });
                    break;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { type = model.CatalogType });
        }
        // 3. EDIT (GET)
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(string type, int id)
        {
            if (!_editableCatalogs.Contains(type))
                return BadRequest("Ce catalogue est en lecture seule.");

            CatalogueItemViewModel? model = null;

            switch (type)
            {
                case "Marques":
                    var m = await _context.Marques.FindAsync(id);
                    if (m != null) model = new CatalogueItemViewModel { Id = id, CatalogType = type, Name = m.Nom_marque ?? "" };
                    break;
                case "Divers":
                    var d = await _context.Divers.FindAsync(id);
                    if (d != null) model = new CatalogueItemViewModel { Id = id, CatalogType = type, Name = d.Nom ?? "" };
                    break;
                case "Poids":
                    var p = await _context.Poids.FindAsync(id);
                    if (p != null) model = new CatalogueItemViewModel { Id = id, CatalogType = type, Name = p.Designation ?? "", PoidsValeur = p.Poids_valeur };
                    break;
            }

            if (model == null) return NotFound();
            return View(model);
        }
        // 3. EDIT (POST)
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CatalogueItemViewModel model)
        {
            if (id != model.Id || !_editableCatalogs.Contains(model.CatalogType))
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            switch (model.CatalogType)
            {
                case "Marques":
                    var m = await _context.Marques.FindAsync(id);
                    if (m == null) return NotFound();
                    m.Nom_marque = model.Name.Trim();
                    break;
                case "Divers":
                    var d = await _context.Divers.FindAsync(id);
                    if (d == null) return NotFound();
                    d.Nom = model.Name.Trim();
                    break;
                case "Poids":
                    var p = await _context.Poids.FindAsync(id);
                    if (p == null) return NotFound();
                    p.Designation = model.Name.Trim();
                    p.Poids_valeur = model.PoidsValeur;
                    break;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { type = model.CatalogType });
        }
        // 4. DELETE (POST)
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string type, int id)
        {
            if (!_editableCatalogs.Contains(type))
                return BadRequest("Ce catalogue est en lecture seule.");

            switch (type)
            {
                case "Marques":
                    var m = await _context.Marques.FindAsync(id);
                    if (m != null) _context.Marques.Remove(m);
                    break;
                case "Divers":
                    var d = await _context.Divers.FindAsync(id);
                    if (d != null) _context.Divers.Remove(d);
                    break;
                case "Poids":
                    var p = await _context.Poids.FindAsync(id);
                    if (p != null) _context.Poids.Remove(p);
                    break;
                case "CodeEURAL":
                    var euralItem = await _context.CodeEURALs.FindAsync(id);
                    if (euralItem != null) _context.CodeEURALs.Remove(euralItem);
                    break;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { type });
        }
    }
}
