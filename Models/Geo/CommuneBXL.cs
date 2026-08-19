using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.Models.Geo
{
    public class CommuneBXL
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id_commune { get; set; }
        
        [DisplayName("Commune principale: ")]
        [MaxLength(64, ErrorMessage = "Le nom de la commune principale ne peut pas dépasser 64 caractères.")]
        [Required(ErrorMessage = "Le nom de la commune principale est obligatoire.")]
        public string Commune_principale { get; set; }
        [DisplayName("Sous-commune: ")]
        public bool Sous_commune { get; set; }
        [DisplayName("Nom en français: ")]
        [MaxLength(64, ErrorMessage = "Le nom en français ne peut pas dépasser 64 caractères.")]
        public string? Nom_fr { get; set; }
        [DisplayName("Nom en néerlandais: ")]
        [MaxLength(64, ErrorMessage = "Le nom en néerlandais ne peut pas dépasser 64 caractères.")]
        public string? Nom_nl { get; set; }
        [DisplayName("Type: ")]
        [MaxLength(64, ErrorMessage = "Le type ne peut pas dépasser 64 caractères.")]
        public string? Type { get; set; }
    }
}
