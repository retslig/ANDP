
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class AuditRecordProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Models.AuditRecord, Data.Repositories.Audit.AuditRecord>()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.ItemTypeId, opt => opt.MapFrom(src => (int)src.ItemType))
                  .ForMember(dest => dest.EquipmentSetupId, opt => opt.MapFrom(src => src.EquipmentId))
                  .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore())
                  .ForMember(dest => dest.ModifiedByUser, opt => opt.Ignore())
                  .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                  .ForMember(dest => dest.DateModified, opt => opt.Ignore())
                  .ForMember(dest => dest.Version, opt => opt.Ignore())
            ;

            CreateMap<Data.Repositories.Audit.AuditRecord, Models.AuditRecord>()
                .ForMember(dest => dest.EquipmentId, opt => opt.MapFrom(src => src.EquipmentSetupId))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ItemType)src.ItemTypeId))
                ;

        }
    }
}
