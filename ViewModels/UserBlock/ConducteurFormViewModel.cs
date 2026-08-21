using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.UserBlock
{
    public class ConducteurFormViewModel
    {
        public int Id_conducteur { get; set; }

        [Required(ErrorMessage = "Le numéro de plaque est obligatoire.")]
        [MaxLength(16, ErrorMessage = "Le numéro de plaque ne peut pas dépasser 16 caractères.")]
        [Display(Name = "N° de Plaque")]
        public string N_plaque { get; set; } = string.Empty;

        [Required(ErrorMessage = "La quantité de palettes est obligatoire.")]
        [Range(0, 100, ErrorMessage = "La quantité de palettes doit être comprise entre 0 et 100.")]
        [Display(Name = "Quantité de palettes")]
        public int Quantite_Npalette { get; set; }

        [Display(Name = "Conducteur (Employé)")]
        public Guid? Id_perso { get; set; }
    }
}
