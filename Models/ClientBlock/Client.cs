using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EcoLogistics.Models.ClientBlock;
using System.ComponentModel.DataAnnotations.Schema;
using EcoLogistics.Models.Geo;
using EcoLogistics.Models.UserBlock;

namespace EcoLogistics.Models.ClientBlock
{
    public class Client
    {
        [Key]
        [ScaffoldColumn(false)]
        public Guid Id_client { get; set; } = Guid.NewGuid();
        [DisplayName( "Nom d'entreprise: ")]
        [MaxLength(64, ErrorMessage ="Le nom d'entreprise ne peut pas dépasser 64 caractères")]
        [Required(ErrorMessage ="Le nom d'entreprise est obligatoire")]
        public string Nom_entreprise { get; set; } = string.Empty;
        [DisplayName("Numéro d'entreprise: ")]
        [MaxLength(20, ErrorMessage = "Le numéro d'entreprise ne peut pas dépasser 20 caractères.")]
        public string? Numero_entreprise { get; set; }
        [DisplayName("BE d'entreprise: ")]
        [Required(ErrorMessage = "Le BE d'entreprise est obligatoire.")]
        [MaxLength(32, ErrorMessage = "Le BE d'entreprise ne peut pas dépasser 32 caractères")]
        public string BE_entreprise { get; set; } = string.Empty;
        [DisplayName("Adresse d'entreprise: ")]
        [Required(ErrorMessage = "L'adresse est obligatoire.")]
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères")]
        public string Adresse { get; set; } = string.Empty;
        [DisplayName("Le numéro de téléphone: ")]
        [MaxLength(32,ErrorMessage ="Le numéro de téléphone ne peut pas dépasser 32 caractères")]
        public string? Telephone { get; set; }
        [DisplayName("Adresse électronique: ")]
        [Required(ErrorMessage = "L'adresse électronique est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        [MaxLength(100, ErrorMessage = "L'adresse électronique ne peut pas dépasser 100 caractères.")]
        public string Email { get; set; } = string.Empty;
        [DisplayName("Remarques: ")]
        [MaxLength(255, ErrorMessage ="La remarque ne peut pas dépasser 255 caractères")]
        public string? Remarques { get; set; }
        [DisplayName("Numéro d'enregistrement BE: ")]
        [MaxLength(32)]
        public string? Enregistrement_BE { get; set; }
        [DisplayName("Numéro d'agrement BE: ")]
        [MaxLength(32)]
        public string? Agrement_BE { get; set; }
        [DisplayName("Type d'enregistrement: ")]
        [MaxLength(32)]
        public string? Type_enregistrement { get; set; }
        [DisplayName("Presentation: ")]
        [MaxLength(255, ErrorMessage ="Presentation ne peut pas dépasser 255 caractères")]
        public string? Presentation {  get; set; }
        [DisplayName("Date de création: ")]
        [DataType(DataType.Date)]
        public DateTime? Created_at { get; set; } = DateTime.Now;
        [DisplayName("Date de dernière mise à jour: ")]
        [DataType(DataType.Date)]
        public DateTime? Updated_at { get; set; }
        [DisplayName("Est supprimé:")]
        public bool Is_deleted { get; set; }

        //Clés étrangères et propriétés de navigation
        [ScaffoldColumn(false)]
        public int? Id_localite { get; set; }
        [ForeignKey("Id_localite")]
        public Localite? Localite { get; set; }
        [ScaffoldColumn(false)]
        public Guid? Id_user { get; set; }
        [ForeignKey("Id_user")]
        public User? User {  get; set; }
        [ScaffoldColumn(false)]
        public int? Id_siege { get; set; }
        [ForeignKey("Id_siege")]
        public SiegeSociale? SiegeSociale { get; set; }




    }
}
