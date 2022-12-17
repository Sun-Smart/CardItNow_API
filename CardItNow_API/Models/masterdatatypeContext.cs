using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class masterdatatypeContext : DbContext
    {
        public masterdatatypeContext(DbContextOptions<masterdatatypeContext> options) : base(options)
        {

        }

        public DbSet<masterdatatype> masterdatatypes { get; set; }

    }
}

