
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.OnDemandConferencingV3;

namespace Common.ApMax
{
    public class OnDemandConferencingV3Service
    {
        private readonly ODConferencingServiceClient _odConferencingService;
        private readonly LoginInformation _loginInformation;

        public OnDemandConferencingV3Service(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://oasislap7:8731/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/
            _odConferencingService = new ODConferencingServiceClient("WSHttpBinding_IODConferencingService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public void DeleteConferenceBySubAddress(string subscriberAddress)
        {
            _odConferencingService.DeleteConferenceBySubAddress(_loginInformation.LoginToken, subscriberAddress);
        }

        public void AddConferencingSub(ConferenceType conferenceType, SubscriberType subscriberType)
        {
            _odConferencingService.AddConferenceSub(_loginInformation.LoginToken, conferenceType, subscriberType);
        }

        internal ConferenceType RetrieveConferenceSubByAddress(string phoneNumber)
        {
           return  _odConferencingService.GetConferenceSubByAddress(_loginInformation.LoginToken, phoneNumber);
        }


    }
}
