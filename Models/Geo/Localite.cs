using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.Geo
{
    public class Localite
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id_localite { get; set; }
        [DisplayName("Localité")]
        [MaxLength(64)]
        public string Nom_localite { get; set; }
        [DisplayName("Code postal")]
        [MaxLength(16)]
        public string Code_postal { get; set; }
        [DisplayName("Province")]
        [MaxLength(64)]
        public string? Province { get; set; }
        [ScaffoldColumn(false)]
        public int? Id_pays { get; set; }
        [ForeignKey("Id_pays")]
        public Pays? Pays { get; set; }
        [ScaffoldColumn(false)]
        public int? Id_commune { get; set; }
        [ForeignKey("Id_commune")]
        public CommuneBXL? CommuneBXL { get; set; }
    }
}
