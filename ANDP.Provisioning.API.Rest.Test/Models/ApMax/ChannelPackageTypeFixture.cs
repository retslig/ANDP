using System;
using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ChannelPackageTypeFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ChannelPackageType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_ChannelPackageType_To_ApMax_IPTV_V3_ChannelPackageType()
        {
            //*** Arrange ***
            var channelPackageType = new ChannelPackageType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
            };

            //*** Act ***
            var iptvChannelPackageType = ObjectFactory.CreateInstanceAndMap<ChannelPackageType, Common.IPTVServiceV3.ChannelPackageType>(_commonMapper, channelPackageType);
            Assert.IsNotNull(iptvChannelPackageType);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_ChannelPackageType_To_ApMax_IPTV_V7_ChannelPackageType()
        {
            //*** Arrange ***
            var channelPackageType = new ChannelPackageType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
            };

            //*** Act ***
            var iptvChannelPackageType = ObjectFactory.CreateInstanceAndMap<ChannelPackageType, Common.IPTVServiceV7.ChannelPackageType>(_commonMapper, channelPackageType);
            Assert.IsNotNull(iptvChannelPackageType);
        }

        [TestMethod]
        public void Can_Map_ApMax_IPTV_V3_ChannelPackageType_To_Provisioning_API_ChannelPackageType()
        {
            //*** Arrange ***
            var channelPackageType = new Common.IPTVServiceV3.ChannelPackageType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
            };

            //*** Act ***
            var iptvChannelPackageType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.ChannelPackageType, ChannelPackageType>(_commonMapper, channelPackageType);
            Assert.IsNotNull(iptvChannelPackageType);
        }

        [TestMethod]
        public void Can_Map_ApMax_IPTV_V7_ChannelPackageType_To_Provisioning_API_ChannelPackageType()
        {
            //*** Arrange ***
            var channelPackageType = new Common.IPTVServiceV7.ChannelPackageType()
            {
                //Just testing that the automapper verification passes and that everything setup. As of right now not interested in verifying values.
                PackageID = Guid.NewGuid().ToString(),
                PackageName = "PackageName"
            };

            //*** Act ***
            var iptvChannelPackageType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.ChannelPackageType, ChannelPackageType>(_commonMapper, channelPackageType);

            //*** Assert ***
            Assert.IsNotNull(iptvChannelPackageType);
            Assert.IsNotNull(iptvChannelPackageType.PackageId);
            Assert.AreEqual("PackageName", iptvChannelPackageType.PackageName);
        }
    }
}
