using System.Collections.Generic;
using ANDP.Lib.Domain.Models;
using Common.ApMax;
using Common.IPTVServiceV7;
using Common.ServiceReport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Lib.Integration.Tests.ApMax
{
    [TestClass]
    public class ApMaxHelperFixture
    {
        private ApMaxHelper _apMaxHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _apMaxHelper = new ApMaxHelper();
        }

        [TestCleanup]
        public void TestCleanup()
        {


        }

        [TestMethod]
        public void Can_Handle_Null_IptvAccountType_When_Populating_Missing_Package_Names()
        {
            //*** ARRANGE ***
            List<IPTVAccountType> iptvAccountTypes = new List<IPTVAccountType>();
            iptvAccountTypes = null;


            //*** ACT ***
            iptvAccountTypes = _apMaxHelper.FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);

            //*** ASSERT ***
            Assert.IsNull(iptvAccountTypes);


        }

        [TestMethod]
        public void Can_Handle_Null_IptvAccountType_When_Populating_Missing_Package_Names_2()
        {
            //*** ARRANGE ***
            List<IPTVAccountType> iptvAccountTypes = new List<IPTVAccountType>();
            iptvAccountTypes.Add(new IPTVAccountType{ChannelPackageList = null});


            //*** ACT ***
            iptvAccountTypes = _apMaxHelper.FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);

            //*** ASSERT ***
            //Assert.IsNull(iptvAccountTypes);

        }

        [TestMethod]
        public void Can_Handle_Null_IptvAccountType_When_Populating_Missing_Package_Names_3()
        {
            //*** ARRANGE ***
            List<IPTVAccountType> iptvAccountTypes = new List<IPTVAccountType>();
            iptvAccountTypes.Add(null);


            //*** ACT ***
            iptvAccountTypes = _apMaxHelper.FindAndPopulateMissingPackageNamesInIptvAccountTypeClass(iptvAccountTypes);

            //*** ASSERT ***
            //Assert.IsNull(iptvAccountTypes);

        }

    }
}
