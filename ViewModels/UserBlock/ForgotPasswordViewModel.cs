using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Veuillez entrer votre identifiant (Email et/ou Pseudo).")]
        [DisplayName("Email et/ou Pseudo")]
        public string Input { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [DisplayName("Nouveau mot de passe: ")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        [MaxLength(64, ErrorMessage = "Le mot de passe doit avoir au maximum 64 caractères.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&=\-+])[a-zA-Z\d@$!%*?&=\-+]{8,64}$",
            ErrorMessage = "Le mot de passe ne correspond pas à la sécurité minimale requise.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirmer le nouveau mot de passe: ")]
        [Compare(nameof(NewPassword), ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string? ConfirmNewPassword { get; set; }
    }
}
