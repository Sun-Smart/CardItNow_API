using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class bodashboardContext : DbContext
    {
        public bodashboardContext(DbContextOptions<bodashboardContext> options) : base(options)
        {

        }

        public DbSet<bodashboard> bodashboards { get; set; }

    }
}

