using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class LoginViewModel
    {
        [DisplayName("Identifiant (Pseudo ou Email) : ")]
        [Required(ErrorMessage = "L'identifiant est obligatoire.")]
        public string LoginInput { get; set; } = string.Empty;

        [DisplayName("Mot de passe : ")]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DisplayName("Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}
