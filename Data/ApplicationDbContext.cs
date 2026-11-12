using EcoLogistics.Models.Catalogues;
using EcoLogistics.Models.ClientBlock;
using EcoLogistics.Models.Geo;
using EcoLogistics.Models.UserBlock;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }
        // --UserBlock
        public DbSet<User> Users { get; set; }
        public DbSet<Donnees_perso> Donnees_persos { get; set; }
        public DbSet<Conducteur> Conducteurs { get; set; }
        // --Geo
        public DbSet<Localite> Localites { get; set; }
        public DbSet<Pays> Pays { get; set; }
        public DbSet<CommuneBXL> CommuneBXLs { get; set; }

        // --ClientBlock
        public DbSet<Client> Clients { get; set; }
        public DbSet<SiegeSociale> SiegeSociales { get; set; }
        public DbSet<PersonneContact> PersonneContacts { get; set; }
        public DbSet<AdresseExploitation> AdressesExploitation { get; set; }

        // --Catalogues
        public DbSet<CodeEURAL> CodeEURALs { get; set; }
        public DbSet<CodeDR> CodeDRs { get; set; }
        public DbSet<Marques> Marques { get; set; }
        public DbSet<Divers> Divers { get; set; }
        public DbSet<Poids> Poids { get; set; }

    }
}
