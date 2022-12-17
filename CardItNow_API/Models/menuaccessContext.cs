using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class menuaccessContext : DbContext
    {
        public menuaccessContext(DbContextOptions<menuaccessContext> options) : base(options)
        {

        }

        public DbSet<menuaccess> menuaccesses { get; set; }

    }
}

