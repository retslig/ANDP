
using Common.Lib.Domain.Common.Models;
using Common.Lib.Mapping;

namespace Common.Lib.Domain.Common.MappingProfiles
{
    public class TenantProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Tenant, Tenant>();

            CreateMap<Tenant, Tenant>();
        }
    }
}
