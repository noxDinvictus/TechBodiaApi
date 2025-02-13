using Microsoft.EntityFrameworkCore;
using TechBodiaApi.Data.Models;

namespace TechBodiaApi.Data
{
    public class TechBodiaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public TechBodiaContext(DbContextOptions<TechBodiaContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors(true);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
