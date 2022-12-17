using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class boreportcolumnContext : DbContext
    {
        public boreportcolumnContext(DbContextOptions<boreportcolumnContext> options) : base(options)
        {

        }

        public DbSet<boreportcolumn> boreportcolumns { get; set; }

    }
}

