using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class SetTopBoxTypeFixtureFixture
    {

        private static ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(SetTopBoxType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }


        [TestMethod]
        public void One() //ToDo: Need to update the naming.
        {
            //*** Arrange ***
            var screenPopSubscriberType = new SetTopBoxType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<SetTopBoxType, Common.IPTVServiceV3.SetTopBoxType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Two()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new SetTopBoxType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<SetTopBoxType, Common.IPTVServiceV7.SetTopBoxType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Three()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.IPTVServiceV7.SetTopBoxType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.SetTopBoxType, SetTopBoxType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Four()
        {
            //*** Arrange ***
            var screenPopSubscriberType = new Common.IPTVServiceV7.SetTopBoxType
            {

            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.SetTopBoxType, SetTopBoxType>(_commonMapper, screenPopSubscriberType);
            Assert.IsNotNull(result);
        }



    }
}
