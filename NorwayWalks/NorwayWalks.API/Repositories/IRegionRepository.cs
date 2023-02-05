using NorwayWalks.API.Models.Domain;

namespace NorwayWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
