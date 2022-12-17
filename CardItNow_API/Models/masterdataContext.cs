using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class masterdataContext : DbContext
    {
        public masterdataContext(DbContextOptions<masterdataContext> options) : base(options)
        {

        }

        public DbSet<masterdata> masterdatas { get; set; }

    }
}

