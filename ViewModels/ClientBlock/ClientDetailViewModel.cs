using System.ComponentModel;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class ClientDetailViewModel
    {
        // 1. INFORMATIONS GÉNÉRALES DU CLIENT

        public Guid Id_client { get; set; }

        [DisplayName("Nom d'entreprise : ")]
        public string Nom_entreprise { get; set; }

        [DisplayName("Numéro d'entreprise (BCE) : ")]
        public int? Numero_entreprise { get; set; }

        [DisplayName("N° BE d'entreprise : ")]
        public string? BE_entreprise { get; set; }

        [DisplayName("Adresse principale : ")]
        public string? Adresse { get; set; }

        [DisplayName("Téléphone général : ")]
        public string? Telephone { get; set; }

        [DisplayName("Adresse électronique (Email) : ")]
        public string Email { get; set; }

        [DisplayName("Numéro d'enregistrement BE : ")]
        public string? Enregistrement_BE { get; set; }

        [DisplayName("Numéro d'agrément BE : ")]
        public string? Agrement_BE { get; set; }

        [DisplayName("Type d'enregistrement : ")]
        public string? Type_enregistrement { get; set; }

        [DisplayName("Remarques : ")]
        public string? Remarques { get; set; }

        [DisplayName("Présentation : ")]
        public string? Presentation { get; set; }

        [DisplayName("Date de création : ")]
        public DateTime? Created_at { get; set; }

        [DisplayName("Dernière mise à jour : ")]
        public DateTime? Updated_at { get; set; }

        [DisplayName("Statut supprimé : ")]
        public bool Is_deleted { get; set; }
        // 2. ADRESSE DE LOCALITÉ DU CLIENT (GEO)
        [DisplayName("Localité : ")]
        public string? Client_Nom_localite { get; set; }

        [DisplayName("Code postal : ")]
        public string? Client_Code_postal { get; set; }

        [DisplayName("Commune : ")]
        public string? Client_Nom_commune { get; set; }

        [DisplayName("Pays : ")]
        public string? Client_Nom_pays { get; set; }
        // 3. UTILISATEUR / GESTIONNAIRE RESPONSABLE
        [DisplayName("Gestionnaire responsable : ")]
        public string? User_Nom_complet { get; set; }

        [DisplayName("Email du gestionnaire : ")]
        public string? User_Email { get; set; }

        // 4. SIÈGE SOCIAL (Informations juridiques)
        public int? Id_siege { get; set; }

        [DisplayName("Raison sociale : ")]
        public string? Siege_Raison_sociale { get; set; }

        [DisplayName("Adresse du siège : ")]
        public string? Siege_Adresse { get; set; }

        [DisplayName("Site internet : ")]
        public string? Siege_Site_internet { get; set; }

        [DisplayName("Secteur d'activité : ")]
        public string? Siege_Secteur_activite { get; set; }

        [DisplayName("Localité du siège : ")]
        public string? Siege_Nom_localite { get; set; }

        [DisplayName("Code postal du siège : ")]
        public string? Siege_Code_postal { get; set; }

        [DisplayName("Pays du siège : ")]
        public string? Siege_Nom_pays { get; set; }
        // 5. LISTES DES ENTITÉS LIÉES
        /// public List<PersonneContactItemViewModel> PersonnesContact { get; set; } = new();
        /// public List<AdresseExploitationItemViewModel> AdressesExploitation { get; set; } = new();
    }
}
