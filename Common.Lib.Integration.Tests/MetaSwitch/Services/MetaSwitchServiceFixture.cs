
using System.Configuration;
using System.Data.SqlClient;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Interfaces;
using Common.Lib.Services;
using Common.MetaSwitch;
using Common.MetaSwitch.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Lib.Integration.Tests.MetaSwitch.Services
{
    [TestClass]
    public class MetaSwitchServiceFixture
    {

        private MetaSwitchService _metaSwitchService;

        [TestInitialize]
        public void TestInitialize()
        {
            ILogger _logger = new NLogWriterService(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ANDP_Entities"].ConnectionString));
            _metaSwitchService = new MetaSwitchService(new EquipmentConnectionSetting(), _logger);

        }

        [TestCleanup]
        public void TestCleanup()
        {


        }
    }
}
