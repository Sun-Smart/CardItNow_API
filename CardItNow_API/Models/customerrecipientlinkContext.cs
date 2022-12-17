using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customerrecipientlinkContext : DbContext
    {
        public customerrecipientlinkContext(DbContextOptions<customerrecipientlinkContext> options) : base(options)
        {

        }

        public DbSet<customerrecipientlink> customerrecipientlinks { get; set; }

    }
}

