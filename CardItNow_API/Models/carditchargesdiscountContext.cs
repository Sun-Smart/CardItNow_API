using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class carditchargesdiscountContext : DbContext
    {
        public carditchargesdiscountContext(DbContextOptions<carditchargesdiscountContext> options) : base(options)
        {

        }

        public DbSet<carditchargesdiscount> carditchargesdiscounts { get; set; }

    }
}

