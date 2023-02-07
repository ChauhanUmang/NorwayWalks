using AutoMapper;

namespace NorwayWalks.API.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.Domain.Walk, Models.DTO.WalksDTO>()
                .ReverseMap();

            CreateMap<Models.Domain.WalkDifficulty, Models.DTO.WalkDifficultyDTO>()
                .ReverseMap();
        }
    }
}
