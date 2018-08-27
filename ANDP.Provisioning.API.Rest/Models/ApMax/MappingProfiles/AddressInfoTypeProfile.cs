using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class AddressInfoTypeProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<AddressInfoType, Common.SubscriberV3.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.SubscriberV4.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.VoicemailV3.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.VoicemailV4.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.VoicemailV5.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.IPTVServiceV3.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<AddressInfoType, Common.IPTVServiceV7.AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
                 ;

            CreateMap<Common.SubscriberV3.AddressInfoType, AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
            ;

            CreateMap<Common.SubscriberV4.AddressInfoType, AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
            ;

            CreateMap<Common.VoicemailV3.AddressInfoType, AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
            ;

            CreateMap<Common.VoicemailV4.AddressInfoType, AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
            ;

            CreateMap<Common.VoicemailV5.AddressInfoType, AddressInfoType>()
                .ForMember(dest => dest.AddressField, opt => opt.MapFrom(src => src.AddressField))
                .ForMember(dest => dest.AddressTypeField, opt => opt.MapFrom(src => src.AddressTypeField))
                //.ForMember(dest => dest.ExtensionData, opt => opt.MapFrom(src => src.ExtensionData))
                .ForMember(dest => dest.ExtensionData, opt => opt.Ignore())
            ;
        }
    }
}