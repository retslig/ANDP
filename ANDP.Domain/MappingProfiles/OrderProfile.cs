
using ANDP.Lib.Domain.Models;
using AutoMapper;
using Common.Lib.Extensions;
using Common.Lib.Mapping;
using Common.Lib.Mapping.CustomResolvers;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class OrderProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<ANDP.Lib.Domain.Models.Order, ANDP.Lib.Data.Repositories.Order.Order>()
                //ignore Mappings
                //.ForMember(dest => dest.Company, opt => opt.Ignore())

                //Static Mappings
                .ForMember(dest => dest.StatusTypeId, opt => opt.UseValue((int)StatusType.Pending))
                .ForMember(dest => dest.CompletionDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.StartDate, opt => opt.UseValue(null))
                .ForMember(dest => dest.Log, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResultMessage, opt => opt.UseValue(""))
                .ForMember(dest => dest.ResponseSent, opt => opt.UseValue(false))

                //Special Mappings
                .ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => (int)src.ActionType))
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services))

                //Mappings that are the same name
                .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified))
                .ForMember(dest => dest.ExternalAccountId, opt => opt.MapFrom(src => src.ExternalAccountId))
                .ForMember(dest => dest.ExternalCompanyId, opt => opt.MapFrom(src => src.ExternalCompanyId))
                .ForMember(dest => dest.ExternalOrderId, opt => opt.MapFrom(src => src.ExternalOrderId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ModifiedByUser, opt => opt.MapFrom(src => src.ModifiedByUser))
                .ForMember(dest => dest.OrginatingIp, opt => opt.MapFrom(src => src.OrginatingIp))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.ProvisionDate, opt => opt.MapFrom(src => src.ProvisionDate))
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(dest => dest.Xml, opt => opt.ResolveUsing<SerializeObjectToXmlCustomResolver>())
            ;

            CreateMap<ANDP.Lib.Data.Repositories.Order.Order, ANDP.Lib.Domain.Models.Order>()
                // Constructor
                .ConstructUsing(CreateInstance)

                //ignore Mappings
                .ForMember(dest => dest.Notes, opt => opt.Ignore())

                .ForMember(dest => dest.ValidationErrors, opt => opt.Ignore())
                .ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.StatusType)src.StatusTypeId))
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ActionType)src.ActionTypeId))
                .ForMember(dest => dest.Log, opt => opt.MapFrom(src => src.Log))
                .ForMember(dest => dest.Account, opt => opt.Ignore())
                .ForMember(dest => dest.CSR, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceProvider, opt => opt.Ignore())
                .ForMember(dest => dest.ClassOfService, opt => opt.Ignore())
                .ForMember(dest => dest.Configuration, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())

                .ForMember(dest => dest.NewNetCaseId, opt => opt.Ignore())
                .ForMember(dest => dest.NewNetTriggerId, opt => opt.Ignore())
                .ForMember(dest => dest.NewNetProcessId, opt => opt.Ignore())
                .ForMember(dest => dest.NewNetRouteIndex, opt => opt.Ignore())
                .ForMember(dest => dest.NewNetTaskId, opt => opt.Ignore())
                .ForMember(dest => dest.NewNetUserId, opt => opt.Ignore())
            ;
        }

        private ANDP.Lib.Domain.Models.Order CreateInstance(ResolutionContext rc)
        {
            var src = (ANDP.Lib.Data.Repositories.Order.Order)rc.SourceValue;
            var dest = src.Xml.DeSerializeStringToObject<ANDP.Lib.Domain.Models.Order>();
            dest.Xml = "";
            return dest;
        }
    }
}