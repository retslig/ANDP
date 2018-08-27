using System.Collections.Generic;
using System.Reflection;
using ANDP.Lib.Domain.Models;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class AddressInfoTypeFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(AddressInfoType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonSubscriberV3AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.SubscriberV3.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonSubscriberV4AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.SubscriberV4.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonVoicemailV3AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.VoicemailV3.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonVoicemailV4AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.VoicemailV4.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonVoicemailV5AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.VoicemailV5.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonIPTVServiceV3AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.IPTVServiceV3.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }

        [TestMethod]
        public void AddressInfoType_To_CommonIPTVServiceV7AddressInfoType()
        {
            //*** Arrange ***
            var accountType = new AddressInfoType()
            {
            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<AddressInfoType, Common.IPTVServiceV7.AddressInfoType>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }
    }
}
