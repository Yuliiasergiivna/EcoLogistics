using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class UserEditViewModel
    {
        public Guid Id_user { get; set; }
        public Guid Id_perso { get; set; }

        // --- Compte Utilisateur ---
        [DisplayName("Pseudo : ")]
        [Required(ErrorMessage = "Le pseudo est obligatoire.")]
        [MaxLength(64)]
        public string Nickname { get; set; } = string.Empty;

        [DisplayName("Adresse électronique : ")]
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;

        [DisplayName("Rôle : ")]
        public string Role { get; set; } = "User"; // "Admin" / "User"

        [DisplayName("Compte actif : ")]
        public bool IsUserActive { get; set; }

        // --- Données Personnelles ---
        [DisplayName("Nom : ")]
        public string? Nom { get; set; } = string.Empty;

        [DisplayName("Prénom : ")]
        public string? Prenom { get; set; } = string.Empty;

        [DisplayName("Poste : ")]
        public string? Poste { get; set; }

        [DisplayName("Adresse : ")]
        public string? Adresse { get; set; }

        [DisplayName("Statut : ")]
        public string Statut { get; set; } = "Actif"; // e.g. "Actif", "Licencié"

        [DisplayName("Localité : ")]
        public int? Id_localite { get; set; }
    }
}
