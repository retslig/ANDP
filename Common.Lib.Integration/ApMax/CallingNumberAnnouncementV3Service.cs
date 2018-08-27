
using System.Collections.Generic;
using System.ServiceModel;
using Common.ApAdmin;
using Common.CallingNumberAnnouncementV3;
using EquipmentConnectionSetting = Common.Lib.Domain.Common.Models.EquipmentConnectionSetting;

namespace Common.ApMax
{
    public class CallingNumberAnnouncementV3Service
    {
        private readonly CNAServiceClient _cnaService;
        private readonly LoginInformation _loginInformation;

        public CallingNumberAnnouncementV3Service(EquipmentConnectionSetting setting)
        {
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://oasislap7:8731/Design_Time_Addresses/CNAService/CNAServiceV3/
            _cnaService = new CNAServiceClient("WSHttpBinding_ICNAService", new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/CNAService/CNAServiceV3/"));

            _loginInformation = apAdmin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public CnaInfo RetrieveCnaAnnouncement(string fromPhoneNumber, string toPhoneNumber)
        {
            var cnaInfo = _cnaService.GetCnaNumber(_loginInformation.LoginToken, fromPhoneNumber);
            return cnaInfo;
        }

        public IEnumerable<CnaInfo> RetrieveAllCnaAnnouncements()
        {
            var cnaInfos = _cnaService.GetAllCnaAnnouncements(_loginInformation.LoginToken);
            return cnaInfos;
        }

        public void AddCnaNumber(CnaInfo cnaInfo)
        {
            //DateTime currentDate = DateTime.Now;
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //TimeSpan currentOffset = localZone.GetUtcOffset( currentDate );
            //DateTime.Now + currentOffset;
            //DateTime.UtcNow;
            //DateTime myDateTime = currentDate.ToLocalTime();
            //DateTime.Parse("2009-10-28T16:32:19.1217873-05:00");
            //currentDate.Add();
            //currentDate.GetDateTimeFormats
            //CnaInfo[] myCnaInfoArray = _callingNameService.GetAllCnaAnnouncements(myLoginInformation.LoginToken);
            //foreach (CnaInfo myCnaInfo in myCnaInfoArray)
            //{
            //    if (myCnaInfo.FromNumber == Object.FromNumber)
            //    {
            //        myCNAService.DeleteCnaNumber(myLoginInformation.LoginToken, myCnaInfo.FromNumber, myCnaInfo.ToNumber);
            //    }
            //}

            _cnaService.SetCnaNumber(_loginInformation.LoginToken, cnaInfo);
        }

        public void DeleteCnaAnnouncement(string fromPhoneNumber, string toPhoneNumber )
        {
            _cnaService.DeleteCnaNumber(_loginInformation.LoginToken, fromPhoneNumber, toPhoneNumber);
        }
    }





}
