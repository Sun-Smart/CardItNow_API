using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class bodynamicformdetailContext : DbContext
    {
        public bodynamicformdetailContext(DbContextOptions<bodynamicformdetailContext> options) : base(options)
        {

        }

        public DbSet<bodynamicformdetail> bodynamicformdetails { get; set; }

    }
}

