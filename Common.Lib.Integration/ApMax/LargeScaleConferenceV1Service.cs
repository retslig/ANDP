using System.ServiceModel;
using Common.ApAdmin;
using Common.LargeScaleConferenceV1;
using Common.Lib.Domain.Common.Models;

namespace Common.ApMax
{
    public class LargeScaleConferenceV1Service
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly LoginInformation _loginInformation;
        private readonly LargeScaleConferenceClient _largeScaleConferenceClient;

        public LargeScaleConferenceV1Service(EquipmentConnectionSetting setting)
        {
            _setting = setting;

            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/
            _largeScaleConferenceClient = new LargeScaleConferenceClient("WSHttpBinding_ILargeScaleConference", 
                new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/"));

            _loginInformation = apAdmin.LoginAdv(_setting.CustomString1, _setting.Username, _setting.Password);
        }

        public string InsertFreeConference(FreeConferenceType freeConferenceType, SubscriberType subscriberType, bool dedicated)
        {
            var response = _largeScaleConferenceClient.InsertFreeConference(_loginInformation.LoginToken, freeConferenceType, dedicated, subscriberType);
            return response;
        }

    }
}
