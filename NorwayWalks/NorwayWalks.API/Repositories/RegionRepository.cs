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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await _db.AddAsync(region);
            await _db.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region is null)
            {
                return null;
            }

            //Delete the region
            _db.Regions.Remove(region);
            await _db.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingregion = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(existingregion is null)
            {
                return null;
            }

            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.Area = region.Area;
            existingregion.Lat = region.Lat;
            existingregion.Long = region.Long;
            existingregion.Population = region.Population;

            //_db.Regions.Update(region);
            await _db.SaveChangesAsync();
            return existingregion;
        }
    }
}
