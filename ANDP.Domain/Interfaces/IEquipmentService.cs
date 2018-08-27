
using System.Collections.Generic;
using ANDP.Lib.Domain.Models;
using Common.Lib.Domain.Common.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IEquipmentService
    {
        DataDictionary RetrieveDictionaryDataTranslation(DataDictionary data);
        DataDictionary RetrieveDictionaryDataTranslationById(int id);
        IEnumerable<DataDictionary> RetrieveDictionaryDataTranslations(int companyId, int? equipmentId, bool? active);
        DataDictionary CreateOrUpdateDictionaryDataTranslation(DataDictionary dataDictionary, string updatingUserId);
        void DeleteDictionaryDataTranslationById(int id, string updatingUserId);
        void DeactivateDictionaryDataTranslationById(int id, string updatingUserId);

        string RetrieveUsocRemoveTranslation(int companyId, int equipmentId, string usocName);
        string RetrieveUsocAddTranslation(int companyId, int equipmentId, string usocName);
        IEnumerable<UsocToCommandTranslation> RetrieveUsocTranslations(int companyId, int? equipmentId, bool? active);
        UsocToCommandTranslation RetrieveUsocTranslation(int companyId, int equipmentId, string usocName);
        UsocToCommandTranslation RetrieveUsocTranslationById(int id);
        UsocToCommandTranslation CreateOrUpdateUsocTranslation(UsocToCommandTranslation usocToCommandTranslation, string updatingUserId);
        void DeleteUsocTranslationById(int id, string updatingUserId);
        void DeactivateUsocTranslationById(int id, string updatingUserId);

        IEnumerable<Company> RetrieveAllCompanies();
        int RetrieveCompanyIdByExternalCompanyId(string externalCompanyId);
        Company RetrieveCompanyByCompanyId(int companyId);

        ProvisioningEquipment RetrieveEquipmentByEquipmentId(int equipmentId);
        IEnumerable<ProvisioningEquipment> RetrieveEquipmentByCompanyId(int companyId);
    }
}
