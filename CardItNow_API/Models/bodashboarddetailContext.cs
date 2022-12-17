using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class bodashboarddetailContext : DbContext
    {
        public bodashboarddetailContext(DbContextOptions<bodashboarddetailContext> options) : base(options)
        {

        }

        public DbSet<bodashboarddetail> bodashboarddetails { get; set; }

    }
}

