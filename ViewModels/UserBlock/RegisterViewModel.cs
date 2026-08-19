using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class RegisterViewModel
    {
        // --- Compte Utilisateur ---
        [DisplayName("Votre pseudo : ")]
        [MaxLength(64, ErrorMessage = "Le pseudo ne peut pas dépasser 64 caractères.")]
        public string? Nickname { get; set; }

        [DisplayName("Adresse électronique : ")]
        [Required(ErrorMessage = "L'adresse électronique est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        [MaxLength(100, ErrorMessage = "L'adresse électronique ne peut pas dépasser 100 caractères.")]
        public string Email { get; set; }

        [DisplayName("Mot de passe : ")]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&=\-+])[a-zA-Z\d@$!%*?&=\-+]{8,64}$",
            ErrorMessage = "Le mot de passe ne correspond pas à la sécurité minimale requise.")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit avoir au minimum 8 caractères.")]
        [MaxLength(64, ErrorMessage = "Le mot de passe doit avoir au maximum 64 caractères.")]
        public string Password { get; set; }

        [DisplayName("Confirmation du mot de passe : ")]
        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }

        // --- Données Personnelles ---
        [DisplayName("Nom de famille : ")]
        [Required(ErrorMessage = "Le nom de famille est obligatoire.")]
        [MaxLength(32, ErrorMessage = "Le nom de famille ne peut pas dépasser 32 caractères.")]
        public string Nom { get; set; }

        [DisplayName("Prénom : ")]
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [MaxLength(32, ErrorMessage = "Le prénom ne peut pas dépasser 32 caractères.")]
        public string Prenom { get; set; }

        [DisplayName("Poste : ")]
        [MaxLength(64, ErrorMessage = "Le poste ne peut pas dépasser 64 caractères.")]
        public string? Poste { get; set; }

        [DisplayName("Adresse : ")]
        [Required(ErrorMessage = "L'adresse est obligatoire.")]
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        public string? Adresse { get; set; }

        [DisplayName("Localité : ")]
        //[Required(ErrorMessage = "La localité est obligatoire.")]
        public int? Id_localite { get; set; }
    }
}
