using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.Catalogues
{
    public class CodeEURAL
    {
        [Key]
        [ScaffoldColumn(false)]
        public int? Id_code_EURAL { get; set; }
        [DisplayName("Code EURAL :")]
        [MaxLength(36, ErrorMessage ="Le code EURAL ne peut pas dépasser 36 caractères ")]
        public string? Code_EURAL { get; set; }
        [DisplayName("Code IBGE :")]
        [MaxLength(36, ErrorMessage = "Le code IBGE ne peut pas dépasser 36 caractères ")]
        public string? Code_IBGE { get; set; }
        [DisplayName("Descriptions :")]
        [MaxLength(255, ErrorMessage ="La description ne peut pas dépasser 255 caractères")]
        public string? Description { get; set; }
        [DisplayName("Directive réf. :")]
        [MaxLength(255, ErrorMessage = "La direcive ne peut pas dépasser 255 caractères")]
        public string? Directive_ref { get; set; }

        //[ScaffoldColumn(false)]
        //public int? Id_produit { get; set; }
        //[ForeignKey("Id_produit")]
        //public Produit? Produit { get; set; }
    }
}
