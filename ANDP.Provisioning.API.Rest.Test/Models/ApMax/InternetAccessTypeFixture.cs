using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class InternetAccessTypeFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(InternetAccessType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV3InternetAccessType()
        {
            //*** Arrange ***
            var internetAccessType = new InternetAccessType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<InternetAccessType, Common.IPTVServiceV3.InternetAccessType>(_commonMapper, internetAccessType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV7InternetAccessType()
        {
            //*** Arrange ***
            var internetAccessType = new InternetAccessType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<InternetAccessType, Common.IPTVServiceV7.InternetAccessType>(_commonMapper, internetAccessType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV3InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var internetAccessType = new Common.IPTVServiceV3.InternetAccessType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.InternetAccessType, InternetAccessType>(_commonMapper, internetAccessType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV7InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var internetAccessType = new Common.IPTVServiceV7.InternetAccessType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.InternetAccessType, InternetAccessType>(_commonMapper, internetAccessType);
            Assert.IsNotNull(result);
        }

    }
}
