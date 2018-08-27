using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Data.Repositories.Equipment;
using Common.ApAdmin;
using Common.ServiceReport;

namespace Common.Lib.Integration
{
    public class ApMaxService : IApMaxService
    {
        private ApAdminClient _apAdmin;
        private LoginInformation _loginInformation;
        public ServiceVersions Versions { get; private set; }
        private ServiceReportClient _serviceReportV1;

        private object _voiceMailService;
        private object _voiceMailBoxType;
        private object _voicemailInternetAccessType;
        private object _mailBoxType;
        private object _notificationInfoType;
        private object _childInfoType;
        private object genSubInfoType;
        private object genNotificationCenterTypeEnum;
        private object _voiceMailServiceAddressTypeEnum;
        private Type _notificationInfoTypeType;

        private object _subscriberService;
        private object _subscriberSubType;
        private object _subscriberInternetAccessType;
        private object genSubscriberType;
        private object genTimezone;
        private object _placementType;

        private VoicemailV5.IVoicemail IVoiceMailBoxService;

        private object _cnaService;
        private object _cnaInfo;


        private object _callingNameService;

        private object _iptvService;

        private object _iptvSubscriberType;

        private object _iptvAccountType;

        //These do not have multiple versions. Yet...
        //Will make objects and use reflection if and when they get multiple version.

        //private TCMServiceClient myTCMServiceV3;
        private object _tcmService;

        //private LargeScaleConferenceClient myLargeScaleConferenceV1;

        //private WirelessOtaClient myWirelessOTAServiceV1;

        //private ODConferencingServiceClient myODConferencingServiceV3;

        //private UCMServiceClient myUCMServiceV3;

        //private CLPServiceClient myCLPServiceV3;

        //private OCMServiceClient myOCMServiceV1;

        //private LocalNumberPortabilityClient myLocalNumberPortabilityServiceV1;

        public ApMaxService(EquipmentConnectionSetting settings)
        {
            _loginInformation = new LoginInformation();
            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            _apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin",
                new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/ServiceReportV1/ServiceReport/
            _serviceReportV1 = new ServiceReportClient("WSHttpBinding_IServiceReport",
                new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/ServiceReportV1/ServiceReport/"));

            ////http://localhost:8731/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/
            //myOCMServiceV1 = new OCMServiceClient("WSHttpBinding_IOCMService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/"));

            ////http://localhost:8731/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/
            //myLocalNumberPortabilityServiceV1 = new LocalNumberPortabilityClient("WSHttpBinding_ILocalNumberPortability",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/"));

            ////http://localhost:8731/Design_Time_Addresses/WirelessOtaV1/WirelessOta/
            //myWirelessOTAServiceV1 = new WirelessOtaClient("WSHttpBinding_IWirelessOta",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/WirelessOtaV1/WirelessOta/"));

            ////http://localhost:8731/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/
            //myLargeScaleConferenceV1 = new LargeScaleConferenceClient("WSHttpBinding_ILargeScaleConference",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/"));

            switch (Versions.Iptv)
            {
                case 3:
                    //http://localhost:8731/Design_Time_Addresses/IPTVServiceV3/IPTVService/
                    _iptvService = new IPTVServiceV3.IPTVServiceClient("WSHttpBinding_IIPTVService",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/IPTVServiceV3/IPTVService/"));

                    _iptvSubscriberType = new IPTVServiceV3.SubscriberType();
                    _iptvAccountType = new IPTVServiceV3.IPTVAccountType();
                    break;
                case 7:
                    //http://localhost:8731/Design_Time_Addresses/IPTVServiceV7/IPTVService/
                    _iptvService = new IPTVServiceV7.IPTVServiceClient("WSHttpBinding_IIPTVService",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/IPTVServiceV7/IPTVService/"));
                    
                    _iptvSubscriberType = new IPTVServiceV7.SubscriberType();
                    _iptvAccountType = new IPTVServiceV7.IPTVAccountType();
                    break;
            }

            ////http://localhost:8731/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/
            //myODConferencingServiceV3 = new ODConferencingServiceClient("WSHttpBinding_IODConferencingService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/"));

            ////http://localhost:8731/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/
            //myTCMServiceV3 = new TCMServiceClient("WSHttpBinding_ITCMService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/"));

            ////http://localhost:8731/Design_Time_Addresses/CNAService/CNAServiceV3/
            //_cnaService = new CNAServiceClient("WSHttpBinding_ICNAService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/CNAService/CNAServiceV3/"));

            //_cnaInfo = new CnaInfo();

            ////http://localhost:8731/Design_Time_Addresses/CallLoggingServiceV3/CLPService/
            //myCLPServiceV3 = new CLPServiceClient("WSHttpBinding_ICLPService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/CallLoggingServiceV3/CLPService/"));

            ////http://localhost:8731/Design_Time_Addresses/UniveralCallManagementV3/UCMService/
            //myUCMServiceV3 = new UCMServiceClient("WSHttpBinding_IUCMService",
            //    new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/UniveralCallManagementV3/UCMService/"));

            //switch (_versions.CallingName)
            //{
            //    case 3:
            //        //http://localhost:8731/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/
            //        _callingNameService = new CallingNameServiceClientV3("WSHttpBinding_ICallingNameService",
            //            new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/"));
            //        break;
            //    case 4:
            //        //http://localhost:8731/Design_Time_Addresses/CallingNameService/CallingNameServiceV4/
            //        _callingNameService = new CallingNameServiceClientV4("WSHttpBinding_ICallingNameService1",
            //            new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/CallingNameService/CallingNameServiceV4/"));
            //        break;
            //}

            //_apAdmin.Login(systemKey, userName, passWord);
            _loginInformation = _apAdmin.LoginAdv(settings.CustomString1, settings.Username, settings.Password);
            Versions = _serviceReportV1.GetAPmaxServiceVersions(_loginInformation.LoginToken);

            switch (Versions.Subscriber)
            {
                case 3:
                    //http://localhost:8731/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/
                    _subscriberService = new SubscriberV3.SubscriberServiceClient("WSHttpBinding_ISubscriberService3",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/"));

                    _subscriberSubType = new SubscriberV3.SubscriberType();
                    _subscriberInternetAccessType = new SubscriberV3.InternetAccessType();
                    break;
                case 4:
                    //http://localhost:8731/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4
                    _subscriberService = new SubscriberV4.SubscriberServiceClient("WSHttpBinding_ISubscriberService4",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4/"));

                    _subscriberSubType = new SubscriberV4.SubscriberType();
                    _subscriberInternetAccessType = new SubscriberV4.InternetAccessType();
                    break;
            }

            switch (Versions.Voicemail)
            {
                case 3:
                    //http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV3/
                    _voiceMailService = new VoicemailV3.VoicemailClient("WSHttpBinding_IVoicemail3",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/VoicemailService/VoicemailV3/"));

                    _voiceMailBoxType = new VoicemailV3.VoiceMailBoxType();
                    _mailBoxType = new VoicemailV3.MailboxType();
                    genSubInfoType = new VoicemailV3.SubInfoType();
                    _notificationInfoType = new VoicemailV3.NotificationInfoType();
                    _voicemailInternetAccessType = new VoicemailV3.InternetAccessType();
                    _childInfoType = new VoicemailV3.ChildInfoType();
                    //genPackageType = new VoicemailV3.PackageType();  
                    genSubscriberType = new VoicemailV3.SubscriberType();
                    genTimezone = VoicemailV3.Timezone_e.ApDefault;
                    _placementType = VoicemailV3.PlacementType_e.PlacementType_None;
                    genNotificationCenterTypeEnum = VoicemailV3.NotificationCenterTypeEnum.typeEmail;
                    _voiceMailServiceAddressTypeEnum = VoicemailV3.AddressType.AddressTypeMailboxNumber;

                    _notificationInfoTypeType = typeof(VoicemailV3.NotificationInfoType);
                    break;
                case 4:
                    //http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV4/
                    _voiceMailService = new VoicemailV4.VoicemailClient("WSHttpBinding_IVoicemail4",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/VoicemailService/VoicemailV4/"));

                    _voiceMailBoxType = new VoicemailV4.VoiceMailBoxType();
                    _mailBoxType = new VoicemailV4.MailboxType();
                    genSubInfoType = new VoicemailV4.SubInfoType();
                    _notificationInfoType = new VoicemailV4.NotificationInfoType();
                    _voicemailInternetAccessType = new VoicemailV4.InternetAccessType();
                    _childInfoType = new VoicemailV4.ChildInfoType();
                    //genPackageType = new VoiceMailServiceV4.PackageType();
                    genSubscriberType = new VoicemailV4.SubscriberType();
                    genTimezone = VoicemailV4.Timezone_e.ApDefault;
                    _placementType = VoicemailV4.PlacementType_e.PlacementType_None;
                    genNotificationCenterTypeEnum = VoicemailV4.NotificationCenterTypeEnum.typeEmail;
                    _voiceMailServiceAddressTypeEnum = VoicemailV4.AddressType.AddressTypeMailboxNumber;

                    _notificationInfoTypeType = typeof(VoicemailV4.NotificationInfoType);
                    break;
                case 5:
                    //http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV5/
                    //IVoiceMailBoxService = new VoicemailV5.VoicemailClient("WSHttpBinding_IVoicemail5",
                     //   new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/VoicemailService/VoicemailV5/"));

                    _voiceMailService = new VoicemailV5.VoicemailClient("WSHttpBinding_IVoicemail5",
                        new EndpointAddress("http://" + settings.Ip + ":" + settings.Port + "/Design_Time_Addresses/VoicemailService/VoicemailV5/"));

                    _voiceMailBoxType = new VoicemailV5.VoiceMailBoxType();
                    _mailBoxType = new VoicemailV5.MailboxType();
                    genSubInfoType = new VoicemailV5.SubInfoType();
                    _notificationInfoType = new VoicemailV5.NotificationInfoType();
                    _voicemailInternetAccessType = new VoicemailV5.InternetAccessType();
                    _childInfoType = new VoicemailV5.ChildInfoType();
                    //genPackageType = new VoiceMailServiceV5.PackageType();
                    genSubscriberType = new VoicemailV5.SubscriberType();
                    genTimezone = VoicemailV5.Timezone_e.ApDefault;
                    _placementType = VoicemailV5.PlacementType_e.PlacementType_None;
                    genNotificationCenterTypeEnum = VoicemailV5.NotificationCenterTypeEnum.typeEmail;
                    _voiceMailServiceAddressTypeEnum = VoicemailV5.AddressType.AddressTypeMailboxNumber;

                    _notificationInfoTypeType = typeof(VoicemailV5.NotificationInfoType);
                    break;
            }
        }

        #region ************************** Subscriber Management *******************************

        public string RetrieveSubscriber(string phoneNumber)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            object result = _subscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, phoneNumber });

            if (result == null)
                return "A subscriber with the default phone number does not exist." + Environment.NewLine;

            return result.SerializeObjectToString();
        }

        public string RetrieveSubscriberServices(string sPhoneNumber)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            object result = _subscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, sPhoneNumber });

            if (result == null)
                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

            //If this is not null mailbox is present so we will delete.
            PropertyInfo pi = result.GetType().GetProperty("SubscriberGuid");
            string id = pi.GetValue(result, null).ToString();

            //mySubscriberServiceV4.GetSubscriberServices(_loginInformation.LoginToken, mySubscriberType.SubscriberGuid);
            result = _subscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, id });
            //now cast as array so we can loop through using the foreach loop.
            var _subscriberServicesArray = result as Array;

