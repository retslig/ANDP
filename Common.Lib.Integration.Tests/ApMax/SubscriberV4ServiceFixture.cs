using ANDP.Lib.Domain.Models;
using Common.ApMax;
using Common.Lib.Domain.Common.Models;
using Common.ServiceReport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Lib.Integration.Tests.ApMax
{
    [TestClass]
    public class SubscriberV4ServiceFixture
    {
        private SubscriberV4Service _subscriberV4Service;

        [TestInitialize]
        public void TestInitialize()
        {
            var equipmentConnectionSetting = new EquipmentConnectionSetting
            {
                CustomInt1 = null,
                CustomString1 = "9056",
                EquipmentId = 6,
                Ip = "127.0.0.1",
                Password = "udpadmin",
                Port = 8731,
                Url = null,
                Username = "udpadmin"
            };

            var serviceVersions = new ServiceVersions
            {
                Subscriber = 4
            };

            _subscriberV4Service = new SubscriberV4Service(equipmentConnectionSetting, serviceVersions);
        }

        [TestCleanup]
        public void TestCleanup()
        {


        }

        [TestMethod]
        public void Can_Retrieve_No_Subscriber_By_Default_Phone_Number()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var subscriberType = _subscriberV4Service.RetrieveSubscriberByDefaultPhoneNumber("345");

            //*** ASSERT ***
            Assert.IsNotNull(subscriberType);


        }


        


    }
}
