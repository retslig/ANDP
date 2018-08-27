using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ANDP.Lib.Data.Repositories.Equipment
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly IANDP_Equipment_Entities _andpEquipmentEntities;

        public EquipmentRepository(IANDP_Equipment_Entities andpEquipmentEntities)
        {
            _andpEquipmentEntities = andpEquipmentEntities;
        }

        #region Dictionary Data

        public IEnumerable<DataDictionary> RetrieveDictionaryDataTranslations(int companyId, int? equipmentId, bool? active)
        {
            var daoData = _andpEquipmentEntities.DataDictionaries.AsNoTracking().
                Where(p =>
                    p.CompanyId == companyId
                    && (equipmentId == null || p.EquipmentId == equipmentId)
                    && (active == null || p.Active == active));
            return daoData.ToList();
        }

        public DataDictionary RetrieveDictionaryDataTranslation(DataDictionary data, bool? active)
        {
            data.Key1 = data.Key1 ?? "";
            data.Key2 = data.Key2 ?? "";
            data.Key3 = data.Key3 ?? "";
            data.Key4 = data.Key4 ?? "";
            data.Key5 = data.Key5 ?? "";
            data.Key6 = data.Key6 ?? "";
            data.Key7 = data.Key7 ?? "";
            data.Key8 = data.Key8 ?? "";
            data.Key9 = data.Key9 ?? "";

            var daoData = _andpEquipmentEntities.DataDictionaries.AsNoTracking().FirstOrDefault(p =>
                p.Key1 == data.Key1
                && p.Key2 == data.Key2
                && p.Key3 == data.Key3
                && p.Key4 == data.Key4
                && p.Key5 == data.Key5
                && p.Key6 == data.Key6
                && p.Key7 == data.Key7
                && p.Key8 == data.Key8
                && p.Key9 == data.Key9
                // ReSharper disable once RedundantBoolCompare
                && (active == null || p.Active == active)
                && p.EquipmentId == data.EquipmentId
                && p.CompanyId == data.CompanyId
                );

            return daoData;
        }

        public DataDictionary RetrieveDictionaryDataTranslationById(int id)
        {
            var daoData = _andpEquipmentEntities.DataDictionaries.AsNoTracking().FirstOrDefault(p => p.Id == id);
            return daoData;
        }

        public DataDictionary CreateOrUpdateDictionaryDataTranslation(DataDictionary dataDictionary, string updatingUserId)
        {
            var data = _andpEquipmentEntities.DataDictionaries.AsNoTracking().FirstOrDefault(p => p.Id == dataDictionary.Id);

            dataDictionary.ModifiedByUser = updatingUserId;
            dataDictionary.DateModified = DateTime.Now;
            dataDictionary.Version = 0;

            if (data != null && data.Id > 0)
            {
                dataDictionary.Id = data.Id;
                dataDictionary.Version = data.Version;
                dataDictionary.CreatedByUser = data.CreatedByUser;
                dataDictionary.DateCreated = data.DateCreated;
            }
            else
            {
                dataDictionary.CreatedByUser = updatingUserId;
                dataDictionary.DateCreated = DateTime.Now;
            }

            _andpEquipmentEntities.AttachEntity(null, dataDictionary, new[] { "Id" }, updatingUserId);
            _andpEquipmentEntities.SaveChanges();
            _andpEquipmentEntities.RefreshEntity(dataDictionary);
            return dataDictionary;
        }

        public void DeleteDictionaryDataTranslationById(int id, string updatingUserId)
        {
            var data = _andpEquipmentEntities.DataDictionaries.FirstOrDefault(x => x.Id == id);

            if (data == null)
                throw new Exception("Dictionary Data not found for id:" + id);

            _andpEquipmentEntities.DataDictionaries.Remove(data);
            _andpEquipmentEntities.SaveChanges();
        }

        public void DeactivateDictionaryDataTranslationById(int id, string updatingUserId)
        {
            var data = _andpEquipmentEntities.DataDictionaries.FirstOrDefault(x => x.Id == id);

            if (data == null)
                throw new Exception("Dictionary Data not found for id:" + id);

            data.Active = false;
            _andpEquipmentEntities.SaveChanges();
        }

        #endregion

        #region Usoc Translations

        public string RetrieveUsocAddTranslation(int companyId, int equipmentId, string usocName)
        {
            var record = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().FirstOrDefault(p =>
                p.UsocName == usocName
                && p.EquipmentId == equipmentId
                && p.CompanyId == companyId
                );
            if (record != null)
                return record.AddCommand;

            return "";
        }

        public string RetrieveUsocRemoveTranslation(int companyId, int equipmentId, string usocName)
        {
            var record = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().FirstOrDefault(p =>
                p.UsocName == usocName
                && p.EquipmentId == equipmentId
                && p.CompanyId == companyId
                );
            if (record != null)
                return record.DeleteCommand;

            return "";
        }

        public IEnumerable<UsocToCommandTranslation> RetrieveUsocTranslations(int companyId, int? equipmentId, bool? active)
        {
            var records = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().
                Where(p =>
                    p.CompanyId == companyId
                    && (equipmentId == null || p.EquipmentId == equipmentId)
                    && (active == null || p.Active == active));
            return records.ToList();
        }

        public UsocToCommandTranslation RetrieveUsocTranslation(int companyId, int equipmentId, string usocName)
        {
            var record = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().FirstOrDefault(p =>
                p.UsocName == usocName
                && p.EquipmentId == equipmentId
                && p.CompanyId == companyId
                );

            return record;
        }

        public UsocToCommandTranslation RetrieveUsocTranslationById(int id)
        {
            var record = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().FirstOrDefault(p =>
                p.Id == id
                );

            return record;
        }

        public UsocToCommandTranslation CreateOrUpdateUsocTranslation(UsocToCommandTranslation usocToCommandTranslation, string updatingUserId)
        {
            var data = _andpEquipmentEntities.UsocToCommandTranslations.AsNoTracking().FirstOrDefault(p => p.Id == usocToCommandTranslation.Id);

            usocToCommandTranslation.ModifiedByUser = updatingUserId;
            usocToCommandTranslation.DateModified = DateTime.Now;
            usocToCommandTranslation.Version = 0;

            if (data != null && data.Id > 0)
            {
                usocToCommandTranslation.Id = data.Id;
                usocToCommandTranslation.Version = data.Version;
                usocToCommandTranslation.CreatedByUser = data.CreatedByUser;
                usocToCommandTranslation.DateCreated = data.DateCreated;
            }
            else
            {
                usocToCommandTranslation.CreatedByUser = updatingUserId;
                usocToCommandTranslation.DateCreated = DateTime.Now;
            }

            _andpEquipmentEntities.AttachEntity(null, usocToCommandTranslation, new[] { "Id" }, updatingUserId);
            _andpEquipmentEntities.SaveChanges();
            _andpEquipmentEntities.RefreshEntity(usocToCommandTranslation);
            return usocToCommandTranslation;
        }

        public void DeleteUsocTranslationById(int id, string updatingUserId)
        {
            var data = _andpEquipmentEntities.UsocToCommandTranslations.FirstOrDefault(x => x.Id == id);

            if (data == null)
                throw new Exception("Usoc not found for id:" + id);

            _andpEquipmentEntities.UsocToCommandTranslations.Remove(data);
            _andpEquipmentEntities.SaveChanges();
        }

        public void DeactivateUsocTranslationById(int id, string updatingUserId)
        {
            var data = _andpEquipmentEntities.UsocToCommandTranslations.FirstOrDefault(x => x.Id == id);

            if (data == null)
                throw new Exception("Usoc not found for id:" + id);

            data.Active = false;
            _andpEquipmentEntities.SaveChanges();
        }

        #endregion

        public IEnumerable<Company> RetrieveAllCompanies()
        {
            return _andpEquipmentEntities.Companies.AsNoTracking().ToList();
        }

        public int RetrieveCompanyIdByExternalCompanyId(string externalCompanyId)
        {
            var company = _andpEquipmentEntities.Companies.AsNoTracking().FirstOrDefault(p => p.ExternalCompanyId == externalCompanyId);
            if (company == null)
                throw new Exception("Company id not found for external company id:" + externalCompanyId);

            return company.Id;
        }

        public Company RetrieveCompanyByCompanyId(int companyId)
        {
            var company = _andpEquipmentEntities.Companies.AsNoTracking().FirstOrDefault(p => p.Id == companyId);
            if (company == null)
                throw new Exception("Company id not found for company id:" + companyId);

            return company;
        }

        public EquipmentSetup RetrieveEquipmentByEquipmentId(int equipmentId)
        {
            var es = _andpEquipmentEntities.EquipmentSetups.AsNoTracking().FirstOrDefault(p => p.Id == equipmentId);
            if (es == null)
                throw new Exception("Equipment settings not found for equipment id:" + equipmentId);

            return es;
        }

        public IEnumerable<EquipmentSetup> RetrieveEquipmentByCompanyId(int companyId)
        {
            var es = _andpEquipmentEntities.EquipmentSetups.AsNoTracking().Where(p => p.CompanyId == companyId).ToList();
            if (es == null)
                throw new Exception("Equipment not found for company id:" + companyId);

            return es;
        }

        public void Dispose()
        {
            _andpEquipmentEntities.Dispose();
        }
    }
}
