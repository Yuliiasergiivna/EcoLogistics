using EcoLogistics.Models.UserBlock;
using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Donnees_perso> Donnees_persos { get; set; }
        public DbSet<Conducteur> Conducteurs { get; set; }
    }
}
