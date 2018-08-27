using System;
using System.Collections.Generic;
using System.Linq;

namespace ANDP.Lib.Data.Repositories.Audit
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IANDP_Audit_Entities _iandpEngineEntities;

        public AuditRepository(IANDP_Audit_Entities iandpEngineEntities)
        {
            _iandpEngineEntities = iandpEngineEntities;
        }


        public void Dispose()
        {
            _iandpEngineEntities.Dispose();
        }

        public void CreateAuditRecords(AuditRecord auditRecord, string updatingUserId)
        {
            _iandpEngineEntities.AttachEntity(null, auditRecord, new[] { "Id" }, updatingUserId);
            _iandpEngineEntities.SaveChanges();
            _iandpEngineEntities.RefreshEntity(auditRecord);
        }

        public IEnumerable<AuditRecord> RetrieveAuditRecords(Guid runNumber, string recordKey, int equipmentSetupId, int companyId)
        {
            return _iandpEngineEntities.AuditRecords.Where(p =>
                p.RunNumber == runNumber
                && p.RecordKey == recordKey
                && p.EquipmentSetupId == equipmentSetupId
                && p.CompanyId == companyId
                ).ToList();
        }
    }
}
