using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ScreenPopSubscriberTypeFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ScreenPopSubscriberType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV3InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopSubscriberType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopSubscriberType, Common.CallingNameV3.ScreenPopSubscriberType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV7InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopSubscriberType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopSubscriberType, Common.CallingNameV4.ScreenPopSubscriberType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV3InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV3.ScreenPopSubscriberType
            {
               
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV3.ScreenPopSubscriberType, ScreenPopSubscriberType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV7InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV4.ScreenPopSubscriberType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV4.ScreenPopSubscriberType, ScreenPopSubscriberType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

    }
}
