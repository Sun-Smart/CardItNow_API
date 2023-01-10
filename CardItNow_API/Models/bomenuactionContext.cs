using Microsoft.EntityFrameworkCore;

namespace carditnow.Models
{
    public class bomenuactionContext : DbContext
    {
        public bomenuactionContext(DbContextOptions<bomenuactionContext> options) : base(options)
        {

        }

        public DbSet<bomenuaction> bomenuactions { get; set; }

    }
}

