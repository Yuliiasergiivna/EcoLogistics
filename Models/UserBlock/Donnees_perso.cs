using EcoLogistics.Models.Geo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.UserBlock
{
    public class Donnees_perso
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id_perso { get; set; } = Guid.NewGuid();
        [MaxLength(32, ErrorMessage = "Le nom de famille ne peut pas dépasser 32 caractères.")]
        [Display(Name = "Nom de famille: ")]
        [Required(ErrorMessage = "Le nom de famille est obligatoire.")]
        public string Nom { get; set; }
        [MaxLength(32, ErrorMessage = "Le prénom ne peut pas dépasser 32 caractères.")]
        [Display(Name = "Prénom: ")]
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        public string Prenom { get; set; }
        [MaxLength(64, ErrorMessage = "Le poste ne peut pas dépasser 64 caractères.")]
        [Display(Name = "Poste: ")]
        public string? Poste { get; set; }
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        [Display(Name = "Adresse: ")]
        public string? Adresse { get; set; }
        [DisplayName("Date de création du profil: ")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date de création du profil est obligatoire.")]
        public DateTime Created_at { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Date de dernière mise à jour du profil: ")]
        public DateTime? Updated_at { get; set; }
        [DisplayName("Date de licenciement: ")]
        [DataType(DataType.Date)]
        public DateTime? Date_licenciement { get; set; }
        [DisplayName("Actif/Employé actuel: ")]
        [Required]
        public bool IsActive { get; set; }
        [MaxLength(20, ErrorMessage = "Le statut ne peut pas dépasser 20 caractères.")]
        [Display(Name = "Statut: ")]
        public string? Statut { get; set; }
        [ScaffoldColumn(false)]
        public int? Id_localite { get; set; }
        [ForeignKey("Id_localite")]
        public Localite? Localite { get; set; }
    }
}
