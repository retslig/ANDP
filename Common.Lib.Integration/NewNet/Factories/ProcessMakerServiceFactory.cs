
using System;
using System.Collections.Generic;
using System.Reflection;
using ANDP.Lib.Domain.Factories;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;
using Common.NewNet.Services;
using Microsoft.Practices.Unity;

namespace Common.NewNet.Factories
{
    public class ProcessMakerServiceFactory
    {
        public static IUnityContainer Container { get; set; }

        public static IProcessMakerService Create(Guid tenantId, int equipmentId)
        {
            if (Container == null)
                throw new Exception("Unity Container Not Initialized.");

            var equipmentService = EquipmentServiceFactory.Create(tenantId);
            var settings = equipmentService.RetrieveEquipmentByEquipmentId(equipmentId).EquipmentConnectionSettings;

            if (settings == null)
                throw new Exception("Equipment record not found for this equipmentId:" + equipmentId);

            //var logger = Container.Resolve<ILogger>();
            //logger.WriteLogEntry(tenantId.ToString(), new List<object> { settings }, string.Format(MethodBase.GetCurrentMethod().Name + " in WebAPI."), LogLevelType.Info);
            
            var service = new Services.ProcessMakerService(settings);
            return service;
        }
    }
}
