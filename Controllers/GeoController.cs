using EcoLogistics.Data;
using EcoLogistics.Models.Geo;
using EcoLogistics.ViewModels.Geo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class GeoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GeoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Geo
        public async Task<IActionResult> Index()
        {
            // Page principal Geo
            var viewModel = new GeoIndexViewModel
            {
                Localites = await _context.Localites
                .Include(l => l.CommuneBXL)
                .Include(l => l.Pays)
                .ToListAsync(),
                Communes = await _context.CommuneBXLs.ToListAsync(),
                PaysList = await _context.Pays.ToListAsync()
            };

            return View(viewModel);
        }
        // GET: Geo/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localite = await _context.Localites
                .Include(l => l.CommuneBXL)
                .Include(l => l.Pays)
                .FirstOrDefaultAsync(m => m.Id_localite == id);

            if (localite == null)
            {
                return NotFound();
            }
            var viewModel = new GeoDetailsViewModel
            {
                Id_localite = localite.Id_localite,
                Nom_localite = localite.Nom_localite,
                Code_postal = localite.Code_postal,
                Province = localite.Province,

                // Pays
                Nom_pays = localite.Pays?.Nom_pays,
                Code_ISO = localite.Pays?.Code_ISO,

                // Communes
                Id_commune = localite.Id_commune,
                Commune_principale = localite.CommuneBXL?.Commune_principale,
                Sous_commune = localite.CommuneBXL?.Sous_commune ?? false,
                Nom_fr = localite.CommuneBXL?.Nom_fr,
                Nom_nl = localite.CommuneBXL?.Nom_nl,
                Type = localite.CommuneBXL?.Type
            };

            return View(viewModel);
        }
    }
}
