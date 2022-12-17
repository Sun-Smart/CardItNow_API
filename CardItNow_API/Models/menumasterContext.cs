using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class menumasterContext : DbContext
    {
        public menumasterContext(DbContextOptions<menumasterContext> options) : base(options)
        {

        }

        public DbSet<menumaster> menumasters { get; set; }

    }
}

