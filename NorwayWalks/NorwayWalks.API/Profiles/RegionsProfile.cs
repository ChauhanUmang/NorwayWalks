using AutoMapper;

namespace NorwayWalks.API.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ReverseMap();

            //CreateMap<src, dest>();
            //Creates a mapping from source to destination

            //in case the name of the property is different in the source and destination model
            // e.g. our source has RegionID instead of ID, so we would map it like below to the Id field in the destination model
            //CreateMap<Models.Domain.Region, Models.DTO.Region>()
            //    .ForMember(dest => dest.Id, options => options.MapFrom(src => src.RegionId));
        }
    }
}
