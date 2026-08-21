using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class ConducteurListViewModel
    {
        public int Id_conducteur { get; set; }

        [DisplayName("N° de Plaque")]
        public string N_plaque { get; set; } = string.Empty;

        [DisplayName("Conducteur")]
        public string NomConducteur { get; set; } = string.Empty;

        [DisplayName("Quantité / N° Palette")]
        public int Quantite_Npalette { get; set; }
    }
}
