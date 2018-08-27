using System;
using ANDP.Lib.Data.Repositories.Equipment;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Services;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Mapping;
using Microsoft.Practices.Unity;

namespace ANDP.Lib.Domain.Factories
{
    public static class EquipmentServiceFactory
    {
        public static IUnityContainer Container { get; set; }
        public static string ConnectionString { get; set; }

        public static IEquipmentService Create(Guid tenantId)
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

            var equipmentEntities = new ANDP_Equipment_Entities(ConnectionString, tenant.Schema);
            var equipmentRepository = new EquipmentRepository(equipmentEntities);
            IEquipmentService service = new EquipmentService(equipmentRepository, iCommonMapper);
            return service;
        }
    }
}
