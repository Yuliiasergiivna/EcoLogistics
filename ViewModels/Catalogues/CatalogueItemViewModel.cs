using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.Catalogues
{
    public class CatalogueItemViewModel
    {
        [ScaffoldColumn(false)]
        public int? Id { get; set; }
        // "Marques", "Divers", "Poids", "CodeEURAL", "CodeDR"
        public string? CatalogType { get; set; }

        [DisplayName("Code / Nom / Désignation :")]
        public string? Name { get; set; }

        [DisplayName("Description :")]
        public string? Description { get; set; }
        [DisplayName("Poids (kg) :")]
        public decimal? PoidsValeur { get; set; }
    }
}
