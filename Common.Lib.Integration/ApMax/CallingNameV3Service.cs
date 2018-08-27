
using System.Collections.Generic;
using System.ServiceModel;
using Common.ApAdmin;
using Common.CallingNameV3;
using EquipmentConnectionSetting = Common.Lib.Domain.Common.Models.EquipmentConnectionSetting;

namespace Common.ApMax
{
    public class CallingNameV3Service
    {
        private readonly CallingNameServiceClient _callingNameService;
        private readonly LoginInformation _loginInformation;


        public CallingNameV3Service(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://oasislap7:8731/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/
            _callingNameService = new CallingNameServiceClient("WSHttpBinding_ICallingNameService",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public void AddCallerName(string dn, string callerName, string presentation)
        {
            _callingNameService.InsertCallingEntry(_loginInformation.LoginToken, dn, 0, presentation, callerName);
        }

        public void DeleteCallerName(string dn)
        {
            _callingNameService.DeleteCallingEntry(_loginInformation.LoginToken, dn, 0);
        }

        public void ModifyCallerName(string dn, string callerName, string presentation)
        {
            DeleteCallerName(dn);
            AddCallerName(dn, callerName, presentation);
        }

        /// <summary>
        /// Reassigns Caller Name.
        /// </summary>
        /// <param name="dn">The Phone Number.</param>
        /// <param name="oldDn">The old Phone Number.</param>
        public void ReassignCallerName(string dn, string oldDn)
        {
            var myCallingNameTypeArray = _callingNameService.GetCallingEntries(_loginInformation.LoginToken, oldDn);
            foreach (var myCallingNameType in myCallingNameTypeArray)
            {

                _callingNameService.InsertCallingEntry(_loginInformation.LoginToken, dn, 0, myCallingNameType.Presentation, myCallingNameType.CName);
            }

            _callingNameService.DeleteCallingEntry(_loginInformation.LoginToken, oldDn, 0);
        }

        /// <summary>
        /// Retrieves Caller Name.
        /// </summary>
        /// <param name="dn">The phone number.</param>
        /// <returns></returns>
        public IEnumerable<CallingNameType> RetrieveCallerName(string dn)
        {
            var callingNameTypes = _callingNameService.GetCallingEntries(_loginInformation.LoginToken, dn);
            return callingNameTypes;
        }

        public void AddScreenPop(ScreenPopSubscriberType screenPopSubscriberType, string npaNxx, string description)
        {
            _callingNameService.InsertScreenPopEntry(_loginInformation.LoginToken, npaNxx, description);
            _callingNameService.InsertScreenPopSubscriber(_loginInformation.LoginToken, screenPopSubscriberType.SubscriberPhoneNumberField, true);
        }

        public void DeleteScreenPop(string calledNumber, string npaNxx)
        {
            _callingNameService.DeleteScreenPopEntry(_loginInformation.LoginToken, npaNxx);
            _callingNameService.DeleteScreenPopSubscriber(_loginInformation.LoginToken, calledNumber);
        }

        public IEnumerable<ScreenPopType> RetrieveAllScreenPopEntries()
        {
            var myScreenPopType = _callingNameService.GetAllScreenPopEntries(_loginInformation.LoginToken);
            return myScreenPopType;
        }

    }
}
