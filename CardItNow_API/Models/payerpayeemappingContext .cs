using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Models
{
    public class payerpayeemappingContext : DbContext
    {
        public payerpayeemappingContext(DbContextOptions<payerpayeeprivateContext> options) : base(options)
        {

        }

        public DbSet<payerpayeeprivate> payerpayeemapping { get; set; }
    }
}
