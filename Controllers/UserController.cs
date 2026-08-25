using EcoLogistics.Data;
using EcoLogistics.Models.UserBlock;
using EcoLogistics.ViewModels.UserBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoLogistics.Controllers
{
    [Authorize(Roles = "Admin")]//Accès réservé aux utilisateurs autorisés
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: User/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var list = await _context.Donnees_persos
                .Include(d => d.Localite)
                .Select(d => new DonneesPersoListViewModel
                {
                    Id_perso = d.Id_perso,
                    Nom = d.Nom,
                    Prenom = d.Prenom,
                    Poste = d.Poste,
                    Statut = d.Statut,
                    IsActive = d.IsActive,
                    Nom_localite = d.Localite != null ? d.Localite.Nom_localite : "—",
                })
                .ToListAsync();

            return View(list);
        }
        // GET: /User/Details/{id}

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            //var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            var user = await _context.Users
                .Include(u => u.Donnees_perso)
                    .ThenInclude(dp => dp.Localite)
                      .ThenInclude(l => l.CommuneBXL)
                .FirstOrDefaultAsync(u => u.Donnees_perso.Id_perso == id);

            if (user == null)
            {
                return NotFound();
            }
            //User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id_perso == id || u.Donnees_perso.Id_perso == id);

            var viewModel = new UserProfileViewModel
            {
                Id_user = user.Id_user,
                Id_perso = user.Id_perso,
                Nickname = user.Nickname,
                Email = user.Email,
                Role = user.Role,
                IsUserActive = user.IsActive,

                Nom = user.Donnees_perso?.Nom,
                Prenom = user.Donnees_perso?.Prenom,
                Poste = user.Donnees_perso?.Poste,
                Adresse = user.Donnees_perso?.Adresse,
                Created_at = user.Donnees_perso?.Created_at ?? DateTime.MinValue,
                Updated_at = user.Donnees_perso?.Updated_at,
                Date_licenciement = user.Donnees_perso?.Date_licenciement,
                IsEmployeeActive = user.Donnees_perso?.IsActive ?? false,
                Statut = user.Donnees_perso?.Statut,

                Id_localite = user.Donnees_perso?.Id_localite,
                Nom_localite = user.Donnees_perso?.Localite?.Nom_localite,
                Code_postal = user.Donnees_perso?.Localite?.Code_postal,

                Id_commune = user.Donnees_perso?.Localite?.Id_commune,
                Nom_commune = user.Donnees_perso?.Localite?.CommuneBXL?.Commune_principale
            };

            return View(viewModel);
        }
        // GET: User/Edit/{id} (Редактирование ролей и данных)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Donnees_perso)
                .FirstOrDefaultAsync(u => u.Donnees_perso.Id_perso == id || u.Id_user == id);

            if (user == null) return NotFound();

            var viewModel = new UserEditViewModel
            {
                Id_user = user.Id_user,
                Id_perso = user.Donnees_perso.Id_perso,
                Nickname = user.Nickname ?? string.Empty,
                Email = user.Email,
                Role = user.Role ?? "User",
                IsUserActive = user.IsActive,
                Nom = user.Donnees_perso.Nom,
                Prenom = user.Donnees_perso.Prenom,
                Poste = user.Donnees_perso.Poste,
                Adresse = user.Donnees_perso.Adresse,
                Statut = user.Donnees_perso.Statut ?? "Actif",
                Id_localite = user.Donnees_perso.Id_localite
            };

            ViewBag.Localites = new SelectList(
                await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                "Id_localite",
                "Nom_localite",
                viewModel.Id_localite
            );

            return View(viewModel);
        }

        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Include(u => u.Donnees_perso)
                    .FirstOrDefaultAsync(u => u.Id_user == model.Id_user);

                if (user == null) return NotFound();

                // Обновление аккаунта (включая роль Admin/User)
                user.Nickname = model.Nickname;
                user.Email = model.Email;
                user.Role = model.Role;
                user.IsActive = model.IsUserActive;

                // Обновление личных данных
                if (user.Donnees_perso != null)
                {
                    user.Donnees_perso.Nom = model.Nom;
                    user.Donnees_perso.Prenom = model.Prenom;
                    user.Donnees_perso.Poste = model.Poste;
                    user.Donnees_perso.Adresse = model.Adresse;
                    user.Donnees_perso.Statut = model.Statut;
                    user.Donnees_perso.Id_localite = model.Id_localite;
                    user.Donnees_perso.Updated_at = DateTime.Now;

                    // Если уволен, фиксируем дату и деактивируем
                    if (model.Statut == "Licencié" || !model.IsUserActive)
                    {
                        user.Donnees_perso.IsActive = false;
                        user.IsActive = false;
                        user.Donnees_perso.Date_licenciement = DateTime.Now;
                    }
                    else
                    {
                        user.Donnees_perso.IsActive = true;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Profile));
            }

            ViewBag.Localites = new SelectList(
                await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                "Id_localite",
                "Nom_localite",
                model.Id_localite
            );

            return View(model);
        }

        // POST: User/ToggleStatus/{id} (Перевод в неактивные / уволенные)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Donnees_perso)
                .FirstOrDefaultAsync(u => u.Donnees_perso.Id_perso == id || u.Id_user == id);

            if (user == null) return NotFound();

            bool newStatus = !user.IsActive;

            user.IsActive = newStatus;
            if (user.Donnees_perso != null)
            {
                user.Donnees_perso.IsActive = newStatus;
                user.Donnees_perso.Statut = newStatus ? "Actif" : "Licencié";
                if (!newStatus)
                {
                    user.Donnees_perso.Date_licenciement = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Profile));
        }
    }
}
