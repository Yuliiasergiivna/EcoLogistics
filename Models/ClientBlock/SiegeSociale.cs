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
        [DisplayName("Raison sociale: ")]
        [MaxLength(100, ErrorMessage = "La raison sociale ne peut pas dépasser 100 caractères")]
        public string? Raison_sociale { get; set; }
        [DisplayName("Adresse: ")]
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères")]
        public string Adresse { get; set; }
        [DisplayName("Site internet: ")]
        [MaxLength(32)]
        public string? Site_internet { get; set; }
        [DisplayName("Secteur d'activité: ")]
        [MaxLength(32)]
        public string? Secteur_activite {  get; set; }
        [ScaffoldColumn(false)]
        public int? Id_localite { get; set; }
        [ForeignKey("Id_localite")]
        public Localite? Localite { get; set; }
  
    }
}
