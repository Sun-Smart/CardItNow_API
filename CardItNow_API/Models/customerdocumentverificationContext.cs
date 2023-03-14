using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customerdocumentverificationContext : DbContext
    {
        public customerdocumentverificationContext(DbContextOptions<customerdocumentverificationContext> options) : base(options)
        {

        }

        public DbSet<customerdocumentverification> customer_document_verification { get; set; }

    }
}

