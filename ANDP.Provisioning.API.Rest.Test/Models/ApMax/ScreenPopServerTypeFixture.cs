using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ScreenPopServerTypeFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ScreenPopServerType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV3InternetAccessType() //ToDo: Need to update naming.
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopServerType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopServerType, Common.CallingNameV3.ScreenPopServerType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void InternetAccessType_To_CommonIPTVServiceV7InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopServerType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopServerType, Common.CallingNameV4.ScreenPopServerType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV3InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV3.ScreenPopServerType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV3.ScreenPopServerType, ScreenPopServerType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CommonIPTVServiceV7InternetAccessType_To_InternetAccessType()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV4.ScreenPopServerType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV4.ScreenPopServerType, ScreenPopServerType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }



    }
}
