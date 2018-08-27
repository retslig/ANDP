using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class CallingNameTypeFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(CallingNameType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void One() //ToDo: Need to update the naming.
        {
            //*** Arrange ***
            var callingNameType = new CallingNameType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<CallingNameType, Common.CallingNameV3.CallingNameType>(_commonMapper, callingNameType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Two()
        {
            //*** Arrange ***
            var callingNameType = new CallingNameType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<CallingNameType, Common.CallingNameV4.CallingNameType>(_commonMapper, callingNameType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Three()
        {
            //*** Arrange ***
            var callingNameType = new Common.CallingNameV3.CallingNameType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV3.CallingNameType, CallingNameType>(_commonMapper, callingNameType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Four()
        {
            //*** Arrange ***
            var callingNameType = new Common.CallingNameV4.CallingNameType()
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.CallingNameV4.CallingNameType, CallingNameType>(_commonMapper, callingNameType);
            Assert.IsNotNull(result);
        }



    }
}
