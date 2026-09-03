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
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.Trim();
                query = query.Where(p =>
                    p.Nom.Contains(searchString) ||
                    (p.Prenom != null && p.Prenom.Contains(searchString)) ||
                    (p.Email != null && p.Email.Contains(searchString)) ||
                    (p.Client != null && p.Client.Nom_entreprise.Contains(searchString))
                );
            }
            var rawContacts = await query.Select(p => new
            {
                Id_p_contact = p.Id_contact,
                p.Nom,
                p.Prenom,
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
                Nom = $"{p.Nom} {p.Prenom}".Trim(),
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

        // 2. Create
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? clientId)
        {
            var model = new PersonneContactFormViewModel();

            if (clientId.HasValue)
            {
                model.Id_client = clientId.Value;
            }


            await PopulateDropdownsAsync(model);
            return View(model);
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonneContactFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contact = new PersonneContact
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.Telephone,
                    Gsm = model.Gsm,
                    Email = model.Email,
                    Id_client = model.Id_client,
                    Id_localite = model.Id_localite
                };

                _context.PersonneContacts.Add(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Client", new { id = model.Id_client });
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }

        // 3. EDIT (GET)
        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _context.PersonneContacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var model = new PersonneContactFormViewModel
            {
                Id_p_contact = contact.Id_contact,
                Nom = contact.Nom,
                Prenom = contact.Prenom,
                Telephone = contact.Telephone,
                Gsm = contact.Gsm,
                Email = contact.Email,
                Id_client = contact.Id_client,
                Id_localite = contact.Id_localite
            };

            await PopulateDropdownsAsync(model);
            return View(model);
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonneContactFormViewModel model)
        {
            if (id != model.Id_p_contact)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var contact = await _context.PersonneContacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }

                contact.Nom = model.Nom;
                contact.Prenom = model.Prenom;
                contact.Telephone = model.Telephone;
                contact.Gsm = model.Gsm;
                contact.Email = model.Email;
                contact.Id_client = model.Id_client;
                contact.Id_localite = model.Id_localite;

                _context.Update(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Client", new { id = model.Id_client });
            }

            await PopulateDropdownsAsync(model);
            return View(model);
        }
        // 4. DELETE (POST)
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await _context.PersonneContacts.FindAsync(id);
            if (contact != null)
            {
                var clientId = contact.Id_client;
                _context.PersonneContacts.Remove(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Client", new { id = clientId });
            }

            return RedirectToAction(nameof(Index));
        }
        // Méthode auxiliaire
        private async Task PopulateDropdownsAsync(PersonneContactFormViewModel model)
        {
            var clients = await _context.Clients
                 .Where(c => !c.Is_deleted)
                 .Select(c => new { c.Id_client, c.Nom_entreprise })
                 .OrderBy(c => c.Nom_entreprise)
                 .ToListAsync();

            var localites = await _context.Localites
                .Select(l => new 
                {
                    l.Id_localite, 
                    Display = l.Code_postal + " - " +
                      (l.CommuneBXL != null ? l.CommuneBXL.Commune_principale : l.Nom_localite) +
                      " (" + (l.Pays != null ? l.Pays.Nom_pays : "") + ")"
                })
                .OrderBy(l => l.Display)
                .ToListAsync();

            model.ClientList = new SelectList(clients, "Id_client", "Nom_entreprise", model.Id_client);
            model.LocaliteList = new SelectList(localites, "Id_localite", "Display", model.Id_localite);

            //if (model is PersonneContactFormViewModel createModel)
            //{
            //    createModel.ClientList = clientList;
            //    createModel.LocaliteList = localiteList;
            //}
            //else if (model is PersonneContactFormViewModel editModel)
            //{
            //    editModel.ClientList = clientList;
            //    editModel.LocaliteList = localiteList;
            //}
        }
    }
}
