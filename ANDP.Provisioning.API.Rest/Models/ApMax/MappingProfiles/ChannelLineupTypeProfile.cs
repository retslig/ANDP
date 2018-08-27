using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ChannelLineupTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ChannelLineupType, Common.IPTVServiceV3.ChannelLineupType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.ChannelPackages, opt => opt.MapFrom(src => src.ChannelPackageTypes))
                .ForMember(dest => dest.Counties, opt => opt.MapFrom(src => src.CountyTypes))
                .ForMember(dest => dest.ServiceAreaID, opt => opt.MapFrom(src => src.ServiceAreaId))
                .ForMember(dest => dest.ServiceAreaIndex, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaName, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaTimeZone, opt => opt.MapFrom(src => src.ServiceAreaTimeZone))
                ;

            CreateMap<ChannelLineupType, Common.IPTVServiceV7.ChannelLineupType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.ChannelPackages, opt => opt.MapFrom(src => src.ChannelPackageTypes))
                .ForMember(dest => dest.Counties, opt => opt.MapFrom(src => src.CountyTypes))
                .ForMember(dest => dest.ServiceAreaID, opt => opt.MapFrom(src => src.ServiceAreaId))
                .ForMember(dest => dest.ServiceAreaIndex, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaName, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaTimeZone, opt => opt.MapFrom(src => src.ServiceAreaTimeZone))
                ;

            CreateMap<Common.IPTVServiceV3.ChannelLineupType, ChannelLineupType>()
                .ForMember(dest => dest.ChannelPackageTypes, opt => opt.MapFrom(src => src.ChannelPackages))
                .ForMember(dest => dest.CountyTypes, opt => opt.MapFrom(src => src.Counties))
                .ForMember(dest => dest.ServiceAreaId, opt => opt.MapFrom(src => src.ServiceAreaID))
                .ForMember(dest => dest.ServiceAreaIndex, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaName, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaTimeZone, opt => opt.MapFrom(src => src.ServiceAreaTimeZone))
                ;

            CreateMap<Common.IPTVServiceV7.ChannelLineupType, ChannelLineupType>()
                .ForMember(dest => dest.ChannelPackageTypes, opt => opt.MapFrom(src => src.ChannelPackages))
                .ForMember(dest => dest.CountyTypes, opt => opt.MapFrom(src => src.Counties))
                .ForMember(dest => dest.ServiceAreaId, opt => opt.MapFrom(src => src.ServiceAreaID))
                .ForMember(dest => dest.ServiceAreaIndex, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaName, opt => opt.MapFrom(src => src.ServiceAreaName))
                .ForMember(dest => dest.ServiceAreaTimeZone, opt => opt.MapFrom(src => src.ServiceAreaTimeZone))
                ;
        }
    }
}