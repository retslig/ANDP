using System;
using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ServiceInfoTypeFixture
    {

        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(SubscriberType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void Can_Map_Provisioning_API_ServiceInfoType_To_ApMax_SubscriberTypeV3_ServiceInfoType()
        {
            //*** Arrange ***
            var provSubscriberType = new ServiceInfoType
            {
                ApSystemId = null,
                BillingServiceAddress = "111 my addres st Sioux Falls, SD 57111",
                BillingServiceID = "2",
                ServiceGuid = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.Iptv
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<ServiceInfoType, Common.SubscriberV3.ServiceInfoType>(_commonMapper, provSubscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_ServiceInfoType_To_ApMax_SubscriberTypeV4_ServiceInfoType()
        {
            //*** Arrange ***
            var provSubscriberType = new ServiceInfoType
            {
                ApSystemId = null,
                BillingServiceAddress = "111 my addres st Sioux Falls, SD 57111",
                BillingServiceID = "2",
                ServiceGuid = Guid.NewGuid().ToString(),
                ServiceType = ServiceType.Iptv
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<ServiceInfoType, Common.SubscriberV4.ServiceInfoType>(_commonMapper, provSubscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }


        [TestMethod]
        public void Can_Map_ApMax_SubscriberTypeV3_ServiceInfoType_To_Provisioning_API_ServiceInfoType()
        {
            //*** Arrange ***
            var subscriberType = new Common.SubscriberV3.ServiceInfoType
            {
                ApSystemId = null, 
                BillingServiceAddress = "111 my addres st Sioux Falls, SD 57111",
                BillingServiceID = "2",
                ServiceGuid = Guid.NewGuid().ToString(),
                ServiceType = Common.SubscriberV3.ServiceType_e.Iptv
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV3.ServiceInfoType, ServiceInfoType>(_commonMapper, subscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void Can_Map_ApMax_SubscriberTypeV4_ServiceInfoType_To_Provisioning_API_ServiceInfoType()
        {
            //*** Arrange ***
            var subscriberType = new Common.SubscriberV4.ServiceInfoType
            {
                ApSystemId = null,
                BillingServiceAddress = "111 my addres st Sioux Falls, SD 57111",
                BillingServiceID = "2",
                ServiceGuid = Guid.NewGuid().ToString(),
                ServiceType = Common.SubscriberV4.ServiceType_e.Iptv
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV4.ServiceInfoType, ServiceInfoType>(_commonMapper, subscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void ServiceInfoType_To_CommonIPTVServiceV3ServiceInfoType()
        {
            //*** Arrange ***
            var serviceInfoType = new ServiceInfoType
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<ServiceInfoType, Common.IPTVServiceV3.ServiceInfoType>(_commonMapper, serviceInfoType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void ServiceInfoType_To_CommonIPTVServiceV7ServiceInfoType()
        {
            //*** Arrange ***
            var serviceInfoType = new ServiceInfoType
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<ServiceInfoType, Common.IPTVServiceV7.ServiceInfoType>(_commonMapper, serviceInfoType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void CommonIPTVServiceV3ServiceInfoType_To_ServiceInfoType()
        {
            //*** Arrange ***
            var serviceInfoType = new Common.IPTVServiceV3.ServiceInfoType
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.ServiceInfoType, ServiceInfoType>(_commonMapper, serviceInfoType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void CommonIPTVServiceV7ServiceInfoType_To_ServiceInfoType()
        {
            //*** Arrange ***
            var serviceInfoType = new Common.IPTVServiceV7.ServiceInfoType
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.ServiceInfoType, ServiceInfoType>(_commonMapper, serviceInfoType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }
    }
}
