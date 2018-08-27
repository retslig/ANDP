using System;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Services;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Interfaces;
using Microsoft.Practices.Unity;

namespace ANDP.Lib.Domain.Factories
{
    public static class ProvisioningEngineServiceFactory
    {
        public static IUnityContainer Container { get; set; }
        public static string ConnectionString { get; set; }

        public static IProvisioningEngineService Create(Guid tenantId)
        {
            if (Container == null)
                throw new Exception("Unity Container Not Initialized.");

            var logger = Container.Resolve<ILogger>();

            var iCommonRepository = new CommonRepository(new Common_Entities(ConnectionString));
            var tenant = iCommonRepository.RetrieveTenantById(tenantId);
            if (tenant == null)
                throw new Exception("Could not find schema for this tenantId:" + tenantId);

            var orderService = OrderServiceFactory.Create(tenantId);
            var engineService = EngineServiceFactory.Create(tenantId);
            IProvisioningEngineService service = new ProvisioningEngineService(engineService, orderService, logger, tenantId);
            return service;
        }
    }
}
