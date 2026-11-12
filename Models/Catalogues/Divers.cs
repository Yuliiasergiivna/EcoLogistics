using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.Models.Catalogues
{
    public class Divers
    {
        [Key]
        [ScaffoldColumn(false)]
        public int? Id_divers { get; set; }
        [DisplayName("Divers")]
        [MaxLength(100, ErrorMessage ="Le nom de divers ne peut pas dépasser 100 caractères")]
        public string? Nom { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Id_produit { get; set; }
        //[ForeignKey("Id_produit")]
        //public Produit? Produit { get; set; }
    }
}
