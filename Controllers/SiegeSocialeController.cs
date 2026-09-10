using EcoLogistics.Data;
using EcoLogistics.Models.ClientBlock;
using EcoLogistics.ViewModels.ClientBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace EcoLogistics.Controllers
{
    [Authorize]
    public class SiegeSocialeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SiegeSocialeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //1. GET: REPERTOIRE DES SIÈGES SOCIAUX (INDEX)
        [HttpGet]
        public async Task<IActionResult> Index( string? searchString)
        {
            var query = _context.SiegeSociales
                .AsNoTracking()
                .Include(s => s.Localite)
                    .ThenInclude(l => l!.CommuneBXL)
                .Include(s => s.Localite)
                    .ThenInclude(l => l!.Pays)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(c =>
                    c.Raison_sociale != null && c.Raison_sociale.Contains(searchString) ||
                    (c.Adresse != null && c.Adresse.Contains(searchString)) ||
                    (c.Site_internet != null && c.Site_internet.Contains(searchString))
                );
            }

            var siegeList = await query.Select(c  => new SiegeSocialeViewModel
            {
                Id_siege = c.Id_siege,
                Raison_sociale = c.Raison_sociale,
                Adresse = c.Adresse,
                Site_internet = c.Site_internet,
                Secteur_activite = c.Secteur_activite,
                Siege_Code_postal = c.Localite != null ? c.Localite.Code_postal : null,
                Siege_Nom_commune = c.Localite != null && c.Localite.CommuneBXL != null ? c.Localite.CommuneBXL.Commune_principale : null,
                Siege_Pays = c.Localite != null && c.Localite.Pays != null ? c.Localite.Pays.Nom_pays : null
            }).ToListAsync();

            ViewData["CurrentFilter"] = searchString;
            return View(siegeList);
        }

        // 2.Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var siege = await _context.SiegeSociales
                .AsNoTracking()
                .Include(c => c.Localite)
                    .ThenInclude(l => l!.CommuneBXL)
                .Include(c => c.Localite)
                    .ThenInclude(l => l!.Pays)
                .FirstOrDefaultAsync( c => c.Id_siege == id);
            if (siege == null) return NotFound();

            var viewModel = new SiegeSocialeViewModel
            {
                Id_siege = siege.Id_siege,
                Raison_sociale = siege.Raison_sociale,
                Adresse = siege.Adresse,
                Site_internet = siege.Site_internet,
                Secteur_activite = siege.Secteur_activite,

                Siege_Code_postal = siege.Localite?.Code_postal,
                Siege_Nom_commune = siege.Localite?.CommuneBXL?.Commune_principale,
                Siege_Pays = siege?.Localite?.Pays?.Nom_pays,

            };
            return View(viewModel);
        }

        // 3. GET: Create
 
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create(Guid? clientId)
        {
            var model = new SiegeSocialeViewModel();

            if(clientId.HasValue)
            {
                model.Id_client = clientId.Value;
            }
            await PopulateLocalitesAsync(model);
            return View(model);
        }

        // POST: Create
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SiegeSocialeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var siege = new SiegeSociale
                    {
                        Raison_sociale = model.Raison_sociale,
                        Adresse = model.Adresse,
                        Site_internet = model.Site_internet,
                        Secteur_activite = model.Secteur_activite,
                        Id_localite = model.Id_localite
                    };
                    _context.SiegeSociales.Add(siege);
                    await _context.SaveChangesAsync();

                    if (model.Id_client.HasValue)
                    {
                        var client = await _context.Clients.FindAsync(model.Id_client.Value);
                        if (client != null)
                        {
                            client.Id_siege = siege.Id_siege;
                            await _context.SaveChangesAsync();
                        }
                        return RedirectToAction("Details", "Client", new { id = model.Id_client.Value });
                    }

                    return RedirectToAction(nameof(Details), new { id = siege.Id_siege });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de la création du Siege Sociale");
                }
            }
            await PopulateLocalitesAsync(model);
            return View(model);
        }

        // GET: Edit
        [Authorize(Roles ="Admin, Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, Guid? clientId)
        {
            var siege = await _context.SiegeSociales.FindAsync(id);
            if (siege == null) return NotFound();

            var model = new SiegeSocialeViewModel
            {
                Id_siege = siege.Id_siege,
                Id_client = clientId,
                Raison_sociale = siege.Raison_sociale,
                Adresse = siege.Adresse,
                Site_internet = siege.Site_internet,
                Secteur_activite = siege.Secteur_activite,
                Id_localite = siege.Id_localite,
            };
            await PopulateLocalitesAsync(model);
            return View(model);
        }

        // POST: Edit
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SiegeSocialeViewModel model)
        {
            if (id != model.Id_siege) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var siege = await _context.SiegeSociales
                        .FirstOrDefaultAsync(c => c.Id_siege == id);
                    if (siege != null)
                    {
                        siege.Raison_sociale = model.Raison_sociale;
                        siege.Adresse = model.Adresse;
                        siege.Site_internet = model.Site_internet;
                        siege.Secteur_activite = model.Secteur_activite;
                        siege.Id_localite = model.Id_localite;

                        _context.Update(siege);
                        await _context.SaveChangesAsync();
                        if (model.Id_client.HasValue)
                        {
                            return RedirectToAction("Details", "Client", new { id = model.Id_client.Value });
                        }
                    }
                    return RedirectToAction(nameof(Details), new { id = model.Id_siege});
                }
                catch(Exception) 
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour.");
                }
            }
                await PopulateLocalitesAsync(model);
                return View(model);
 
        }

        // POST: Delete
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Guid? clientId)
        {
            var siege = await _context.SiegeSociales.FindAsync(id);
            if (siege != null)
            {
                var siegeId = siege.Id_siege;
                _context.SiegeSociales.Remove(siege);
                await _context.SaveChangesAsync();
            }
            if (clientId.HasValue)
            {
                    return RedirectToAction("Details", "Client", new { id = clientId.Value });
            }

                return RedirectToAction(nameof(Index));
        }
        private async Task PopulateLocalitesAsync(SiegeSocialeViewModel model)
        {
            var localites = await _context.Localites
                .AsNoTracking()
                .Select(l => new 
                {
                    Value = l.Id_localite.ToString(),
                    Text = l.Code_postal + " - " +
                           (l.CommuneBXL != null ? l.CommuneBXL.Commune_principale : l.Nom_localite) +
                           " (" + (l.Pays !=null ? l.Pays.Nom_pays : "") + ")"
                })
                .OrderBy(item => item.Text)
                .ToListAsync();
            model.LocaliteList = new SelectList(localites, "Value", "Text", model.Id_localite);
        }
    }
}
