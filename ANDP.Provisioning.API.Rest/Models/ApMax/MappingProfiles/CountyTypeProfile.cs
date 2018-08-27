using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class CountyTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<CountyType, Common.IPTVServiceV3.CountyType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.CountyCode, opt => opt.MapFrom(src => src.CountyCode))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.StateCode))
                ;

            CreateMap<CountyType, Common.IPTVServiceV7.CountyType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.CountyCode, opt => opt.MapFrom(src => src.CountyCode))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.StateCode))
                ;

            CreateMap<Common.IPTVServiceV3.CountyType, CountyType>()
                .ForMember(dest => dest.CountyCode, opt => opt.MapFrom(src => src.CountyCode))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.StateCode))
                ;

            CreateMap<Common.IPTVServiceV7.CountyType, CountyType>()
                .ForMember(dest => dest.CountyCode, opt => opt.MapFrom(src => src.CountyCode))
                .ForMember(dest => dest.StateCode, opt => opt.MapFrom(src => src.StateCode))
                ;
        }
    }
}