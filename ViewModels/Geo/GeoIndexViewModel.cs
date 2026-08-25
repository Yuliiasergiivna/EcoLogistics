using EcoLogistics.Models.Geo;

namespace EcoLogistics.ViewModels.Geo
{
    public class GeoIndexViewModel
    {
        public IEnumerable<Localite> Localites { get; set; } = new List<Localite>();
        public IEnumerable<CommuneBXL> Communes { get; set; } = new List<CommuneBXL>();
        public IEnumerable<Pays> PaysList { get; set; } = new List<Pays>();
    }
}
