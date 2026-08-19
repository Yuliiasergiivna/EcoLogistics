using System.ComponentModel;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class UserProfileViewModel
    {
        public Guid Id_user { get; set; }
        public Guid? Id_perso { get; set; }
        public int? Id_localite { get; set; }
        public int? Id_commune { get; set; }

        //--- User ---

        [DisplayName("Votre pseudo : ")]
        public string? Nickname { get; set; }

        [DisplayName("Adresse électronique : ")]
        public string Email { get; set; }

        [DisplayName("Rôle : ")]
        public string? Role { get; set; }

        [DisplayName("Compte actif : ")]
        public bool IsUserActive { get; set; }

        //--- Données personnelles ---

        [DisplayName("Nom de famille : ")]
        public string? Nom { get; set; }

        [DisplayName("Prénom : ")]
        public string? Prenom { get; set; }

        [DisplayName("Poste : ")]
        public string? Poste { get; set; }

        [DisplayName("Adresse : ")]
        public string? Adresse { get; set; }

        [DisplayName("Date de création du profil : ")]
        public DateTime Created_at { get; set; }

        [DisplayName("Dernière mise à jour : ")]
        public DateTime? Updated_at { get; set; }

        [DisplayName("Date de licenciement : ")]
        public DateTime? Date_licenciement { get; set; }

        [DisplayName("Employé actuel : ")]
        public bool IsEmployeeActive { get; set; }

        [DisplayName("Statut : ")]
        public string? Statut { get; set; }

        //--- Localité ---

        [DisplayName("Localité : ")]
        public string? Nom_localite { get; set; }

        [DisplayName("Code postal : ")]
        public string? Code_postal { get; set; }

        //---Commune BXL---

        [DisplayName("Commune : ")]
        public string? Nom_commune { get; set; }

    }
}
