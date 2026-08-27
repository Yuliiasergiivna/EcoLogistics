using System.ComponentModel;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class ClientListViewModel
    {
        // --- 1. Données principales du Client (Client) ---
        public Guid Id_client { get; set; }
        [DisplayName("N° d'entreprise : ")]
        public string? Numero_entreprise { get; set; }
        [DisplayName("Nom du producteur / Entreprise : ")]
        public string Nom_entreprise { get; set; }= string.Empty;
        [DisplayName("N° BE d'entreprise : ")]
        public string? BE_entreprise { get; set; }
        [DisplayName("Remarques : ")]
        public string? Remarques { get; set; }
        [DisplayName("Présentation : ")]
        public string? Presentation { get; set; }
        [DisplayName("Statut supprimé : ")]
        public bool Is_deleted { get; set; }

        // --- 2. Personne de contact principale (PersonneContact) ---
        public int? Id_p_contact { get; set; }
        [DisplayName("Personne de contact : ")]
        public string? Contact_nom { get; set; }
        [DisplayName("Téléphone fixe : ")]
        public string? Contact_telephone { get; set; }
        [DisplayName("Téléphone mobile (GSM) : ")]
        public string? Contact_gsm { get; set; }
        [DisplayName("Adresse électronique du contact : ")]
        public string? Contact_email { get; set; }

        // --- 3. Adresse d'exploitation (AdresseExploitation + Geo) ---
        public int? Id_adresse_exp { get; set; }
        [DisplayName("Nom du site d'exploitation : ")]
        public string? Production_nom_site { get; set; }
        [DisplayName("Rue (Exploitation) : ")]
        public string? Production_rue { get; set; }
        [DisplayName("N° (Exploitation) : ")]
        public string? Production_numero { get; set; }
        [DisplayName("Code postal (Exploitation) : ")]
        public string? Production_code_postal { get; set; }
        [DisplayName("Commune (Exploitation) : ")]
        public string? Production_commune { get; set; }
        [DisplayName("Pays (Exploitation) : ")]
        public string? Production_pays { get; set; }

        // --- 4. Siège social / Juridique (SiegeSociale + Geo) ---
        public int? Id_siege { get; set; }
        [DisplayName("Raison sociale : ")]
        public string? Raison_sociale { get; set; }
        [DisplayName("Adresse (Siège social) : ")]
        public string? Siege_adresse { get; set; }
        [DisplayName("Code postal (Siège social) : ")]
        public string? Siege_code_postal { get; set; }
        [DisplayName("Commune (Siège social) : ")]
        public string? Siege_commune { get; set; }
        [DisplayName("Pays (Siège social) : ")]
        public string? Siege_pays { get; set; }
        [DisplayName("Site internet : ")]
        public string? Site_internet { get; set; }
        [DisplayName("Secteur d'activité : ")]
        public string? Secteur_activite { get; set; }
    }
}
