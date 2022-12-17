using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class boreportdetailContext : DbContext
    {
        public boreportdetailContext(DbContextOptions<boreportdetailContext> options) : base(options)
        {

        }

        public DbSet<boreportdetail> boreportdetails { get; set; }

    }
}

