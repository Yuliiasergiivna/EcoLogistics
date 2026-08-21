using System.ComponentModel;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class DonneesPersoListViewModel
    {
        public Guid Id_perso { get; set; }
        [DisplayName("Nom de famille : ")]
        public string Nom { get; set; } = string.Empty;
        [DisplayName("Prénom : ")]
        public string Prenom { get; set; } = string.Empty;
        [DisplayName("Poste : ")]
        public string? Poste { get; set; }
        [DisplayName("Statut : ")]
        public string Statut { get; set; } = "Actif";
        public bool IsActive { get; set; } = true;
        public string? Nom_localite { get; set; }
    }
}
