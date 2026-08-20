using System.ComponentModel;


namespace EcoLogistics.ViewModels.Geo
    {
        public class GeoDetailsViewModel
        {
            // --- Localité ---
            public int Id_localite { get; set; }

            [DisplayName("Localité : ")]
            public string? Nom_localite { get; set; }

            [DisplayName("Code postal : ")]
            public string? Code_postal { get; set; }

            [DisplayName("Province : ")]
            public string? Province { get; set; }

            // --- Pays ---
            public int Id_pays { get; set; }

            [DisplayName("Nom du pays : ")]
            public string? Nom_pays { get; set; }

            [DisplayName("Code ISO : ")]
            public string? Code_ISO { get; set; }

            // --- Commune BXL ---
            public int? Id_commune { get; set; }

            [DisplayName("Sous-commune : ")]
            public bool Sous_commune { get; set; }

            [DisplayName("Commune principale : ")]
            public string? Commune_principale { get; set; }

            [DisplayName("Nom en français : ")]
            public string? Nom_fr { get; set; }

            [DisplayName("Nom en néerlandais : ")]
            public string? Nom_nl { get; set; }

            [DisplayName("Type : ")]
            public string? Type { get; set; }
        }
    }


