
using System.Collections.Generic;
using ANDP.Lib.Data.Repositories.Equipment;
using ANDP.Lib.Domain.Interfaces;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Company = ANDP.Lib.Data.Repositories.Equipment.Company;
using DataDictionary = ANDP.Lib.Domain.Models.DataDictionary;
using UsocToCommandTranslation = ANDP.Lib.Domain.Models.UsocToCommandTranslation;

namespace ANDP.Lib.Domain.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ICommonMapper _iCommonMapper;

        public EquipmentService(IEquipmentRepository equipmentRepository, ICommonMapper iCommonMapper)
        {
            _equipmentRepository = equipmentRepository;
            _iCommonMapper = iCommonMapper;
        }

        #region Dictionary Data

        public DataDictionary RetrieveDictionaryDataTranslation(DataDictionary data)
        {
            var daodata = ObjectFactory.CreateInstanceAndMap<DataDictionary, Data.Repositories.Equipment.DataDictionary>(_iCommonMapper, data);
            daodata = _equipmentRepository.RetrieveDictionaryDataTranslation(daodata, data.Active);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.DataDictionary, DataDictionary>(_iCommonMapper, daodata);
        }

        public DataDictionary RetrieveDictionaryDataTranslationById(int id)
        {
            var daodata = _equipmentRepository.RetrieveDictionaryDataTranslationById(id);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.DataDictionary, DataDictionary>(_iCommonMapper, daodata);
        }

        public IEnumerable<DataDictionary> RetrieveDictionaryDataTranslations(int companyId, int? equipmentId, bool? active)
        {
            var daodata = _equipmentRepository.RetrieveDictionaryDataTranslations(companyId, equipmentId, active);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<Data.Repositories.Equipment.DataDictionary>, IEnumerable<DataDictionary>>(_iCommonMapper, daodata);
        }

        public DataDictionary CreateOrUpdateDictionaryDataTranslation(DataDictionary dataDictionary, string user)
        {
            var daodata = ObjectFactory.CreateInstanceAndMap<DataDictionary, Data.Repositories.Equipment.DataDictionary>(_iCommonMapper, dataDictionary);
            daodata = _equipmentRepository.CreateOrUpdateDictionaryDataTranslation(daodata, user);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.DataDictionary, DataDictionary>(_iCommonMapper, daodata);
        }

        public void DeleteDictionaryDataTranslationById(int id, string updatingUserId)
        {
            _equipmentRepository.DeleteDictionaryDataTranslationById(id, updatingUserId);
        }

        public void DeactivateDictionaryDataTranslationById(int id, string updatingUserId)
        {
            _equipmentRepository.DeactivateDictionaryDataTranslationById(id, updatingUserId);
        }

        #endregion

        #region USOC Translations

        public string RetrieveUsocAddTranslation(int companyId, int equipmentId, string usocName)
        {
            return _equipmentRepository.RetrieveUsocAddTranslation(companyId,  equipmentId, usocName);
        }

        public string RetrieveUsocRemoveTranslation(int companyId, int equipmentId, string usocName)
        {
            return _equipmentRepository.RetrieveUsocRemoveTranslation(companyId, equipmentId, usocName);
        }

        public IEnumerable<UsocToCommandTranslation> RetrieveUsocTranslations(int companyId, int? equipmentId, bool? active)
        {
            var daodata = _equipmentRepository.RetrieveUsocTranslations(companyId, equipmentId, active);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<Data.Repositories.Equipment.UsocToCommandTranslation>, IEnumerable<UsocToCommandTranslation>>(_iCommonMapper, daodata);
        }

        public UsocToCommandTranslation RetrieveUsocTranslation(int companyId, int equipmentId, string usocName)
        {
            var daodata = _equipmentRepository.RetrieveUsocTranslation(companyId, equipmentId, usocName);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.UsocToCommandTranslation, UsocToCommandTranslation>(_iCommonMapper, daodata);
        }

        public UsocToCommandTranslation RetrieveUsocTranslationById(int id)
        {
            var daodata = _equipmentRepository.RetrieveUsocTranslationById(id);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.UsocToCommandTranslation, UsocToCommandTranslation>(_iCommonMapper, daodata);
        }

        public UsocToCommandTranslation CreateOrUpdateUsocTranslation(UsocToCommandTranslation usocToCommandTranslation, string updatingUserId)
        {
            var data = ObjectFactory.CreateInstanceAndMap<UsocToCommandTranslation, Data.Repositories.Equipment.UsocToCommandTranslation>(_iCommonMapper, usocToCommandTranslation);
            data = _equipmentRepository.CreateOrUpdateUsocTranslation(data, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<Data.Repositories.Equipment.UsocToCommandTranslation, UsocToCommandTranslation>(_iCommonMapper, data);
        }

        public void DeleteUsocTranslationById(int id, string updatingUserId)
        {
            _equipmentRepository.DeleteUsocTranslationById(id, updatingUserId);
        }

        public void DeactivateUsocTranslationById(int id, string updatingUserId)
        {
            _equipmentRepository.DeactivateUsocTranslationById(id, updatingUserId);
        }

        #endregion

        public IEnumerable<Models.Company> RetrieveAllCompanies()
        {
            var daoCompaines = _equipmentRepository.RetrieveAllCompanies();
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<Company>, IEnumerable<Models.Company>>(_iCommonMapper, daoCompaines);
        }

        public int RetrieveCompanyIdByExternalCompanyId(string externalCompanyId)
        {
            return _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
        }

        public Models.Company RetrieveCompanyByCompanyId(int companyId)
        {
            var daoCompaines = _equipmentRepository.RetrieveCompanyByCompanyId(companyId);
            return ObjectFactory.CreateInstanceAndMap<Company, Models.Company>(_iCommonMapper, daoCompaines);
        }

        public ProvisioningEquipment RetrieveEquipmentByEquipmentId(int equipmentId)
        {
            var equipment = _equipmentRepository.RetrieveEquipmentByEquipmentId(equipmentId);
            return ObjectFactory.CreateInstanceAndMap<EquipmentSetup, ProvisioningEquipment>(_iCommonMapper, equipment);
        }

        public IEnumerable<ProvisioningEquipment> RetrieveEquipmentByCompanyId(int companyId)
        {
            var equipment = _equipmentRepository.RetrieveEquipmentByCompanyId(companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<EquipmentSetup>, IEnumerable<ProvisioningEquipment>>(_iCommonMapper, equipment);
        }
    }
}

