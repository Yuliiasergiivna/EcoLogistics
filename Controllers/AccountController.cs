using EcoLogistics.Data;
using EcoLogistics.Models.UserBlock;
using EcoLogistics.ViewModels.UserBlock;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Claims;

namespace EcoLogistics.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: Account/Register
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Localites = new SelectList(
                await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                "Id_localite",
                "Nom_localite"
            );
            return View("Login");
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Email) && await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "L'adresse électronique est déjà utilisée.");
                    ViewBag.Localites = new SelectList(
                        await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                        "Id_localite",
                        "Nom_localite",
                        model.Id_localite);
                    ViewData["ShowRegister"] = true;
                    ViewData["RegisterModel"] = model;

                    return View("Login", new LoginViewModel());
                }
                if (await _context.Users.AnyAsync(u => u.Nickname == model.Nickname))
                {
                    ModelState.AddModelError("Nickname", "Ce pseudo est déjà utilisé.");
                    ViewBag.Localites = new SelectList(await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                        "Id_localite",
                        "Nom_localite",
                        model.Id_localite);
                    ViewData["ShowRegister"] = true;
                    ViewData["RegisterModel"] = model;
                    return View("Login", new LoginViewModel());
                }

                var perso = new Donnees_perso
                {
                    Id_perso = Guid.NewGuid(),
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Poste = model.Poste,
                    Adresse = model.Adresse,
                    Created_at = DateTime.Now,
                    IsActive = true,
                    Statut = "Actif",
                    Id_localite = model.Id_localite
                };

                if (model.Id_localite.HasValue)
                {
                    perso.Id_localite = model.Id_localite.Value;
                }

                _context.Donnees_persos.Add(perso);

                var user = new User
                {
                    Id_user = Guid.NewGuid(),
                    Nickname = model.Nickname,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "User",
                    IsActive = true,
                    Donnees_perso = perso
                };
                user.Password = _passwordHasher.HashPassword(user, model.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    return RedirectToAction("Profile", "User");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id_user.ToString()),
                    new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(user.Nickname) ? user.Nickname : (user.Email ?? "Utilisateur")),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.Role ?? "User")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Localites = new SelectList(
                await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                "Id_localite",
                "Nom_localite", 
                model.Id_localite
            );
            ViewData["ShowRegister"] = true;
            ViewData["RegisterModel"] = model;
            return View("Login", new LoginViewModel());
        }

        // GET: Account/Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.Localites = new SelectList(
                await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                "Id_localite",
                "Nom_localite"
                );
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .Include(u => u.Donnees_perso)
                    .FirstOrDefaultAsync(u => u.Nickname == model.LoginInput | u.Email == model.LoginInput);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Identifiant ou mot de passe incorrect.");
                    ViewBag.Localites = new SelectList(await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(),
                        "Id_localite",
                        "Nom_localite");
                    return View(model);
                }
                if (!user.IsActive || (user.Donnees_perso != null && !user.Donnees_perso.IsActive))
                {
                    ModelState.AddModelError(string.Empty, "Votre compte a été désactivé.");
                    ViewBag.Localites = new SelectList(await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(), 
                        "Id_localite", 
                        "Nom_localite");
                    return View(model);
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError(string.Empty, "Votre compte est désactivé.");
                        return View(model);
                    }

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id_user.ToString()),
                            new Claim(ClaimTypes.Name, !string.IsNullOrEmpty(user.Nickname) ? user.Nickname :( user.Email ?? "Utilisateur")),
                            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                            new Claim(ClaimTypes.Role, user.Role ?? "User"),
                            //new Claim("Nickname", user.Nickname ?? string.Empty)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // Rediriger vers l'action Profil dans le contrôleur User
                        return RedirectToAction("Index", "Home");
                }
                

                ModelState.AddModelError(string.Empty, "Identifiant ou mot de passe incorrect.");
            }
            ViewBag.Localites = new SelectList(await _context.Localites.OrderBy(l => l.Nom_localite).ToListAsync(), "Id_localite", "Nom_localite");
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        //AJAX
        [HttpGet]

        public async Task<IActionResult> CheckAccountType(string input)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == input || u.Email == input);

            if (user == null)
            {
                return Json(new{exists = false});
            }

            bool hasEmail = !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@");
            return Json(new {exists = true, hasEmail = hasEmail});
        }

        //Post: Réinitialisation du mot de passe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Nickname == model.Input || u.Email == model.Input);

                if(user != null && !string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"))
                {
                    if(!string.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.ConfirmNewPassword)
                    {
                        user.Password = _passwordHasher.HashPassword(user, model.NewPassword);
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        TempData["WarningMessage"] = "Votre mot de passe a été mis à jour avec succès. Vous pouvez maintenant vous connecter.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["WarningMessage"] = "Les mots de passe ne correspondent pas.";
                    }
                }
            }
            TempData["WarningMessage"] = "Une erreur est survenue lors de la réinitialisation.";
            return RedirectToAction("Login");
        }
    }
}