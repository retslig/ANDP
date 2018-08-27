using Common.Lib.Mapping;

namespace ANDP.Provisioning.API.Rest.Models.ApMax.MappingProfiles
{
    public class ServiceVersionsProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Common.ServiceReport.ServiceVersions, ANDP.Provisioning.API.Rest.Models.ApMax.ServiceVersions>()
                ;
        }
    }
}