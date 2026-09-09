using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcoLogistics.ViewModels.ClientBlock
{
    public class SiegeSocialeViewModel
    {
        [ScaffoldColumn(false)]
        public int Id_siege {  get; set; }
        public Guid? Id_client { get; set; }
        public int? Id_localite { get; set; }
        [DisplayName("Raison sociale: ")]
        public string? Raison_sociale { get; set; }
        [DisplayName("Adresse: ")]
        public string? Adresse {  get; set; }
        [DisplayName("Site internet: ")]
        public string? Site_internet { get; set; }
        [DisplayName("Secteur d'activité: ")]
        public string? Secteur_activite { get; set; }
        [DisplayName("Localité : ")]
        public string? Localite_Info { get; set; }
        public string? Siege_Code_postal { get;  set; }
        public string? Siege_Nom_commune { get;  set; }
        public string? Siege_Pays { get; set; }
        public SelectList? LocaliteList { get; set; }
    }
}
