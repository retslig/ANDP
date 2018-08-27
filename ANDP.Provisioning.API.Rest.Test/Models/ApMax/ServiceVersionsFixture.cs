using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class ServiceVersionsFixture
    {
        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(ANDP.Provisioning.API.Rest.Models.ApMax.ServiceVersions)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void ServiceVersions_To_ServiceVersions()
        {
            //*** Arrange ***
            var accountType = new Common.ServiceReport.ServiceVersions()
            {

            };

            //*** Act ***
            var apmaxAccountType = ObjectFactory.CreateInstanceAndMap<Common.ServiceReport.ServiceVersions, ANDP.Provisioning.API.Rest.Models.ApMax.ServiceVersions>(_commonMapper, accountType);
            Assert.IsNotNull(apmaxAccountType);
        }
    }
}
