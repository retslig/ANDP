
using AutoMapper;
using Common.Lib.Extensions;
using Common.Lib.Mapping;
using Common.Lib.Mapping.CustomResolvers;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EquipmentProfile : CommonMappingProfile
    {
        protected override void Configure()
        {

            CreateMap<ANDP.Lib.Domain.Models.Equipment, ANDP.Lib.Data.Repositories.Order.Equipment>()
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)ANDP.Lib.Domain.Models.StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))

                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.Item, opt => opt.Ignore())
                .ForMember(dest => dest.EquipmentItemId, opt => opt.Ignore())
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<ANDP.Lib.Data.Repositories.Order.Equipment, ANDP.Lib.Domain.Models.Equipment>()
                .ConstructUsing(CreateInstance)
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.Xml, opt => opt.Ignore())
            ;
        }

        private ANDP.Lib.Domain.Models.Equipment CreateInstance(ResolutionContext rc)
        {
            var dest = new Models.Equipment();

            var src = (ANDP.Lib.Data.Repositories.Order.Equipment)rc.SourceValue;

            if (!string.IsNullOrEmpty(src.Xml))
                dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.Equipment>();
            dest.Xml = "";
            return dest;
        }

    }
}
