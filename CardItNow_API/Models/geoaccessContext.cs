using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class geoaccessContext : DbContext
    {
        public geoaccessContext(DbContextOptions<geoaccessContext> options) : base(options)
        {

        }

        public DbSet<geoaccess> geoaccesses { get; set; }

    }
}

