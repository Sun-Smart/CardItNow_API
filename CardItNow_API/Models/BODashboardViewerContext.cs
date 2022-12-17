using Microsoft.EntityFrameworkCore;
using carditnow.Models;
namespace nTireBO.Models
{
    public class BODashboardViewerContext : DbContext
    {
        public BODashboardViewerContext(DbContextOptions<BODashboardViewerContext> options) : base(options)
        {

        }
        public DbSet<bodashboard> bodashboards { get; set; }

        public DbSet<bodashboarddetail> bodashboarddetails { get; set; }
        public DbSet<boconfigvalue> boconfigvalues { get; set; }
        public DbSet<masterdatatype> masterdatatypes { get; set; }

        




    }
}