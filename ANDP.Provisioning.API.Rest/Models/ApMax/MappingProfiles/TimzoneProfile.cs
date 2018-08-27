using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class TimezoneProfile : CommonMappingProfile
    {

        protected override void Configure()
        {
            //CreateMap<Timezone, Common.SubscriberV3.Timezone_e>()
            //    .ForMember(dest => dest, opt => opt.MapFrom(src => src))
            //    ;

            //CreateMap<Timezone, Common.SubscriberV4.Timezone_e>()
            //    .ForMember(dest => dest, opt => opt.MapFrom(src => src))
            //    ;
        }
    }
}