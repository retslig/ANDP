
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.TerminatingCallManagementV3;

namespace Common.ApMax
{
    public class TerminatingCallManagementV3Service
    {
        private TCMServiceClient _tcmService;
        private readonly LoginInformation _loginInformation;

        public TerminatingCallManagementV3Service(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://oasislap7:8731/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/
            _tcmService = new TCMServiceClient("WSHttpBinding_ITCMService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public TCMType RetrieveTcmSubscriber(string phoneNumber)
        {
            return _tcmService.GetTCMSubscriber(_loginInformation.LoginToken, phoneNumber);
        }

        public DNDContactListType[] RetrieveTcmSubscriberContacts(string subscriberAddress)
        {
            var dndContactListTypes = _tcmService.GetSubscribersContacts(_loginInformation.LoginToken, subscriberAddress);
            return dndContactListTypes;
        }

        public void AddTcmSubscriber(string phoneNumber, bool tcmEnabled, bool dndEnabled, SubscriberType subscriberType)
        {
            _tcmService.AddTCMSubscriber(_loginInformation.LoginToken, phoneNumber, tcmEnabled, dndEnabled, subscriberType);
        }

        public void DeleteTcmSubscriber(string phoneNumber)
        {
            _tcmService.DeleteTCMSubscriber(_loginInformation.LoginToken, phoneNumber);
        }

        public void SendDndFeature(string subscriberAddress, bool enableStatus)
        {
            _tcmService.DNDFeature(_loginInformation.LoginToken, subscriberAddress, enableStatus);
        }

        public void SetTcmFeature(string subscriberAddress, bool enableStatus)
        {
            _tcmService.TCMFeature(_loginInformation.LoginToken, subscriberAddress, enableStatus);
        }
    }
}
