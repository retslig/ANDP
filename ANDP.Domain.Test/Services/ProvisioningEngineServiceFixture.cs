using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Infrastructure;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ANDP.Domain.Test.Services
{
    [TestClass]
    public class ProvisioningEngineServiceFixture
    {
        private IUnityContainer _container;
        private IOrderRepository _iOrderRepository;
        private IEngineRepository _iEngineRepository;
        private IProvisioningEngineService _iProvisioningEngineService;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = BootStrapper.Initialize();
            _iProvisioningEngineService = _container.Resolve<IProvisioningEngineService>();
        }

        [TestMethod]
        public void Can_Provision_Order()
        {
            var orderStatus = _iProvisioningEngineService.ProvisionOrder(70, 54, true, true, false, "Unit Test");
            Assert.AreEqual(orderStatus.Id, 0);
        }

        [TestMethod]
        public void Can_Provision_Service()
        {
            var serviceStatus = _iProvisioningEngineService.ProvisionService(70, 54, 3, true, true, false, "Unit Test");
            Assert.AreEqual(serviceStatus.Id, 0);
        }
    }
}
