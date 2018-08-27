using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ScreenPopTypeFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ScreenPopType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void One() //ToDo: Need to update the naming.
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopType, Common.CallingNameV3.ScreenPopType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Two()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new ScreenPopType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<ScreenPopType, Common.CallingNameV4.ScreenPopType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Three()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV3.ScreenPopType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV3.ScreenPopType, ScreenPopType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Four()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.CallingNameV4.ScreenPopType()
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV4.ScreenPopType, ScreenPopType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }



    }
}
