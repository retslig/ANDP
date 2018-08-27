using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class SubscriberTypeFixture
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
        public void Can_Map_Provisioning_API_SubscriberType_To_ApMax_SubscriberTypeV3()
        {
            //*** Arrange ***
            var subscriberType = new SubscriberType()
            {
                SubscriberName = "Bob Bobby",
                SubscriberDefaultPhoneNumber = "6055551234",
                PlacementType = PlacementType.PlacementType_None,
                BillingAccountNumber = "123456789",
                SubscriberTimezone = Timezone.CentralTime

            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.SubscriberV3.SubscriberType>(_commonMapper, subscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_SubscriberType_To_ApMax_SubscriberTypeV4()
        {
            //*** Arrange ***
            var provSubscriberType = new SubscriberType()
            {
                SubscriberName = "Bob Bobby",
                SubscriberDefaultPhoneNumber = "6055551234",
                PlacementType = PlacementType.PlacementType_None,
                SubscriberTimezone = Timezone.MountainTime

            };

            //*** Act ***
            var apmaxSubscriberTypeV4 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.SubscriberV4.SubscriberType>(_commonMapper, provSubscriberType);
            Assert.IsNotNull(apmaxSubscriberTypeV4);
        }

        [TestMethod]
        public void CommonSubscriberV3SubscriberType_To_SubscriberType()
        {
            //*** Arrange ***
            var subscriber = new Common.SubscriberV3.SubscriberType()
            {
                SubscriberName = "Bob Bobby",
                SubscriberDefaultPhoneNumber = "6055551234",
                PlacementType = Common.SubscriberV3.PlacementType_e.PlacementType_None,
                SubscriberTimezone = Common.SubscriberV3.Timezone_e.Alaska,
                LastUpdateTime = "20140730",
                BillingAccountNumber = "123456789"
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV3.SubscriberType, SubscriberType>(_commonMapper, subscriber);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void Can_MapApMax_SubscriberTypeV4_To_Provisioning_API_SubscriberType()
        {
            //*** Arrange ***
            var subscriber = new Common.SubscriberV4.SubscriberType()
            {
                SubscriberName = "Bob Bobby",
                SubscriberDefaultPhoneNumber = "6055551234",
                PlacementType = Common.SubscriberV4.PlacementType_e.PlacementType_None,
                BillingAccountNumber = "123456789"

            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV4.SubscriberType, SubscriberType>(_commonMapper, subscriber);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void SubscriberType_To_CommonIPTVServiceV3SubscriberType()
        {
            //*** Arrange ***
            var subscriber = new SubscriberType()
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.IPTVServiceV3.SubscriberType>(_commonMapper, subscriber);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }

        [TestMethod]
        public void SubscriberType_To_CommonIPTVServiceV7SubscriberType()
        {
            //*** Arrange ***
            var subscriber = new SubscriberType()
            {
            };

            //*** Act ***
            var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.IPTVServiceV7.SubscriberType>(_commonMapper, subscriber);
            Assert.IsNotNull(apmaxSubscriberTypeV3);
        }
    }
}
