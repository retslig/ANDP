using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class CompanyProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ANDP.Lib.Domain.Models.Company, ANDP.Lib.Data.Repositories.Engine.Company>()
                .ForMember(dest => dest.ProvisioningEngineSettings, opt => opt.Ignore());

            CreateMap<ANDP.Lib.Data.Repositories.Engine.Company, ANDP.Lib.Domain.Models.Company>()
                ;

            CreateMap<ANDP.Lib.Domain.Models.Company, ANDP.Lib.Data.Repositories.Equipment.Company>()
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.DataDictionaries, opt => opt.Ignore())
                .ForMember(dest => dest.EquipmentSetups, opt => opt.Ignore())
                .ForMember(dest => dest.UsocToCommandTranslations, opt => opt.Ignore())
                ;

            CreateMap<ANDP.Lib.Data.Repositories.Equipment.Company, ANDP.Lib.Domain.Models.Company>()
                ;
        }
    }
}
