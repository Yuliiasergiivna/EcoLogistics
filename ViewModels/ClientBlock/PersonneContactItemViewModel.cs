using System.ComponentModel;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class PersonneContactItemViewModel
    {
        public int Id_p_contact { get; set; }

        [DisplayName("Nom et prénom : ")]
        public string Nom { get; set; }

        [DisplayName("Téléphone fixe : ")]
        public string? Telephone { get; set; }

        [DisplayName("GSM : ")]
        public string? Gsm { get; set; }

        [DisplayName("Email : ")]
        public string? Email { get; set; }

        [DisplayName("Adresse : ")]
        public string? Adresse { get; set; }

        [DisplayName("Localité : ")]
        public string? Localite_Info { get; set; }
    }
}
