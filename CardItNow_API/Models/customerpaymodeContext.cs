using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customerpaymodeContext : DbContext
    {
        public customerpaymodeContext(DbContextOptions<customerpaymodeContext> options) : base(options)
        {

        }

        public DbSet<customerpaymode> customerpaymodes { get; set; }

    }
}

