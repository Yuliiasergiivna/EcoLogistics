using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.Models.Geo
{
    public class Pays
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id_pays { get; set; }
        [DisplayName("Nom du pays: ")]
        [Required(ErrorMessage = "Le nom du pays est obligatoire.")]
        [MaxLength(100, ErrorMessage = "Le nom du pays ne peut pas dépasser 100 caractères.")]
        public string Nom_pays { get; set; }  
        [DisplayName("Code ISO: ")]
        [Required(ErrorMessage = "Le code ISO est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le code ISO doit contenir  3 caractères.")]
        public string Code_ISO { get; set; }
    }
}
