using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ScreenPopServerTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Common.CallingNameV3.ScreenPopServerType, ScreenPopServerType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddressField))
                .ForMember(dest => dest.PortNumber, opt => opt.MapFrom(src => src.PortNumberField))
                .ForMember(dest => dest.ScreenPopServerTypeEnum, opt => opt.Ignore())
                ;

            CreateMap<Common.CallingNameV4.ScreenPopServerType, ScreenPopServerType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddressField))
                .ForMember(dest => dest.PortNumber, opt => opt.MapFrom(src => src.PortNumberField))
                .ForMember(dest => dest.ScreenPopServerTypeEnum, opt => opt.MapFrom(src => src.ServerType))
                ;

            CreateMap<ScreenPopServerType, Common.CallingNameV3.ScreenPopServerType>()
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IpAddressField, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.PortNumberField, opt => opt.MapFrom(src => src.PortNumber))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<ScreenPopServerType, Common.CallingNameV4.ScreenPopServerType>()
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IpAddressField, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.PortNumberField, opt => opt.MapFrom(src => src.PortNumber))
                .ForMember(dest => dest.ServerType, opt => opt.MapFrom(src => src.ScreenPopServerTypeEnum))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;
        }
    }
}