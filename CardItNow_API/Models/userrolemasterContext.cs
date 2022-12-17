using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class userrolemasterContext : DbContext
    {
        public userrolemasterContext(DbContextOptions<userrolemasterContext> options) : base(options)
        {

        }

        public DbSet<userrolemaster> userrolemasters { get; set; }

    }
}

