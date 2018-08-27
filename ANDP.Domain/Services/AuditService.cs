
using System;
using System.Collections.Generic;
using ANDP.Lib.Data.Repositories.Audit;
using ANDP.Lib.Domain.Interfaces;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _iAuditRepository;
        private readonly ICommonMapper _iCommonMapper;

        public AuditService(IAuditRepository auditRepository, ICommonMapper commonMapper)
        {
            _iAuditRepository = auditRepository;
            _iCommonMapper = commonMapper;
        }

        public void CreateAuditRecords(Models.AuditRecord auditRecord, string updatingUserId)
        {
            var daoAuditRecord = ObjectFactory.CreateInstanceAndMap<Models.AuditRecord, AuditRecord>(_iCommonMapper, auditRecord);
            _iAuditRepository.CreateAuditRecords(daoAuditRecord, updatingUserId);
        }

        public IEnumerable<Models.AuditRecord> RetrieveAuditRecords(Guid runNumber, string recordKey, int equipmentSetupId, int companyId)
        {
            var daoAuditRecords = _iAuditRepository.RetrieveAuditRecords(runNumber, recordKey, equipmentSetupId, companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<AuditRecord>, IEnumerable<Models.AuditRecord>>(_iCommonMapper, daoAuditRecords);
        }
    }
}
