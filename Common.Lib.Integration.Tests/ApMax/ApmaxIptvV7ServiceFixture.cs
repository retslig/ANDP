//using System.Collections.Generic;
//using ANDP.Lib.Domain.Models;
//using Common.ApMax;
//using Common.IPTVServiceV7;
//using Common.ServiceReport;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Common.Lib.Integration.Tests.ApMax
//{
//    [TestClass]
//    public class ApmaxIptvV7ServiceFixture
//    {
//        private ApmaxIptvV7Service _iptvV7Service;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            var equipmentConnectionSetting = new EquipmentConnectionSetting
//            {
//                CustomInt1 = null,
//                CustomString1 = "9056",
//                Id = 6,
//                Ip = "127.0.0.1",
//                Password = "udpadmin",
//                Port = 8731,
//                Url = null,
//                Username = "udpadmin"
//            };

//            var serviceVersions = new ServiceVersions
//            {
//                Subscriber = 4,
//                Iptv = 7
//            };

//            _iptvV7Service = new ApmaxIptvV7Service(equipmentConnectionSetting, serviceVersions);
//        }

//        [TestCleanup]
//        public void TestCleanup()
//        {


//        }

//        [TestMethod]
//        public void Can_Handle_Null_IptvAccountType_When_Populating_Missing_Package_Names()
//        {
//            //*** ARRANGE ***
//            List<IPTVAccountType> iptvAccountTypes = new List<IPTVAccountType>();
//            iptvAccountTypes = null;


//            //*** ACT ***
//            iptvAccountTypes = _iptvV7Service.FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);

//            //*** ASSERT ***
//            Assert.IsNull(iptvAccountTypes);


//        }



        

//    }
//}
