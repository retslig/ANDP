using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ScreenPopSubscriberType : CommonMappingProfile
    {
        protected override void Configure()
        {

            CreateMap<ANDP.Provisioning.API.Rest.Models.ApMax.ScreenPopSubscriberType, Common.CallingNameV3.ScreenPopSubscriberType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.ScreenPopEnabledField, opt => opt.MapFrom(src => src.ScreenPopEnabled))
                .ForMember(dest => dest.SubscriberPhoneNumberField, opt => opt.MapFrom(src => src.SubscriberPhoneNumber))
                ;

            CreateMap<ANDP.Provisioning.API.Rest.Models.ApMax.ScreenPopSubscriberType, Common.CallingNameV4.ScreenPopSubscriberType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.ScreenPopEnabledField, opt => opt.MapFrom(src => src.ScreenPopEnabled))
                .ForMember(dest => dest.SubscriberPhoneNumberField, opt => opt.MapFrom(src => src.SubscriberPhoneNumber))
                .ForMember(dest => dest.MacAddresses, opt => opt.MapFrom(src => src.MacAddresses))
                ;

            CreateMap<Common.CallingNameV3.ScreenPopSubscriberType, ANDP.Provisioning.API.Rest.Models.ApMax.ScreenPopSubscriberType>()
                .ForMember(dest => dest.SubscriberPhoneNumber, opt => opt.MapFrom(src => src.SubscriberPhoneNumberField))
                .ForMember(dest => dest.MacAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.ScreenPopEnabled, opt => opt.MapFrom(src => src.ScreenPopEnabledField))
                ;

            CreateMap<Common.CallingNameV4.ScreenPopSubscriberType, ANDP.Provisioning.API.Rest.Models.ApMax.ScreenPopSubscriberType>()
                .ForMember(dest => dest.SubscriberPhoneNumber, opt => opt.MapFrom(src => src.SubscriberPhoneNumberField))
                .ForMember(dest => dest.MacAddresses, opt => opt.MapFrom(src => src.MacAddresses))
                .ForMember(dest => dest.ScreenPopEnabled, opt => opt.MapFrom(src => src.ScreenPopEnabledField))
                ;
        }
    }
}