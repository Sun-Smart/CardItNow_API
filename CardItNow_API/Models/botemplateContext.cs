using Microsoft.EntityFrameworkCore;

namespace carditnow.Models
{
    public class botemplateContext : DbContext
    {
        public botemplateContext(DbContextOptions<botemplateContext> options) : base(options)
        {

        }

        public DbSet<botemplate> botemplates { get; set; }

    }
}

