using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.UserBlock
{
    [Table("users")]
    public class User
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id_user { get; set; } = Guid.NewGuid();
        [MaxLength(64)]
        [DisplayName("Votre pseudo: ")]
        public string? Nickname { get; set; }
        [DisplayName("Adresse électronique : ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        [MaxLength(100)]
        public string? Email { get; set; }
        [DisplayName("Mot de passe : ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le mot de passe doit avoir au maximum 64 caractères.")]
        public string Password { get; set; }
        [DisplayName("Rôle : ")]
        [MaxLength(24)]
        public string Role { get; set; } = "User";
        [DisplayName("Actif : ")]
        public bool IsActive { get; set; }
        [ScaffoldColumn(false)]
        public Guid? Id_perso { get; set; }
        [ForeignKey("Id_perso")]
        public Donnees_perso? Donnees_perso { get; set; }
    }
}
