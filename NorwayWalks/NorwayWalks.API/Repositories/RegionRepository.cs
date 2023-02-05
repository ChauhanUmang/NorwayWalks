using Microsoft.EntityFrameworkCore;
using NorwayWalks.API.Data;
using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NorwayWalksDbContext _db;

        public RegionRepository(NorwayWalksDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }
    }
}
