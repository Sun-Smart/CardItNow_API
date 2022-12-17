using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customerdetailContext : DbContext
    {
        public customerdetailContext(DbContextOptions<customerdetailContext> options) : base(options)
        {

        }

        public DbSet<customerdetail> customerdetails { get; set; }

    }
}

