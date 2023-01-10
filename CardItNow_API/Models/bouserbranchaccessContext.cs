using Microsoft.EntityFrameworkCore;

namespace SunSmartnTireProducts.Models
{
    public class bouserbranchaccessContext : DbContext
    {
        public bouserbranchaccessContext(DbContextOptions<bouserbranchaccessContext> options) : base(options)
        {

        }

        public DbSet<bouserbranchaccess> bouserbranchaccesses { get; set; }

    }
}

