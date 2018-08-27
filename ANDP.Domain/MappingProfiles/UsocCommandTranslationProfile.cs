using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class UsocCommandTranslation : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Data.Repositories.Equipment.UsocToCommandTranslation, Models.UsocToCommandTranslation>()
                ;

            CreateMap<Models.UsocToCommandTranslation, Data.Repositories.Equipment.UsocToCommandTranslation>()
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.EquipmentSetup, opt => opt.Ignore());
        }
    }
}
