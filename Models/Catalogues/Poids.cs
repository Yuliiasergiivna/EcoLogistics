using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.Catalogues
{
    public class Poids
    {
        [Key]
        [ScaffoldColumn(false)]
        public int? Id_poids {  get; set; }
        [DisplayName("Designation")]
        [MaxLength(150, ErrorMessage ="La designation ne peut pas dépasser 150 caractères")]
        public string? Designation {  get; set; }
        [DisplayName("Poids")]
        [Column("Poids", TypeName ="decimal(8,2)")]
        public decimal? Poids_valeur { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Id_produit { get; set; }
        //[ForeignKey("Id_produit")]
        //public Produit? Produit { get; set; }
    }
}
