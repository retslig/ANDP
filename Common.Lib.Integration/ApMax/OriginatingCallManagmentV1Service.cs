
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.OriginatingCallManagementV1;

namespace Common.ApMax
{
    public class OriginatingCallManagmentV1Service
    {

        private OCMServiceClient _ocmService;
        private readonly LoginInformation _loginInformation;


        public OriginatingCallManagmentV1Service(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/
            _ocmService = new OCMServiceClient("WSHttpBinding_IOCMService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public OCMSubcriberType GetSubscriberBySubGuid(string subscriberGuid)
        {
           return _ocmService.GetSubscriberBySubGuid(_loginInformation.LoginToken, subscriberGuid);
        }

        public void AddSubscriberRecord(OCMSubcriberType ocmSubcriberType, SubscriberType subscriberType)
        {
            _ocmService.AddSubscriberRecord(_loginInformation.LoginToken, ocmSubcriberType, subscriberType);
        }

        public void DeleteSubscriberRecord(string subscriberGuid)
        {
            _ocmService.DeleteSubscriberRecord(_loginInformation.LoginToken, subscriberGuid);
        }

    }
}
