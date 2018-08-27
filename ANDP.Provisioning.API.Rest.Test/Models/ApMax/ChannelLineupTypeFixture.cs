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
    public class ChannelLineupTypeFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ChannelLineupType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Common_IPTVServiceV3_ChannelLineupType_To_ChannelLineupType()
        {
            //*** Arrange ***
            var mock = new Mock<Common.IPTVServiceV3.ChannelLineupType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.ChannelLineupType, ChannelLineupType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Common_IPTVServiceV7_ChannelLineupType_To_ChannelLineupType()
        {
            //*** Arrange ***
            var mock = new Mock<Common.IPTVServiceV7.ChannelLineupType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.ChannelLineupType, ChannelLineupType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChannelLineupType_To_Common_IPTVServiceV3_ChannelLineupType()
        {
            //*** Arrange ***
            var mock = new Mock<ChannelLineupType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ChannelLineupType, Common.IPTVServiceV3.ChannelLineupType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChannelLineupType_To_Common_IPTVServiceV7_ChannelLineupType()
        {
            //*** Arrange ***
            var mock = new Mock<ChannelLineupType>();
            mock.SetupAllProperties();

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ChannelLineupType, Common.IPTVServiceV7.ChannelLineupType>(_commonMapper, mock.Object);

            //*** Assert ***
            Assert.IsNotNull(result);
        }
    }
}
