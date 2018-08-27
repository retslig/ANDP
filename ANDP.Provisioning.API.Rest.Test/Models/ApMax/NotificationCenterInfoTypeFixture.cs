using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class NotificationCenterInfoTypeFixture
    {

        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(NotificationCenterInfoType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationCenterInfoType_To_ApMax_VoicemailV3_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationCenterInfoType
            {
                Description = "ssssss"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationCenterInfoType, Common.VoicemailV3.NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationCenterInfoType_To_ApMax_VoicemailV4_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationCenterInfoType
            {
                Description = "ssssss"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationCenterInfoType, Common.VoicemailV4.NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_NotificationCenterInfoType_To_ApMax_VoicemailV5_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new NotificationCenterInfoType
            {
                Description = "ssssss"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<NotificationCenterInfoType, Common.VoicemailV5.NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV3_NotificationCenterInfoType_To_Provisioning_API_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV3.NotificationCenterInfoType
            {
               DescriptionField = "ssdsd"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV3.NotificationCenterInfoType, NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV4_NotificationCenterInfoType_To_Provisioning_API_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV4.NotificationCenterInfoType
            {
                DescriptionField = "ssdsd"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV4.NotificationCenterInfoType, NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_Map_ApMax_VoicemailV5_NotificationCenterInfoType_To_Provisioning_API_NotificationCenterInfoType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new Common.VoicemailV5.NotificationCenterInfoType
            {
                DescriptionField = "ssdsd"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.VoicemailV5.NotificationCenterInfoType, NotificationCenterInfoType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }
    }
}
