using ANDP.Lib.Data.Repositories.Order;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EquipmentStatusProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Equipment, ANDP.Lib.Domain.Models.EquipmentStatus>()
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.ExternalOrderId, opt => opt.Ignore())
            ;
        }
    }
}
