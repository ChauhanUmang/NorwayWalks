using Microsoft.EntityFrameworkCore;
using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Data
{
    public class NorwayWalksDbContext : DbContext
    {
        public NorwayWalksDbContext(DbContextOptions<NorwayWalksDbContext> options) : base(options)
        {

        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulties { get; set; }
    }
}
