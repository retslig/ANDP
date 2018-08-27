using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class CallingNameTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Common.CallingNameV3.CallingNameType, CallingNameType>()
                .ForMember(dest => dest.BgId, opt => opt.MapFrom(src => src.BgId))
                .ForMember(dest => dest.CallingNumber, opt => opt.MapFrom(src => src.CallingNumber))
                .ForMember(dest => dest.Cname, opt => opt.MapFrom(src => src.CName))
                .ForMember(dest => dest.Presentation, opt => opt.MapFrom(src => src.Presentation))
                .ForMember(dest => dest.UserOverride, opt => opt.Ignore())
                ;

            CreateMap<Common.CallingNameV4.CallingNameType, CallingNameType>()
                .ForMember(dest => dest.BgId, opt => opt.MapFrom(src => src.BgId))
                .ForMember(dest => dest.CallingNumber, opt => opt.MapFrom(src => src.CallingNumber))
                .ForMember(dest => dest.Cname, opt => opt.MapFrom(src => src.CName))
                .ForMember(dest => dest.Presentation, opt => opt.MapFrom(src => src.Presentation))
                .ForMember(dest => dest.UserOverride, opt => opt.MapFrom(src => src.UserOverride))
                ;

            CreateMap<CallingNameType, Common.CallingNameV3.CallingNameType>()
                .ForMember(dest => dest.BgId, opt => opt.MapFrom(src => src.BgId))
                .ForMember(dest => dest.CallingNumber, opt => opt.MapFrom(src => src.CallingNumber))
                .ForMember(dest => dest.CName, opt => opt.MapFrom(src => src.Cname))
                .ForMember(dest => dest.Presentation, opt => opt.MapFrom(src => src.Presentation))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<CallingNameType, Common.CallingNameV4.CallingNameType>()
                .ForMember(dest => dest.BgId, opt => opt.MapFrom(src => src.BgId))
                .ForMember(dest => dest.CallingNumber, opt => opt.MapFrom(src => src.CallingNumber))
                .ForMember(dest => dest.CName, opt => opt.MapFrom(src => src.Cname))
                .ForMember(dest => dest.Presentation, opt => opt.MapFrom(src => src.Presentation))
                .ForMember(dest => dest.UserOverride, opt => opt.MapFrom(src => src.UserOverride))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;
        }
    }
}