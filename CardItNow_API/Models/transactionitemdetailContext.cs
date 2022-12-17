using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class transactionitemdetailContext : DbContext
    {
        public transactionitemdetailContext(DbContextOptions<transactionitemdetailContext> options) : base(options)
        {

        }

        public DbSet<transactionitemdetail> transactionitemdetails { get; set; }

    }
}

