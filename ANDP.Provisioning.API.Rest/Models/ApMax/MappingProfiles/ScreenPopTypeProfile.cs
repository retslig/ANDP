using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ScreenPopTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Common.CallingNameV3.ScreenPopType, ScreenPopType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                .ForMember(dest => dest.NpaNxx, opt => opt.MapFrom(src => src.NpaNxx))
                .ForMember(dest => dest.ScreenPopServerTypes, opt => opt.MapFrom(src => src.ServersField))
                ;

            CreateMap<Common.CallingNameV4.ScreenPopType, ScreenPopType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                .ForMember(dest => dest.NpaNxx, opt => opt.MapFrom(src => src.NpaNxx))
                .ForMember(dest => dest.ScreenPopServerTypes, opt => opt.MapFrom(src => src.ServersField))
                ;

            CreateMap<ScreenPopType, Common.CallingNameV3.ScreenPopType>()
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NpaNxx, opt => opt.MapFrom(src => src.NpaNxx))
                .ForMember(dest => dest.ServersField, opt => opt.MapFrom(src => src.ScreenPopServerTypes))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<ScreenPopType, Common.CallingNameV4.ScreenPopType>()
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NpaNxx, opt => opt.MapFrom(src => src.NpaNxx))
                .ForMember(dest => dest.ServersField, opt => opt.MapFrom(src => src.ScreenPopServerTypes))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;
        }
    }
}