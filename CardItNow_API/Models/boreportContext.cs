using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class boreportContext : DbContext
    {
        public boreportContext(DbContextOptions<boreportContext> options) : base(options)
        {

        }

        public DbSet<boreport> boreports { get; set; }

    }
}

