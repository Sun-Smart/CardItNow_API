using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using nTireBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nTireBO.Models
{
    public class boconfigvalueContext : DbContext
    {
        
        public boconfigvalueContext(DbContextOptions<boconfigvalueContext> options) : base(options)
        {

        }
        
        public DbSet<boconfigvalue> boconfigvalues { get; set; }
        /*
        private readonly HttpContext context;
        private readonly Multitenancy multitenancy;

        public boconfigvalueContext(IHttpContextAccessor httpContextAccessor, Multitenancy mt)
        {
            context = httpContextAccessor.HttpContext;
            multitenancy = mt;

            Database.EnsureCreated();
        }

        public boconfigvalueContext(DbContextOptions<boconfigvalueContext> options, IHttpContextAccessor httpContextAccessor, Multitenancy mt) : base(options)
        {
            context = httpContextAccessor.HttpContext;
            multitenancy = mt;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenant = multitenancy.Tenants
                    .Where(t => t.Hostnames.Contains(context.Request.Host.Value)).FirstOrDefault();

            if (tenant != null)
            {
                optionsBuilder.UseNpgsql(tenant.ConnectionString);
            }
        }
        */
    }
}

