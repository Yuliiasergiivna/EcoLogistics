using EcoLogistics.Data;
using EcoLogistics.ViewModels.ClientBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }
        // 1. REPERTOIRE DES CLIENTS (INDEX)
        [HttpGet]
        public async Task<IActionResult> Index(string? searchString)
        {
            var query = _context.Clients
                .Include(c => c.Localite)
                .Include(c => c.SiegeSociale)
                    .ThenInclude(s => s!.Localite)
                .Include(c => c.PersonnesContact)
                .Include(c => c.AdressesExploitation)
                    .ThenInclude(a => a.Localite)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(c =>
                    c.Nom_entreprise.Contains(searchString) ||
                    (c.Numero_entreprise != null && c.Numero_entreprise.Contains(searchString)) ||
                    (c.Email != null && c.Email.Contains(searchString))
                );
            }

            var clientsList = await query.Select(c => new ClientListViewModel
            {
                Id_client = c.Id_client,
                Numero_entreprise = c.Numero_entreprise,
                Nom_entreprise = c.Nom_entreprise,
                BE_entreprise = c.BE_entreprise,
                Remarques = c.Remarques,
                Presentation = c.Presentation,
                Is_deleted = c.Is_deleted,

                // Premier contact
                Id_p_contact = c.PersonnesContact.Select(p => (int?)p.Id_contact).FirstOrDefault(),
                Contact_nom = c.PersonnesContact.Select(p => p.Nom).FirstOrDefault(),
                Contact_telephone = c.PersonnesContact.Select(p => p.Telephone).FirstOrDefault(),
                Contact_gsm = c.PersonnesContact.Select(p => p.Gsm).FirstOrDefault(),
                Contact_email = c.PersonnesContact.Select(p => p.Email).FirstOrDefault(),

                // Site principal
                Id_adresse_exp = c.AdressesExploitation.Select(a => (int?)a.Id_adresse_exp).FirstOrDefault(),
                Production_nom_site = c.AdressesExploitation.Select(a => a.Nom_site).FirstOrDefault(),
                Production_rue = c.AdressesExploitation.Select(a => a.Rue).FirstOrDefault(),
                Production_numero = c.AdressesExploitation.Select(a => a.Numero).FirstOrDefault(),
                Production_code_postal = c.AdressesExploitation.Select(a => a.Localite != null ? a.Localite.Code_postal : null).FirstOrDefault(),
                //Production_commune = c.AdressesExploitation.Select(a => a.Localite != null ? a.Localite.CommuneBXL : null).FirstOrDefault(),
                //Production_pays = c.AdressesExploitation.Select(a => a.Localite != null ? a.Localite.Pays : null).FirstOrDefault(),

                // Adresse légale (Siege)
                Id_siege = c.SiegeSociale != null ? c.SiegeSociale.Id_siege : null,
                Raison_sociale = c.SiegeSociale != null ? c.SiegeSociale.Raison_sociale : null,
                Siege_adresse = c.SiegeSociale != null ? c.SiegeSociale.Adresse : null,
                Siege_code_postal = c.SiegeSociale != null && c.SiegeSociale.Localite != null ? c.SiegeSociale.Localite.Code_postal : null,
                //Siege_commune = c.SiegeSociale != null && c.SiegeSociale.Localite != null ? c.SiegeSociale.Localite.CommuneBXL : null,
                //Siege_pays = c.SiegeSociale != null && c.SiegeSociale.Localite != null ? c.SiegeSociale.Localite.Pays : null,
                Site_internet = c.SiegeSociale != null ? c.SiegeSociale.Site_internet : null,
                Secteur_activite = c.SiegeSociale != null ? c.SiegeSociale.Secteur_activite : null
            }).ToListAsync();

            ViewData["CurrentFilter"] = searchString;
            return View(clientsList);
        }
    }
}
