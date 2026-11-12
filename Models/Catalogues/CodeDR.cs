using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.Models.Catalogues
{
    public class CodeDR
    {
        [Key]
        [ScaffoldColumn(false)]
        public int? Id_code_DR { get; set; }
        [DisplayName("Code DR")]
        [MaxLength(16, ErrorMessage ="Le code DR ne peut pas dépasser 16 caractères")]
        public string? Code_DR { get; set; }
        [DisplayName("Description")]
        [MaxLength(255, ErrorMessage ="La description ne peut pas dépasser 255 caractères")]
        public string? Description { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Id_produit { get; set; }
        //[ForeignKey("Id_produit")]
        //public Produit? Produit { get; set; }
    }
}
