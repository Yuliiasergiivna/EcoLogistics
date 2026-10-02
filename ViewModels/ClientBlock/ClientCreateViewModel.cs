using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class ClientCreateViewModel
    {
        // 1. INFORMATIONS DU CLIENT (Entreprise)
        
        public Guid Id_client {  get; set; }
        [DisplayName("N° Client : ")]
        public string? Numero_client { get; set; }

        [DisplayName("Nom d'entreprise : ")]
        public string? Nom_entreprise { get; set; }

        [DisplayName("Numéro d'entreprise (BCE) : ")]
        public string? Numero_entreprise { get; set; }

        [DisplayName("N° BE d'entreprise : ")]
        public string? BE_entreprise { get; set; }

        [DisplayName("Adresse principale : ")]
        public string? Adresse { get; set; }

        [DisplayName("Téléphone général : ")]
        public string? Telephone { get; set; }

        [DisplayName("Adresse électronique (Email) : ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        public string? Email { get; set; } 

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

        [DisplayName("Gestionnaire / Utilisateur responsable : ")]
        public Guid? Id_user { get; set; }

        [DisplayName("Localité principale : ")]
        public int? Id_localite { get; set; }

        // 2. SIÈGE SOCIAL (Juridique)

        [DisplayName("Raison sociale : ")]
        public string? Siege_Raison_sociale { get; set; }

        [DisplayName("Adresse du siège social : ")]
        public string? Siege_Adresse { get; set; }

        [DisplayName("Site internet : ")]
        public string? Siege_Site_internet { get; set; }

        [DisplayName("Secteur d'activité : ")]
        public string? Siege_Secteur_activite { get; set; }

        [DisplayName("Localité du siège social : ")]
        public int? Siege_Id_localite { get; set; }

        // 3. PERSONNE DE CONTACT PRINCIPALE

        [DisplayName("Nom et prénom du contact : ")]
        public string? Contact_Nom { get; set; }

        [DisplayName("Téléphone fixe du contact : ")]
        public string? Contact_Telephone { get; set; }

        [DisplayName("Téléphone mobile: ")]
        public string? Contact_Gsm { get; set; }

        [DisplayName("Email du contact : ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique du contact n'est pas valide.")]
        public string? Contact_Email { get; set; }

        [DisplayName("Localité du contact : ")]
        public int? Contact_Id_localite { get; set; }

        // 4. PREMIÈRE ADRESSE D'EXPLOITATION (Site)

        [DisplayName("Nom du site d'exploitation : ")]
        public string? Site_Nom_site { get; set; }

        [DisplayName("Rue du site : ")]
        public string? Site_Rue { get; set; }

        [DisplayName("Numéro du site : ")]
        public string? Site_Numero { get; set; }

        [DisplayName("Localité du site : ")]
        public int? Site_Id_localite { get; set; }

        // LISTES DÉROULANTES (SelectLists pour Razor Views)

        public IEnumerable<SelectListItem>? LocaliteList { get; set; }
        public IEnumerable<SelectListItem>? UserList { get; set; }
    }
}
