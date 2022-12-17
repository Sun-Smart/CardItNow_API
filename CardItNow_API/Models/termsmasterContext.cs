using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class termsmasterContext : DbContext
    {
        public termsmasterContext(DbContextOptions<termsmasterContext> options) : base(options)
        {

        }

        public DbSet<termsmaster> termsmasters { get; set; }

    }
}

