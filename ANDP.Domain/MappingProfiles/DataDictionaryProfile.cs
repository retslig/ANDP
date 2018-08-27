using ANDP.Lib.Data.Repositories.Equipment;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class DataDictionaryProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<DataDictionary, ANDP.Lib.Domain.Models.DataDictionary>()
                ;

            CreateMap<ANDP.Lib.Domain.Models.DataDictionary, DataDictionary>()
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.EquipmentSetup, opt => opt.Ignore());
        }
    }
}
