using Microsoft.EntityFrameworkCore;
using NorwayWalks.API.Data;
using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NorwayWalksDbContext _db;

        public WalksRepository(NorwayWalksDbContext db)
        {
            _db = db;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new GUID ID
            walk.Id = Guid.NewGuid();

            await _db.Walks.AddAsync(walk);
            await _db.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = _db.Walks.FirstOrDefault(x => x.Id == id);
            if (walk is null)
            {
                return null;
            }

            //Delete the region
            _db.Walks.Remove(walk);
            await _db.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _db.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await _db.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalk = await _db.Walks.FirstOrDefaultAsync(x => x.Id == id);
            
            if(existingwalk != null)
            {
                existingwalk.Name = walk.Name;
                existingwalk.Length = walk.Length;
                existingwalk.RegionId = walk.RegionId;
                existingwalk.WalkDifficultyId = walk.WalkDifficultyId;

                await _db.SaveChangesAsync();
                return existingwalk;
            }
            
            return null;
        }
    }
}
