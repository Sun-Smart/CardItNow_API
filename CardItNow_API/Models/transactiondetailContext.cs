using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class transactiondetailContext : DbContext
    {
        public transactiondetailContext(DbContextOptions<transactiondetailContext> options) : base(options)
        {

        }

        public DbSet<transactiondetail> transactiondetails { get; set; }

    }
}

