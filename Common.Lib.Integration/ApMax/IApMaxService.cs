using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ServiceReport;

namespace Common.Lib.Integration
{
    public interface IApMaxService
    {
        ServiceVersions Versions { get; }

        #region ************************** Subscriber Management *******************************
        string RetrieveSubscriber(string phoneNumber);
        string RetrieveSubscriberServices(string sPhoneNumber);
        void DeleteSubscriberByGuid(string sGuid);
        void DeleteSubscriberByPhoneNumber(string sPhoneNumber);
        void ReassignSubscriber(string sNewPhoneNumber, string sOldPhoneNumber);
        void CreateOrUpdateSubscriber(string sSubscriberTypeUnformatedXml, string sInternetAccessTypeUnformatedXml);
        #endregion

        #region ************************** Voicemail Management *******************************
        string RetrieveVoicemail(string phoneNumber);
        //void DeleteVoicemail(string sGuid);
        void CreateVoicemail(string phoneNumber, string mailBoxDescription, string vmPackageName,
            string subscriberName, string mailBoxType, string notificationCenter, string billingAccountNumber);

        #endregion
    }
}
