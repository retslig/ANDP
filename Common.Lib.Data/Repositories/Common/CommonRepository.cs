using System;
using System.Data.Entity;
using System.Linq;

namespace Common.Lib.Data.Repositories.Common
{
    public class CommonRepository : ICommonRepository, IDisposable
    {
        private readonly ICommon_Entities _iCommonEntities;

        public CommonRepository(ICommon_Entities iCommonEntities)
        {
            _iCommonEntities = iCommonEntities;
        }

        public Tenant RetrieveTenantById(Guid guid)
        {
            //Have to no 'AsNoTracking()' so that entities doesn't track the entity and throw an error 
            //if we decided to push back changes after we've mapped it back and forth from domain to dao.

            return _iCommonEntities.Tenants.AsNoTracking().FirstOrDefault(p => p.Guid == guid);
        }
        
        public void Dispose()
        {
            _iCommonEntities.Dispose();
        }
    }
}
