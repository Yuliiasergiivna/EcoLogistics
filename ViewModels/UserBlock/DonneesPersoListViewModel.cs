namespace EcoLogistics.ViewModels.UserBlock
{
    public class DonneesPersoListViewModel
    {
        public Guid Id_perso { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string? Poste { get; set; }
        public string Statut { get; set; } = "Actif";
        public string? Nom_localite { get; set; }
    }
}
