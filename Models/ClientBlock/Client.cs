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
        public Guid Id_client {  get; set; }
        [DisplayName( "Nom d'entreprise: ")]
        [MaxLength(64, ErrorMessage ="Le nom d'entreprise ne peut pas dépasser 64 caractères")]
        [Required(ErrorMessage ="Le nom d'entreprise est obligatoire")]
        public string Nom_entreprise { get; set; }
        [DisplayName("Numéro d'entreprise: ")]
        public int? Numero_entreprise { get; set; }
        [DisplayName("BE d'entreprise: ")]
        [MaxLength(32, ErrorMessage ="Le BE d'entreprise ne peut pas dépasser 32 caractères")]
        public string BE_entreprise { get; set; }
        [DisplayName("Adresse d'entreprise: ")]
        [MaxLength(100, ErrorMessage ="L'adresse ne peut pas dépasser 100 caractères")]
        public string Adresse { get; set; }
        [DisplayName("Le numéro de téléphone: ")]
        [MaxLength(32,ErrorMessage ="Le numéro de téléphone ne peut pas dépasser 32 caractères")]
        public string? Telephone { get; set; }
        [DisplayName("Adresse électronique: ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        public string Email { get; set; }
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
        public DateTime? Created_at { get; set; }
        [DisplayName("Date de dernière mise à jour: ")]
        [DataType(DataType.Date)]
        public DateTime? Updated_at { get; set; }
        [DisplayName("Est supprimé:")]
        public bool Is_deleted { get; set; }
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
