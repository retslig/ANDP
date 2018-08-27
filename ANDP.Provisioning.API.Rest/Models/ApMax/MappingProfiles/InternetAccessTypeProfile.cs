using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class InternetAccessType : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.VoicemailV3.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.VoicemailV4.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;


            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.SubscriberV3.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.SubscriberV4.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;


            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.IPTVServiceV3.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.IPTVServiceV7.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;


            CreateMap<Common.SubscriberV3.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.SubscriberV4.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<API.Rest.Models.ApMax.InternetAccessType, Common.VoicemailV5.InternetAccessType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.VoicemailV3.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.VoicemailV4.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.VoicemailV5.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.IPTVServiceV3.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;

            CreateMap<Common.IPTVServiceV7.InternetAccessType, API.Rest.Models.ApMax.InternetAccessType>()
                .ForMember(dest => dest.MobileEnabled, opt => opt.MapFrom(src => src.MobileEnabled))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.ServiceEnabled, opt => opt.MapFrom(src => src.ServiceEnabled))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                ;
        }



    }
}