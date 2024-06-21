using Microsoft.EntityFrameworkCore;
using PathPro.Models.Domain;

namespace PathPro.Data
{
    public class PathProDbContext : DbContext
    {
        public PathProDbContext(DbContextOptions<PathProDbContext> options) : base(options)
        {
        }
        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}

