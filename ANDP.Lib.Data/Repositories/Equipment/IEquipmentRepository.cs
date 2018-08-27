using System;
using System.Collections.Generic;

namespace ANDP.Lib.Data.Repositories.Equipment
{
    public interface IEquipmentRepository : IDisposable
    {
        DataDictionary RetrieveDictionaryDataTranslation(DataDictionary data, bool? active);
        IEnumerable<DataDictionary> RetrieveDictionaryDataTranslations(int companyId, int? equipmentId, bool? active);
        DataDictionary RetrieveDictionaryDataTranslationById(int id);
        DataDictionary CreateOrUpdateDictionaryDataTranslation(DataDictionary daodata, string updatingUserId);
        void DeleteDictionaryDataTranslationById(int id, string updatingUserId);
        void DeactivateDictionaryDataTranslationById(int id, string updatingUserId);

        string RetrieveUsocAddTranslation(int companyId, int equipmentId, string usocName);
        string RetrieveUsocRemoveTranslation(int companyId, int equipmentId, string usocName);
        IEnumerable<UsocToCommandTranslation> RetrieveUsocTranslations(int companyId, int? equipmentId, bool? active);
        UsocToCommandTranslation RetrieveUsocTranslation(int companyId, int equipmentId, string usocName);
        UsocToCommandTranslation RetrieveUsocTranslationById(int id);
        UsocToCommandTranslation CreateOrUpdateUsocTranslation(UsocToCommandTranslation usocToCommandTranslation, string updatingUserId);
        void DeleteUsocTranslationById(int id, string updatingUserId);
        void DeactivateUsocTranslationById(int id, string updatingUserId);

        IEnumerable<Company> RetrieveAllCompanies();
        int RetrieveCompanyIdByExternalCompanyId(string externalCompanyId);
        Company RetrieveCompanyByCompanyId(int companyId);

        EquipmentSetup RetrieveEquipmentByEquipmentId(int equipmentId);
        IEnumerable<EquipmentSetup> RetrieveEquipmentByCompanyId(int companyId);
    }
}
