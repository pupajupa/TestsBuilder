using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsBuilder.Models;

namespace TestsBuilder.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }    
        public DbSet<Test> Tests { get; set; }
        public DbSet<Exp> Exps { get; set; }
        public DbSet<Variant> Variants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
