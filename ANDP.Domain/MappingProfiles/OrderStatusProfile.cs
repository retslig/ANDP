using ANDP.Lib.Data.Repositories.Order;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class OrderStatusProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Order, ANDP.Lib.Domain.Models.OrderStatus>()
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
            ;
        }
    }
}
