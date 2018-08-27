using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ChannelPackageTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ChannelPackageType, Common.IPTVServiceV3.ChannelPackageType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.PackageID, opt => opt.MapFrom(src => src.PackageId))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                ;

            CreateMap<ChannelPackageType, Common.IPTVServiceV7.ChannelPackageType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.PackageID, opt => opt.MapFrom(src => src.PackageId))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                ;

            CreateMap<Common.IPTVServiceV3.ChannelPackageType, ChannelPackageType>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageID))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                ;

            CreateMap<Common.IPTVServiceV7.ChannelPackageType, ChannelPackageType>()
                .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageID))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.PackageName))
                ;
        }
    }
}