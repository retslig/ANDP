using System.Collections.Generic;
using System.Reflection;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Provisioning.API.Rest.Test.Models.ApMax
{
    [TestClass]
    public class PackageTypeFixture
    {

        private ICommonMapper _commonMapper;

        [TestInitialize]
        public void Initialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(PackageType)).GetName().Name,
                        Namespace = null
                    }
            };

            _commonMapper = new CommonMapper(settings);
        }

        [TestMethod]
        public void Can_Map_Provisioning_API_PackageType_To_ApMax_VoicemailV3_PackageType()
        {
            //*** Arrange ***
            var notificationInfoTypes = new PackageType
            {
               Description = "sssssssss"
            };

            //*** Act ***
            var result = ObjectFactory.CreateInstanceAndMap<PackageType, Common.VoicemailV3.PackageType>(_commonMapper, notificationInfoTypes);
            Assert.IsNotNull(result);
        }
    }
}
