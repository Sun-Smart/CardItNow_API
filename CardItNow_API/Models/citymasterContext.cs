using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class citymasterContext : DbContext
    {
        public citymasterContext(DbContextOptions<citymasterContext> options) : base(options)
        {

        }

        public DbSet<citymaster> citymasters { get; set; }

    }
}

