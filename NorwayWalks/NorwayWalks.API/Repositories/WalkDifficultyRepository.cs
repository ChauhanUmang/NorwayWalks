using Microsoft.EntityFrameworkCore;
using NorwayWalks.API.Data;
using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NorwayWalksDbContext _db;

        public WalkDifficultyRepository(NorwayWalksDbContext db)
        {
            _db = db;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            //Assign new GUID
            walkDifficulty.Id = Guid.NewGuid();

            await _db.WalkDifficulties.AddAsync(walkDifficulty);
            await _db.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty = await _db.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if(walkDifficulty is null)
            {
                return null;
            }

            _db.WalkDifficulties.Remove(walkDifficulty);
            await _db.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _db.WalkDifficulties.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _db.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _db.WalkDifficulties.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalkDifficulty != null)
            {
                existingWalkDifficulty.Code = walkDifficulty.Code;
                await _db.SaveChangesAsync();
                return existingWalkDifficulty;
            }

            return null;
        }
    }
}
