using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class initiatorrecipientprivateContext : DbContext
    {
        public initiatorrecipientprivateContext(DbContextOptions<initiatorrecipientprivateContext> options) : base(options)
        {

        }

        public DbSet<initiatorrecipientprivate> initiatorrecipientprivates { get; set; }

    }
}

