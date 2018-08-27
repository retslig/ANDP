
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.UniveralCallManagementV3;

namespace Common.ApMax
{
    public class UniversalCallManagementV3
    {
        private UCMServiceClient _ucmService;
        private readonly LoginInformation _loginInformation;

        public UniversalCallManagementV3(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/UniveralCallManagementV3/UCMService/
            _ucmService = new UCMServiceClient("WSHttpBinding_IUCMService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/UniveralCallManagementV3/UCMService/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public UniversalCallMgrSubType GetUniversalCallManagerNumber(string phoneNumber)
        {
            return _ucmService.GetUniversalCallManagerNumber(_loginInformation.LoginToken, phoneNumber);
        }

        public void AddUniversalCallManagerSubscriber(UniversalCallMgrSubType universalCallMgrSubType, SubscriberType subscriberType)
        {
            _ucmService.AddUniversalCallManagerSubscriber(_loginInformation.LoginToken, universalCallMgrSubType, subscriberType);
        }

        public void DeleteUniversalCallManagerSubscriber(string subscriberAddress)
        {
            _ucmService.DeleteUniversalCallManagerSubscriber(_loginInformation.LoginToken, subscriberAddress);
        }

    }
}
