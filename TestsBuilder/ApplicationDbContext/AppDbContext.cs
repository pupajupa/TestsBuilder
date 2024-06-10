using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestsBuilder.Models;

namespace TestsBuilder.ApplicationDbContext
{
    public class AppDbContext : DbContext
    { 
        public DbSet<Test> Tests { get; set; }
        public DbSet<Example> Exps { get; set; }
        public DbSet<ExampleVariant> ExampleVariants { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Student> Students {  get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
