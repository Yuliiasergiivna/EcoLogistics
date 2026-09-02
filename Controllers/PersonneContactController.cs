using EcoLogistics.Data;
using EcoLogistics.ViewModels.ClientBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Controllers
{
    [Authorize]
    public class PersonneContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonneContactController(ApplicationDbContext context)
        {
            _context = context;
        }
        // 1. REGISTRE GÉNÉRAL DES CONTACTS (INDEX)
        [HttpGet]
        public async Task<IActionResult> Index(string? searchString)
        {
            var query = _context.PersonneContacts
                .AsNoTracking()
                //.Include(p => p.Client)
                //.Include(p => p.Localite)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(p =>
                    p.Nom.Contains(searchString) ||
                    (p.Email != null && p.Email.Contains(searchString)) ||
                    (p.Client != null && p.Client.Nom_entreprise.Contains(searchString))
                );
            }
            var rawContacts = await query.Select(p => new
            {
                Id_p_contact = p.Id_contact,
                p.Nom,
                p.Telephone,
                p.Gsm,
                p.Email,
                p.Id_client,
                Nom_client = p.Client != null ? p.Client.Nom_entreprise : "—",
                CodePostal = p.Localite != null ? p.Localite.Code_postal : null,
                CommuneBXl = p.Localite != null && p.Localite.CommuneBXL != null ? p.Localite.CommuneBXL.Commune_principale : null,
                NomLocalite = p.Localite != null ? p.Localite.Nom_localite : null
            }).ToListAsync();

            var contactsList = rawContacts.Select(p => new PersonneContactItemViewModel
            {
                Id_p_contact = p.Id_p_contact,
                Nom = p.Nom,
                Telephone = p.Telephone,
                Gsm = p.Gsm,
                Email = p.Email,
                Id_client = p.Id_client,
                Nom_client = p.Nom_client,
                Localite_Info = p.CodePostal != null 
                    ? $"{p.CodePostal} {(!string.IsNullOrEmpty(p.CommuneBXl) ? p.CommuneBXl : p.NomLocalite)}"
                    : null
            }).ToList();

            ViewData["CurrentFilter"] = searchString;
            return View(contactsList);
        }
    }
}
