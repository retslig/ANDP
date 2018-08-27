using ANDP.Lib.Data.Repositories.Order;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class AccountProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ANDP.Lib.Domain.Models.Account, Account>()
                //.ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.StatusTypeId, opt => opt.MapFrom(src => (int)src.StatusType))
                .ForMember(dest => dest.Company, opt => opt.Ignore())
            ;

        }
    }
}
