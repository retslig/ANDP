using System.Collections.Generic;
using System.ServiceModel;
using Common.ApAdmin;
using Common.ApMax.Models;
using Common.Lib.Domain.Common.Models;
using Common.ServiceReport;

namespace Common.ApMax
{
    public class ApMaxCore
    {
        private readonly LoginInformation _loginInformation;
        public ServiceVersions Versions;
        private readonly ServiceReportClient _serviceReportV1;

        public ApMaxCore(EquipmentConnectionSetting settings)
        {
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            _loginInformation = apAdmin.LoginAdv(settings.CustomString1, settings.Username, settings.Password);

            //http://localhost:8731/Design_Time_Addresses/ServiceReportV1/ServiceReport/
            _serviceReportV1 = new ServiceReportClient("WSHttpBinding_IServiceReport", new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/ServiceReportV1/ServiceReport/"));
        }

        public Dictionary<Enums.ApmaxVersion, int> ReturnVersions()
        {
            Versions = _serviceReportV1.GetAPmaxServiceVersions(_loginInformation.LoginToken);

            var dictionary = new Dictionary<Enums.ApmaxVersion, int>
            {
                {Enums.ApmaxVersion.Voicemail, Versions.Voicemail},
                {Enums.ApmaxVersion.Iptv, Versions.Iptv},
                {Enums.ApmaxVersion.Subscriber, Versions.Subscriber},
                {Enums.ApmaxVersion.CallingName, Versions.CallingName}
            };

            return dictionary;
        }

        public ServiceVersions GetVersions()
        {
            return _serviceReportV1.GetAPmaxServiceVersions(_loginInformation.LoginToken);
        }
    }
}
