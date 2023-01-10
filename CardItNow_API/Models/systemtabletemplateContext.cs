using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class systemtabletemplateContext : DbContext
    {
        public systemtabletemplateContext(DbContextOptions<systemtabletemplateContext> options) : base(options)
        {

        }

        public DbSet<systemtabletemplate> systemtabletemplates { get; set; }

    }
}

