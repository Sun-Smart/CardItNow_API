using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class initiatorrecipientmappingContext : DbContext
    {
        public initiatorrecipientmappingContext(DbContextOptions<initiatorrecipientmappingContext> options) : base(options)
        {

        }

        public DbSet<initiatorrecipientmapping> initiatorrecipientmappings { get; set; }

    }
}

