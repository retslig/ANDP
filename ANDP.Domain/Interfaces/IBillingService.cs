
using System.Collections.Generic;
using ANDP.Lib.Domain.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IBillingService
    {

        List<BillingRecord> RetrieveBillingRecordsByAccountNumberRange(string externalCompanyId, string startExternalAccountId, string endExternalAccountId);
        List<BillingRecord> RetrieveBillingRecordsByPhoneRange(string externalCompanyId, int npa, int nxx, int startStation, int endStation);
    }
}
