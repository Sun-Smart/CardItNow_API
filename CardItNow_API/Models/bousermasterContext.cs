using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class bousermasterContext : DbContext
    {
        public bousermasterContext(DbContextOptions<bousermasterContext> options) : base(options)
        {

        }

        public DbSet<bousermastercontext> bousermasters { get; set; }

    }
}

