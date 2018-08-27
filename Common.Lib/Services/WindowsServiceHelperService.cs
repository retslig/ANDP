
using System;
using System.ServiceProcess;

namespace Common.Lib.Services
{
    public static class WindowsServiceHelperService
    {
        public static ServiceControllerStatus RetrieveServiceStatus(string serviceName)
        {
            var sc = new ServiceController(serviceName);
            return sc.Status;
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }


        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                // ...
            }
        }


        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }
    }
}
