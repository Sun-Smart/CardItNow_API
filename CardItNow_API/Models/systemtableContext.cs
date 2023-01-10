using Microsoft.EntityFrameworkCore;

namespace carditnow.Models

{
    public class systemtableContext : DbContext
    {
        public systemtableContext(DbContextOptions<systemtableContext> options) : base(options)
        {

        }

        public DbSet<systemtable> systemtables { get; set; }

    }
}

