using System.Collections.Generic;
using ANDP.Lib.Infrastructure;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Lib.Tests.Services
{
    [TestClass]
    public class NLogWriterServiceFixture
    {
        static readonly IUnityContainer Container = BootStrapper.Initialize();
        readonly ILogger _logger = Container.Resolve<ILogger>();

        [TestMethod]
        public void Can_Write_Log_Entry()
        {
            //*** Arrange ***

            //*** Act ***
            var result = _logger.WriteLogEntry("Testing", LogLevelType.Info);

            //*** Assert ***
            Assert.IsNotNull(result);
        }
    }
}
