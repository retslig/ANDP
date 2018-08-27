using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ServiceInfoTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ServiceInfoType, Common.SubscriberV3.ServiceInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<ServiceInfoType, Common.SubscriberV4.ServiceInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<Common.SubscriberV3.ServiceInfoType, ServiceInfoType>()
                ;

            CreateMap<Common.SubscriberV4.ServiceInfoType, ServiceInfoType>()
                ;

            CreateMap<ServiceInfoType, Common.IPTVServiceV3.ServiceInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<ServiceInfoType, Common.IPTVServiceV7.ServiceInfoType>()
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                ;

            CreateMap<Common.IPTVServiceV3.ServiceInfoType, ServiceInfoType>()
                ;

            CreateMap<Common.IPTVServiceV7.ServiceInfoType, ServiceInfoType>()
                ;
        }
    }
}