            //If there are no services than we can delete else throw error.
            if (_subscriberServicesArray != null)
            {
                if (_subscriberServicesArray.Length > 0)
                    return result.SerializeObjectToString() + Environment.NewLine;
            }

            return "There are no services on this subscriber.";
        }

        public void DeleteSubscriberByGuid(string sGuid)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            //mySubscriberServiceV4.GetSubscriberServices(_loginInformation.LoginToken, mySubscriberType.SubscriberGuid);
            var result = _subscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, sGuid });
            //now cast as array so we can loop through using the foreach loop.
            var array = result as Array;

            //If there are no services than we can delete else throw error.
            if (array != null)
            {
                if (array.Length > 0)
                    throw new Exception("This subscriber still has services connect. " + Environment.NewLine +
                        result.SerializeObjectToString() + Environment.NewLine);
            }

            var pip = _subscriberSubType.GetType().GetProperty("SubscriberGuid");
            pip.SetValue(_subscriberSubType, sGuid, null);
            pip = _subscriberSubType.GetType().GetProperty("PlacementType");
            pip.SetValue(_subscriberSubType, _placementType, null);

            var objParms = new[] { _loginInformation.LoginToken, _subscriberSubType };
            _subscriberService.GetType().InvokeMember("RemoveSubscriberProv", BindingFlags.InvokeMethod, null, _subscriberService, objParms);
        }

        public void DeleteSubscriberByPhoneNumber(string sPhoneNumber)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            //mySubscriberServiceV4.GetSubscribersByBillingAccountNumberProv(_loginInformation.LoginToken, accnm);
            object result = _subscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, sPhoneNumber });

            if (result == null)
                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

            //If this is not null mailbox is present so we will delete.
            PropertyInfo pi = result.GetType().GetProperty("SubscriberGuid");
            string id = pi.GetValue(result, null).ToString();

            //mySubscriberServiceV4.GetSubscriberServices(_loginInformation.LoginToken, mySubscriberType.SubscriberGuid);
            result = _subscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, id });
            //now cast as array so we can loop through using the foreach loop.
            var array = result as Array;

            //If there are no services than we can delete else throw error.
            if (array != null)
            {
                if (array.Length > 0)
                    throw new Exception("This subscriber still has services connect. " + Environment.NewLine +
                        result.SerializeObjectToString() + Environment.NewLine);
            }

            var pip = _subscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            pip.SetValue(_subscriberSubType, sPhoneNumber, null);
            pip = _subscriberSubType.GetType().GetProperty("PlacementType");
            pip.SetValue(_subscriberSubType, _placementType, null);

            var objParms = new[] { _loginInformation.LoginToken, _subscriberSubType };
            _subscriberService.GetType().InvokeMember("RemoveSubscriberProv", BindingFlags.InvokeMethod, null, _subscriberService, objParms);
        }

        public void ReassignSubscriber(string sNewPhoneNumber, string sOldPhoneNumber)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            //Attempts to changethe defaultsubscriberPhonenumber.
            object result = _subscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, sOldPhoneNumber });

            if (result == null)
                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

            PropertyInfo pi = result.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            pi.SetValue(result, sNewPhoneNumber, null);

            //mySubscriberServiceV3.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, mySubscriberType);
            _subscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new[] { _loginInformation.LoginToken, result });
        }

        public void CreateOrUpdateSubscriber(string sSubscriberTypeUnformatedXml, string sInternetAccessTypeUnformatedXml)
        {
            if (Versions.Subscriber < 1)
                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

            if (_subscriberService == null)
                throw new Exception("This version (" + Versions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

            //This will take string xml and deserialize to an object.
            _subscriberSubType = sSubscriberTypeUnformatedXml.DeSerializeStringToObject(_subscriberSubType.GetType());
            if (_subscriberSubType == null)
                throw new Exception("Method CreateOrUpdateSubscriber encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + sSubscriberTypeUnformatedXml + Environment.NewLine);

            var internetAccess = sInternetAccessTypeUnformatedXml.DeSerializeStringToObject(_subscriberInternetAccessType.GetType());

            PropertyInfo pip = _subscriberSubType.GetType().GetProperty("InternetAccess");
            pip.SetValue(_subscriberSubType, internetAccess, null);

            //SubscriberServiceSubscriberTypeV3 mySubscriberType = (SubscriberServiceSubscriberTypeV3)sSubscriberTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberServiceSubscriberTypeV3));
            //mySubscriberType.InternetAccess = (SubscriberInternetAccessType)sInternetAccessTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberInternetAccessType));
            //mySubscriberServiceV3.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, mySubscriberType);
            _subscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new[] { _loginInformation.LoginToken, _subscriberSubType });
        }

        #endregion

        #region ************************** IPTV *******************************

        public void SetIptvAccount(string iptvAccountTypeUnformatedXml, string subscriberTypeUnformatedXml)
        {
            if (Versions.Iptv < 1)
                throw new Exception("Apmax does not currently have any IPTV Service setup.");

            if (_iptvService == null)
                throw new Exception("This version (" + Versions.Iptv + ") of IPTV Service has not been implemented by Oasis.");

            //  <IPTVAccountType>
            //    <ExtensionData />
            //    <AccountDescription />
            //    <Active>true</Active>
            //    <ChannelPackageList>
            //      <ChannelPackageType>
            //        <ExtensionData />
            //        <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
            //        <PackageName>Test</PackageName>
            //      </ChannelPackageType>
            //    </ChannelPackageList>
            //    <CurrentAmountCharged>0</CurrentAmountCharged>
            //    <DeactivateReason />
            //    <FIPSCountyCode>35</FIPSCountyCode>
            //    <FIPSStateCode>46</FIPSStateCode>
            //    <MaxBandwidthKbps>0</MaxBandwidthKbps>
            //    <MaxChargingLimit>0</MaxChargingLimit>
            //    <PurchasePIN />
            //    <RatingPIN />
            //    <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
            //    <ServiceReference>6059974200</ServiceReference>
            //    <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
            //    <SubscriberName>CHR Solutions</SubscriberName>
            //  </IPTVAccountType>

            //This will take string xml and deserialize to an object.
            _iptvAccountType = iptvAccountTypeUnformatedXml.DeSerializeStringToObject(_iptvAccountType.GetType());
            if (_iptvAccountType == null)
                throw new Exception("Method SetIPTVAccount encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + iptvAccountTypeUnformatedXml + Environment.NewLine);

            _iptvSubscriberType = iptvAccountTypeUnformatedXml.DeSerializeStringToObject(_iptvSubscriberType.GetType());
            if (_iptvSubscriberType == null)
                throw new Exception("Method SetIPTVAccount encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + iptvAccountTypeUnformatedXml + Environment.NewLine);

            //May need this if the subscriber already exists.
            var pi = _iptvSubscriberType.GetType().GetProperty("BillingServiceAddress");
            string billingServiceAddress = pi.GetValue(_iptvSubscriberType, null).ToString();
            pi = _iptvSubscriberType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            string subscriberDefaultPhoneNumber = pi.GetValue(_iptvSubscriberType, null).ToString();

            //Get the existing subscriber info.
            var result = _subscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, subscriberDefaultPhoneNumber });
            if (result != null)
            {
                //Take what we got from the RetrieveSubscriberByNumberProv and send everything back as is.
                //We must
                //_iptvSubscriberType = result.SerializeObjectToString().DeSerializeStringToObject(_iptvSubscriberType.GetType());

                //Always Retrieve the guid on the account if the subscriber already exists.
                pi = result.GetType().GetProperty("SubscriberGuid");
                string SubscriberGuid = pi.GetValue(result, null).ToString();
                //Then set the guid on the iptvaccount.
                pi = _iptvSubscriberType.GetType().GetProperty("SubscriberID");
                pi.SetValue(_iptvSubscriberType, SubscriberGuid, null);

                //Always set the Billing Service Address on the account if the subscriber already exists.
                //pi = _iptvSubscriberType.GetType().GetProperty("BillingServiceAddress");
                //pi.SetValue(_iptvSubscriberType, billingServiceAddress, null);
            }

            //myIPTVServiceClientV3.SetIPTVAccount(_loginInformation.LoginToken, myIPTVAccountType, myIPTVSubscriberType);
            _iptvService.GetType().InvokeMember("SetIPTVAccount", BindingFlags.InvokeMethod, null,
                _iptvService, new object[] { _loginInformation.LoginToken, _iptvAccountType, result });
        }

        public void DeleteIptvAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
        {

        }

        public void ForceDeleteIptvAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
        {

        }

        public void SetIptvChannelPackageList(string sSubAddress, string sServiceReference, string sChannelPackageListUnformatedXml)
        {

        }

        public void DisableIptvAccount(string sSubAddress, string sServiceReference)
        {

        }

        public void EnableIptvAccount(string sSubAddress, string sServiceReference)
        {

        }

        public void RemoveStbFromIptvAccount(string sSubAddress, string sServiceReference, string sMacAddress)
        {

        }

        public void DeauthorizeStb(string sSubAddress, string sServiceReference, string sMacAddress)
        {

        }

        public string RetrieveIptvSubscribersByMac(string sMacAddress)
        {
            return "";
        }

        public string RetrieveIptvSubscribersBySerialNumber(string sSerialNumber)
        {
            return "";
        }

        public string RetrieveIptvAccountsBySubAddress(string sSubAddress)
        {
            return "";
        }

        public string RetrieveIptvAccountBySubAddressAndServiceRef(string sSubAddress, string sServiceReference)
        {
            return "";
        }

        public string RetrieveAllChannelLineups()
        {
            return "";
        }
        #endregion

        #region ************************** VoiceMail *******************************

        public void CreateVoicemail(string phoneNumber, string mailBoxDescription, string vmPackageName, 
            string subscriberName, string mailBoxType, string notificationCenter, string billingAccountNumber)
        {
            //This will take string xml and deserialize to an object.
            //var myNewVoiceMailBoxTypeArray = sVoiceMailBoxUnformatedXml.DeSerializeStringToObject(_voiceMailBoxType.GetType());
            //if (myNewVoiceMailBoxTypeArray == null)
            //    throw new Exception("Method UpdateVoiceMailBoxFull encountered exception: Unable to parse xml or invalid xml." +
            //        Environment.NewLine + "XML recieved: " + sVoiceMailBoxUnformatedXml + Environment.NewLine);

            //Once you have an object find the property we need and get its value.
            //PropertyInfo test = myNewVoiceMailBoxTypeArray.GetType().GetProperty("DescriptionField");
            //string sPhoneNumber = test.GetValue(myNewVoiceMailBoxTypeArray, null).ToString();

            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currenly have any Voicemail setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            if (phoneNumber.Length < 10)
                throw new Exception("PhoneNumber must be 10 digits." + Environment.NewLine);

            string sPackageGuid = "";
            //Gets a list of all available vm packages
            //myVoiceMailServiceV3.GetAllPackages(_loginInformation.LoginToken);
            object result = _voiceMailService.GetType().InvokeMember("GetAllPackages", BindingFlags.InvokeMethod, null, _voiceMailService, new object[] { _loginInformation.LoginToken });
            //now cast as array so we can loop thorugh using the foreach loop.
            Array genPackagesArray = result as Array;


            //object resultArray = Convert.ChangeType(result, genPackageTypeArray.GetType()); //Changes type from a generic object to an object of my specific type.
            //now cast as array so we can loop thorugh using the foreach loop.
            //Array array = resultArray as Array;

            //Here we are searching thorugh all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (var myPackageType in genPackagesArray)
            {
                PropertyInfo pi = myPackageType.GetType().GetProperty("DescriptionField");
                string description = pi.GetValue(myPackageType, null).ToString();

                if (description.Equals(vmPackageName, StringComparison.OrdinalIgnoreCase))
                {
                    pi = myPackageType.GetType().GetProperty("GuidField");
                    sPackageGuid = pi.GetValue(myPackageType, null).ToString();
                    break;
                }
            }

            if (sPackageGuid.Length == 0)
                throw new Exception("Could not find this Package in the ApMax system." + Environment.NewLine +
                          "Note: Case sentivity does not matter." + Environment.NewLine +
                          "Please check that the correct package was sent to this method." + Environment.NewLine);


            int iNotificationCenterID = -1;

            //Gets all the notification centers
            //NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(_loginInformation.LoginToken);
            result = _voiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, _voiceMailService, new object[] { _loginInformation.LoginToken });
            Array genNotificationCentersArray = result as Array;

            //Here we are searching thorugh all the NotificationCenters and matching up the description with what we want.
            //Because we need to send the centerid.
            foreach (var myNotificationType in genNotificationCentersArray)
            {
                PropertyInfo pi = myNotificationType.GetType().GetProperty("DescriptionField");
                string description = pi.GetValue(myNotificationType, null).ToString();

                if (description.Equals(notificationCenter, StringComparison.OrdinalIgnoreCase))
                {
                    pi = myNotificationType.GetType().GetProperty("CenterIdField");
                    iNotificationCenterID = Convert.ToInt32(pi.GetValue(myNotificationType, null));
                    break;
                }
            }

            //iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
            if (iNotificationCenterID < 0)
                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
                          "Note: Case sentivity does not matter." + Environment.NewLine +
                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

            _mailBoxType = Enum.Parse(_mailBoxType.GetType(), mailBoxType, true);

            PropertyInfo pip = genSubscriberType.GetType().GetProperty("SubscriberName");
            pip.SetValue(genSubscriberType, subscriberName, null);
            pip = genSubscriberType.GetType().GetProperty("BillingAccountNumber");
            pip.SetValue(genSubscriberType, billingAccountNumber, null);
            pip = genSubscriberType.GetType().GetProperty("SubscriberTimezone");
            pip.SetValue(genSubscriberType, genTimezone, null);
            pip = genSubscriberType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            pip.SetValue(genSubscriberType, phoneNumber, null);
            pip = genSubscriberType.GetType().GetProperty("PlacementType");
            pip.SetValue(genSubscriberType, _placementType, null);

            object[] objParms = new[] {_loginInformation.LoginToken, phoneNumber, phoneNumber, sPackageGuid, 
                _mailBoxType, iNotificationCenterID, phoneNumber, genSubscriberType};

            //AddNewVoiceMailBox(_loginInformation.LoginToken, sPhoneNumber, sMailBoxDescription, sPackageGuid,myMailboxType, iNotificationCenterID, sPhoneNumber, mySubscriberType);
            result = _voiceMailService.GetType().InvokeMember("AddNewVoiceMailBox", BindingFlags.InvokeMethod, null, _voiceMailService, objParms);
        }

        public void DeleteVoiceMailBox(string sVMPhone, bool DeleteSubscriber)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            //This will get the current mailbox info before we delete the box.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sVMPhone });
            //now cast as array so we can loop through using the foreach loop.
            Array array = result as Array;

            //If this is not null mailbox is present so we will delete.
            if (array == null || array.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            foreach (var VoiceMailBoxType in array)
            {
                PropertyInfo pi = VoiceMailBoxType.GetType().GetProperty("IdField");
                string id = pi.GetValue(VoiceMailBoxType, null).ToString();

                //myVoiceMailServiceV3.DeleteVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField);
                _voiceMailService.GetType().InvokeMember("DeleteVoiceMailBox", BindingFlags.InvokeMethod, null,
                    _voiceMailService, new object[] { _loginInformation.LoginToken, id });
            }

            if (DeleteSubscriber)
                DeleteSubscriberByPhoneNumber(sVMPhone);
        }

        public void UpdateVoiceMailBoxPackage(string sPhoneNumber, string sVmPackageName)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            string sPackageGuid = "";
            //Gets a list of all available vm packages
            //myVoiceMailServiceV3.GetAllPackages(_loginInformation.LoginToken);
            object result = _voiceMailService.GetType().InvokeMember("GetAllPackages", BindingFlags.InvokeMethod, null, _voiceMailService, new object[] { _loginInformation.LoginToken });
            //now cast as array so we can loop through using the foreach loop.
            Array array = result as Array;

            //object resultArray = Convert.ChangeType(result, genPackageTypeArray.GetType()); //Changes type from a generic object to an object of my specific type.
            //now cast as array so we can loop through using the foreach loop.
            //Array array = resultArray as Array;

            if (array == null || array.Length == 0)
                throw new Exception("Failed to return packages." + Environment.NewLine);

            //Here we are searching through all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (var myPackageType in array)
            {
                PropertyInfo pi = myPackageType.GetType().GetProperty("DescriptionField");
                string description = pi.GetValue(myPackageType, null).ToString();

                if (description.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
                {
                    pi = myPackageType.GetType().GetProperty("GuidField");
                    sPackageGuid = pi.GetValue(myPackageType, null).ToString();
                    break;
                }
            }

            if (sPackageGuid.Length == 0)
                throw new Exception("Could not find this Package in the ApMax system." + Environment.NewLine +
                          "Note: Case sentivity does not matter." + Environment.NewLine +
                          "Please check that the correct package was sent to this method." + Environment.NewLine);

            //This will get the current mailbox info.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sPhoneNumber });
            //now cast as array so we can loop through using the foreach loop.
            array = result as Array;

            //If this is not null mailbox is present so we will delete.
            if (array == null || array.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            foreach (var vmbox in array)
            {
                foreach (PropertyInfo pi in vmbox.GetType().GetProperties())
                {
                    //This will set all property fields to null except the Id field.
                    if (!(pi.Name == "IdField" || pi.Name == "OptionsPackageField" || pi.Name == "SubscriberListField"))
                        pi.SetValue(vmbox, null, null);

                    //Put in the new package guid.
                    if (pi.Name == "OptionsPackageField")
                        pi.SetValue(vmbox, sPackageGuid, null);

                    //Per Innovatives request set the guid to null on all subtypeguids.
                    if (pi.Name == "SubscriberListField")
                    {
                        genSubInfoType = pi.GetValue(vmbox, null);
                        Array subInfoTypeArray = genSubInfoType as Array;
                        foreach (var subInfoType in subInfoTypeArray)
                        {
                            PropertyInfo pic = subInfoType.GetType().GetProperty("SubscriberGuidField");
                            pic.SetValue(subInfoType, null, null);
                        }
                    }
                }

                //myVoiceMailServiceV3.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
                _voiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
                    _voiceMailService, new[] { _loginInformation.LoginToken, vmbox, null });
            }
        }

        public void UpdateVoiceMailBoxType(string sPhoneNumber, string sMailBoxType, string sInternetPassword,
            string sInternetUserName, bool blnInternetAccess)
        {


        }

        public void AddVoiceMailBoxInternetAccess(string sVoiceMailBoxUnformatedXml, string sInternetAccessUnformatedXml, string sSubscriberTypeUnformatedXml)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            //This will take string xml and deserialize to an object.
            _voiceMailBoxType = sVoiceMailBoxUnformatedXml.DeSerializeStringToObject(_voiceMailBoxType.GetType());
            if (_voiceMailBoxType == null)
                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + sVoiceMailBoxUnformatedXml + Environment.NewLine);

            //This will take string xml and deserialize to an object.
            _voicemailInternetAccessType = sInternetAccessUnformatedXml.DeSerializeStringToObject(_voicemailInternetAccessType.GetType());
            if (_voicemailInternetAccessType == null)
                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + sInternetAccessUnformatedXml + Environment.NewLine);

            //This will take string xml and deserialize to an object.
            _subscriberSubType = sSubscriberTypeUnformatedXml.DeSerializeStringToObject(_subscriberSubType.GetType());
            if (_subscriberSubType == null)
                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + sSubscriberTypeUnformatedXml + Environment.NewLine);

            //Get the phone number.
            PropertyInfo pi = _voiceMailBoxType.GetType().GetProperty("DescriptionField");
            string sPhoneNumber = pi.GetValue(_voiceMailBoxType, null).ToString();

            //Get the email address.
            pi = _subscriberSubType.GetType().GetProperty("SubscriberEmail");
            string sEmailAddress = pi.GetValue(_subscriberSubType, null).ToString();

            //This will get the current mailbox info.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sPhoneNumber });

            //object result = IVoiceMailBoxService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, sPhoneNumber);

            //now cast as array so we can loop through using the foreach loop.
            Array _voiceMailBoxTypeArray = result as Array;

            //If this is not null mailbox is present so we can add internet access.)
            if (_voiceMailBoxTypeArray == null || _voiceMailBoxTypeArray.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            object notificationInfoTypeArray = null;
            string sSubscriberGuid = "";
            string sSubscriberName = "";

            foreach (var vmbox in _voiceMailBoxTypeArray)
            {
                foreach (PropertyInfo pinfo in vmbox.GetType().GetProperties())
                {
                    //Need to keep the notification list.
                    if (pinfo.Name == "NotificationListField")
                        notificationInfoTypeArray = pinfo.GetValue(vmbox, null);

                    //This will set all property fields to null except the Id field.
                    if (!(pinfo.Name == "IdField" || pinfo.Name == "SubscriberListField"))
                        pinfo.SetValue(vmbox, null, null);

                    //Per Innovatives request set the guid to null on all subtypeguids.
                    if (pinfo.Name == "SubscriberListField")
                    {
                        genSubInfoType = pinfo.GetValue(vmbox, null);
                        Array subInfoTypeArray = genSubInfoType as Array;
                        foreach (var subInfoType in subInfoTypeArray)
                        {
                            //Need sSubscriberGuid later so store this.
                            pi = subInfoType.GetType().GetProperty("SubscriberGuidField");
                            sSubscriberGuid = pi.GetValue(subInfoType, null).ToString();

                            pi = subInfoType.GetType().GetProperty("SubscriberGuidField");
                            pi.SetValue(subInfoType, null, null);

                            pi = subInfoType.GetType().GetProperty("SubscriberDefaultPhoneNumberField");
                            string subscriberDefaultPhoneNumber = pi.GetValue(subInfoType, null).ToString();

                            if (subscriberDefaultPhoneNumber == sPhoneNumber)
                            {
                                //Need sSubscriberName later so store this.
                                pi = subInfoType.GetType().GetProperty("SubscriberNameField");
                                sSubscriberName = pi.GetValue(subInfoType, null).ToString();
                            }
                        }
                    }
                }
            }

            int iNotificationCenterId = -1;

            //Gets all the notification centers
            //NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(_loginInformation.LoginToken);
            result = _voiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, _voiceMailService, new object[] { _loginInformation.LoginToken });
            var notificationTypeArray = result as Array;

            if (notificationTypeArray == null || notificationTypeArray.Length == 0)
                throw new Exception("Failed to return notification centers." + Environment.NewLine);

            //Here we are searching through all the NotificationCenters and matching up the description with what we want.
            //Because we need to send the centerid.
            foreach (var notificationType in notificationTypeArray)
            {
                pi = notificationType.GetType().GetProperty("TypeField");
                string notificationCenterTypeField = pi.GetValue(notificationType, null).ToString();

                if (notificationCenterTypeField.Equals(genNotificationCenterTypeEnum.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    pi = notificationType.GetType().GetProperty("CenterIdField");
                    iNotificationCenterId = Convert.ToInt32(pi.GetValue(notificationType, null));
                    break;
                }
            }

            //iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
            if (iNotificationCenterId < 0)
                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
                          "Note: Case sentivity does not matter." + Environment.NewLine +
                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

            //Change our current array to a list of NotificationInfoType.
            pi = _notificationInfoType.GetType().GetProperty("AddressField");
            pi.SetValue(_notificationInfoType, sEmailAddress, null);
            pi = _notificationInfoType.GetType().GetProperty("CenterField");
            pi.SetValue(_notificationInfoType, iNotificationCenterId, null);
            pi = _notificationInfoType.GetType().GetProperty("EnabledField");
            pi.SetValue(_notificationInfoType, true, null);

            int length = (notificationInfoTypeArray as Array).Length;
            //Create new Array with one additional element.
            Array ArrayOfNotificationInfo = Array.CreateInstance(_notificationInfoTypeType, length + 1);
            //Copy in old data into new array.
            (notificationInfoTypeArray as Array).CopyTo(ArrayOfNotificationInfo, 0);
            //Set the value of that of the above created new notification and add to the last element in array.
            ArrayOfNotificationInfo.SetValue(_notificationInfoType, length);

            //Darrin from hood wants to allow email deletion and add the email notification as well; the below 2 lines do this.
            pi = _voiceMailBoxType.GetType().GetProperty("AllowEmailDeletionField");
            pi.SetValue(_voiceMailBoxType, true, null);
            pi = _voiceMailBoxType.GetType().GetProperty("NotificationListField");
            pi.SetValue(_voiceMailBoxType, ArrayOfNotificationInfo, null);

            //Get current InternetAccess Username information.
            result = _subscriberService.GetType().InvokeMember("GetSubscriberInternetAccess", BindingFlags.InvokeMethod, null,
                _subscriberService, new object[] { _loginInformation.LoginToken, sSubscriberGuid });

            if (result == null)
            {
                foreach (var voiceMailBoxType in _voiceMailBoxTypeArray as Array)
                {
                    _voiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null, _voiceMailService, new object[]
                                                                    {
                                                                        _loginInformation.LoginToken, voiceMailBoxType,
                                                                        _voicemailInternetAccessType
                                                                    });
                }
            }
            else
            {
                pi = _voicemailInternetAccessType.GetType().GetProperty("UserName");
                string sInternetUserName = pi.GetValue(_voicemailInternetAccessType, null).ToString();

                pi = result.GetType().GetProperty("UserName");
                string sSubscriberInternetUserName = pi.GetValue(result, null).ToString();

                //else if username are equal modify status
                if (sInternetUserName == sSubscriberInternetUserName)
                {
                    //This re-enables the internet access.
                    foreach (var voiceMailBoxType in _voiceMailBoxTypeArray)
                    {
                        _voiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
                                                                   _voiceMailService, new object[]
                                                                                            {
                                                                                                _loginInformation.LoginToken,
                                                                                                voiceMailBoxType,
                                                                                                _voicemailInternetAccessType
                                                                                            });
                    }
                }
                else
                {
                    //DeleteVoiceMailBoxInternetAccess();
                    //AddVoiceMailBoxInternetAccess();
                    //This means they have internet access but the username is not what we expect.
                    throw new Exception("A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine);
                }
            }

            //Set the SubscriberName on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("SubscriberName");
            pi.SetValue(_subscriberSubType, sSubscriberName, null);

            //Set the SubscriberDefaultPhoneNumber on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            pi.SetValue(_subscriberSubType, sPhoneNumber, null);

            //Set the PlacementType on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("PlacementType");
            pi.SetValue(_subscriberSubType, _placementType, null);

            //Set the SubscriberGuid on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("SubscriberGuid");
            pi.SetValue(_subscriberSubType, sSubscriberGuid, null);

            //Set the SubscriberEmail on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("SubscriberEmail");
            pi.SetValue(_subscriberSubType, sEmailAddress, null);

            //enable service on subscriber for internet access.
            pi = _subscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
            pi.SetValue(_subscriberInternetAccessType, true, null);

            //Set the InternetAccess on the Subscriber.
            pi = _subscriberSubType.GetType().GetProperty("InternetAccess");
            pi.SetValue(_subscriberSubType, _subscriberInternetAccessType, null);

            //This re-enables the internet access.
            _subscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null, _subscriberService,
                new object[] { _loginInformation.LoginToken, _subscriberSubType });
        }

        public void DeleteVoiceMailBoxInternetAccess(string sPhoneNumber, bool RemoveSubscriberAccess)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");


            //This will get the current mailbox info.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sPhoneNumber });
            //now cast as array so we can loop through using the foreach loop.
            Array genVoiceMailBoxArray = result as Array;

            //If this is not null mailbox is present so we can add internet access.
            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            //Gets all the notification centers
            //NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(_loginInformation.LoginToken);
            result = _voiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, _voiceMailService, new object[] { _loginInformation.LoginToken });
            Array genNotificationCentersArray = result as Array;

            int notificationCenterID = -1;

            //Here we are searching thorugh all the NotificationCenters and matching up the description with what we want.
            //Because we need to send the remove the email notification.
            foreach (var notificationType in genNotificationCentersArray)
            {
                PropertyInfo pi = notificationType.GetType().GetProperty("TypeField");
                string notificationCenterTypeField = pi.GetValue(notificationType, null).ToString();

                if (notificationCenterTypeField.Equals(genNotificationCenterTypeEnum.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    pi = notificationType.GetType().GetProperty("CenterIdField");
                    notificationCenterID = Convert.ToInt32(pi.GetValue(notificationType, null));
                    break;
                }
            }

            //iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
            if (notificationCenterID < 0)
                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
                          "Note: Case sentivity does not matter." + Environment.NewLine +
                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
            {
                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
                {
                    //This will set all property fields to null except the Id field.
                    if (!(pi.Name == "IdField" || pi.Name == "SubscriberListField" || pi.Name == "NotificationListField"))
                        pi.SetValue(genVoiceMailBox, null, null);

                    //Darrin from hood wants to allow email deletion and add the email notification as well; the below 2 lines do this.
                    if (pi.Name == "AllowEmailDeletionField")
                        pi.SetValue(genVoiceMailBox, false, null);

                    if (pi.Name == "NotificationListField")
                    {
                        object genNotificationArray = pi.GetValue(genVoiceMailBox, null);

                        IList genNotificationCenterIList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(_notificationInfoTypeType));
                        foreach (var notificationType in genNotificationArray as Array)
                        {
                            PropertyInfo prinfo = notificationType.GetType().GetProperty("CenterField");
                            int centerField = Convert.ToInt32(prinfo.GetValue(notificationType, null));

                            if (centerField != notificationCenterID)
                                genNotificationCenterIList.Add(notificationType);
                        }

                        int length = genNotificationCenterIList.Count;
                        //Create new Array.
                        Array genNotificationInfoArray = Array.CreateInstance(_notificationInfoTypeType, length);
                        //Copy in old data into new array.
                        genNotificationCenterIList.CopyTo(genNotificationInfoArray, 0);

                        pi.SetValue(genVoiceMailBox, genNotificationInfoArray, null);
                    }

                    if (pi.Name == "SubscriberListField")
                    {
                        genSubInfoType = pi.GetValue(genVoiceMailBox, null);
                        foreach (var info in genSubInfoType as Array)
                        {
                            PropertyInfo pic = info.GetType().GetProperty("SubscriberGuidField");
                            pic.SetValue(info, null, null);
                        }
                    }
                }

                //disable service on voicemail for internet access.
                PropertyInfo pinfo = _voicemailInternetAccessType.GetType().GetProperty("ServiceEnabled");
                pinfo.SetValue(_voicemailInternetAccessType, false, null);

                //myVoiceMailServiceV3.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
                _voiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
                    _voiceMailService, new[] { _loginInformation.LoginToken, genVoiceMailBox, _voicemailInternetAccessType });

                if (RemoveSubscriberAccess)
                {
                    //Set the SubscriberName on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("SubscriberName");
                    pinfo.SetValue(_subscriberSubType, null, null);

                    //Set the SubscriberDefaultPhoneNumber on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
                    pinfo.SetValue(_subscriberSubType, sPhoneNumber, null);

                    //Set the PlacementType on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("PlacementType");
                    pinfo.SetValue(_subscriberSubType, _placementType, null);

                    //Set the SubscriberGuid on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("SubscriberGuid");
                    pinfo.SetValue(_subscriberSubType, null, null);

                    //Set the SubscriberEmail on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("SubscriberEmail");
                    pinfo.SetValue(_subscriberSubType, "", null);

                    //enable service on subscriber for internet access.
                    pinfo = _subscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
                    pinfo.SetValue(_subscriberInternetAccessType, false, null);

                    //Set the InternetAccess on the Subscriber.
                    pinfo = _subscriberSubType.GetType().GetProperty("InternetAccess");
                    pinfo.SetValue(_subscriberSubType, _subscriberInternetAccessType, null);

                    //This disables the internet access.
                    _subscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod,
                                                                null, _subscriberService,
                                                                new object[]
                                                                    {
                                                                        _loginInformation.LoginToken, _subscriberSubType
                                                                    });
                }
            }
        }

        public void AddVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            //This will get the current mailbox info.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sPhoneNumber });
            //now cast as array so we can loop through using the foreach loop.
            Array genVoiceMailBoxArray = result as Array;

            //If this is not null mailbox is present so we can add internet access.
            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            string parentId = "";

            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
            {
                //Submailboxes  will change per returned mailbox.
                int iCurrentNumberOfSubBoxes = 0;

                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
                {
                    if (pi.Name == "IdField")
                        parentId = pi.GetValue(genVoiceMailBox, null).ToString();

                    //Only look for submailbox types.  Ignore outdial types etc...  
                    //also when you delete a child it does not get rid of this it only clears the NameField...
                    //Dont ask me why... This is the most redicules API I have had to deal with.
                    if (pi.Name == "ChildListField")
                    {
                        genSubInfoType = pi.GetValue(genVoiceMailBox, null);
                        foreach (var info in genSubInfoType as Array)
                        {
                            PropertyInfo pic = info.GetType().GetProperty("TypeField");
                            object typeField = pi.GetValue(genVoiceMailBox, null);

                            pic = info.GetType().GetProperty("NameField");
                            string nameField = pic.GetValue(genSubInfoType, null).ToString();

                            if (typeField.GetType().Equals(_voiceMailServiceAddressTypeEnum) && nameField.Length > 0)
                                iCurrentNumberOfSubBoxes++;
                        }
                    }
                }

                //myVoiceMailServiceV3.AddNewChildMailBox(_loginInformation.LoginToken, parentId, new ChildInfoType
                //{
                //    //We have to get the next available digit field. The array is pre-populated and sorted with available fields.
                //    DigitField = sDigitField,
                //    NameField = sPhoneNumber + (iCurrentNumberOfSubBoxes + 1),
                //    DescriptionField = "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")",
                //    TypeField = VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber
                //}, MailboxType.FAMILY_CHILD);

                _mailBoxType = Enum.Parse(_mailBoxType.GetType(), "FAMILY_CHILD", true);

                //Set the DigitField on the Child Info Type.
                PropertyInfo pinfo = _childInfoType.GetType().GetProperty("DigitField");
                pinfo.SetValue(_childInfoType, sDigitField, null);

                //Set the NameField on the Child Info Type.
                pinfo = _childInfoType.GetType().GetProperty("NameField");
                pinfo.SetValue(_childInfoType, sPhoneNumber + (iCurrentNumberOfSubBoxes + 1), null);

                //Set the DescriptionField on the Child Info Type.
                pinfo = _childInfoType.GetType().GetProperty("DescriptionField");
                pinfo.SetValue(_childInfoType, "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")", null);

                //Set the TypeField on the Child Info Type.
                pinfo = _childInfoType.GetType().GetProperty("TypeField");
                pinfo.SetValue(_childInfoType, _voiceMailServiceAddressTypeEnum, null);

                //This disables the internet access.
                _voiceMailService.GetType().InvokeMember("AddNewChildMailBox", BindingFlags.InvokeMethod,
                                                            null, _voiceMailService,
                                                            new object[]
                                                                    {
                                                                        _loginInformation.LoginToken, 
                                                                        parentId,
                                                                        _childInfoType,
                                                                        _mailBoxType
                                                                    });
            }
        }

        public void DeleteVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            //This will get the current mailbox info.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, sPhoneNumber });
            //now cast as array so we can loop through using the foreach loop.
            Array genVoiceMailBoxArray = result as Array;

            //If this is not null mailbox is present so we can add internet access.
            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            string parentId = "";

            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
            {
                object genChildInfoArray = null;
                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
                {
                    if (pi.Name == "IdField")
                        parentId = pi.GetValue(genVoiceMailBox, null).ToString();

                    if (pi.Name == "ChildListField")
                        genChildInfoArray = pi.GetValue(genVoiceMailBox, null);
                }

                _childInfoType = (genChildInfoArray as Array).RetrieveAddressFieldFromDigitField(sDigitField);

                if (_childInfoType == null)
                    throw new Exception("Cannot find submailbox with that DigitField ID." + Environment.NewLine);

                //myVoiceMailServiceV3.DeleteChildMailBox(_loginInformation.LoginToken, sParentID, myChildInfoType);
                _voiceMailService.GetType().InvokeMember("DeleteChildMailBox", BindingFlags.InvokeMethod, null,
                    _voiceMailService, new object[] { _loginInformation.LoginToken, parentId, _childInfoType });
            }
        }

        public void UpdateVoiceSubMailbox(string sPhoneNumber, int iNumberOfSubMailboxes, int iMaxNumberOfSubMailboxesAllowed)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public void UpdateVoiceMailBox(string sVoiceMailBoxUnformatedXml, string sInternetAccessUnformatedXml)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public void AddOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber, string sOutDialRoutingNumber)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public void DeleteOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public void ReassignVMBoxNumber(string sOldPhoneNumber, string sNewPhoneNumber)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public void InternetPasswordReset(string sPhoneNumber, string sInternetPassword)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            PropertyInfo pi = _subscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
            pi.SetValue(_subscriberInternetAccessType, true, null);
            pi = _subscriberInternetAccessType.GetType().GetProperty("Password");
            pi.SetValue(_subscriberInternetAccessType, sInternetPassword, null);

            pi = _subscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
            pi.SetValue(_subscriberSubType, sPhoneNumber, null);
            pi = _subscriberSubType.GetType().GetProperty("PlacementType");
            pi.SetValue(_subscriberSubType, _placementType, null);
            pi = _subscriberSubType.GetType().GetProperty("InternetAccess");
            pi.SetValue(_subscriberSubType, _subscriberInternetAccessType, null);

            //mySubscriberServiceV3.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, mySubscriberType);
            _subscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
                _subscriberService, new[] { _loginInformation.LoginToken, _subscriberSubType });
        }

        public void VMPasswordReset(string sPhoneNumber, string sNewPin)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");
        }

        public string RetrieveVoicemail(string phoneNumber)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            //This will get the current mailbox.
            //VoiceMailBoxType[] myVoiceMailBoxTypeArray =
            //    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
            //if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            //    return "No Mail Box found.";

            object result = _voiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
                _voiceMailService, new object[] { _loginInformation.LoginToken, phoneNumber });
            //now cast as array so we can loop thorugh using the foreach loop.
            Array genVoiceMailBoxesArray = result as Array;

            //If this is not null mailbox is present so we will delete.
            if (genVoiceMailBoxesArray == null || genVoiceMailBoxesArray.Length == 0)
                throw new Exception("Mailbox does not exist." + Environment.NewLine);

            return genVoiceMailBoxesArray.SerializeObjectToString();
        }

        public string RetrieveVoiceMailBoxByID(string sMailboxID)
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            return "";
        }

        public string RetrieveAllPackages()
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            return "";
        }

        public string RetrieveAllNotificationCenters()
        {
            if (Versions.Voicemail < 1)
                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

            if (_voiceMailService == null)
                throw new Exception("This version (" + Versions.Voicemail + ") of voicemail has not been implemented by Oasis.");

            return "";
        }
        #endregion

        #region ************************** Calling Number Announcement CNA *******************************

        public void AddCnaNumber(string sXmlCnaInfo)
        {
            if (Versions.ChangeNumberAnnouncements < 1)
                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

            if (_cnaService == null)
                throw new Exception("This version (" + Versions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

            //This will take string xml and deserialize to an object.
            _cnaInfo = sXmlCnaInfo.DeSerializeStringToObject(_cnaInfo.GetType());
            if (_cnaInfo == null)
                throw new Exception("Method AddCnaNumber encountered exception: Unable to parse xml or invalid xml." +
                    Environment.NewLine + "XML recieved: " + sXmlCnaInfo + Environment.NewLine);

            if (Versions.ChangeNumberAnnouncements < 1)
                throw new Exception("Apmax does not currently have any Change Number Announcements setup.");

            if (_cnaService == null)
                throw new Exception("This version (" + Versions.ChangeNumberAnnouncements + ") of CNA has not been implemented by Oasis.");

            //DateTime currentDate = DateTime.Now;
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            //TimeSpan currentOffset = localZone.GetUtcOffset( currentDate );
            //DateTime.Now + currentOffset;
            //DateTime.UtcNow;
            //DateTime myDateTime = currentDate.ToLocalTime();
            //DateTime.Parse("2009-10-28T16:32:19.1217873-05:00");
            //currentDate.Add();
            //currentDate.GetDateTimeFormats

            PropertyInfo pi = _cnaInfo.GetType().GetProperty("FromNumber");
            string fromNumber = pi.GetValue(_cnaInfo, null).ToString();
            pi = _cnaInfo.GetType().GetProperty("ToNumber");
            string toNumber = pi.GetValue(_cnaInfo, null).ToString();

            //CnaInfo[] myCnaInfoArray = myCNAServiceV3.GetAllCnaAnnouncements(_loginInformation.LoginToken);
            object result = _cnaService.GetType().InvokeMember("GetAllCnaAnnouncements", BindingFlags.InvokeMethod, null,
                _cnaService, new[] { _loginInformation.LoginToken });
            //now cast as array so we can loop through using the foreach loop.
            Array array = result as Array;

            if (array != null)
            {
                foreach (var oldCnaInfo in array)
                {
                    pi = oldCnaInfo.GetType().GetProperty("FromNumber");
                    string oldFromNumber = pi.GetValue(oldCnaInfo, null).ToString();
                    pi = oldCnaInfo.GetType().GetProperty("toNumber");
                    string oldToNumber = pi.GetValue(oldCnaInfo, null).ToString();

                    //check and see if this CNA number already is in the system if it is remove all occurences.
                    if (oldFromNumber.Equals(fromNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        //myCNAServiceV3.DeleteCnaNumber(_loginInformation.LoginToken, myCnaInfo.FromNumber, myCnaInfo.ToNumber);
                        _cnaService.GetType().InvokeMember("DeleteCnaNumber", BindingFlags.InvokeMethod, null,
                            _cnaService, new object[] { _loginInformation.LoginToken, oldFromNumber, oldToNumber });
                    }
                }
            }

            //Now once we no there are not other occurences add this CNA number.
            //myCNAServiceV3.SetCnaNumber(_loginInformation.LoginToken, Object);
            _cnaService.GetType().InvokeMember("SetCnaNumber", BindingFlags.InvokeMethod, null,
                    _cnaService, new object[] { _loginInformation.LoginToken, fromNumber, toNumber });
        }

        public void DeleteCnaNumber(string sFromPhoneNumber, string sToPhoneNumber)
        {
            if (Versions.ChangeNumberAnnouncements < 1)
                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

            if (_cnaService == null)
                throw new Exception("This version (" + Versions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

            //myCNAServiceV3.DeleteCnaNumber(_loginInformation.LoginToken, sFromPhoneNumber, sToPhoneNumber);
            _cnaService.GetType().InvokeMember("DeleteCnaNumber", BindingFlags.InvokeMethod, null,
                _cnaService, new[] { _loginInformation.LoginToken, sFromPhoneNumber, sToPhoneNumber });
        }

        public string GetCnaAnnouncement(string sFromPhoneNumber, string sToPhoneNumber)
        {
            if (Versions.ChangeNumberAnnouncements < 1)
                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

            if (_cnaService == null)
                throw new Exception("This version (" + Versions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

            object result = _cnaService.GetType().InvokeMember("GetAllCnaAnnouncements", BindingFlags.InvokeMethod, null,
                _cnaService, new[] { _loginInformation.LoginToken });
            //now cast as array so we can loop through using the foreach loop.
            Array array = result as Array;

            if (array != null)
            {
                foreach (var oldCnaInfo in array)
                {
                    PropertyInfo pi = oldCnaInfo.GetType().GetProperty("FromNumber");
                    string oldFromNumber = pi.GetValue(oldCnaInfo, null).ToString();
                    pi = oldCnaInfo.GetType().GetProperty("toNumber");
                    string oldToNumber = pi.GetValue(oldCnaInfo, null).ToString();

                    //check and see if this CNA number already is in the system if it is remove all occurences.
                    if (oldFromNumber.Equals(sFromPhoneNumber, StringComparison.OrdinalIgnoreCase) &&
                        oldToNumber.Equals(sToPhoneNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        return oldCnaInfo.SerializeObjectToString();
                    }
                }
            }

            return "No matching To and From numbers found.";
        }
        #endregion

        #region ************************** ScreenPop *******************************

        public void AddScreenPop(string sDN)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //myCallingNameServiceV3.InsertScreenPopSubscriber(_loginInformation.LoginToken, sDN, true);
            _callingNameService.GetType().InvokeMember("InsertScreenPopSubscriber", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN, true });
        }

        public void DeleteScreenPop(string sDN)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //myCallingNameServiceV3.DeleteScreenPopSubscriber(_loginInformation.LoginToken, sDN);
            _callingNameService.GetType().InvokeMember("DeleteScreenPopSubscriber", BindingFlags.InvokeMethod, null,
                _callingNameService, new[] { _loginInformation.LoginToken, sDN });
        }

        public string GetAllScreenPopEntries()
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //ScreenPopType[] myScreenPopType = myCallingNameServiceV3.GetAllScreenPopEntries(_loginInformation.LoginToken);
            //sResponse = myScreenPopType.SerializeObjectToString();
            return _callingNameService.GetType().InvokeMember("GetAllScreenPopEntries", BindingFlags.InvokeMethod, null,
                _callingNameService, new[] { _loginInformation.LoginToken }).SerializeObjectToString();
        }
        #endregion

        #region ************************** CallingName *******************************

        public void AddCallerName(string sDN, string sCallerName, string sPresentation)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //myCallingNameServiceV3.InsertCallingEntry(_loginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);
            _callingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN, 0, sPresentation, sCallerName });
        }

        public void DeleteCallerName(string sDN)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //myCallingNameServiceV3.DeleteCallingEntry(_loginInformation.LoginToken, sDN, 0);
            _callingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN, 0 });
        }

        public void ModifyCallerName(string sDN, string sCallerName, string sPresentation)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //myCallingNameServiceV3.DeleteCallingEntry(_loginInformation.LoginToken, sDN, 0);
            _callingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN, 0 });

            //myCallingNameServiceV3.InsertCallingEntry(_loginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);
            _callingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN, 0, sPresentation, sCallerName });
        }

        public void ReassignCallerName(string sDN, string sOldDN, ref string sResponse)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            object result = _callingNameService.GetType().InvokeMember("GetCallingEntries", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sOldDN });
            //now cast as array so we can loop through using the foreach loop.
            Array array = result as Array;

            if (array == null || array.Length == 0)
                throw new Exception("Failed to return any caller name entries." + Environment.NewLine);

            foreach (var callerNameEntry in array)
            {
                PropertyInfo pi = callerNameEntry.GetType().GetProperty("Presentation");
                string presentation = pi.GetValue(callerNameEntry, null).ToString();
                pi = callerNameEntry.GetType().GetProperty("CName");
                string cName = pi.GetValue(callerNameEntry, null).ToString();

                //myCallingNameServiceV3.InsertCallingEntry(_loginInformation.LoginToken, sDN, 0, myCallingNameType.Presentation, myCallingNameType.CName);
                _callingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
                    _callingNameService, new object[] { _loginInformation.LoginToken, sDN, 0, presentation, cName });
            }

            //myCallingNameServiceV3.DeleteCallingEntry(_loginInformation.LoginToken, sOldDN, 0);
            _callingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sOldDN, 0 });
        }

        public string GetCallerName(string sDN)
        {
            if (Versions.CallingName < 1)
                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

            if (_callingNameService == null)
                throw new Exception("This version (" + Versions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

            //CallingNameType[] myCallingNameTypeArray = myCallingNameServiceV3.GetCallingEntries(_loginInformation.LoginToken, sDN);
            //sResponse = myCallingNameTypeArray.SerializeObjectToString();

            return _callingNameService.GetType().InvokeMember("GetCallingEntries", BindingFlags.InvokeMethod, null,
                _callingNameService, new object[] { _loginInformation.LoginToken, sDN }).SerializeObjectToString();
        }

        #endregion

        public void Close()
        {
            // Invoke the close methods using reflection.
            _voiceMailService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _voiceMailService, null);
            _subscriberService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _subscriberService, null);
            _callingNameService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _callingNameService, null);
            _cnaService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _cnaService, null);
            _iptvService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, _iptvService, null);

            //Close all the other instantiations.
            //_localNumberPortabilityServiceV1.Close();
            //myOCMServiceV1.Close();
            //myLargeScaleConferenceV1.Close();
            //myCLPServiceV3.Close();
            //myUCMServiceV3.Close();
            //myODConferencingServiceV3.Close();
            //myTCMServiceV3.Close();
            _serviceReportV1.Close();

            _apAdmin.Logout(_loginInformation.LoginToken);
            _apAdmin.Close();
        }

    }

    
    //Extension methods must be defined in a static class
    public static class ExtensionClass
    {
        //1. Shows up in intellisense
        //2. add new methods to objects that were already defined, especially when you don't own/control the source to the original object
        //3. The only advantage of extension methods is code readability

        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        //public static ChildInfoType GetAddressFieldFromDigitField(this ChildInfoType[] myChildInfoTypeArray, string sDigitField)
        //{
        //    foreach (var myChild in myChildInfoTypeArray)
        //    {
        //        if (myChild.DigitField.Equals(sDigitField, StringComparison.OrdinalIgnoreCase))
        //        {
        //            return myChild;
        //        }
        //    }

        //    return null;
        //}

        public static object RetrieveAddressFieldFromDigitField(this Array childInfoArray, string sDigitField)
        {
            foreach (var child in childInfoArray)
            {
                var pi = child.GetType().GetProperty("DigitField");
                string digitField = pi.GetValue(child, null).ToString();

                if (digitField.Equals(sDigitField, StringComparison.OrdinalIgnoreCase))
                {
                    return child;
                }
            }

            return null;
        }

        //public static ChildInfoType GetChildInfoByHighestDigitField(this ChildInfoType[] myChildInfoTypeArray)
        //{
        //    ChildInfoType CurrentChildInfoType = new ChildInfoType() { DigitField = "-1"};
        //    foreach (var myChild in myChildInfoTypeArray)
        //    {
        //        //Submailboxes are always AddressType of AddressTypeMailboxNumber.  Outdialnumbers are AddressTypeDN.
        //        if (Convert.ToInt32(myChild.DigitField) > Convert.ToInt32(CurrentChildInfoType.DigitField)
        //            && myChild.TypeField == VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber)
        //            CurrentChildInfoType = myChild;
        //    }

        //    return CurrentChildInfoType.DigitField == "-1" ? null : CurrentChildInfoType;
        //}

        public static int WordCount(this String str)
        {
            return str.Split(new[] {' ', '.', '?'}, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        //public static string HtmlDecode(this string str)
        //{
        //    return HttpUtility.HtmlDecode(str);
        //}

        public static object DeSerializeStringToObject(this string sxml, Type type)
        {
            using (var xreader = new XmlTextReader(new StringReader(sxml.Replace("&", "&amp;"))))
            {
                var xs = new XmlSerializer(type);
                return xs.Deserialize(xreader);
            }
        }

        public static string SerializeObjectToString(this object obj)
        {
            using (var stream = new MemoryStream())
            {
                var x = new XmlSerializer(obj.GetType());
                x.Serialize(stream, obj);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }
    }
}
