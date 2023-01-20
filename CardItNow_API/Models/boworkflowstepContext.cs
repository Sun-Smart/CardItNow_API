using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class boworkflowstepContext : DbContext
    {
        public boworkflowstepContext(DbContextOptions<boworkflowstepContext> options) : base(options)
        {

        }

        public DbSet<boworkflowstep> boworkflowsteps { get; set; }

    }
}

