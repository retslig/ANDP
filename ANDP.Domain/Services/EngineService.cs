
using System;
using System.Linq;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Models;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.Services
{
    public class EngineService : IEngineService
    {
        private readonly IEngineRepository _iEngineRepository;
        private readonly ICommonMapper _iCommonMapper;

        public EngineService(IEngineRepository iEngineRepository, ICommonMapper iCommonMapper)
        {
            _iEngineRepository = iEngineRepository;
            _iCommonMapper = iCommonMapper;
        }

        public EngineSetting RetrieveProvisioningEngineSetting(int companyId)
        {
            var setting = _iEngineRepository.RetrieveProvisioningEngineSetting(companyId);
            var domainEngineSetting = ObjectFactory.CreateInstanceAndMap<ProvisioningEngineSetting, EngineSetting>(_iCommonMapper, setting);

            if (domainEngineSetting.ProvisionableItemActionTypes == null || !domainEngineSetting.ProvisionableItemActionTypes.Any())
            {
                domainEngineSetting.ProvisionableItemActionTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToList().Select(item => new ItemActionType
                {
                    ItemType = item,
                    ActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList()
                }).ToList();
            }

            if (domainEngineSetting.ProvisionableOrderOrServiceActionTypes == null || !domainEngineSetting.ProvisionableOrderOrServiceActionTypes.Any())
                domainEngineSetting.ProvisionableOrderOrServiceActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>().ToList();

            return domainEngineSetting;
        }

        public void PauseProvisioning(int companyId, string updatingUserId)
        {
            var settings = _iEngineRepository.RetrieveProvisioningEngineSetting(companyId);
            settings.ProvisioningPaused = true;
            _iEngineRepository.UpdateProvisioningEngineSettings(settings, updatingUserId);
        }

        public void UnPauseProvisioning(int companyId, string updatingUserId)
        {
            var settings = _iEngineRepository.RetrieveProvisioningEngineSetting(companyId);
            settings.ProvisioningPaused = false;
            _iEngineRepository.UpdateProvisioningEngineSettings(settings, updatingUserId);
        }
    }
}
