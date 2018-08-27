using System;
using System.Collections.Generic;

namespace Common.Lib.Data.Repositories.Common
{
    public interface ICommonRepository
    {
        Tenant RetrieveTenantById(Guid guid);
        //IEnumerable<UserClaim> RetrieveUserClaimsByName(string userName);
        //bool RetrieveVerifyPassword(string userName, string md5HashedPassword);
    }
}
