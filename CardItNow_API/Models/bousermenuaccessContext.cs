using Microsoft.EntityFrameworkCore;

namespace carditnow.Models
{
    public class bousermenuaccessContext : DbContext
    {
        public bousermenuaccessContext(DbContextOptions<bousermenuaccessContext> options) : base(options)
        {

        }

        public DbSet<bousermenuaccess> bousermenuaccesses { get; set; }

    }
}

