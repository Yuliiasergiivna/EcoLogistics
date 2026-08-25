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
    }
}
