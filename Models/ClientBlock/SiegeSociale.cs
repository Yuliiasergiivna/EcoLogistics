using EcoLogistics.Models.Geo;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.ClientBlock
{
    public class SiegeSociale
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id_siege { get; set; }
        public Guid Id_client { get; set; }
        [ForeignKey("Id_client")]
        public Client? Client { get; set; }
        [DisplayName("Raison sociale: ")]
        [MaxLength(255, ErrorMessage = "La raison sociale ne peut pas dépasser 255 caractères")]
        public string? Raison_sociale { get; set; }
        [DisplayName("Rue: ")]
        [MaxLength(255, ErrorMessage = "Nom de rue ne peut pas dépasser 255 caractères")]
        public string? Rue { get; set; }
        [DisplayName("N°: ")]
        [MaxLength(100, ErrorMessage = "Le numero de rue ne peut pas dépasser 255 caractères")]
        public string? Numero { get; set; }
        [DisplayName("Site internet: ")]
        [MaxLength(255)]
        public string? Site_internet { get; set; }
        [DisplayName("Secteur d'activité: ")]
        [MaxLength(255)]
        public string? Secteur_activite {  get; set; }

        [ScaffoldColumn(false)]
        public int? Id_localite { get; set; }
        [ForeignKey("Id_localite")]
        public Localite? Localite { get; set; }

  
    }
}
