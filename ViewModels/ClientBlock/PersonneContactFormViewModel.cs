using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class PersonneContactFormViewModel
    {
        public int Id_p_contact { get; set; }

        [DisplayName("Client / Entreprise :")]
        [Required(ErrorMessage = "Le client est obligatoire.")]
        public Guid Id_client { get; set; }

        [DisplayName("Nom :")]
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le nom ne peut pas dépasser 64 caractères.")]
        public string Nom { get; set; } = string.Empty;

        [DisplayName("Prénom :")]
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [MaxLength(64, ErrorMessage = "Le prénom ne peut pas dépasser 64 caractères.")]
        public string Prenom { get; set; } = string.Empty;

        [DisplayName("Téléphone fixe :")]
        [MaxLength(32, ErrorMessage = "Le numéro ne peut pas dépasser 32 caractères.")]
        public string? Telephone { get; set; }

        [DisplayName("GSM :")]
        [MaxLength(32, ErrorMessage = "Le numéro GSM ne peut pas dépasser 32 caractères.")]
        public string? Gsm { get; set; }

        [DisplayName("Email :")]
        [EmailAddress(ErrorMessage = "L'adresse électronique n'est pas valide.")]
        [MaxLength(64, ErrorMessage = "L'email ne peut pas dépasser 64 caractères .")]
        public string? Email { get; set; }

        [DisplayName("Localité :")]
        public int? Id_localite { get; set; }

       
        public SelectList? ClientList { get; set; }
        public SelectList? LocaliteList { get; set; }
    }
}
