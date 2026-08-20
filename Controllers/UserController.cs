using EcoLogistics.Data;
using EcoLogistics.ViewModels.UserBlock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcoLogistics.Controllers
{
    [Authorize(Roles ="Admin")]//Accès réservé aux utilisateurs autorisés
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
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .Include(u => u.Donnees_perso)
                    .ThenInclude(p => p.Localite)
                        .ThenInclude(l => l.CommuneBXL)
                .FirstOrDefaultAsync(u => u.Id_user == userId);

            if (user == null)
            {
                return NotFound();
            }

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
    }
}
