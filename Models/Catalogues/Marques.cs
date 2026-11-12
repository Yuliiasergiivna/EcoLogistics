using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.Models.Catalogues
{
    public class Marques
    {
        [Key]
        [ScaffoldColumn(false)]
        public int? Id_marque {  get; set; }
        [DisplayName("Marques: ")]
        [MaxLength(100, ErrorMessage ="Le nom de marque ne peut pas dépasser 100 caractères")]
        public string? Nom_marque {  get; set; }

        //[ScaffoldColumn(false)]
        //public int? Id_produit { get; set; }
        //[ForeignKey("Id_produit")]
        //public Produit? Produit { get; set; }
    }
}
