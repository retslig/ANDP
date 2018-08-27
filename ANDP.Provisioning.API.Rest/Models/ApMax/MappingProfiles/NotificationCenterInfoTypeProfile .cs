using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class NotificationCenterInfoTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<NotificationCenterInfoType, Common.VoicemailV3.NotificationCenterInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.CenterIdField, opt => opt.Ignore())
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TypeField, opt => opt.Ignore())
                 ;

            CreateMap<NotificationCenterInfoType, Common.VoicemailV4.NotificationCenterInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.CenterIdField, opt => opt.Ignore())
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TypeField, opt => opt.Ignore())
                 ;

            CreateMap<NotificationCenterInfoType, Common.VoicemailV5.NotificationCenterInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.CenterIdField, opt => opt.Ignore())
                .ForMember(dest => dest.DescriptionField, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TypeField, opt => opt.Ignore())
                 ;

            CreateMap<Common.VoicemailV3.NotificationCenterInfoType, NotificationCenterInfoType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                ;

            CreateMap<Common.VoicemailV4.NotificationCenterInfoType, NotificationCenterInfoType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                ;

            CreateMap<Common.VoicemailV5.NotificationCenterInfoType, NotificationCenterInfoType>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescriptionField))
                ;
        }
    }
}