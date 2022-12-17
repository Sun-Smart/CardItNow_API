using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class usermasterContext : DbContext
    {
        public usermasterContext(DbContextOptions<usermasterContext> options) : base(options)
        {

        }

        public DbSet<usermaster> usermasters { get; set; }

    }
}

