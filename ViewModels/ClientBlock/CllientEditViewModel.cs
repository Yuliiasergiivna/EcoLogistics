using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class CllientEditViewModel
    {
        // 1. IDENTIFIANTS (Clés primaires)
        [Required]
        public Guid Id_client { get; set; }

        public int? Id_siege { get; set; }

        public int? Id_p_contact { get; set; }

        public int? Id_adresse_exp { get; set; }

        // 2. INFORMATIONS DU CLIENT (Entreprise)

        [DisplayName("Nom d'entreprise: ")]
        [Required(ErrorMessage = "Le nom d'entreprise est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le nom d'entreprise ne peut pas dépasser 64 caractères.")]
        public string Nom_entreprise { get; set; }

        [DisplayName("Numéro d'entreprise (BCE): ")]
        public int? Numero_entreprise { get; set; }

        [DisplayName("N° BE d'entreprise: ")]
        [MaxLength(32, ErrorMessage = "Le N° BE ne peut pas dépasser 32 caractères.")]
        public string? BE_entreprise { get; set; }

        [DisplayName("Adresse principale: ")]
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        public string? Adresse { get; set; }

        [DisplayName("Téléphone: ")]
        [MaxLength(32, ErrorMessage = "Le numéro de téléphone ne peut pas dépasser 32 caractères.")]
        public string? Telephone { get; set; }

        [DisplayName("Adresse électronique: ")]
        [Required(ErrorMessage = "L'adresse électronique est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas d'un format valide.")]
        public string Email { get; set; }

        [DisplayName("Numéro d'enregistrement BE: ")]
        [MaxLength(32)]
        public string? Enregistrement_BE { get; set; }

        [DisplayName("Numéro d'agrément BE: ")]
        [MaxLength(32)]
        public string? Agrement_BE { get; set; }

        [DisplayName("Type d'enregistrement: ")]
        [MaxLength(32)]
        public string? Type_enregistrement { get; set; }

        [DisplayName("Remarques: ")]
        [MaxLength(255, ErrorMessage = "La remarque ne peut pas dépasser 255 caractères.")]
        public string? Remarques { get; set; }

        [DisplayName("Présentation: ")]
        [MaxLength(255, ErrorMessage = "La présentation ne peut pas dépasser 255 caractères.")]
        public string? Presentation { get; set; }

        [DisplayName("Statut supprimé: ")]
        public bool Is_deleted { get; set; }

        [DisplayName("Gestionnaire / Utilisateur responsable: ")]
        public Guid? Id_user { get; set; }

        [DisplayName("Localité principale : ")]
        public int? Id_localite { get; set; }

        // 3. SIÈGE SOCIAL (Juridique)

        [DisplayName("Raison sociale: ")]
        [MaxLength(100, ErrorMessage = "La raison sociale ne peut pas dépasser 100 caractères.")]
        public string? Siege_Raison_sociale { get; set; }

        [DisplayName("Adresse du siège social: ")]
        [MaxLength(100, ErrorMessage = "L'adresse ne peut pas dépasser 100 caractères.")]
        public string? Siege_Adresse { get; set; }

        [DisplayName("Site internet: ")]
        [MaxLength(32)]
        public string? Siege_Site_internet { get; set; }

        [DisplayName("Secteur d'activité: ")]
        [MaxLength(32)]
        public string? Siege_Secteur_activite { get; set; }

        [DisplayName("Localité du siège social: ")]
        public int? Siege_Id_localite { get; set; }

        // 4. PERSONNE DE CONTACT 

        [DisplayName("Nom et prénom du contact: ")]
        [MaxLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères.")]
        public string? Contact_Nom { get; set; }

        [DisplayName("Téléphone fixe du contact: ")]
        [MaxLength(32, ErrorMessage = "Le téléphone ne peut pas dépasser 32 caractères.")]
        public string? Contact_Telephone { get; set; }

        [DisplayName("Téléphone mobile: ")]
        [MaxLength(32, ErrorMessage = "Le numéro de GSM ne peut pas dépasser 32 caractères.")]
        public string? Contact_Gsm { get; set; }

        [DisplayName("Email: ")]
        [EmailAddress(ErrorMessage = "L'adresse électronique du contact n'est pas valide.")]
        [MaxLength(64)]
        public string? Contact_Email { get; set; }

        [DisplayName("Adresse du contact: ")]
        [MaxLength(100)]
        public string? Contact_Adresse { get; set; }

        [DisplayName("Localité du contact: ")]
        public int? Contact_Id_localite { get; set; }

        // 5. ADRESSE D'EXPLOITATION 

        [DisplayName("Nom du site d'exploitation: ")]
        [MaxLength(64, ErrorMessage = "Le nom du site ne peut pas dépasser 64 caractères.")]
        public string? Site_Nom { get; set; }

        [DisplayName("Rue du site: ")]
        [MaxLength(64, ErrorMessage = "La rue ne peut pas dépasser 64 caractères.")]
        public string? Site_Rue { get; set; }

        [DisplayName("Numéro du site: ")]
        [MaxLength(16, ErrorMessage = "Le numéro ne peut pas dépasser 16 caractères.")]
        public string? Site_Numero { get; set; }

        [DisplayName("Localité du site: ")]
        public int? Site_Id_localite { get; set; }

        // LISTES DÉROULANTES (SelectLists)

        public IEnumerable<SelectListItem>? LocaliteList { get; set; }
        public IEnumerable<SelectListItem>? UserList { get; set; }
    }
}
