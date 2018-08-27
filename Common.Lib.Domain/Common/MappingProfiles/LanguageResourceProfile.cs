using Common.Lib.Data.Repositories.LanguageResource;
using Common.Lib.Mapping;

namespace Common.Lib.Domain.Common.MappingProfiles
{
    public class LanguageResourceProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<LanguageResource, Models.LanguageResource>()
                .ForMember(dst => dst.Key, opt => opt.MapFrom(src => src.ResourceKey))
                .ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.ResourceValue))
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
