using ANDP.Lib.Data.Repositories.Order;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class ServiceStatusProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Service, ANDP.Lib.Domain.Models.ServiceStatus>()
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (StatusTypeEnum)src.StatusTypeId))
                .ForMember(dest => dest.ExternalOrderId, opt => opt.Ignore())
            ;
        }
    }
}
