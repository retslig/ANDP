using System.Collections.Generic;
using System.Linq;
using ANDP.Lib.Domain.Models;
using AutoMapper;
using Common.Lib.Extensions;
using Common.Lib.Mapping;
using Common.Lib.Mapping.CustomResolvers;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class ServiceProfile : CommonMappingProfile
    {
        protected override void Configure()
        {

            CreateMap<ANDP.Lib.Domain.Models.Service, Data.Repositories.Order.Service>()
                //Static Mappings
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))

                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser))
                .ForMember(dest => dest.ModifiedByUser, opt => opt.MapFrom(src => src.ModifiedByUser))
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.ResolveUsing<PhoneVideoInternetItemsToItemsCustomResolver>())
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<Data.Repositories.Order.Service, ANDP.Lib.Domain.Models.Service>()
                //Constructor
                .ConstructUsing(CreateInstance)

                //ignore Mappings
                .ForMember(dest => dest.Notes, opt => opt.Ignore())

                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.Locations, opt => opt.Ignore())
                .ForMember(dest => dest.Xml, opt => opt.Ignore())
                .ForMember(dest => dest.Log, opt => opt.MapFrom(src => src.Log))
                .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore())
            ;
        }

        private ANDP.Lib.Domain.Models.Service CreateInstance(ResolutionContext rc)
        {
            var src = (Data.Repositories.Order.Service)rc.SourceValue;
            var dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.Service>();
            dest.Xml = "";

            foreach (var location in dest.Locations)
            {
                foreach (var item in location.InternetItems)
                {
                    var tempItem = src.Items.FirstOrDefault(p => p.ExternalItemId == item.ExternalItemId);
                    if (tempItem != null)
                    {
                        item.Id = tempItem.Id;
                        item.ServiceId = tempItem.Id;
                        item.ResultMessage = tempItem.ResultMessage;
                        item.Log = tempItem.Log;
                        item.CompletionDate = tempItem.CompletionDate;
                        item.StartDate = tempItem.StartDate;
                        item.StatusType = (StatusType)tempItem.StatusTypeId;
                    }
                }

                foreach (var item in location.PhoneItems)
                {
                    var tempItem = src.Items.FirstOrDefault(p => p.ExternalItemId == item.ExternalItemId);
                    if (tempItem != null)
                    {
                        item.Id = tempItem.Id;
                        item.ServiceId = tempItem.Id;
                        item.ResultMessage = tempItem.ResultMessage;
                        item.Log = tempItem.Log;
                        item.CompletionDate = tempItem.CompletionDate;
                        item.StartDate = tempItem.StartDate;
                        item.StatusType = (StatusType)tempItem.StatusTypeId;
                    }
                }

                foreach (var item in location.VideoItems)
                {
                    var tempItem = src.Items.FirstOrDefault(p => p.ExternalItemId == item.ExternalItemId);
                    if (tempItem != null)
                    {
                        item.Id = tempItem.Id;
                        item.ServiceId = tempItem.Id;
                        item.ResultMessage = tempItem.ResultMessage;
                        item.Log = tempItem.Log;
                        item.CompletionDate = tempItem.CompletionDate;
                        item.StartDate = tempItem.StartDate;
                        item.StatusType = (StatusType)tempItem.StatusTypeId;
                    }
                }
            }

            return dest;
        }
    }

    public class PhoneVideoInternetItemsToItemsCustomResolver : ValueResolver<ANDP.Lib.Domain.Models.Service, List<Data.Repositories.Order.Item>>
    {
        protected override List<Data.Repositories.Order.Item> ResolveCore(ANDP.Lib.Domain.Models.Service myService)
        {
            var domainPhoneItems = new List<PhoneItem>();
            var domainInternetItems = new List<InternetItem>();
            var domainVideoItems = new List<VideoItem>();

            if (myService.Locations.Any())
            {
                foreach (var location in myService.Locations)
                {
                    if (location.PhoneItems != null && location.PhoneItems.Any())
                    {
                        var phoneItems = location.PhoneItems.ToList();
                        if (phoneItems.Any())
                        {
                            domainPhoneItems.AddRange(phoneItems);
                        }
                    }

                    if (location.InternetItems != null && location.InternetItems.Any())
                    {
                        var internetItems = location.InternetItems.ToList();
                        if (internetItems.Any())
                        {
                            domainInternetItems.AddRange(internetItems);
                        } 
                    }

                    if (location.VideoItems != null && location.VideoItems.Any())
                    {
                        var videoItems = location.VideoItems.ToList();
                        if (videoItems.Any())
                        {
                            domainVideoItems.AddRange(videoItems);
                        } 
                    }
                }
            }

            var daoPhoneItems = Mapper.Map<IEnumerable<PhoneItem>, IEnumerable<Data.Repositories.Order.Item>>(domainPhoneItems).ToList();
            var daoVideoItems = Mapper.Map<IEnumerable<VideoItem>, IEnumerable<Data.Repositories.Order.Item>>(domainVideoItems).ToList();
            var daoInternetItems = Mapper.Map<IEnumerable<InternetItem>, IEnumerable<Data.Repositories.Order.Item>>(domainInternetItems).ToList();

            var daoItems = daoPhoneItems.Union(daoVideoItems.Union(daoInternetItems)).ToList();

            return daoItems;
        }
    }
}
