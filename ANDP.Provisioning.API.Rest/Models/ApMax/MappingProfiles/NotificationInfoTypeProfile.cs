using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class NotificationInfoTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<NotificationInfoType, Common.VoicemailV3.NotificationInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.EnabledField, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.CenterField, opt => opt.MapFrom(src => src.Center))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<NotificationInfoType, Common.VoicemailV4.NotificationInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.EnabledField, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.CenterField, opt => opt.MapFrom(src => src.Center))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<NotificationInfoType, Common.VoicemailV5.NotificationInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.EnabledField, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.CenterField, opt => opt.MapFrom(src => src.Center))
                .ForMember(dest => dest.DescriptionField, opt => opt.Ignore())
                .ForMember(dest => dest.DeviceIdField, opt => opt.Ignore())
                .ForMember(dest => dest.LastAccessField, opt => opt.Ignore())
                .ForMember(dest => dest.NotificationAddressType, opt => opt.Ignore())
                .ForMember(dest => dest.EmailFormat, opt => opt.Ignore())
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<Common.VoicemailV3.NotificationInfoType, NotificationInfoType>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnabledField))
                .ForMember(dest => dest.Center, opt => opt.MapFrom(src => src.CenterField))
                ;

            CreateMap<Common.VoicemailV4.NotificationInfoType, NotificationInfoType>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnabledField))
                .ForMember(dest => dest.Center, opt => opt.MapFrom(src => src.CenterField))
                ;

            CreateMap<Common.VoicemailV5.NotificationInfoType, NotificationInfoType>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.EnabledField))
                .ForMember(dest => dest.Center, opt => opt.MapFrom(src => src.CenterField))
                ;
        }
    }
}