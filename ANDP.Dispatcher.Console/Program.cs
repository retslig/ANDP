
using System;
using System.Configuration;
using ANDP.Lib.Domain.Factories;
using Topshelf;
using BootStrapper = ANDP.Dispatcher.Console.Infrastructure.BootStrapper;

namespace ANDP.Dispatcher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var tenantId = Guid.Parse(ConfigurationManager.AppSettings["tenantId"]);
            int companyId = int.Parse(ConfigurationManager.AppSettings["companyId"]);
            BootStrapper.Initialize();
            
            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();

                x.SetDescription(DispatcherServiceFactory.RetrieveServiceDisplayName(tenantId));
                x.SetDisplayName(DispatcherServiceFactory.RetrieveServiceDisplayName(tenantId));
                x.SetServiceName(DispatcherServiceFactory.RetrieveServiceName(tenantId));
                x.Service(factory =>
                {
                    return DispatcherServiceFactory.Create(tenantId, companyId);
                });
            });

            System.Console.ReadLine();
        }
    }
}
