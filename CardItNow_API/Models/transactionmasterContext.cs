using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class transactionmasterContext : DbContext
    {
        public transactionmasterContext(DbContextOptions<transactionmasterContext> options) : base(options)
        {

        }

        public DbSet<transactionmaster> transactionmasters { get; set; }

    }
}

