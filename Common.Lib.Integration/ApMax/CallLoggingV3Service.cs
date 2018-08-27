
using System.ServiceModel;
using Common.ApAdmin;
using Common.CallLoggingV3;
using Common.Lib.Domain.Common.Models;

namespace Common.ApMax
{
    public class CallLoggingV3Service
    {
        private readonly LoginInformation _loginInformation;
        private CLPServiceClient _callLoggingService;

        public CallLoggingV3Service(EquipmentConnectionSetting setting)
        {

            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/CallLoggingServiceV3/CLPService/
            _callLoggingService = new CLPServiceClient("WSHttpBinding_ICLPService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/CallLoggingServiceV3/CLPService/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        internal CallLoggingType[] FindCallLoggingSubscribers(string searchNumber)
        {
            return _callLoggingService.FindCallLoggingSubscribers(_loginInformation.LoginToken, searchNumber);
        }

        internal void AddClpSubscriberRecord(CallLoggingType callLoggingType, SubscriberType subscriberType)
        {
            _callLoggingService.AddClpSubscriberRecord(_loginInformation.LoginToken, callLoggingType, subscriberType);
        }

        internal void RemoveClpSubscriberRecord(string phoneNumber)
        {
            _callLoggingService.RemoveClpSubscriberRecord(_loginInformation.LoginToken, phoneNumber);
        }
    }
}
