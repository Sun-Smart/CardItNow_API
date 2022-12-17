using Microsoft.EntityFrameworkCore;
using nTireBO.Models;
namespace nTireBO.Models
{
    public class ReportViewerContext : DbContext
    {
        public ReportViewerContext(DbContextOptions<ReportViewerContext> options) : base(options)
        {

        }
        public DbSet<boreport> boreports { get; set; }
        public DbSet<bodashboard> bodashboards { get; set; }
        

    }
}