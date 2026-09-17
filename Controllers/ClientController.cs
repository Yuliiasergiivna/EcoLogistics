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
                    .AsNoTracking()
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

                Client_code_postal = c.Localite != null ? c.Localite.Code_postal : null,
                Client_commune = c.Localite != null && c.Localite.CommuneBXL != null ? c.Localite.CommuneBXL.Commune_principale : null,
                Client_pays = c.Localite != null && c.Localite.Pays != null ? c.Localite.Pays.Nom_pays : null,

                Contact_nom = c.PersonnesContact.Select(p => p.Nom).FirstOrDefault(),
                Contact_telephone = c.PersonnesContact.Select(p =>p.Telephone).FirstOrDefault(),
                Contact_gsm = c.PersonnesContact.Select(p => p.Gsm).FirstOrDefault(),
                Contact_email = c.PersonnesContact.Select(p => p.Email).FirstOrDefault(),

                Production_nom_site = c.AdressesExploitation.Select(a => a.Nom_site).FirstOrDefault(),
                Production_rue = c.AdressesExploitation.Select( a => a.Rue ).FirstOrDefault(),
                Production_numero = c.AdressesExploitation.Select( a => a.Numero).FirstOrDefault(),
                Production_code_postal = c.AdressesExploitation.Select( a => a.Localite != null ? a.Localite.Code_postal : null).FirstOrDefault(),
                Production_commune = c.AdressesExploitation.Select( a => a.Localite != null && a.Localite.CommuneBXL != null ? a.Localite.CommuneBXL.Commune_principale : null).FirstOrDefault(),
                Production_pays = c.AdressesExploitation.Select( a => a.Localite != null && a.Localite.Pays != null ? a.Localite.Pays.Nom_pays : null).FirstOrDefault(),

                Id_siege = c.SiegeSociale != null ? c.SiegeSociale.Id_siege : null,
                Raison_sociale = c.SiegeSociale != null ? c.SiegeSociale.Raison_sociale : null,
                Siege_adresse = c.SiegeSociale != null ? c.SiegeSociale.Adresse : null,
                Siege_code_postal = c.SiegeSociale != null && c.SiegeSociale.Localite != null ? c.SiegeSociale.Localite.Code_postal : null,
                Siege_commune = c.SiegeSociale != null && c.SiegeSociale.Localite != null && c.SiegeSociale.Localite.CommuneBXL != null ? c.SiegeSociale.Localite.CommuneBXL.Commune_principale : null,
                Siege_pays = c.SiegeSociale != null && c.SiegeSociale.Localite != null && c.SiegeSociale.Localite.Pays != null ? c.SiegeSociale.Localite.Pays.Nom_pays : null,
                Site_internet = c.SiegeSociale != null ? c.SiegeSociale.Site_internet : null,
                Secteur_activite = c.SiegeSociale != null ? c.SiegeSociale.Secteur_activite : null
            }).ToListAsync();

                ViewData["CurrentFilter"] = searchString;
                return View(clientsList);
            }

            // 2. FICHE DÉTAILLÉE (DETAILS)
            [HttpGet]
            public async Task<IActionResult> Details(Guid id)
            {
                var client = await _context.Clients
                    .AsNoTracking()
                    .Include(c => c.Localite)
                        .ThenInclude(l => l!.CommuneBXL)
                    .Include(c => c.Localite)
                        .ThenInclude(l => l!.Pays)

                    .Include(c => c.User)
                        .ThenInclude(u => u!.Donnees_perso)

                    .Include(c => c.SiegeSociale)
                        .ThenInclude(s => s!.Localite)
                            .ThenInclude(l => l!.CommuneBXL)
                    .Include(c => c.SiegeSociale )
                        .ThenInclude(s => s!.Localite)
                            .ThenInclude(l => l!.Pays)

                    .Include(c => c.PersonnesContact)
                        .ThenInclude(p => p.Localite)
                            .ThenInclude(l => l!.CommuneBXL)
                    .Include(c => c.PersonnesContact)
                        .ThenInclude(p => p.Localite)
                            .ThenInclude(l => l!.Pays)

                    .Include(c => c.AdressesExploitation)
                        .ThenInclude(a => a.Localite)
                            .ThenInclude(l => l!.CommuneBXL)
                    .Include(c => c.AdressesExploitation)
                        .ThenInclude(a => a.Localite)
                            .ThenInclude(l => l!.Pays)

                    .FirstOrDefaultAsync(c => c.Id_client == id);

                if (client == null) return NotFound();
            var primaryContact = client.PersonnesContact.FirstOrDefault();
            var primaryExploitation = client.AdressesExploitation.FirstOrDefault();

            var viewModel = new ClientDetailViewModel
                {
                    Id_client = client.Id_client,
                    Nom_entreprise = client.Nom_entreprise,
                    Numero_entreprise = client.Numero_entreprise,
                    BE_entreprise = client.BE_entreprise,
                    Adresse = client.Adresse,
                    Telephone = client.Telephone,
                    Email = client.Email,
                    Enregistrement_BE = client.Enregistrement_BE,
                    Agrement_BE = client.Agrement_BE,
                    Type_enregistrement = client.Type_enregistrement,
                    Remarques = client.Remarques,
                    Presentation = client.Presentation,
                    Created_at = client.Created_at,
                    Updated_at = client.Updated_at,
                    Is_deleted = client.Is_deleted,

                    Client_Adresse = client.Localite?.Nom_localite,
                    Client_Code_postal = client.Localite?.Code_postal,
                    Client_Nom_commune = client.Localite?.CommuneBXL?.Commune_principale ?? client.Localite?.Nom_localite ,
                    Client_Nom_pays = client.Localite?.Pays?.Nom_pays,

                    User_Nom_complet = client.User != null
                        ? (!string.IsNullOrWhiteSpace(client.User.Nickname)
                            ? client.User.Nickname : client.User.Donnees_perso != null
                                ? $"{client.User.Donnees_perso.Nom} {client.User.Donnees_perso.Prenom}".Trim()
                                : client.User.Email) : "Non assigné",
                    User_Email = client.User?.Email,

                    Id_siege = client.SiegeSociale?.Id_siege,
                    Siege_Raison_sociale = client.SiegeSociale?.Raison_sociale,
                    Siege_Adresse = client.SiegeSociale?.Adresse,
                    Siege_Site_internet = client.SiegeSociale?.Site_internet,
                    Siege_Secteur_activite = client.SiegeSociale?.Secteur_activite,
                    Siege_Nom_localite = client.SiegeSociale?.Localite?.CommuneBXL?.Commune_principale,
                    Siege_Code_postal = client.SiegeSociale?.Localite?.Code_postal,
                    Siege_Nom_pays = client.SiegeSociale?.Localite?.Pays?.Nom_pays,

                    PersonnesContact = client.PersonnesContact.Select(p => new PersonneContactItemViewModel
                    {
                        Id_p_contact = p.Id_contact,
                        Nom = p.Nom,
                        Prenom = p.Prenom,
                        Telephone = p.Telephone,
                        Gsm = p.Gsm,
                        Email = p.Email,
                        Id_client = p.Id_client,
                        Nom_client = client.Nom_entreprise,
                        Localite_Info = p.Localite != null ? $"{p.Localite.Code_postal} {p.Localite.CommuneBXL?.Commune_principale}".Trim() : null
                    }).ToList(),

                    AdressesExploitation = client.AdressesExploitation.Select(a => new AdresseExploitationItemViewModel
                    {
                        Id_adresse_exp = a.Id_adresse_exp,
                        Nom_site = a.Nom_site ?? string.Empty,
                        Rue = a.Rue ?? string.Empty,
                        Numero = a.Numero ?? string.Empty,
                        Id_client = a.Id_client,
                        Id_localite = a.Id_localite,
                        Adresse_Code_postal = a.Localite?.Code_postal,
                        Adresse_Commune = a.Localite?.CommuneBXL?.Commune_principale,
                        Adresse_Pays = a.Localite?.Pays?.Nom_pays
                    }).ToList()
                };

                return View(viewModel);
            }

            // 3. CRÉATION (CREATE)
            [Authorize(Roles = "Admin,Manager")]
            [HttpGet]
            public async Task<IActionResult> Create()
            {
                var model = new ClientCreateViewModel();
                await PopulateDropdownsAsync(model);
                return View(model);
            }

            [Authorize(Roles = "Admin,Manager")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ClientCreateViewModel model)
            {
                if (ModelState.IsValid)
                {
                //using var transaction = await _context.Database.BeginTransactionAsync();
                try
                    {
                        SiegeSociale? siege = null;
                    if (!string.IsNullOrWhiteSpace(model.Siege_Raison_sociale) || !string.IsNullOrWhiteSpace(model.Siege_Adresse))
                    {
                        siege = new SiegeSociale
                        {
                            Raison_sociale = model.Siege_Raison_sociale,
                            Adresse = model.Siege_Adresse,
                            Site_internet = model.Siege_Site_internet,
                            Secteur_activite = model.Siege_Secteur_activite,
                            Id_localite = model.Siege_Id_localite
                        };
                        _context.SiegeSociales.Add(siege);
                        await _context.SaveChangesAsync();
                    }

                        var client = new Client
                        {
                            Id_client = Guid.NewGuid(),
                            Nom_entreprise = model.Nom_entreprise,
                            Numero_entreprise = model.Numero_entreprise,
                            BE_entreprise = model.BE_entreprise,
                            Adresse = model.Adresse,
                            Telephone = model.Telephone,
                            Email = model.Email,
                            Enregistrement_BE = model.Enregistrement_BE,
                            Agrement_BE = model.Agrement_BE,
                            Type_enregistrement = model.Type_enregistrement,
                            Remarques = model.Remarques,
                            Presentation = model.Presentation,
                            Id_user = model.Id_user,
                            Id_localite = model.Id_localite,
                            Id_siege = siege?.Id_siege,
                            Created_at = DateTime.Now
                        };

                        _context.Clients.Add(client);

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Details), new { id = client.Id_client });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Une erreur est survenue lors de la création du client.");
                    }
                }

                await PopulateDropdownsAsync(model);
                return View(model);
            }

            // 4. MODIFICATION (EDIT)
            [Authorize(Roles = "Admin,Manager")]
            [HttpGet]
            public async Task<IActionResult> Edit(Guid id)
            {
                var client = await _context.Clients
                    .Include(c => c.SiegeSociale)
                    .FirstOrDefaultAsync(c => c.Id_client == id);

                if (client == null) return NotFound();

                var model = new ClientEditViewModel
                {
                    Id_client = client.Id_client,

                    Nom_entreprise = client.Nom_entreprise,
                    Numero_entreprise = client.Numero_entreprise,
                    BE_entreprise = client.BE_entreprise,
                    Adresse = client.Adresse,
                    Telephone = client.Telephone,
                    Email = client.Email,
                    Enregistrement_BE = client.Enregistrement_BE,
                    Agrement_BE = client.Agrement_BE,
                    Type_enregistrement = client.Type_enregistrement,
                    Remarques = client.Remarques,
                    Presentation = client.Presentation,
                    Is_deleted = client.Is_deleted,
                    Id_user = client.Id_user,
                    Id_localite = client.Id_localite,

                    Siege_Raison_sociale = client.SiegeSociale?.Raison_sociale,
                    Siege_Adresse = client.SiegeSociale?.Adresse,
                    Siege_Site_internet = client.SiegeSociale?.Site_internet,
                    Siege_Secteur_activite = client.SiegeSociale?.Secteur_activite,
                    Siege_Id_localite = client.SiegeSociale?.Id_localite,

                };

                await PopulateDropdownsAsync(model);
                return View(model);
            }

            [Authorize(Roles = "Admin,Manager")]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Guid id, ClientEditViewModel model)
            {
                if (id != model.Id_client) return NotFound();

                if (ModelState.IsValid)
                {
                    try
                    {
                        var client = await _context.Clients
                            .Include(c => c.SiegeSociale)
                            .FirstOrDefaultAsync(c => c.Id_client == id);

                        if (client == null) return NotFound();

                        client.Nom_entreprise = model.Nom_entreprise;
                        client.Numero_entreprise = model.Numero_entreprise;
                        client.BE_entreprise = model.BE_entreprise;
                        client.Adresse = model.Adresse;
                        client.Telephone = model.Telephone;
                        client.Email = model.Email;
                        client.Enregistrement_BE = model.Enregistrement_BE;
                        client.Agrement_BE = model.Agrement_BE;
                        client.Type_enregistrement = model.Type_enregistrement;
                        client.Remarques = model.Remarques;
                        client.Presentation = model.Presentation;
                        client.Is_deleted = model.Is_deleted;
                        client.Id_user = model.Id_user;
                        client.Id_localite = model.Id_localite;
                        client.Updated_at = DateTime.Now;

                    //SiegeSociale
                        if (client.SiegeSociale != null)
                    {
                        client.SiegeSociale.Raison_sociale = model.Siege_Raison_sociale;
                        client.SiegeSociale.Adresse = model.Siege_Adresse;
                        client.SiegeSociale.Site_internet = model.Siege_Site_internet;
                        client.SiegeSociale.Secteur_activite = model.Siege_Secteur_activite;
                        client.SiegeSociale.Id_localite = model.Siege_Id_localite;
                    }
                    else if (!string.IsNullOrWhiteSpace(model.Siege_Raison_sociale) || !string.IsNullOrWhiteSpace(model.Siege_Adresse))
                    {
                        var newSiege = new SiegeSociale
                        {
                            Raison_sociale = model.Siege_Raison_sociale,
                            Adresse = model.Siege_Adresse,
                            Site_internet = model.Siege_Site_internet,
                            Secteur_activite = model.Siege_Secteur_activite,
                            Id_localite = model.Siege_Id_localite
                        };
                         _context.SiegeSociales.Add(newSiege);
                        await _context.SaveChangesAsync();
                        client.Id_siege = newSiege.Id_siege;
                    }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = client.Id_client });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Une erreur est survenue lors de la mise à jour.");
                    }
                }

                await PopulateDropdownsAsync(model);
                return View(model);
            }

        // 5. SOFT DELETE
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSoftDelete(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            client.Is_deleted = !client.Is_deleted;
            client.Updated_at = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Méthodes auxiliaires

        private async Task PopulateDropdownsAsync(ClientCreateViewModel model)
        {
            model.LocaliteList = await GetLocaliteSelectListAsync();
            model.UserList = await GetUserSelectListAsync();
        }

        private async Task PopulateDropdownsAsync(ClientEditViewModel model)
        {
            model.LocaliteList = await GetLocaliteSelectListAsync();
            model.UserList = await GetUserSelectListAsync();
        }

        private async Task<List<SelectListItem>> GetLocaliteSelectListAsync()
        {
            return await _context.Localites
                .AsNoTracking()
                .Select(l => new SelectListItem
                {
                    Value = l.Id_localite.ToString(),
                    Text = l.Code_postal + " - " +
                       (l.CommuneBXL.Commune_principale ?? l.Nom_localite) +
                       " (" + (l.Pays.Nom_pays ?? "") + ")"
                })
                .OrderBy(item =>item.Text)
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> GetUserSelectListAsync()
        {
            var rawUsers = await _context.Users
                .AsNoTracking()
                .Select(u => new
                { 
                    u.Id_user,
                    u.Nickname,
                    u.Email,
                    Nom = u.Donnees_perso.Nom,
                    Prenom = u.Donnees_perso.Prenom
                })
                .ToListAsync();
            return rawUsers
                
                .Select(u => new SelectListItem
                {
                    Value = u.Id_user.ToString(),
                    Text = !string.IsNullOrWhiteSpace(u.Nickname)
                        ? u.Nickname
                        : (!string.IsNullOrWhiteSpace(u.Nom) || !string.IsNullOrWhiteSpace(u.Prenom)
                        ? $"{u.Nom}{u.Prenom}"
                        .Trim() : u.Email)
                })
                .OrderBy(item => item.Text)
                .ToList();
        }

    }

}


