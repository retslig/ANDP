using System;
using System.Collections.Generic;
using ANDP.Lib.Domain.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IAuditService
    {
        void CreateAuditRecords(AuditRecord auditRecord, string updatingUserId);
        IEnumerable<AuditRecord> RetrieveAuditRecords(Guid runNumber, string recordKey, int equipmentSetupId, int companyId);
    }
}