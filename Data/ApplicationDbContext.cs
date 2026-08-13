using Microsoft.EntityFrameworkCore;

namespace EcoLogistics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }
    }
}
