using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class NotificationInfoTypeFixture
    {

        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(NotificationInfoType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationInfoType_To_ApMax_VoicemailV3_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationInfoType
            {
                Address = "12234 my address",
                Center = 2,
                Enabled = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationInfoType, Common.VoicemailV3.NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationInfoType_To_ApMax_VoicemailV4_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationInfoType
            {
                Address = "12234 my address",
                Center = 2,
                Enabled = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationInfoType, Common.VoicemailV4.NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationInfoType_To_ApMax_VoicemailV5_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationInfoType
            {
                Address = "12234 my address",
                Center = 2,
                Enabled = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationInfoType, Common.VoicemailV5.NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV3_NotificationInfoType_To_Provisioning_API_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV3.NotificationInfoType
            {
               AddressField = "ssssss",
               CenterField = 2,
               EnabledField = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV3.NotificationInfoType, NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV4_NotificationInfoType_To_Provisioning_API_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV4.NotificationInfoType
            {
                AddressField = "ssssss",
                CenterField = 2,
                EnabledField = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV4.NotificationInfoType, NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV5_NotificationInfoType_To_Provisioning_API_NotificationInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV5.NotificationInfoType
            {
                AddressField = "ssssss",
                CenterField = 2,
                EnabledField = true
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV5.NotificationInfoType, NotificationInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }
    }
}
