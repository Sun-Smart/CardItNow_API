using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class avatarmasterContext : DbContext
    {
        public avatarmasterContext(DbContextOptions<avatarmasterContext> options) : base(options)
        {

        }

        public DbSet<avatarmaster> avatarmasters { get; set; }

    }
}

