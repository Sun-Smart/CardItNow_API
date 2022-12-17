using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customersecurityquestionshistoryContext : DbContext
    {
        public customersecurityquestionshistoryContext(DbContextOptions<customersecurityquestionshistoryContext> options) : base(options)
        {

        }

        public DbSet<customersecurityquestionshistory> customersecurityquestionshistories { get; set; }

    }
}

