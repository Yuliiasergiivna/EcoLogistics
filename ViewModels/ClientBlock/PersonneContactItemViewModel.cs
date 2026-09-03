using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class PersonneContactItemViewModel
    {
        [ScaffoldColumn(false)]
        public int Id_p_contact { get; set; }
        public Guid Id_client { get; set; }
        [DisplayName("Client / Entreprise :")]
        public string? Nom_client { get; set; }

        [DisplayName("Nom : ")]
        public string Nom { get; set; } = string.Empty;
        [DisplayName("Prénom : ")]

        [DisplayName("Téléphone fixe : ")]
        public string? Telephone { get; set; }

        [DisplayName("GSM : ")]
        public string? Gsm { get; set; }

        [DisplayName("Email : ")]
        public string? Email { get; set; }

        //[DisplayName("Adresse : ")]
        //public string? Adresse { get; set; }

        [DisplayName("Localité : ")]
        public string? Localite_Info { get; set; }
    }

}
