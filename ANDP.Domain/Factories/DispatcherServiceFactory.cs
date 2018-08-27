using System;
using System.Linq;
using ANDP.Lib.Domain.Services;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Interfaces;
using Microsoft.Practices.Unity;

namespace ANDP.Lib.Domain.Factories
{
    public static class DispatcherServiceFactory
    {
        public static IUnityContainer Container { get; set; }

        public static DispatcherService Create(Guid tenantId, int companyId)
        {
            if (Container == null)
                throw new Exception("Unity Container Not Initialized.");

            var logger = Container.Resolve<ILogger>();
            var service = new DispatcherService(tenantId, companyId, logger);
            return service;
        }

        public static string RetrieveServiceName(Guid tenantId)
        {
            if (Container == null)
                throw new Exception("Unity Container Not Initialized.");

            var commonRepo = Container.Resolve<ICommonRepository>();
            var tenant = commonRepo.RetrieveTenantById(tenantId);
            return new string(("ANDPDispatcherServiceFor" + tenant.Name).Where(Char.IsLetterOrDigit).ToArray());
        }

        public static string RetrieveServiceDisplayName(Guid tenantId)
        {
            if (Container == null)
                throw new Exception("Unity Container Not Initialized.");

            var commonRepo = Container.Resolve<ICommonRepository>();
            var tenant = commonRepo.RetrieveTenantById(tenantId);
            return "ANDP Dispatcher Service For " + tenant.Name;
        }
    }
}
