using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class recipientdiscountContext : DbContext
    {
        public recipientdiscountContext(DbContextOptions<recipientdiscountContext> options) : base(options)
        {

        }

        public DbSet<recipientdiscount> recipientdiscounts { get; set; }

    }
}

