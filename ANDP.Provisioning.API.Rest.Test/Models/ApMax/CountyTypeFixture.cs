using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class CountyTypeFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(CountyType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Common_IPTVServiceV3_CountyType_To_CountyType()
        {
            //*** Arrange ***
            var mock = new Mock<Common.IPTVServiceV3.CountyType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.CountyType, CountyType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Common_IPTVServiceV7_CountyType_To_CountyType()
        {
            //*** Arrange ***
            var mock = new Mock<Common.IPTVServiceV7.CountyType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.CountyType, CountyType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CountyType_To_Common_IPTVServiceV3_CountyType()
        {
            //*** Arrange ***
            var mock = new Mock<CountyType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<CountyType, Common.IPTVServiceV3.CountyType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CountyType_To_Common_IPTVServiceV7_CountyType()
        {
            //*** Arrange ***
            var mock = new Mock<CountyType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<CountyType, Common.IPTVServiceV7.CountyType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }
    }
}
