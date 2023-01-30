using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardItNow.Models
{
    public class payerpayeeprivateContext  : DbContext
    {
        public payerpayeeprivateContext(DbContextOptions<payerpayeeprivateContext> options) : base(options)
        {

        }

        public DbSet<payerpayeeprivate> payerpayeeprivate { get; set; }
    }
}
