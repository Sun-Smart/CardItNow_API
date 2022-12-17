using Microsoft.EntityFrameworkCore;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carditnow.Models
{
    public class customersecurityquestionContext : DbContext
    {
        public customersecurityquestionContext(DbContextOptions<customersecurityquestionContext> options) : base(options)
        {

        }

        public DbSet<customersecurityquestion> customersecurityquestions { get; set; }

    }
}

