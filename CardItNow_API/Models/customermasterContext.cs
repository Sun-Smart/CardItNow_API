using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customermasterContext : DbContext
    {
        public customermasterContext(DbContextOptions<customermasterContext> options) : base(options)
        {

        }

        public DbSet<customermaster> customermasters { get; set; }

    }
}

