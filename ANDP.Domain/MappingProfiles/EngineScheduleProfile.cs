using ANDP.Lib.Data.Repositories.Engine;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EngineScheduleProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ANDP.Lib.Domain.Models.EngineSchedule, ProvisioningEngineSchedule>()
                .ForMember(dest => dest.ProvisioningEngineSettingsId, opt => opt.Ignore())
                .ForMember(dest => dest.ProvisioningEngineSetting, opt => opt.Ignore())
            ;

            CreateMap<ProvisioningEngineSchedule, ANDP.Lib.Domain.Models.EngineSchedule>()

            ;
        }
    }
}
