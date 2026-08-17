using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoLogistics.Models.UserBlock
{
    public class Conducteur
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [MaxLength(16, ErrorMessage = "Le numéro de plaque ne peut pas dépasser 16 caractères.")]
        [Display(Name = "Numéro de plaque: ")]
        [Required(ErrorMessage = "Le numéro de plaque est obligatoire.")]
        public string N_plaque { get; set; }
        
        [Display(Name = "Quantité de palettes: ")]
        [Range(0, 100, ErrorMessage = "La quantité de palettes doit être comprise entre 0 et 100.")]
        public int Quantite_Npalette { get; set; }
        [ScaffoldColumn(false)]
        public Guid? Id_perso { get; set; }
        [ForeignKey("Id_perso")]
        public Donnees_perso? Donnees_perso { get; set; }
    }
}
