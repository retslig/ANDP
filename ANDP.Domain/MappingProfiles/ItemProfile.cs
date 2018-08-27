using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.Models;
using AutoMapper;
using Common.Lib.Extensions;
using Common.Lib.Mapping;
using Common.Lib.Mapping.CustomResolvers;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class ItemProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ANDP.Lib.Domain.Models.PhoneItem, Item>()
                //Static Mappings
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))

                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.Service, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypeId, opt => opt.UseValue((int)ItemTypeEnum.Phone))
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Equipments))
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<ANDP.Lib.Domain.Models.InternetItem, Item>()
                //Static Mappings
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))

                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.Service, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypeId, opt => opt.UseValue((int)ItemTypeEnum.Internet))
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Equipments))
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<ANDP.Lib.Domain.Models.VideoItem, Item>()
                //Static Mappings
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))

                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.Service, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypeId, opt => opt.UseValue((int)ItemTypeEnum.Video))
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Equipments))
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<Item, ANDP.Lib.Domain.Models.PhoneItem>()
                .ConstructUsing(CreatePhoneInstance)

                //ignore Mappings
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.CustomId, opt => opt.Ignore())

                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.Log, opt => opt.MapFrom(src => src.Log))
                .ForMember(dest => dest.LineInformation, opt => opt.Ignore())
                .ForMember(dest => dest.Features, opt => opt.Ignore())
                .ForMember(dest => dest.Directory, opt => opt.Ignore())
                .ForMember(dest => dest.Plant, opt => opt.Ignore())
                .ForMember(dest => dest.OldPlant, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore())
            ;

            CreateMap<Item, ANDP.Lib.Domain.Models.InternetItem>()
                .ConstructUsing(CreateInternetInstance)

                //ignore Mappings
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.CustomId, opt => opt.Ignore())

                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.Log, opt => opt.MapFrom(src => src.Log))
                .ForMember(dest => dest.Features, opt => opt.Ignore())
                .ForMember(dest => dest.PrimaryUser, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Plant, opt => opt.Ignore())
                .ForMember(dest => dest.OldPlant, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore())
            ;

            CreateMap<Item, ANDP.Lib.Domain.Models.VideoItem>()
                .ConstructUsing(CreateVideoInstance)

                //ignore Mappings
                .ForMember(dest => dest.Notes, opt => opt.Ignore())
                .ForMember(dest => dest.CustomId, opt => opt.Ignore())

                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.Log, opt => opt.MapFrom(src => src.Log))
                .ForMember(dest => dest.MaxPurchaseLimit, opt => opt.Ignore())
                .ForMember(dest => dest.PurchasePin, opt => opt.Ignore())
                .ForMember(dest => dest.RatingPin, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceArea, opt => opt.Ignore())
                .ForMember(dest => dest.OldServiceArea, opt => opt.Ignore())
                .ForMember(dest => dest.FipsCountyCode, opt => opt.Ignore())
                .ForMember(dest => dest.OldFipsCountyCode, opt => opt.Ignore())
                .ForMember(dest => dest.FipsStateCode, opt => opt.Ignore())
                .ForMember(dest => dest.OldFipsStateCode, opt => opt.Ignore())
                .ForMember(dest => dest.ScreenPopPhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.OldScreenPopPhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Features, opt => opt.Ignore())
                .ForMember(dest => dest.Plant, opt => opt.Ignore())
                .ForMember(dest => dest.OldPlant, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore())
            ;
        }

        private ANDP.Lib.Domain.Models.VideoItem CreateVideoInstance(ResolutionContext rc)
        {
            var src = (Item)rc.SourceValue;
            var dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.VideoItem>();
            dest.Xml = "";
            return dest;
        }

        private ANDP.Lib.Domain.Models.InternetItem CreateInternetInstance(ResolutionContext rc)
        {
            var src = (Item)rc.SourceValue;
            var dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.InternetItem>();
            dest.Xml = "";
            return dest;
        }

        private ANDP.Lib.Domain.Models.PhoneItem CreatePhoneInstance(ResolutionContext rc)
        {
            var src = (Item)rc.SourceValue;
            var dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.PhoneItem>();
            dest.Xml = "";
            return dest;
        }
    }
}
