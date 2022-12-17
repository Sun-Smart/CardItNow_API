using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customertermsacceptanceContext : DbContext
    {
        public customertermsacceptanceContext(DbContextOptions<customertermsacceptanceContext> options) : base(options)
        {

        }

        public DbSet<customertermsacceptance> customertermsacceptances { get; set; }

    }
}

