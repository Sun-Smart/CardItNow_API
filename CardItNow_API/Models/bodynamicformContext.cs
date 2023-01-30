using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class bodynamicformContext : DbContext
    {
        public bodynamicformContext(DbContextOptions<bodynamicformContext> options) : base(options)
        {

        }

        public DbSet<bodynamicform> bodynamicforms { get; set; }

    }
}

