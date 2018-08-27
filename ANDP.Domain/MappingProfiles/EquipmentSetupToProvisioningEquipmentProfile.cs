using ANDP.Lib.Data.Repositories.Equipment;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EquipmentSetupToProvisioningEquipmentProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<EquipmentSetup, ProvisioningEquipment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.EquipmentConnectionSettings, opt => opt.MapFrom(src => src.EquipmentConnectionSetting))
                .ForMember(dest => dest.EquipmentType, opt => opt.MapFrom(src => (EquipmentType)src.EquipmentTypeId))
                .AfterMap(
                    (src, dest) =>
                    {
                        dest.EquipmentConnectionSettings.EquipmentId = src.Id;
                    }
                );
        }
    }
}
