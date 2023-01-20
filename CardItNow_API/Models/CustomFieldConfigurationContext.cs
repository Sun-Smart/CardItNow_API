using carditnow.Models;
using Microsoft.EntityFrameworkCore;
using nTireBO.Models;

namespace SunSmartnTireProducts.Models
{
    public class customfieldconfigurationContext : DbContext
    {
        public customfieldconfigurationContext(DbContextOptions<customfieldconfigurationContext> options) : base(options)
        {
        }

        public DbSet<systemtable> systemtables { get; set; }
        public DbSet<bodynamicform> bodynamicforms { get; set; }
        public DbSet<bodynamicformdetail> bodynamicformdetails { get; set; }
        public DbSet<customfieldconfiguration> customfieldconfigurations { get; set; }
        public DbSet<boconfigvalue> boconfigvalues { get; set; }
        public DbSet<columnvisibility> columnvisibilities { get; set; }
    }
}