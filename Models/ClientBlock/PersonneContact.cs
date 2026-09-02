using EcoLogistics.Models.Geo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.ClientBlock
{
    public class PersonneContact
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id_contact { get; set; }
        [DisplayName("Nom: ")]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères")]
        public string Nom { get; set; } = string.Empty;
        [DisplayName("Prénom: ")]
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le prénom ne peut pas dépasser 64 caractères")]
        public string Prenom { get; set; } = string.Empty;
        [DisplayName("Téléphone fixe: ")]
        [MaxLength(32, ErrorMessage = "Le numéro de téléphone ne peut pas dépasser 32 caractères")]
        public string? Telephone { get; set; }
        [DisplayName("Téléphone mobile (GSM): ")]
        [MaxLength(32, ErrorMessage = "Le numéro de GSM ne peut pas dépasser 32 caractères")]
        public string? Gsm { get; set; }
        [DisplayName("Adresse électronique: ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        [MaxLength(64, ErrorMessage = "L'adresse électronique ne peut pas dépasser 64 caractères")]
        public string? Email { get; set; }

        [ScaffoldColumn(false)]
        public Guid Id_client { get; set; }
        [ForeignKey("Id_client")]
        public Client? Client { get; set; }
        [ScaffoldColumn(false)]
        public int? Id_localite { get; set; }
        [ForeignKey("Id_localite")]
        public Localite? Localite { get; set; }




    }
}
