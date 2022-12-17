using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class boreportothertableContext : DbContext
    {
        public boreportothertableContext(DbContextOptions<boreportothertableContext> options) : base(options)
        {

        }

        public DbSet<boreportothertable> boreportothertables { get; set; }

    }
}

