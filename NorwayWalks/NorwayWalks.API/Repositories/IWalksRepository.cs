using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetAsync(Guid id);
        Task<Walk> AddAsync(Walk walk);

        Task<Walk> DeleteAsync(Guid id);

        Task<Walk> UpdateAsync(Guid id, Walk walk);
    }
}
