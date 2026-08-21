using EcoLogistics.Data;
using EcoLogistics.Models.UserBlock;
using EcoLogistics.ViewModels.UserBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class ConducteurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConducteurController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /Conducteur/Index
        [HttpGet]
        public async Task< IActionResult> Index()
        {
            var conducteurs = await _context.Conducteurs
                .Include(c => c.Donnees_perso)
                .Select(c => new ConducteurListViewModel
                {
                    Id_conducteur = c.Id_conducteur,
                    N_plaque = c.N_plaque,
                    NomConducteur = c.Donnees_perso != null
                        ? $"{c.Donnees_perso.Nom} {c.Donnees_perso.Prenom}"
                        : "Non assigné",
                    Quantite_Npalette = c.Quantite_Npalette
                })
                .ToListAsync();

            return View(conducteurs);
        }
        // GET: /Conducteur/Create (Pour Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateEmployesDropDownList();
            return View(new ConducteurFormViewModel());
        }

        // POST: /Conducteur/Create ( Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConducteurFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var conducteur = new Conducteur
                {
                    N_plaque = model.N_plaque,
                    Quantite_Npalette = model.Quantite_Npalette,
                    Id_perso = model.Id_perso
                };

                _context.Conducteurs.Add(conducteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateEmployesDropDownList(model.Id_perso);
            return View(model);
        }

        // GET: /Conducteur/Edit/5 (Admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var conducteur = await _context.Conducteurs.FindAsync(id);
            if (conducteur == null) return NotFound();

            var viewModel = new ConducteurFormViewModel
            {
                Id_conducteur = conducteur.Id_conducteur,
                N_plaque = conducteur.N_plaque,
                Quantite_Npalette = conducteur.Quantite_Npalette,
                Id_perso = conducteur.Id_perso
            };

            await PopulateEmployesDropDownList(conducteur.Id_perso);
            return View(viewModel);
        }

        // POST: /Conducteur/Edit/5 ( Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConducteurFormViewModel model)
        {
            if (id != model.Id_conducteur) return NotFound();

            if (ModelState.IsValid)
            {
                var conducteur = await _context.Conducteurs.FindAsync(id);
                if (conducteur == null) return NotFound();

                conducteur.N_plaque = model.N_plaque;
                conducteur.Quantite_Npalette = model.Quantite_Npalette;
                conducteur.Id_perso = model.Id_perso;

                _context.Conducteurs.Update(conducteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateEmployesDropDownList(model.Id_perso);
            return View(model);
        }

        // POST: /Conducteur/Delete/5 ( Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var conducteur = await _context.Conducteurs.FindAsync(id);
            if (conducteur != null)
            {
                _context.Conducteurs.Remove(conducteur);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateEmployesDropDownList(object? selectedEmploye = null)
        {
            var employesData = await _context.Donnees_persos
                .Where(d => d.IsActive)
                .Select(d => new
                {
                    d.Id_perso,
                    d.Nom,
                    d.Prenom
                })
                .ToListAsync();
            var employes = employesData
                .Select(d => new
                {
                    Id_perso = d.Id_perso,
                    FullName = $"{d.Nom}{d.Prenom}"
                })
                .OrderBy(d => d.FullName)
                .ToList();

            ViewBag.Employes = new SelectList(employes, "Id_perso", "FullName", selectedEmploye);
        }
    }
}
