
using System.Collections.Generic;
using System.Linq;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Domain.Models;
using AutoMapper;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EngineSettingsProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<EngineSetting, ProvisioningEngineSetting>()
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.ProvisioningEngineItemActionTypesSettings, opt => opt.Ignore())
                //Need to implement many to one tables some time
                .ForMember(dest => dest.ProvisioningEngineOrderOrServiceActionTypesSettings, opt => opt.Ignore())
                .ForMember(dest => dest.ProvisioningEngineSchedules, opt => opt.Ignore())
                .ForMember(dest => dest.ProvisionByMethodTypeId, opt => opt.MapFrom(src => (int) src.ProvisionByMethod))
                ;

            CreateMap<ProvisioningEngineSetting, EngineSetting>()
                .ForMember(dest => dest.ProvisionableItemActionTypes,
                    opt =>
                        opt.ResolveUsing
                            <ProvisioningEngineItemActionTypesSettingsToProvisionableActionTypesCustomResolver>()
                            .FromMember(src => src.ProvisioningEngineItemActionTypesSettings))
                .ForMember(dest => dest.ProvisionableOrderOrServiceActionTypes,
                    opt =>
                        opt.ResolveUsing<ProvisioningEngineOrderOrServiceActionTypesSettingsToProvisionableItemTypesCustomResolver>()
                            .FromMember(src => src.ProvisioningEngineOrderOrServiceActionTypesSettings))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.ProvisioningEngineSchedules))
                .ForMember(dest => dest.ProvisionByMethod, opt => opt.MapFrom(src => (ANDP.Lib.Domain.Models.ProvisionByMethodType)src.ProvisionByMethodTypeId))
            ;

        }
    }

    internal class ProvisioningEngineItemActionTypesSettingsToProvisionableActionTypesCustomResolver : ValueResolver<List<ProvisioningEngineItemActionTypesSetting>, List<ItemActionType>>
    {
        protected override List<ItemActionType> ResolveCore(List<ProvisioningEngineItemActionTypesSetting> provisioningEngineItemActionTypesSettings)
        {
            var result = new List<ItemActionType>();
            foreach (var group in provisioningEngineItemActionTypesSettings.GroupBy(p=>p.ItemTypeEnumId))
            {
                var item = new ItemActionType {ItemType = (ItemType) group.Key, ActionTypes = new List<ActionType>()};
                foreach (var provisioningEngineItemActionTypesSetting in group)
                {
                    item.ActionTypes.Add((ActionType)provisioningEngineItemActionTypesSetting.ActionTypeEnumId); 
                }
                result.Add(item);
            }

            return result;
        }
    }

    internal class ProvisioningEngineOrderOrServiceActionTypesSettingsToProvisionableItemTypesCustomResolver : ValueResolver<List<ProvisioningEngineOrderOrServiceActionTypesSetting>, List<ActionType>>
    {
        protected override List<ActionType> ResolveCore(List<ProvisioningEngineOrderOrServiceActionTypesSetting> provisioningEngineOrderOrServiceActionTypesSettings)
        {
            return provisioningEngineOrderOrServiceActionTypesSettings.Select(actionType => (ActionType)actionType.ActionTypeEnumId).ToList();
        }
    }
}
