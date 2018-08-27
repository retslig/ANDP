using System;
using System.Collections.Generic;

namespace ANDP.Lib.Data.Repositories.Audit
{
    public interface IAuditRepository : IDisposable
    {
        void CreateAuditRecords(AuditRecord auditRecord, string updatingUserId);
        IEnumerable<AuditRecord> RetrieveAuditRecords(Guid runNumber, string recordKey, int equipmentSetupId, int companyId);
    }
}
