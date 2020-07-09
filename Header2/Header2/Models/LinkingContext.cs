using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Header2.Models
{
    public class LinkingContext : DbContext
    {
        public LinkingContext(DbContextOptions<LinkingContext> options) : base(options)
        {

        }

        public DbSet<Documents> Documents { get; set; }
        public DbSet<Procedures> Procedures { get; set; }
        public DbSet<Links> Links { get; set; }
    }
}
