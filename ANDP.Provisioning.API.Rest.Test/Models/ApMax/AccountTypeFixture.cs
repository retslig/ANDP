using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class AccountTypeFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(IptvAccountType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_AccountType_To_ApMax_IPTVAccountTypeV3()
        {
            //*** Arrange ***
            var accountType = new IptvAccountType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
                PurchasePin = "12345",
                RatingPin = "54321"
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV3.IPTVAccountType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_AccountType_To_ApMax_IPTVAccountTypeV7()
        {
            //*** Arrange ***
            var accountType = new IptvAccountType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
                PurchasePin = "12345",
                RatingPin = "54321"
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV7.IPTVAccountType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void Can_Map_ApMax_IPTVAccountTypeV3_To_Provisioning_API_AccountType()
        {
            //*** Arrange ***
            var accountType = new Common.IPTVServiceV3.IPTVAccountType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
                PurchasePIN = "12345",
                RatingPIN = "54321"
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.IPTVAccountType, IptvAccountType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void Can_Map_ApMax_IPTVAccountTypeV7_To_Provisioning_API_AccountType()
        {
            //*** Arrange ***
            var accountType = new Common.IPTVServiceV7.IPTVAccountType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
                PurchasePIN = "12345",
                RatingPIN = "54321"
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.IPTVAccountType, IptvAccountType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }
    }
}
