using System;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Services;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Mapping;
using Microsoft.Practices.Unity;

namespace ANDP.Lib.Domain.Factories
{
    public static class EngineServiceFactory
    {
        public static IUnityContainer Container { get; set; }
        public static string ConnectionString { get; set; }

        public static IEngineService Create(Guid tenantId)
        {
            if (Container == null)
                throw new ArgumentNullException("Container", "Unity Container Not Initialized.");

            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("ConnectionString", "ConnectionString is empty.");

            var iCommonMapper = Container.Resolve<ICommonMapper>();
            var iCommonRepository = new CommonRepository(new Common_Entities(ConnectionString));
            var tenant = iCommonRepository.RetrieveTenantById(tenantId);
            iCommonRepository.Dispose();
            if (tenant == null)
                throw new Exception("Could not find schema for this tenantId:" + tenantId);

            var engineEntities = new ANDP_Engine_Entities(ConnectionString, tenant.Schema);
            var engineRepository = new EngineRepository(engineEntities);
            IEngineService service = new EngineService(engineRepository, iCommonMapper);
            return service;
        }
    }
}
