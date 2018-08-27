
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ANDP.Lib.Data.Repositories.Engine
{
    public class EngineRepository : IEngineRepository
    {
        private readonly IANDP_Engine_Entities _iandpEngineEntities;

        public EngineRepository(IANDP_Engine_Entities iandpEngineEntities)
        {
            _iandpEngineEntities = iandpEngineEntities;
        }

        public int RetrieveCompanyIdByExternalCompanyId(string externalCompanyId)
        {
            var company = _iandpEngineEntities.Companies.AsNoTracking().FirstOrDefault(p => p.ExternalCompanyId == externalCompanyId);
            if (company == null)
                throw new Exception("Cannot find this ExternalCompanyId: " + externalCompanyId + " Please make sure it is correct or that it is setup in the companies table.");

            return company.Id;
        }

        public ProvisioningEngineSetting RetrieveProvisioningEngineSetting(int companyId)
        {
            return _iandpEngineEntities.ProvisioningEngineSettings.AsNoTracking()
                .Include(p=>p.ProvisioningEngineItemActionTypesSettings)
                .Include(p=>p.ProvisioningEngineOrderOrServiceActionTypesSettings)
                .Include(p=>p.ProvisioningEngineSchedules)
                .FirstOrDefault(p => p.CompanyId == companyId);
        }

        public ProvisioningEngineSetting UpdateProvisioningEngineSettings(ProvisioningEngineSetting settings, string updatingUserId)
        {
            var data = _iandpEngineEntities.ProvisioningEngineSettings.AsNoTracking().FirstOrDefault(p => p.Id == settings.Id);

            settings.ModifiedByUser = updatingUserId;
            settings.DateModified = DateTime.Now;
            settings.Version = 0;

            if (data != null && data.Id > 0)
            {
                settings.Id = data.Id;
                settings.Version = data.Version;
                settings.CreatedByUser = data.CreatedByUser;
                settings.DateCreated = data.DateCreated;
            }
            else
            {
                settings.CreatedByUser = updatingUserId;
                settings.DateCreated = DateTime.Now;
            }

            _iandpEngineEntities.AttachEntity(null, settings, new[] { "Id" }, updatingUserId);
            _iandpEngineEntities.SaveChanges();
            _iandpEngineEntities.RefreshEntity(settings);
            return settings;
        }

        public void Dispose()
        {
            _iandpEngineEntities.Dispose();
        }
    }
}
