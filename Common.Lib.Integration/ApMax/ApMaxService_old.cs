//DeleteSubscriberByPhoneNumberusing System;
//using System.Collections;
//using System.IO;
//using Common.IPTVServiceV3;
//using Common.ServiceReport;
//using Common.VoicemailV3;
//using Routrek.SSHC;
//using System.Linq;
//using System.Reflection;
//using System.ServiceModel;
//using System.Text;
//using System.Web;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using System.Xml;
//using System.Xml.Serialization;
//using OasisProvisioning.ApAdmin;
//using OasisProvisioning.CallingName;
//using OasisProvisioning.IPTVServiceV3;
//using OasisProvisioning.TCMServiceV3;
//using OasisProvisioning.CNAServiceV3;
//using OasisProvisioning.LargeScaleConferenceV1;
//using OasisProvisioning.ODConferencingServiceV3;
//using OasisProvisioning.NotificationCenterInfo;
//using OasisProvisioning.SubscriberMgmt;
//using OasisProvisioning.VoiceMailService;
//using OasisProvisioning.ServiceReportV1;
//using OasisProvisioning.VoiceMailServiceV3;
//using OasisProvisioning.WirelessOtaV;
//using OasisProvisioning.UCMServiceV3;
//using OasisProvisioning.OCMServiceV1;
//using OasisProvisioning.CallLoggingServiceV3;
//using OasisProvisioning.LocalNumberPortabilityServiceV1;
//using CallingNameServiceClientV3 = OasisProvisioning.CallingNameServiceV3.CallingNameServiceClient;
//using CallingNameServiceClientV4 = OasisProvisioning.CallingNameServiceV4.CallingNameServiceClient;
//using CallingNameTypeV3 = OasisProvisioning.CallingNameServiceV3.CallingNameType;
//using CallingNameTypeV4 = OasisProvisioning.CallingNameServiceV4.CallingNameType;
//using ODInternetAccessType = OasisProvisioning.ODConferencingServiceV3.InternetAccessType;
//using CPLInternetAccessType = OasisProvisioning.CallLoggingServiceV3.InternetAccessType;
//using TCMInternetAccessType = OasisProvisioning.TCMServiceV3.InternetAccessType;
//using OCMInternetAccessType = OasisProvisioning.OCMServiceV1.InternetAccessType;
//using UCMInternetAccessType = OasisProvisioning.UCMServiceV3.InternetAccessType;
//using ODCInternetAccessType = OasisProvisioning.ODConferencingServiceV3.InternetAccessType;
//using VoicemailInternetAccessType = OasisProvisioning.VoiceMailServiceV3.InternetAccessType;
//using SubscriberInternetAccessType = OasisProvisioning.SubscriberServiceV3.InternetAccessType;
//using SubInternetAccessType = OasisProvisioning.SubscriberServiceV3.SubscriberInternetAccessType;
//using SubscriberPlacementType = OasisProvisioning.SubscriberServiceV3.PlacementType_e;
//using SubscriberServiceSubscriberTypeV3 = OasisProvisioning.SubscriberServiceV3.SubscriberType;
//using SubscriberServiceSubscriberTypeV4 = OasisProvisioning.SubscriberServiceV4.SubscriberType;
//using TCMSubscriberType = OasisProvisioning.TCMServiceV3.SubscriberType;
//using ODSubscriberType = OasisProvisioning.ODConferencingServiceV3.SubscriberType;
//using VoicemailSubscriberType = OasisProvisioning.VoiceMailServiceV3.SubscriberType;
//using CPLSubscriberType = OasisProvisioning.CallLoggingServiceV3.SubscriberType;
//using OCMSubscriberType = OasisProvisioning.OCMServiceV1.SubscriberType;
//using UCMSubscriberType = OasisProvisioning.UCMServiceV3.SubscriberType;
//using IPTVSubscriberType = OasisProvisioning.IPTVServiceV3.SubscriberType;
//using SubServiceInfoType = OasisProvisioning.SubscriberServiceV3.ServiceInfoType;
//using OCMType = OasisProvisioning.OCMServiceV1.OCMSubcriberType;
//using UCMType = OasisProvisioning.UCMServiceV3.UniversalCallMgrSubType;



//Note: IMPORTANT anytime a refrence is added, deleted, or changed you must copy the contents the app.config into the Oasis.exe.config!

//namespace OasisProvisioning
//{
//    [ClassInterface(ClassInterfaceType.AutoDual)]
//    [ProgId("VoiceMail.OasisApMaxV3")]
//    [ComVisible(true), Guid("998E6D24-768A-4fc9-B90D-7A92F45CF679")]
//    Do not Add code to this for added functionality!
//    This is only kept for backwards compatibility!
//    All new functionality goes to the OasisApMax class.
//    public class OasisApMaxV3
//    {

//        #region ************************** Admin Vars *******************************
//        [ComVisible(false)]
//        private ApAdminClient myApAdmin;
//        [ComVisible(true)]
//        public LoginInformation myLoginInformation;
//        [ComVisible(false)]
//        private ServiceReportClient myServiceReportV1;
//        [ComVisible(true)]
//        public ServiceVersions myVersions;
//        #endregion

//        #region ************************** Subscriber Vars *******************************
//        [ComVisible(false)]
//        private SubscriberServiceV3.SubscriberServiceClient mySubscriberServiceV3;
//        private SubscriberServiceV4.SubscriberServiceClient mySubscriberServiceV4;
//        #endregion

//        #region ************************** Voicemail Service V3 Vars *******************************
//        [ComVisible(false)]
//        private VoiceMailServiceV3.VoicemailClient myVoiceMailServiceV3;
//        #endregion

//        #region ************************** Calling Name Vars *******************************
//        [ComVisible(false)]
//        private CallingNameServiceClientV3 myCallingNameService;
//        #endregion

//        #region ************************** IPTV Vars *******************************
//        [ComVisible(false)]
//        private IPTVServiceClient myIPTVService;
//        #endregion

//        #region **************************  Calling Number Announcement CNA  Vars *******************************
//        [ComVisible(false)]
//        private CNAServiceClient myCNAService;
//        #endregion

//        #region ************************** TCM Vars *******************************
//        [ComVisible(false)]
//        private TCMServiceClient myTCMService;
//        #endregion

//        #region ************************** Large Scale Conference Vars *******************************
//        [ComVisible(false)]
//        private LargeScaleConferenceClient myLargeScaleConference;
//        #endregion

//        #region ************************** Voicemail ServiceV4 Vars *******************************
//        [ComVisible(false)]
//        private VoiceMailServiceV4.VoicemailClient myVoiceMailServiceV4;
//        #endregion

//        #region ************************** On Demand Conferencing Service Vars *******************************
//        [ComVisible(false)]
//        private ODConferencingServiceClient myODConferencingService;
//        #endregion

//        #region ************************** Universal Call Management Service Vars *******************************
//        [ComVisible(false)]
//        private UCMServiceClient myUCMService;
//        #endregion

//        #region ************************** Call Logging Service Vars *******************************
//        [ComVisible(false)]
//        private CLPServiceClient myCLPService;
//        #endregion

//        #region ************************** Orginating Call Management Service Vars *******************************
//        [ComVisible(false)]
//        private OCMServiceClient myOCMService;
//        #endregion

//        #region ************************** Local Number Portability Service Vars *******************************
//        [ComVisible(false)]
//        private LocalNumberPortabilityClient myLocalNumberPortabilityService;
//        #endregion

//        [ComVisible(true)]
//         <summary> This method intializes the apmax interface.</summary>
//         <param name="IP">IP of the provisioning host.</param>
//         <param name="Port">Port of the provisioning host.</param>
//         <param name="UserName">User of the provisioning host.</param>
//         <param name="PassWord">Pasword of the provisioning host.</param>
//         <param name="SystemKey">SystemKey of the provisioning host.</param>
//         <param name="sError">If the return value is false and error happens then this will be populated.</param>
//         <returns> The return value is a boolean value letting you know if it succeded or failed.</returns>
//        public bool Initialize(string IP, int Port, string UserName, string PassWord, string SystemKey, ref string sError)
//        {
//            try
//            {
//                myLoginInformation = new LoginInformation();

//                Note: IMPORTANT anytime a reference is added, deleted, or changed you must copy the contents the app.config into the Oasis.exe.config!
//                Note: The reason for this is because anytime a DLL is called the calling application config file is the one that will be checked.
//                Note: Which means the configuration will not work unless you copy the DLLs app.config into the Oasis.exe.config.

//                http://localhost:8731/Design_Time_Addresses/ServiceReportV1/ServiceReport/
//                myServiceReportV1 = new ServiceReportClient("WSHttpBinding_IServiceReport",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/ServiceReportV1/ServiceReport/"));

//                http://oasislap7:8731/Design_Time_Addresses/LoginService/ApAdmin/
//                myApAdmin = new ApAdminClient("WSHttpBinding_IApAdmin",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

//                http://oasislap7:8731/Design_Time_Addresses/VoicemailService/VoicemailV3/
//                myVoiceMailServiceV3 = new VoicemailClient("WSHttpBinding_IVoicemail3",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/VoicemailService/VoicemailV3/"));

//                http://oasislap7:8731/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/
//                myCallingNameService = new CallingNameServiceClientV3("WSHttpBinding_ICallingNameService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/CallingNameService/CallingNameServiceV3/"));

//                http://oasislap7:8731/Design_Time_Addresses/IPTVServiceV3/IPTVService/
//                myIPTVService = new IPTVServiceClient("WSHttpBinding_IIPTVService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/IPTVServiceV3/IPTVService/"));

//                http://oasislap7:8731/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/
//                myODConferencingService = new ODConferencingServiceClient("WSHttpBinding_IODConferencingService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/"));

//                http://localhost:8731/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/
//                myLargeScaleConference = new LargeScaleConferenceClient("WSHttpBinding_ILargeScaleConference",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/"));

//                http://oasislap7:8731/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/
//                myTCMService = new TCMServiceClient("WSHttpBinding_ITCMService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/"));

//                http://localhost:8731/Design_Time_Addresses/VoiceMailServiceV4/Voicemail/
//                myVoiceMailServiceV4 = new VoiceMailServiceV4.VoicemailClient("WSHttpBinding_IVoicemail4",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/VoiceMailServiceV4/Voicemail/"));

//                http://oasislap7:8731/Design_Time_Addresses/CNAService/CNAServiceV3/
//                myCNAService = new CNAServiceClient("WSHttpBinding_ICNAService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/CNAService/CNAServiceV3/"));

//                http://oasislap7:8731/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/
//                mySubscriberServiceV3 = new SubscriberServiceV3.SubscriberServiceClient("WSHttpBinding_ISubscriberService3",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/"));

//                http://localhost:8731/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4
//                mySubscriberServiceV4 = new SubscriberServiceV4.SubscriberServiceClient("WSHttpBinding_ISubscriberService4",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4/"));

//                http://localhost:8731/Design_Time_Addresses/CallLoggingServiceV3/CLPService/
//                myCLPService = new CLPServiceClient("WSHttpBinding_ICLPService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/CallLoggingServiceV3/CLPService/"));

//                http://localhost:8731/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/
//                myLocalNumberPortabilityService = new LocalNumberPortabilityClient("WSHttpBinding_ILocalNumberPortability",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/"));

//                http://localhost:8731/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/
//                myOCMService = new OCMServiceClient("WSHttpBinding_IOCMService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/"));

//                http://localhost:8731/Design_Time_Addresses/UniveralCallManagementV3/UCMService/
//                myUCMService = new UCMServiceClient("WSHttpBinding_IUCMService",
//                    new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/UniveralCallManagementV3/UCMService/"));

//                myApAdmin.Login(SystemKey, UserName, PassWord);
//                myLoginInformation = myApAdmin.LoginAdv(SystemKey, UserName, PassWord);
//                myVersions = myServiceReportV1.GetAPmaxServiceVersions(myLoginInformation.LoginToken);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method Initialize encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool Close()
//        {
//            myApAdmin.Logout(myLoginInformation.LoginToken);
//            myCLPService.Close();
//            myLocalNumberPortabilityService.Close();
//            myOCMService.Close();
//            myUCMService.Close();
//            mySubscriberServiceV3.Close();
//            myODConferencingService.Close();
//            myTCMService.Close();
//            myLargeScaleConference.Close();
//            myIPTVService.Close();
//            myVoiceMailServiceV4.Close();
//            myCNAService.Close();
//            myVoiceMailServiceV3.Close();
//            myCallingNameService.Close();
//            myApAdmin.Close();
//            return true;
//        }

//        [ComVisible(true)]
//        public string GetApmaxVersions()
//        {
//            return myVersions.SerializeObjectToString();
//            <?xml version="1.0"?>
//            <ServiceVersions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
//              <ExtensionData />
//              <CallLogging>3</CallLogging>
//              <CallingName>4</CallingName>
//              <ChangeNumberAnnouncements>3</ChangeNumberAnnouncements>
//              <Iptv>3</Iptv>
//              <LargeScaleConference>-1</LargeScaleConference>
//              <LocalNumberPortability>-1</LocalNumberPortability>
//              <OnDemandConference>3</OnDemandConference>
//              <OriginatingCallManagement>1</OriginatingCallManagement>
//              <SipAcs>-1</SipAcs>
//              <Subscriber>4</Subscriber>
//              <TerminatingCallManagement>3</TerminatingCallManagement>
//              <UniversalCallManagement>3</UniversalCallManagement>
//              <Voicemail>3</Voicemail>
//              <WirelessOta>-1</WirelessOta>
//            </ServiceVersions>
//        }

//        #region ************************** VoiceMail *******************************
//        [ComVisible(true)]
//        public bool AddNewVoiceMailBox(string sPhoneNumber, string sMailBoxDescription, string sVmPackageName,
//            string sSubscriberName, string sMailBoxType, string sNotificationCenter, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length < 10)
//                {
//                    sError = "sPhoneNumber must be 10 digits." + Environment.NewLine;
//                    return false;
//                }

//                string sPackageGuid = "";
//                int iNotificationCenterID = -1;

//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                Here we are searching through all the packages and matching up the description with what we want.
//                Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.DescriptionField.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sPackageGuid = myPackageType.GuidField;
//                        break;
//                    }
//                }

//                if (sPackageGuid.Length == 0)
//                {
//                    sError = "Could not find this Package in the ApMax system." + Environment.NewLine +
//                              "Note: Case sentivity does not matter." + Environment.NewLine +
//                              "Please check that the correct package was sent to this method." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                Gets all the notification centers
//                NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                Because we need to send the centerid.
//                foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                {
//                    if (myNotificationCenterInfoType.DescriptionField.Equals(sNotificationCenter,
//                                                                             StringComparison.OrdinalIgnoreCase))
//                    {
//                        iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
//                        break;
//                    }
//                }

//                iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
//                if (iNotificationCenterID < 0)
//                {
//                    sError = "Unable to determine notification center from \"" + sNotificationCenter + "\"." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                If they would like specific settings set for the subscriber you could add xml to 
//                the Oasis.exe.config file under the ApmaxV3DefaultSubscriberType tag.
//                Then thus would pull these settings in from file and then you are set.
//                mySubscriberType = (SubscriberType)OasisUtilitiesHelper.GetConnectionString("ApmaxV3DefaultSubscriberType").HtmlDecode().Deserialize(typeof(SubscriberType));

//                VoicemailSubscriberType mySubscriberType = new VoicemailSubscriberType
//                {
//                    SubscriberName = sSubscriberName,
//                    SubscriberTimezone = VoiceMailServiceV3.Timezone_e.ApDefault,
//                    SubscriberDefaultPhoneNumber = sPhoneNumber,
//                    PlacementType =
//                        VoiceMailServiceV3.PlacementType_e.PlacementType_None
//                };

//                MailboxType myMailboxType = (MailboxType)Enum.Parse(typeof(MailboxType), sMailBoxType, true);

//                myVoiceMailServiceV3.AddNewVoiceMailBox(myLoginInformation.LoginToken, sPhoneNumber, sMailBoxDescription, sPackageGuid,
//                                        myMailboxType, iNotificationCenterID, sPhoneNumber, mySubscriberType);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddNewVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }


//        public bool AddNewVoiceMailBox2(string sPhoneNumber, string sMailBoxDescription, string sVmPackageName,
//            string sSubscriberName, string sMailBoxType, string sNotificationCenter, string BillingNumber, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length < 10)
//                {
//                    sError = "sPhoneNumber must be 10 digits." + Environment.NewLine;
//                    return false;
//                }

//                string sPackageGuid = "";
//                int iNotificationCenterID = -1;

//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                Here we are searching through all the packages and matching up the description with what we want.
//                Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.DescriptionField.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sPackageGuid = myPackageType.GuidField;
//                        break;
//                    }
//                }

//                if (sPackageGuid.Length == 0)
//                {
//                    sError = "Could not find this Package in the ApMax system." + Environment.NewLine +
//                              "Note: Case sentivity does not matter." + Environment.NewLine +
//                              "Please check that the correct package was sent to this method." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                Gets all the notification centers
//                NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                Because we need to send the centerid.
//                foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                {
//                    if (myNotificationCenterInfoType.DescriptionField.Equals(sNotificationCenter,
//                                                                             StringComparison.OrdinalIgnoreCase))
//                    {
//                        iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
//                        break;
//                    }
//                }

//                iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
//                if (iNotificationCenterID < 0)
//                {
//                    sError = "Unable to determine notification center from \"" + sNotificationCenter + "\"." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                If they would like specific settings set for the subscriber you could add xml to 
//                the Oasis.exe.config file under the ApmaxV3DefaultSubscriberType tag.
//                Then thus would pull these settings in from file and then you are set.
//                mySubscriberType = (SubscriberType)OasisUtilitiesHelper.GetConnectionString("ApmaxV3DefaultSubscriberType").HtmlDecode().Deserialize(typeof(SubscriberType));

//                VoicemailSubscriberType mySubscriberType = new VoicemailSubscriberType
//                {
//                    SubscriberName = sSubscriberName,
//                    BillingAccountNumber = BillingNumber,
//                    SubscriberTimezone = VoiceMailServiceV3.Timezone_e.ApDefault,
//                    SubscriberDefaultPhoneNumber = sPhoneNumber,
//                    PlacementType =
//                        VoiceMailServiceV3.PlacementType_e.PlacementType_None
//                };

//                MailboxType myMailboxType = (MailboxType)Enum.Parse(typeof(MailboxType), sMailBoxType, true);

//                myVoiceMailServiceV3.AddNewVoiceMailBox(myLoginInformation.LoginToken, sPhoneNumber, sMailBoxDescription, sPackageGuid,
//                                        myMailboxType, iNotificationCenterID, sPhoneNumber, mySubscriberType);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddNewVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool DeleteVoiceMailBox(string sVMPhone, bool DeleteSubscriber, ref string sError)
//        {
//            try
//            {
//                Check if want to delete VMbox
//                if (sVMPhone.Length != 0)
//                {
//                    This will get the current mailbox info before we delete the box.
//                    VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                        myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//                    If this is not null mailbox is present so we will delete.
//                    if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                    {
//                        sError = "Mailbox does not exist." + Environment.NewLine;
//                    }
//                    else
//                    {
//                        myVoiceMailServiceV3.DeleteVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField);

//                        if (DeleteSubscriber)
//                            return DeleteSubscriberByPhoneNumber(sVMPhone, ref sError);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool UpdateVoiceMailBoxPackage(string sPhoneNumber, string sVmPackageName, ref string sError)
//        {
//            try
//            {
//                string sPackageGuid = "";

//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                Here we are searching through all the packages and matching up the description with what we want.
//                Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.DescriptionField.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sPackageGuid = myPackageType.GuidField;
//                        break;
//                    }
//                }

//                if (sPackageGuid.Length == 0)
//                {
//                    sError = "Could not find this Package in the ApMax system." + Environment.NewLine +
//                              "Note: Case sentivity does not matter." + Environment.NewLine +
//                              "Please check that the correct package was sent to this method." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
//                myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
//                myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
//                myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
//                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
//                myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
//                myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
//                myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
//                myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
//                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
//                myVoiceMailBoxTypeArray[0].CallScreeningField = null;
//                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                myVoiceMailBoxTypeArray[0].ChildListField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
//                myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
//                myVoiceMailBoxTypeArray[0].DescriptionField = null;
//                myVoiceMailBoxTypeArray[0].DirectoryField = null;
//                myVoiceMailBoxTypeArray[0].DistributionListField = null;
//                myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
//                myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
//                myVoiceMailBoxTypeArray[0].ExtensionData = null;
//                myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
//                myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
//                myVoiceMailBoxTypeArray[0].ForwardingListField = null;
//                myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
//                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                myVoiceMailBoxTypeArray[0].IdField = null;
//                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                myVoiceMailBoxTypeArray[0].LanguageField = null;
//                myVoiceMailBoxTypeArray[0].LastAccessField = null;
//                myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
//                myVoiceMailBoxTypeArray[0].LoginField = null;
//                myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
//                myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
//                myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
//                myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
//                myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
//                myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
//                myVoiceMailBoxTypeArray[0].MessageCountField = null;
//                myVoiceMailBoxTypeArray[0].MessageListField = null;
//                myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
//                myVoiceMailBoxTypeArray[0].NameField = null;
//                myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
//                myVoiceMailBoxTypeArray[0].NotificationListField = null;
//                myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
//                myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
//                myVoiceMailBoxTypeArray[0].PagerListField = null;
//                myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
//                myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
//                myVoiceMailBoxTypeArray[0].ParentListField = null;
//                myVoiceMailBoxTypeArray[0].PasswordField = null;
//                myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
//                myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
//                myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
//                myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
//                myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
//                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
//                myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
//                myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

//                myVoiceMailBoxTypeArray[0].OptionsPackageField = sPackageGuid;


//                Per Innovatives request set the guid to null.
//                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                {
//                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                    {
//                        mySubInfoType.SubscriberGuidField = null;
//                    }

//                    mySubInfoTypeList.Add(mySubInfoType);
//                }

//                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);

//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateVoiceMailBoxPackage() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool UpdateVoiceMailBoxType(string sPhoneNumber, string sMailBoxType, string sInternetPassword,
//            string sInternetUserName, bool blnInternetAccess, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info before we delete the box.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present so we will delete.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox " + sPhoneNumber + " does not exist." + Environment.NewLine;
//                    return false;
//                }

//                if (!DeleteVoiceMailBox(sPhoneNumber, false, ref sError))
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                string sEmailAddress = "";
//                string sNotificationCenter = "";
//                foreach (var type in myVoiceMailBoxTypeArray[0].NotificationListField)
//                {
//                    if (type.AddressField.Equals(sPhoneNumber))
//                    {
//                        Gets all the notification centers
//                        NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                        Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                        Because we need to send the centerid.
//                        foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                        {
//                            if (myNotificationCenterInfoType.CenterIdField == type.CenterField)
//                            {
//                                sNotificationCenter = myNotificationCenterInfoType.DescriptionField;
//                                break;
//                            }
//                        }
//                    }

//                    if (type.CenterField.Equals(3))
//                        sEmailAddress = type.AddressField;
//                }

//                string sVmPackageName = "";
//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                Here we are searching through all the packages and matching up the description with what we want.
//                Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.GuidField.Equals(myVoiceMailBoxTypeArray[0].OptionsPackageField, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sVmPackageName = myPackageType.DescriptionField;
//                        break;
//                    }
//                }

//                if (!AddNewVoiceMailBox(sPhoneNumber, myVoiceMailBoxTypeArray[0].DescriptionField, sVmPackageName,
//                    myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField,
//                    sMailBoxType, sNotificationCenter, ref sError))
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                if (myVoiceMailBoxTypeArray[0].ChildListField.Length > 0)
//                {
//                    if (!UpdateVoiceSubMailbox(sPhoneNumber, myVoiceMailBoxTypeArray[0].ChildListField.Length,
//                        (int)myVoiceMailBoxTypeArray[0].MaxSubmailboxesField, ref sError))
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }
//                }

//                if (blnInternetAccess)
//                {
//                    if (!AddVoiceMailBoxInternetAccess(sPhoneNumber, sEmailAddress, sInternetPassword, sInternetUserName, ref sError))
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateVoiceMailBoxType() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool AddVoiceMailBoxInternetAccess(string sPhoneNumber, string sEmailAddress, string sInternetPassword, string sInternetUserName, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length != 0)
//                {
//                    This will get the current mailbox info.
//                    VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                        myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                    If this is not null mailbox is present.
//                    if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                    {
//                        sError = "Mailbox does not exist." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    Setting these to null will keep them from getting updated.
//                    myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
//                    myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
//                    myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                    myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
//                    myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
//                    myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                    myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                    myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
//                    myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
//                    myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
//                    myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
//                    myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
//                    myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                    myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
//                    myVoiceMailBoxTypeArray[0].CallScreeningField = null;
//                    myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                    myVoiceMailBoxTypeArray[0].ChildListField = null;
//                    myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                    myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                    myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                    myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
//                    myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
//                    myVoiceMailBoxTypeArray[0].DescriptionField = null;
//                    myVoiceMailBoxTypeArray[0].DirectoryField = null;
//                    myVoiceMailBoxTypeArray[0].DistributionListField = null;
//                    myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
//                    myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
//                    myVoiceMailBoxTypeArray[0].ExtensionData = null;
//                    myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
//                    myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
//                    myVoiceMailBoxTypeArray[0].ForwardingListField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                    myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                    Must have ID field.
//                    myVoiceMailBoxTypeArray[0].IdField = null;
//                    myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                    myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                    myVoiceMailBoxTypeArray[0].LanguageField = null;
//                    myVoiceMailBoxTypeArray[0].LastAccessField = null;
//                    myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
//                    myVoiceMailBoxTypeArray[0].LoginField = null;
//                    myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
//                    myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
//                    myVoiceMailBoxTypeArray[0].MessageCountField = null;
//                    myVoiceMailBoxTypeArray[0].MessageListField = null;
//                    myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
//                    myVoiceMailBoxTypeArray[0].NameField = null;
//                    myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
//                    myVoiceMailBoxTypeArray[0].NotificationListField = null;
//                    myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
//                    myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
//                    myVoiceMailBoxTypeArray[0].PagerListField = null;
//                    myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
//                    myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
//                    myVoiceMailBoxTypeArray[0].ParentListField = null;
//                    myVoiceMailBoxTypeArray[0].PasswordField = null;
//                    myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
//                    myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
//                    myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
//                    myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
//                    myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                    myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
//                    myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
//                    myVoiceMailBoxTypeArray[0].SubscriberListField = null;
//                    myVoiceMailBoxTypeArray[0].TimezoneField = null;
//                    myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;


//                    int iNotificationCenterID = -1;

//                    Gets all the notification centers
//                    NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                    Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                    Because we need to send the centerid.
//                    foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                    {
//                        if (myNotificationCenterInfoType.TypeField == NotificationCenterTypeEnum.typeEmail)
//                        {
//                            iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
//                            break;
//                        }
//                    }

//                    if (iNotificationCenterID < 0)
//                    {
//                        sError = "Unable to determine notification center from TypeField \"NotificationCenterType.typeEmail\"." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    List<NotificationInfoType> myNotificationInfoTypeList = new List<NotificationInfoType>();
//                    foreach (NotificationInfoType myNotificationInfoType in myVoiceMailBoxTypeArray[0].NotificationListField)
//                    {
//                        myNotificationInfoTypeList.Add(myNotificationInfoType);
//                    }

//                    NotificationInfoType myNotificationInfoType2 = new NotificationInfoType
//                    {
//                        AddressField = sEmailAddress,
//                        CenterField = iNotificationCenterID,
//                        EnabledField = true
//                    };
//                    myNotificationInfoTypeList.Add(myNotificationInfoType2);

//                    Darrin from hood wants to allow email deletion and add the email notification as well the below 2 lines does this.
//                    myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = true;
//                    myVoiceMailBoxTypeArray[0].NotificationListField = myNotificationInfoTypeList.ToArray();

//                    string sSubscriberGuid = "";
//                    string sSubscriberName = "";

//                    Per Innovatives request set the guid to null.
//                    List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                    foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                    {
//                        if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                        {
//                            sSubscriberName = mySubInfoType.SubscriberNameField;
//                            sSubscriberGuid = mySubInfoType.SubscriberGuidField;
//                            mySubInfoType.SubscriberGuidField = null;
//                        }

//                        mySubInfoTypeList.Add(mySubInfoType);
//                    }

//                    myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();
//                    myVoiceMailBoxTypeArray[0].TimezoneField = null;
//                    myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

//                    SubscriberServiceV3.SubscriberType mySubscriberType =
//                        mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//                    if (mySubscriberType == null)
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile("A subscriber with the default phone number does not exist." + Environment.NewLine, "ApMax");
//                        sError = "A subscriber with the default phone number does not exist." + Environment.NewLine;
//                        return false;
//                    }

//                    Get current InternetAccess Username information.
//                    SubscriberServiceV3.SubscriberInternetAccessType mySubscriberInternetAccessType =
//                        mySubscriberServiceV3.GetSubscriberInternetAccess(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//                    if (mySubscriberInternetAccessType == null)
//                    {
//                        If the username did not exist we need to add email etc...
//                        myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken,
//                            myVoiceMailBoxTypeArray[0], new VoicemailInternetAccessType
//                            {
//                                Password = sInternetPassword,
//                                UserName = sInternetUserName,
//                                ServiceEnabled = true
//                            });

//                        mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken,
//                                                                      new SubscriberServiceSubscriberTypeV3
//                                                                      {
//                                                                          SubscriberName = sSubscriberName,
//                                                                          SubscriberDefaultPhoneNumber = sPhoneNumber,
//                                                                          PlacementType = SubscriberPlacementType.PlacementType_None,
//                                                                          SubscriberGuid = sSubscriberGuid,
//                                                                          SubscriberEmail = sEmailAddress
//                                                                      });
//                    }
//                    else
//                    {
//                        else if username are equal modify status
//                        if (sInternetUserName == mySubscriberInternetAccessType.UserName)
//                        {
//                            This re-enables the internet access.
//                            myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], new VoicemailInternetAccessType
//                            {
//                                ServiceEnabled = true
//                            });

//                            This re-enables the internet access.
//                            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken,
//                                              new SubscriberServiceSubscriberTypeV3
//                                              {
//                                                  SubscriberName = sSubscriberName,
//                                                  SubscriberDefaultPhoneNumber = sPhoneNumber,
//                                                  PlacementType = SubscriberPlacementType.PlacementType_None,
//                                                  SubscriberGuid = sSubscriberGuid,
//                                                  SubscriberEmail = sEmailAddress,
//                                                  InternetAccess = new SubscriberInternetAccessType
//                                                  {
//                                                      ServiceEnabled = true
//                                                  }
//                                              });
//                        }
//                        else
//                        {
//                            DeleteVoiceMailBoxInternetAccess();
//                            AddVoiceMailBoxInternetAccess();
//                            This means they have internet access but the username is not what we expect.
//                            OasisUtilitiesHelper.WriteToErrorFile("A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine, "ApMax");
//                            sError = "A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine;
//                            return false;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddVoiceMailBoxInternetAccess encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool DeleteVoiceMailBoxInternetAccess(string sPhoneNumber, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length != 0)
//                {
//                    This will get the current mailbox info.
//                    VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                        myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                    If this is not null mailbox is present.
//                    if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                    {
//                        sError = "Mailbox does not exist." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    int iNotificationCenterID = -1;

//                    Gets all the notification centers
//                    NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                    Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                    Because we need to send the centerid.
//                    foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                    {
//                        if (myNotificationCenterInfoType.DescriptionField.Equals("email", StringComparison.OrdinalIgnoreCase))
//                        {
//                            iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
//                            break;
//                        }
//                    }

//                    if (iNotificationCenterID < 0)
//                    {
//                        sError = "Unable to determine notification center from \"email\"." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    List<NotificationInfoType> myNotificationInfoTypeList = new List<NotificationInfoType>();
//                    foreach (NotificationInfoType myNotificationInfoType in myVoiceMailBoxTypeArray[0].NotificationListField)
//                    {
//                        if (myNotificationInfoType.CenterField != iNotificationCenterID)
//                            myNotificationInfoTypeList.Add(myNotificationInfoType);
//                    }

//                    Darrin from hood wants to allow email deletion and add the email notification as well the below 2 lines does this.
//                    myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = false;
//                    myVoiceMailBoxTypeArray[0].NotificationListField = myNotificationInfoTypeList.ToArray();

//                    Setting these to null will keep them from getting updated.
//                    myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
//                    myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
//                    myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                    myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
//                    myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
//                    myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                    myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
//                    myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
//                    myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
//                    myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
//                    myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                    myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
//                    myVoiceMailBoxTypeArray[0].CallScreeningField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                    myVoiceMailBoxTypeArray[0].ChildListField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                    myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
//                    myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
//                    myVoiceMailBoxTypeArray[0].DescriptionField = null;
//                    myVoiceMailBoxTypeArray[0].DirectoryField = null;
//                    myVoiceMailBoxTypeArray[0].DistributionListField = null;
//                    myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
//                    myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
//                    myVoiceMailBoxTypeArray[0].ExtensionData = null;
//                    myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
//                    myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
//                    myVoiceMailBoxTypeArray[0].ForwardingListField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                    myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                    myVoiceMailBoxTypeArray[0].IdField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                    this one is important
//                    myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                    myVoiceMailBoxTypeArray[0].LanguageField = null;
//                    myVoiceMailBoxTypeArray[0].LastAccessField = null;
//                    myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
//                    myVoiceMailBoxTypeArray[0].LoginField = null;
//                    myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
//                    myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
//                    myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
//                    myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
//                    myVoiceMailBoxTypeArray[0].MessageCountField = null;
//                    myVoiceMailBoxTypeArray[0].MessageListField = null;
//                    myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
//                    myVoiceMailBoxTypeArray[0].NameField = null;
//                    myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
//                    myVoiceMailBoxTypeArray[0].NotificationListField = null;
//                    myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
//                    myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
//                    myVoiceMailBoxTypeArray[0].PagerListField = null;
//                    myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
//                    myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
//                    myVoiceMailBoxTypeArray[0].ParentListField = null;
//                    myVoiceMailBoxTypeArray[0].PasswordField = null;
//                    myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
//                    myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
//                    myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
//                    myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
//                    myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                    myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
//                    myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;

//                    string sSubscriberGuid = "";
//                    string sSubscriberName = "";
//                    SubAddressInfoType[] SubscriberAddressArray;

//                    Per Innovatives request set the guid to null.
//                    List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                    foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                    {
//                        if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                        {
//                            SubscriberAddressArray = mySubInfoType.SubscriberAddressListField;
//                            sSubscriberGuid = mySubInfoType.SubscriberGuidField;
//                            sSubscriberName = mySubInfoType.SubscriberNameField;
//                            mySubInfoType.SubscriberGuidField = null;
//                        }

//                        mySubInfoTypeList.Add(mySubInfoType);
//                    }
//                    SubscriberAddressArray[].AddressField;
//                    SubscriberAddressArray[].AddressTypeField;

//                    myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();
//                    myVoiceMailBoxTypeArray[0].TimezoneField = null;
//                    myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

//                    myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], new VoicemailInternetAccessType { ServiceEnabled = false });

//                    mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken,
//                                                                  new SubscriberServiceSubscriberTypeV3
//                                                                  {
//                                                                      SubscriberName = sSubscriberName,
//                                                                      SubscriberDefaultPhoneNumber = sPhoneNumber,
//                                                                      PlacementType = SubscriberPlacementType.PlacementType_None,
//                                                                      SubscriberGuid = sSubscriberGuid,
//                                                                      SubscriberEmail = "",
//                                                                      InternetAccess = new SubscriberInternetAccessType { ServiceEnabled = false }
//                                                                  });
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteVoiceMailBoxInternetAccess encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool AddVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                int iCurrentNumberOfSubBoxes = 0;
//                ChildInfoType[] myChildInfoTypeArray = myVoiceMailBoxTypeArray[0].ChildListField;
//                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
//                {
//                    Only look for submailbox types.  Ignore outdial types etc...  
//                    also when you delete a child it does not get rid of this it only clears the NameField...
//                    Dont ask me why... This is the most redicules API I have had to deal with.
//                    if (myChildInfoType.TypeField.Equals(VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber) &&
//                        myChildInfoType.NameField.Length > 0)
//                        iCurrentNumberOfSubBoxes++;
//                }

//                string sParentID = myVoiceMailBoxTypeArray[0].IdField;

//                myVoiceMailServiceV3.AddNewChildMailBox(myLoginInformation.LoginToken, sParentID, new ChildInfoType
//                {
//                    We have to get the next available digit field. The array is pre-populated and sorted with available fields.
//                    DigitField = sDigitField,
//                    NameField = sPhoneNumber + (iCurrentNumberOfSubBoxes + 1),
//                    DescriptionField = "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")",
//                    TypeField = VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber
//                }, MailboxType.FAMILY_CHILD);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddVoiceSubMailbox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool DeleteVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }
//                string sParentID = myVoiceMailBoxTypeArray[0].IdField;

//                ChildInfoType myChildInfoType = myVoiceMailBoxTypeArray[0].ChildListField.GetAddressFieldFromDigitField(sDigitField);
//                if (myChildInfoType == null)
//                {
//                    sError = "Cannot find submailbox with that DigitField ID." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                myVoiceMailServiceV3.DeleteChildMailBox(myLoginInformation.LoginToken, sParentID, myChildInfoType);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteVoiceSubMailbox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool UpdateVoiceSubMailbox(string sPhoneNumber, int iNumberOfSubMailboxes, int iMaxNumberOfSubMailboxesAllowed, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length != 0)
//                {
//                    if (iNumberOfSubMailboxes > iMaxNumberOfSubMailboxesAllowed)
//                    {
//                        sError = "Cannot create more mailboxes then the max allowed." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    This will get the current mailbox info.
//                    VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                        myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                    If this is not null mailbox is present.
//                    if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                    {
//                        sError = "Mailbox does not exist." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    Set some important values we will use later.
//                    Need to know current submailboxes.  excludes outdial types etc...
//                    int iCurrentNumberOfSubBoxes = 0;
//                    ChildInfoType[] myChildInfoTypeArray = myVoiceMailBoxTypeArray[0].ChildListField;
//                    foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
//                    {
//                        Only look for submailbox types.  Ignore outdial types etc...  
//                        also when you delete a child it does not get rid of this it only clears the NameField...
//                        Dont ask me why... This is the most redicules API I have had to deal with.
//                        if (myChildInfoType.TypeField.Equals(VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber) &&
//                            myChildInfoType.NameField.Length > 0)
//                            iCurrentNumberOfSubBoxes++;
//                    }

//                    string sParentID = myVoiceMailBoxTypeArray[0].IdField;
//                    int iNumberOfChildBoxes = myVoiceMailBoxTypeArray[0].ChildListField.Length;

//                    if (iNumberOfSubMailboxes == iCurrentNumberOfSubBoxes)
//                    {
//                        sError = "No mailboxes to update. Account has " + iNumberOfSubMailboxes + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    Remove Mailboxes
//                    if (iNumberOfSubMailboxes < iCurrentNumberOfSubBoxes)
//                    {
//                        Takes the total mailboxes and minus current to find out how many to delete
//                        int iNumberOfSubMailboxesToDelete = iCurrentNumberOfSubBoxes - iNumberOfSubMailboxes;
//                        for (int count = 1; count <= iNumberOfSubMailboxesToDelete; count++)
//                        {
//                            ChildInfoType myChildInfoType = myChildInfoTypeArray.GetChildInfoByHighestDigitField();
//                            List<ChildInfoType> myChildInfoTypeList = new List<ChildInfoType>();

//                            Must remove the child we found from this array so it is not selected again.
//                            foreach (ChildInfoType myChildInfo in myChildInfoTypeArray)
//                            {
//                                Adds all the childs that are not equal to the digitfield we are looking for.
//                                if (!myChildInfoType.DigitField.Equals(myChildInfo.DigitField))
//                                    myChildInfoTypeList.Add(myChildInfo);
//                            }
//                            myChildInfoTypeArray = myChildInfoTypeList.ToArray();

//                            if (myChildInfoType != null)
//                                myVoiceMailServiceV3.DeleteChildMailBox(myLoginInformation.LoginToken, sParentID, myChildInfoType);

//                            iCurrentNumberOfSubBoxes--;
//                        }
//                    }
//                    myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                    myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                    myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                    myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                    myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                    myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                    myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                    myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                    myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                    myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                    myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                    myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                    myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                    myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
//                    myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
//                    Important step:  Must set children to null since we deleted we dont want to re-add them.
//                    myVoiceMailBoxTypeArray[0].ChildListField = null;
//                    myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = iMaxNumberOfSubMailboxesAllowed;

//                    string sDN = "";

//                    Per Innovatives request set the guid to null.
//                    List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                    foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                    {
//                        if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                        {
//                            mySubInfoType.SubscriberGuidField = null;
//                            sDN = mySubInfoType.SubscriberDefaultPhoneNumberField;
//                        }

//                        mySubInfoTypeList.Add(mySubInfoType);
//                    }

//                    myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

//                    myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);


//                    iNumberOfChildBoxes = myVoiceMailBoxTypeArray[0].ChildListField.Length;
//                    iCurrentNumberOfSubBoxes = 0;
//                    foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
//                    {
//                        Only look for submailbox types.  Ignore outdial types etc...
//                        if (myChildInfoType.TypeField.Equals(VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber))
//                            iCurrentNumberOfSubBoxes++;
//                    }

//                    Add Mailboxes
//                    if (iNumberOfSubMailboxes > iCurrentNumberOfSubBoxes)
//                    {
//                        Always add one right away so we goto next available mailbox.
//                        iCurrentNumberOfSubBoxes++;

//                        GetAvailableDigitFieldsFromChildList method populate a integer array of avaible digitfields in a sorted order of lowest number first.
//                        List<int> AvaiableDigitFiledNumbers = GetAvailableDigitFieldsFromChildList(myChildInfoTypeArray, iMaxNumberOfSubMailboxesAllowed);

//                        Takes the total mailboxes and minus current to find out how many to add.
//                        for (int count = iCurrentNumberOfSubBoxes; count <= iNumberOfSubMailboxes; count++)
//                        {
//                            Example Description     <DescriptionField>(Child 1)</DescriptionField>
//                            myVoiceMailServiceV3.AddNewChildMailBox(myLoginInformation.LoginToken, sParentID, new ChildInfoType
//                            {
//                                We have to get the next available digit field. The array is pre-populated and sorted with available fields.
//                                DigitField = AvaiableDigitFiledNumbers[0].ToString(),
//                                NameField = sDN + count,
//                                DescriptionField = "(Child " + count + ")",
//                                TypeField = VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber
//                            }, MailboxType.FAMILY_CHILD);

//                            Since we use this one out of the list we must remove it.
//                            AvaiableDigitFiledNumbers.RemoveAt(0);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateVoiceSubMailbox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool UpdateVoiceMailBox(string sPhoneNumber, int iMaxMailboxTime, int iMaxMessages, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                Per Hood wants max MaxSubmailboxesField set to zero by default.
//                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                myVoiceMailBoxTypeArray[0].MaxMessagesField = iMaxMessages;
//                myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = iMaxMailboxTime;

//                Per Innovatives request set the guid to null.
//                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                {
//                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                        mySubInfoType.SubscriberGuidField = null;

//                    mySubInfoTypeList.Add(mySubInfoType);
//                }

//                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool UpdateVoiceMailBoxFull(string sPhoneNumber, string sVoiceMailBoxUnformatedXml, string sInternetAccessUnformatedXml, ref string sError)
//        {
//            try
//            {
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                Per Innovatives request set the guid to null.
//                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                {
//                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                    {
//                        mySubInfoType.SubscriberGuidField = null;
//                    }

//                    mySubInfoTypeList.Add(mySubInfoType);
//                }

//                VoiceMailBoxType myNewVoiceMailBoxTypeArray = (VoiceMailBoxType)sVoiceMailBoxUnformatedXml.DeSerializeStringToObject(typeof(VoiceMailBoxType));
//                if (myNewVoiceMailBoxTypeArray == null)
//                {
//                    sError = "Method UpdateVoiceMailBoxFull encountered exception: Unable to parse xml or invalid xml." +
//                        Environment.NewLine + "XML recieved: " + sVoiceMailBoxUnformatedXml + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                myNewVoiceMailBoxTypeArray.IdField = myVoiceMailBoxTypeArray[0].IdField;
//                myNewVoiceMailBoxTypeArray.SubscriberListField = mySubInfoTypeList.ToArray();

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myNewVoiceMailBoxTypeArray,
//                    (sInternetAccessUnformatedXml.Length > 0 ? (VoicemailInternetAccessType)sInternetAccessUnformatedXml.DeSerializeStringToObject(typeof(VoicemailInternetAccessType)) : null));
//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateVoiceMailBoxFull encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool AddOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber, string sOutDialRoutingNumber, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                myVoiceMailServiceV3.AddNewChildMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField, new ChildInfoType
//                {
//                    AddressField = sOutDialPhoneNumber,
//                    DigitField = sOutDialRoutingNumber,
//                    TypeField = VoiceMailServiceV3.AddressType.AddressTypeDN
//                }, MailboxType.OUTDIAL);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddOutDialNumber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool DeleteOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                Loop through all the child mailboxs and to find the box we want to delete.
//                foreach (ChildInfoType myChild in myVoiceMailBoxTypeArray[0].ChildListField)
//                {
//                    if (myChild.AddressField == sOutDialPhoneNumber)
//                    {
//                        myVoiceMailServiceV3.DeleteChildMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField, myChild);
//                        break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteOutDialNumber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool ReassignVMBoxNumber(string sOldPhoneNumber, string sNewPhoneNumber, string sMailBoxDescription,
//            string sSubscriberName, string sInternetPassword, string sInternetUserName,
//            bool blnInternetAccess, bool blnDeleteSubscriber, ref string sError)
//        {
//            try
//            {
//                //This will get the current mailbox info before we delete the box.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sOldPhoneNumber);
//                If this is not null mailbox is present so we will delete.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Old Mailbox " + sOldPhoneNumber + " does not exist." + Environment.NewLine;
//                    return false;
//                }

//                if (!DeleteVoiceMailBox(sOldPhoneNumber, (blnDeleteSubscriber ? true : false), ref sError))
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                string sEmailAddress = "";
//                string sNotificationCenter = "";
//                foreach (var type in myVoiceMailBoxTypeArray[0].NotificationListField)
//                {
//                    if (type.AddressField.Equals(sOldPhoneNumber))
//                    {
//                        Gets all the notification centers
//                        NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                        Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                        Because we need to send the centerid.
//                        foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                        {
//                            if (myNotificationCenterInfoType.CenterIdField == type.CenterField)
//                            {
//                                sNotificationCenter = myNotificationCenterInfoType.DescriptionField;
//                                break;
//                            }
//                        }
//                    }

//                    if (type.CenterField.Equals(3))
//                        sEmailAddress = type.AddressField;
//                }

//                string sVmPackageName = "";
//                //Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                //Here we are searching through all the packages and matching up the description with what we want.
//                //Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.GuidField.Equals(myVoiceMailBoxTypeArray[0].OptionsPackageField, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sVmPackageName = myPackageType.DescriptionField;
//                        break;
//                    }
//                }

//                if (!AddNewVoiceMailBox(sNewPhoneNumber, sMailBoxDescription, sVmPackageName,
//                    myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField,
//                    Enum.GetName(typeof(MailboxType), myVoiceMailBoxTypeArray[0].MailboxTypeField), sNotificationCenter, ref sError))
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                if (myVoiceMailBoxTypeArray[0].ChildListField.Length > 0)
//                {
//                    if (!UpdateVoiceSubMailbox(sNewPhoneNumber, myVoiceMailBoxTypeArray[0].ChildListField.Length,
//                        (int)myVoiceMailBoxTypeArray[0].MaxSubmailboxesField, ref sError))
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }
//                }

//                if (blnInternetAccess)
//                {
//                    if (!AddVoiceMailBoxInternetAccess(sNewPhoneNumber, sEmailAddress, sInternetPassword, sInternetUserName, ref sError))
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method ReassignVMBoxNumber() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool InternetPasswordReset(string sPhoneNumber, string sInternetPassword, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length != 0)
//                {
//                    SubscriberServiceV3.SubscriberType mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//                    if (mySubscriberType == null)
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile("A subscriber with the default phone number does not exist." + Environment.NewLine, "ApMax");
//                        sError = "A subscriber with the default phone number does not exist." + Environment.NewLine;
//                        return false;
//                    }

//                    SubscriberServiceV3.SubscriberInternetAccessType mySubscriberInternetAccessType = mySubscriberServiceV3.GetSubscriberInternetAccess(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//                    if (mySubscriberInternetAccessType == null)
//                    {
//                        OasisUtilitiesHelper.WriteToErrorFile("A subscriber with this guid does not exist." + Environment.NewLine, "ApMax");
//                        sError = "A subscriber with this guid does not exist." + Environment.NewLine;
//                        return false;
//                    }

//                    internet access is not returned from get subscriber
//                    mySubscriberType.InternetAccess = new SubscriberServiceV3.InternetAccessType
//                    {
//                        ServiceEnabled =
//                            mySubscriberInternetAccessType.InternetAccessEnabled,
//                        Password = sInternetPassword
//                    };

//                    mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberType);
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method InternetPasswordReset encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public bool VMPasswordReset(string sPhoneNumber, string sNewPin, ref string sError)
//        {
//            try
//            {
//                This will get the current mailbox info.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);
//                If this is not null mailbox is present.
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                {
//                    sError = "Mailbox does not exist." + Environment.NewLine;
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }

//                 change what we need to change here to myVMBoxInfo.
//                myVoiceMailBoxTypeArray[0].PasswordField = sNewPin;

//                myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
//                myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
//                myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
//                myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
//                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
//                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
//                myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
//                myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
//                myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
//                myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
//                myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
//                myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
//                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
//                myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
//                myVoiceMailBoxTypeArray[0].CallScreeningField = null;
//                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
//                myVoiceMailBoxTypeArray[0].ChildListField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
//                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
//                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
//                myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
//                myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
//                myVoiceMailBoxTypeArray[0].DescriptionField = null;
//                myVoiceMailBoxTypeArray[0].DirectoryField = null;
//                myVoiceMailBoxTypeArray[0].DistributionListField = null;
//                myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
//                myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
//                myVoiceMailBoxTypeArray[0].ExtensionData = null;
//                myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
//                myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
//                myVoiceMailBoxTypeArray[0].ForwardingListField = null;
//                myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
//                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
//                myVoiceMailBoxTypeArray[0].GreetingListField = null;
//                myVoiceMailBoxTypeArray[0].HolidayListField = null;
//                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
//                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
//                myVoiceMailBoxTypeArray[0].LanguageField = null;
//                myVoiceMailBoxTypeArray[0].LastAccessField = null;
//                myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
//                myVoiceMailBoxTypeArray[0].LoginField = null;
//                myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
//                myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
//                myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
//                myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
//                myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
//                myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
//                myVoiceMailBoxTypeArray[0].MessageCountField = null;
//                myVoiceMailBoxTypeArray[0].MessageListField = null;
//                myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
//                myVoiceMailBoxTypeArray[0].NameField = null;
//                myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
//                myVoiceMailBoxTypeArray[0].NotificationListField = null;
//                myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
//                myVoiceMailBoxTypeArray[0].PagerListField = null;
//                myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
//                myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
//                myVoiceMailBoxTypeArray[0].ParentListField = null;
//                myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
//                myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
//                myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
//                myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
//                myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
//                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
//                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
//                myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
//                myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
//                myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

//                Per Innovatives request set the guid to null.
//                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
//                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
//                {
//                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == sPhoneNumber)
//                    {
//                        mySubInfoType.SubscriberGuidField = null;
//                    }

//                    mySubInfoTypeList.Add(mySubInfoType);
//                }

//                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method VMPasswordReset() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public string GetVoiceMailBox(string sVMPhone)
//        {
//            try
//            {
//                This will get the current mailbox.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                    return "No Mail Box found.";

//                return myVoiceMailBoxTypeArray.SerializeObjectToString();
//            }
//            catch (Exception ex)
//            {
//                OasisUtilitiesHelper.WriteToErrorFile("Method GetVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine, "ApMax");
//                return "Method GetVoiceMailBox encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//            }
//        }

//        [ComVisible(true)]
//        public string GetVoiceMailBoxByID(string sMailboxID)
//        {
//            try
//            {
//                This will get the current mailbox.
//                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                    myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxId(myLoginInformation.LoginToken, sMailboxID);
//                if (myVoiceMailBoxTypeArray == null)
//                    return "No Mail Box found.";

//                if (myVoiceMailBoxTypeArray.Length == 0)
//                    return "No Mail Box found.";

//                return myVoiceMailBoxTypeArray.SerializeObjectToString();
//            }
//            catch (Exception ex)
//            {
//                OasisUtilitiesHelper.WriteToErrorFile("Method GetVoiceMailBoxByID encountered exception:" + Environment.NewLine + ex + Environment.NewLine, "ApMax");
//                return "Method GetVoiceMailBoxByID encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//            }
//        }

//        [ComVisible(true)]
//        public bool GetAllPackages(ref string sPackageList)
//        {
//            string sError = "";
//            try
//            {
//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                sPackageList = XmlHelper.SerializeObject(myPackageTypeArray, ref sError);
//                if (sError != "SUCCESS")
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method GetAllPackages() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//        [ComVisible(true)]
//        public string GetAllNotificationCenters()
//        {
//            return myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken).SerializeObjectToString();
//        }
//        #endregion

//        #region ************************** CallingName *******************************
//        [ComVisible(true)]
//        public bool AddCallerName(string sDN, string sCallerName, ref string sPresentation, ref string sResponse, ref string sError)
//        {
//            try
//            {
//                myCallingNameService.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);

//                CallingNameTypeV3[] myCallingNameType = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sDN);
//                sResponse = myCallingNameType.SerializeObjectToString();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }

        //[ComVisible(true)]
        //public bool DeleteCallerName(string sDN, ref string sError)
        //{
        //    try
        //    {
        //        CallingNameType[] myCallingNameType = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sDN);

        //        foreach (CallingNameType callingname in myCallingNameType)
        //        {
        //            if (callingname.cName.Equals(CallerName, StringComparison.CurrentCultureIgnoreCase))
        //                myCallingNameService.DeleteCallingEntry(myLoginInformation.LoginToken, sDN, 0, false);
        //        }
        //        myCallingNameService.DeleteCallingEntry(myLoginInformation.LoginToken, sDN, 0);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sError = "Method DeleteCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
        //        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
        //        return false;
        //    }
        //}

//ported
//        [ComVisible(true)]
//        public bool ModifyCallerName(string sDN, string sCallerName, string sPresentation, ref string sResponse, ref string sError)
//        {
//            try
//            {
//                myCallingNameService.DeleteCallingEntry(myLoginInformation.LoginToken, sDN, 0);

//                myCallingNameService.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);

//                CallingNameTypeV3[] myCallingNameType = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sDN);
//                sResponse = myCallingNameType.SerializeObjectToString();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method ModifyCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }

//ported
//        [ComVisible(true)]
//        public bool ReassignCallerName(string sDN, string sOldDN, ref string sResponse, ref string sError)
//        {
//            try
//            {
//                CallingNameTypeV3[] myCallingNameTypeArray = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sOldDN);
//                foreach (CallingNameTypeV3 myCallingNameType in myCallingNameTypeArray)
//                {

//                    myCallingNameService.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, myCallingNameType.Presentation, myCallingNameType.CName);
//                }

//                myCallingNameService.DeleteCallingEntry(myLoginInformation.LoginToken, sOldDN, 0);

//                myCallingNameTypeArray = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sDN);
//                sResponse = myCallingNameTypeArray.SerializeObjectToString();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method ReassignCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }

//        [ComVisible(true)]
//        public bool GetCallerName(string sDN, ref string sResponse, ref string sError)
//        {
//            try
//            {
//                CallingNameTypeV3[] myCallingNameTypeArray = myCallingNameService.GetCallingEntries(myLoginInformation.LoginToken, sDN);
//                sResponse = myCallingNameTypeArray.SerializeObjectToString();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method GetCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }

//        #endregion

//        #region ************************** Subscriber Mangement *******************************

//ported
//        [ComVisible(true)]
//        public string GetSubscriber(string sPhoneNumber)
//        {
//            try
//            {
//                sPhoneNumber = sPhoneNumber.PadLeft(10, '0');

//                SubscriberServiceV3.SubscriberType mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//                if (mySubscriberType == null)
//                {
//                    OasisUtilitiesHelper.WriteToErrorFile("A subscriber with the default phone number does not exist." + Environment.NewLine, "ApMax");
//                    return "A subscriber with the default phone number does not exist." + Environment.NewLine;
//                }

//                SubscriberServiceV3.SubscriberInternetAccessType mySubscriberInternetAccessType =
//                    mySubscriberServiceV3.GetSubscriberInternetAccess(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//                if (mySubscriberInternetAccessType == null)
//                {
//                    return "No Internet Access Found." + Environment.NewLine + mySubscriberType.SerializeObjectToString() + Environment.NewLine;
//                }

//                return mySubscriberInternetAccessType.SerializeObjectToString() + Environment.NewLine + mySubscriberType.SerializeObjectToString() + Environment.NewLine;
//            }
//            catch (Exception ex)
//            {
//                OasisUtilitiesHelper.WriteToErrorFile("Method GetSubscriber encountered exception:" + Environment.NewLine + ex + Environment.NewLine, "ApMax");
//                return "Method GetSubscriber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//            }
//        }


//ported
//        [ComVisible(true)]
//        public bool UpdateSubscriber(string sPhoneNumber, string sEmailAddress, string sSubscriberName, ref string sError)
//        {
//            try
//            {
//                if (sPhoneNumber.Length != 0)
//                {
//                    SubscriberServiceV3.SubscriberType mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);

//                    if (mySubscriberType == null)
//                    {
//                        sError = "A subscriber with the default phone number does not exist." + Environment.NewLine;
//                        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                        return false;
//                    }

//                    mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken,
//                                                                  new SubscriberServiceSubscriberTypeV3
//                                                                  {
//                                                                      SubscriberName = sSubscriberName,
//                                                                      SubscriberDefaultPhoneNumber = sPhoneNumber,
//                                                                      PlacementType = SubscriberPlacementType.PlacementType_None,
//                                                                      SubscriberGuid = mySubscriberType.SubscriberGuid,
//                                                                      SubscriberEmail = sEmailAddress
//                                                                  });
//                }
//            }
//            catch (Exception ex)
//            {
//                sError = "Method UpdateSubscriber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//ported
//        [ComVisible(true)]
//        public bool DeleteSubscriberByGuid(string sGuid, ref string sError)
//        {
//            try
//            {
//                mySubscriberServiceV3.RemoveSubscriberProv(myLoginInformation.LoginToken,
//                    new SubscriberServiceSubscriberTypeV3
//                    {
//                        SubscriberGuid = sGuid,
//                        PlacementType = SubscriberPlacementType.PlacementType_None
//                    });
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteSubscriberByGuid encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }

//ported
//        [ComVisible(true)]
//        public bool DeleteSubscriberByPhoneNumber(string sPhoneNumber, ref string sError)
//        {
//            try
//            {
//                mySubscriberServiceV3.RemoveSubscriberProv(myLoginInformation.LoginToken,
//                    new SubscriberServiceSubscriberTypeV3
//                    {
//                        SubscriberDefaultPhoneNumber = sPhoneNumber,
//                        PlacementType = SubscriberPlacementType.PlacementType_None
//                    });
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteSubscriberByPhoneNumber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }


//ported
//        [ComVisible(true)]
//        public void ReassignSubscriber(string sNewPhoneNumber, string sOldPhoneNumber, string sInternetPassword)
//        {
//            string sError = "";

//            SubscriberServiceSubscriberTypeV3 mySubscriberServiceSubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sOldPhoneNumber);
//            if (mySubscriberServiceSubscriberType == null)
//                throw new Exception("Subscriber does not exist for DN " + sOldPhoneNumber + ".");

//            SubInternetAccessType mySubscriberInternetAccessType = mySubscriberServiceV3.GetSubscriberInternetAccess(myLoginInformation.LoginToken,
//                mySubscriberServiceSubscriberType.SubscriberGuid);

//            if (mySubscriberInternetAccessType != null)
//            {
//                internet access is not returned from get subscriber
//                mySubscriberServiceSubscriberType.InternetAccess = new SubscriberServiceV3.InternetAccessType
//                {
//                    ServiceEnabled = mySubscriberInternetAccessType.InternetAccessEnabled,
//                    Password = sInternetPassword,
//                    UserName = mySubscriberInternetAccessType.UserName
//                };
//            }

//            //Creates new subscriber.
//            mySubscriberServiceSubscriberType.SubscriberGuid = "";
//            mySubscriberServiceSubscriberType.SubscriberDefaultPhoneNumber = sNewPhoneNumber;
//            mySubscriberServiceSubscriberType.ServiceInformation = null;
//            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberServiceSubscriberType);

//            This will get the current mailbox info before we delete the box.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sOldPhoneNumber);
//            //If this is not null mailbox is present so we will delete.
//            if (myVoiceMailBoxTypeArray != null)
//            {
//                if (!DeleteVoiceMailBox(sOldPhoneNumber, false, ref sError))
//                    throw new Exception(sError);

//                string sNotificationCenter = "";
//                foreach (var type in myVoiceMailBoxTypeArray[0].NotificationListField)
//                {
//                    if (type.AddressField.Equals(sOldPhoneNumber))
//                    {
//                        //Gets all the notification centers
//                        NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);

//                        //Here we are searching through all the NotificationCenters and matching up the description with what we want.
//                        //Because we need to send the centerid.
//                        foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
//                        {
//                            if (myNotificationCenterInfoType.CenterIdField == type.CenterField)
//                            {
//                                sNotificationCenter = myNotificationCenterInfoType.DescriptionField;
//                                break;
//                            }
//                        }
//                    }
//                }

//                string sVmPackageName = "";
//                Gets a list of all available vm packages
//                PackageType[] myPackageTypeArray = myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);

//                Here we are searching through all the packages and matching up the description with what we want.
//                Because we need to send the guid of the description.
//                foreach (PackageType myPackageType in myPackageTypeArray)
//                {
//                    if (myPackageType.GuidField.Equals(myVoiceMailBoxTypeArray[0].OptionsPackageField, StringComparison.OrdinalIgnoreCase))
//                    {
//                        sVmPackageName = myPackageType.DescriptionField;
//                        break;
//                    }
//                }

//                if (!AddNewVoiceMailBox(sNewPhoneNumber, myVoiceMailBoxTypeArray[0].DescriptionField, sVmPackageName,
//                    myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField,
//                    Enum.GetName(typeof(MailboxType), myVoiceMailBoxTypeArray[0].MailboxTypeField), sNotificationCenter, ref sError))
//                    throw new Exception(sError);
//            }

//            ConferenceType myConferenceType = myODConferencingService.GetConferenceSubByAddress(myLoginInformation.LoginToken, sOldPhoneNumber);
//            if (myConferenceType != null)
//            {
//                ODSubscriberType myODSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(ODSubscriberType)) as ODSubscriberType;
//                myConferenceType.SubscriberAddress = sNewPhoneNumber;
//                myConferenceType.SubscriberGuid = "";
//                Add Conference
//                myODConferencingService.AddConferenceSub(myLoginInformation.LoginToken, myConferenceType, myODSubscriberType);

//                Delete Conference
//                myODConferencingService.DeleteConferenceBySubAddress(myLoginInformation.LoginToken, sOldPhoneNumber);
//            }

//            CallLoggingType[] myCallLoggingTypeArray = myCLPService.FindCallLoggingSubscribers(myLoginInformation.LoginToken, sOldPhoneNumber);
//            if (myCallLoggingTypeArray.Length > 0)
//            {
//                CPLSubscriberType myCPLSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(CPLSubscriberType)) as CPLSubscriberType;
//                myCallLoggingTypeArray[0].SubscriberDn = sNewPhoneNumber;
//                myCallLoggingTypeArray[0].SubscriberID = "";
//                Add CLP
//                myCLPService.AddClpSubscriberRecord(myLoginInformation.LoginToken,
//                    new CallLoggingType
//                    {
//                        EmailAddress = myCallLoggingTypeArray[0].EmailAddress,
//                        SubscriberDn = sNewPhoneNumber,
//                        WebPortalEnabled = myCallLoggingTypeArray[0].WebPortalEnabled
//                    },
//                        myCPLSubscriberType);

//                Delete Conference
//                myCLPService.RemoveClpSubscriberRecord(myLoginInformation.LoginToken, sOldPhoneNumber);
//            }

//            OCMType myOCMType = myOCMService.GetSubscriberBySubGuid(myLoginInformation.LoginToken, mySubscriberServiceSubscriberType.SubscriberGuid);
//            if (myOCMType != null)
//            {
//                OCMSubscriberType myOCMSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(OCMSubscriberType)) as OCMSubscriberType;
//                myOCMType.SubscriberGuid = "";
//                Add OCM
//                myOCMService.AddSubscriberRecord(myLoginInformation.LoginToken, myOCMType, myOCMSubscriberType);

//                Delete OCM
//                myOCMService.DeleteSubscriberRecord(myLoginInformation.LoginToken, mySubscriberServiceSubscriberType.SubscriberGuid);
//            }

//            UCMType myUCMType = myUCMService.GetUniversalCallManagerNumber(myLoginInformation.LoginToken, sOldPhoneNumber);
//            if (myUCMType != null)
//            {
//                UCMSubscriberType myUCMSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(UCMSubscriberType)) as UCMSubscriberType;
//                myUCMType.SubscriberID = "";
//                Add OCM
//                myUCMService.AddUniversalCallManagerSubscriber(myLoginInformation.LoginToken, myUCMType, myUCMSubscriberType);

//                Delete OCM
//                myUCMService.DeleteUniversalCallManagerSubscriber(myLoginInformation.LoginToken, sOldPhoneNumber);
//            }

//            TCMType myTCMType = myTCMService.GetTCMSubscriber(myLoginInformation.LoginToken, sOldPhoneNumber);
//            if (myTCMType != null)
//            {
//                TCMSubscriberType myTCMSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(TCMSubscriberType)) as TCMSubscriberType;
//                myTCMType.SubscriberDn = sNewPhoneNumber;
//                myTCMType.SubscriberGuid = "";
//                Add TCM
//                myTCMService.AddTCMSubscriber(myLoginInformation.LoginToken, sNewPhoneNumber, (bool)myTCMType.TcmFeatureEnabled, (bool)myTCMType.DndFeatureEnabled, myTCMSubscriberType);

//                Delete TCM
//                myTCMService.DeleteTCMSubscriber(myLoginInformation.LoginToken, sOldPhoneNumber);
//            }

//            Removes the old number.
//            mySubscriberServiceV3.RemoveSubscriberProv(myLoginInformation.LoginToken,
//            new SubscriberServiceSubscriberTypeV3
//            {
//                SubscriberDefaultPhoneNumber = sOldPhoneNumber,
//                PlacementType = SubscriberPlacementType.PlacementType_None
//            });
//        }


//ported
//        [ComVisible(true)]
//        public void AddorUpdateSubscriber(string sSubscriberTypeUnformatedXml, string sInternetAccessTypeUnformatedXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = (SubscriberServiceSubscriberTypeV3)sSubscriberTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberServiceSubscriberTypeV3));
//            mySubscriberType.InternetAccess = (SubscriberInternetAccessType)sInternetAccessTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberInternetAccessType));
//            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberType);
//        }

//        #endregion


//ported
//        #region ************************** ScreenPop *******************************
        //[ComVisible(true)]
        //public bool AddScreenPop(string sDN, ref string sError)
        //{
        //    try
        //    {
        //        myCallingNameService.InsertScreenPopEntry(myLoginInformation.LoginToken, "NPANXX", "description");
        //        myCallingNameService.InsertScreenPopSubscriber(myLoginInformation.LoginToken, sDN, true);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sError = "Method AddScreenPop() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
        //        OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
        //        return false;
        //    }
        //}

//        [ComVisible(true)]
//        public bool DeleteScreenPop(string sDN, ref string sError)
//        {
//            try
//            {
//                myCallingNameService.DeleteScreenPopEntry(myLoginInformation.LoginToken, "NPANXX");
//                myCallingNameService.DeleteScreenPopSubscriber(myLoginInformation.LoginToken, sDN);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteScreenPop() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }


//ported
//        [ComVisible(true)]
//        public bool GetAllScreenPopEntries(ref string sResponse)
//        {
//            try
//            {
//                CallingNameServiceV3.ScreenPopType[] myScreenPopType = myCallingNameService.GetAllScreenPopEntries(myLoginInformation.LoginToken);
//                sResponse = myScreenPopType.SerializeObjectToString();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                sResponse = "Method GetCallerName() encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sResponse, "ApMax");
//                return false;
//            }
//        }
//        #endregion

//        #region ************************** Calling Number Announcement CNA *******************************

//ported
//        [ComVisible(true)]
//        public bool AddCnaNumber(string sXml, ref string sError)
//        {
//            try
//            {
//                var Object = (CnaInfo)sXml.DeSerializeStringToObject(typeof(CnaInfo));

//                DateTime currentDate = DateTime.Now;
//                TimeZone localZone = TimeZone.CurrentTimeZone;
//                TimeSpan currentOffset = localZone.GetUtcOffset( currentDate );
//                DateTime.Now + currentOffset;
//                DateTime.UtcNow;
//                DateTime myDateTime = currentDate.ToLocalTime();
//                DateTime.Parse("2009-10-28T16:32:19.1217873-05:00");
//                currentDate.Add();
//                currentDate.GetDateTimeFormats
//                CnaInfo[] myCnaInfoArray = myCNAService.GetAllCnaAnnouncements(myLoginInformation.LoginToken);
//                foreach (CnaInfo myCnaInfo in myCnaInfoArray)
//                {
//                    if (myCnaInfo.FromNumber == Object.FromNumber)
//                    {
//                        myCNAService.DeleteCnaNumber(myLoginInformation.LoginToken, myCnaInfo.FromNumber, myCnaInfo.ToNumber);
//                    }
//                }

//                myCNAService.SetCnaNumber(myLoginInformation.LoginToken, Object);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method AddCnaNumber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//            return true;
//        }


//ported
//        [ComVisible(true)]
//        public bool DeleteCnaNumber(string sFromPhoneNumber, string sToPhoneNumber, ref string sError)
//        {
//            try
//            {
//                myCNAService.DeleteCnaNumber(myLoginInformation.LoginToken, sFromPhoneNumber, sToPhoneNumber);
//            }
//            catch (Exception ex)
//            {
//                sError = "Method DeleteCnaNumber encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, public"ApMax");
//                return false;
//            }
//            return true;
//        }

//ported
//        [ComVisible(true)]
//        public bool GetCnaAnnouncement(string sFromPhoneNumber, string sToPhoneNumber, ref string sResponse, ref string sError)
//        {
//            try
//            {
//                CnaInfo[] myCnaInfoArray = myCNAService.GetAllCnaAnnouncements(myLoginInformation.LoginToken);
//                foreach (CnaInfo myCnaInfo in myCnaInfoArray)
//                {
//                    if (myCnaInfo.FromNumber == sFromPhoneNumber && myCnaInfo.ToNumber == sToPhoneNumber)
//                    {
//                        sResponse = myCnaInfo.SerializeObjectToString();
//                        return true;
//                    }
//                }
//                sError = "No results found.";
//                return false;
//            }
//            catch (Exception ex)
//            {
//                sError = "Method GetCnaAnnouncement encountered exception:" + Environment.NewLine + ex + Environment.NewLine;
//                OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
//                return false;
//            }
//        }
//        #endregion

//        #region ************************** IPTV *******************************

//ported
//        [ComVisible(true)]
//        public void SetIPTVAccount(string sIPTVAccountTypeUnformatedXml, string sSubscriberTypeUnformatedXml, string sServiceInfoTypeUnformatedXml)
//        {
//            //  <IPTVAccountType>
//            //    <ExtensionData />
//            //    <AccountDescription />
//            //    <Active>true</Active>
//            //    <ChannelPackageList>
//            //      <ChannelPackageType>
//            //        <ExtensionData />
//            //        <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
//            //        <PackageName>Test</PackageName>
//            //      </ChannelPackageType>
//            //    </ChannelPackageList>
//            //    <CurrentAmountCharged>0</CurrentAmountCharged>
//            //    <DeactivateReason />
//            //    <FIPSCountyCode>35</FIPSCountyCode>
//            //    <FIPSStateCode>46</FIPSStateCode>
//            //    <MaxBandwidthKbps>0</MaxBandwidthKbps>
//            //    <MaxChargingLimit>0</MaxChargingLimit>
//            //    <PurchasePIN />
//            //    <RatingPIN />
//            //    <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
//            //    <ServiceReference>6059974200</ServiceReference>
//            //    <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
//            //    <SubscriberName>CHR Solutions</SubscriberName>
//            //  </IPTVAccountType>

//            IPTVSubscriberType myIPTVSubscriberType = (IPTVSubscriberType)sSubscriberTypeUnformatedXml.Deserialize(typeof(IPTVSubscriberType));
//            if (myIPTVSubscriberType == null) throw new Exception("Could not deserialize SubscriberType xml. XML Sent: " + sSubscriberTypeUnformatedXml);

//            //This is in case the customer does not have a phone.  They then can use the account number which is only 7 digits but AP requires 10.
//            string subscriberDefaultPhoneNumber = myIPTVSubscriberType.SubscriberDefaultPhoneNumber.PadLeft(10, '0');

//            IPTVAccountType myIPTVAccountType = (IPTVAccountType)sIPTVAccountTypeUnformatedXml.Deserialize(typeof(IPTVAccountType));
//            if (myIPTVAccountType == null) throw new Exception("Could not deserialize IPTVAccountType xml. XML Sent: " + sIPTVAccountTypeUnformatedXml);

//            //Get the existing subscriber info.
//            SubscriberServiceSubscriberTypeV4 mySubscriberServiceSubscriberType = mySubscriberServiceV4.GetSubscriberByNumberProv(myLoginInformation.LoginToken, subscriberDefaultPhoneNumber);
//            if (mySubscriberServiceSubscriberType == null)
//            {
//                SubscriberServiceV4.ServiceInfoType myServiceInfoType = (SubscriberServiceV4.ServiceInfoType)sServiceInfoTypeUnformatedXml.Deserialize(typeof(SubscriberServiceV4.ServiceInfoType));
//                if (myServiceInfoType == null) throw new Exception("Could not deserialize ServiceInfoType xml. XML Sent: " + sServiceInfoTypeUnformatedXml);

//                mySubscriberServiceSubscriberType = (SubscriberServiceSubscriberTypeV4)sSubscriberTypeUnformatedXml.Deserialize(typeof(SubscriberServiceSubscriberTypeV4));

//                mySubscriberServiceV4.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberServiceSubscriberType);
//                mySubscriberServiceSubscriberType = mySubscriberServiceV4.GetSubscriberByNumberProv(myLoginInformation.LoginToken, subscriberDefaultPhoneNumber);
//            }

//            //Always set the guid.
//            myIPTVSubscriberType.SubscriberGuid = mySubscriberServiceSubscriberType.SubscriberGuid;
//            myIPTVAccountType.SubscriberID = mySubscriberServiceSubscriberType.SubscriberGuid;

//            myIPTVService.SetIPTVAccount(myLoginInformation.LoginToken, myIPTVAccountType, myIPTVSubscriberType);
//        }

//ported
        //[ComVisible(true)]
        //public void SetIPTVAccount(string sIPTVAccountTypeUnformatedXml, string sSubscriberTypeUnformatedXml)
        //{
        //      //<IPTVAccountType>
        //      //  <ExtensionData />
        //      //  <AccountDescription />
        //      //  <Active>true</Active>
        //      //  <ChannelPackageList>
        //      //    <ChannelPackageType>
        //      //      <ExtensionData />
        //      //      <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
        //      //      <PackageName>Test</PackageName>
        //      //    </ChannelPackageType>
        //      //  </ChannelPackageList>
        //      //  <CurrentAmountCharged>0</CurrentAmountCharged>
        //      //  <DeactivateReason />
        //      //  <FIPSCountyCode>35</FIPSCountyCode>
        //      //  <FIPSStateCode>46</FIPSStateCode>
        //      //  <MaxBandwidthKbps>0</MaxBandwidthKbps>
        //      //  <MaxChargingLimit>0</MaxChargingLimit>
        //      //  <PurchasePIN />
        //      //  <RatingPIN />
        //      //  <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
        //      //  <ServiceReference>6059974200</ServiceReference>
        //      //  <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
        //      //  <SubscriberName>CHR Solutions</SubscriberName>
        //      //</IPTVAccountType>

        //    IPTVSubscriberType myIPTVSubscriberType = (IPTVSubscriberType)sSubscriberTypeUnformatedXml.DeSerializeStringToObject(typeof(IPTVSubscriberType));
        //    if (myIPTVSubscriberType == null) throw new Exception("Could not deserialize SubscriberType xml. XML Sent: " + sSubscriberTypeUnformatedXml);

        //    //May need this if the subscriber already exists.
        //    string BillingServiceAddress = myIPTVSubscriberType.BillingServiceAddress;

        //    //This is in case the customer does not have a phone.  They then can use the account number which is only 7 digits but AP requires 10.
        //    string subscriberDefaultPhoneNumber = myIPTVSubscriberType.SubscriberDefaultPhoneNumber.PadLeft(10, '0');

        //    //IPTVAccountType myIPTVAccountType = (IPTVAccountType)sIPTVAccountTypeUnformatedXml.DeSerializeStringToObject(typeof(IPTVAccountType));
        //    if (myIPTVAccountType == null) throw new Exception("Could not deserialize IPTVAccountType xml. XML Sent: " + sIPTVAccountTypeUnformatedXml);

        //    //Get the existing subscriber info.
        //    SubscriberServiceSubscriberTypeV4 mySubscriberServiceSubscriberType = mySubscriberServiceV4.GetSubscriberByNumberProv(myLoginInformation.LoginToken, subscriberDefaultPhoneNumber);
        //    if (mySubscriberServiceSubscriberType != null)
        //    {
        //        myIPTVSubscriberType = (IPTVSubscriberType)mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(IPTVSubscriberType));

        //        //Always set the guid on the account if the subscriber already exists.
        //        myIPTVAccountType.SubscriberID = myIPTVSubscriberType.SubscriberGuid;

        //        //Always set the Billing Service Address on the account if the subscriber already exists.
        //        myIPTVSubscriberType.BillingServiceAddress = BillingServiceAddress;
        //    }

        //    myIPTVService.SetIPTVAccount(myLoginInformation.LoginToken, myIPTVAccountType, myIPTVSubscriberType);
        //}


//ported
//        [ComVisible(true)]
//        public void DeleteIPTVAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = null;

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            myIPTVService.DeleteIPTVAccount(myLoginInformation.LoginToken, myCurrentIPTVAccountType.SubscriberID, sServiceReference);

//            if (blnDeleteSubscriber)
//            {
//                string sError = "";
//                if (!DeleteSubscriberByGuid(myCurrentIPTVAccountType.SubscriberID, ref sError))
//                    throw new Exception("Subsciber deletion failed: " + sError);
//            }
//        }


//ported
//        [ComVisible(true)]
//        public void ForceDeleteIPTVAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = null;

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            myIPTVService.ForceDeleteIPTVAccount(myLoginInformation.LoginToken, myCurrentIPTVAccountType.SubscriberID, sServiceReference);

//            if (blnDeleteSubscriber)
//            {
//                string sError = "";
//                if (!DeleteSubscriberByGuid(myCurrentIPTVAccountType.SubscriberID, ref sError))
//                    throw new Exception("Subsciber deletion failed: " + sError);
//            }
//        }


//ported
//        [ComVisible(true)]
//        public void SetIPTVChannelPackageList(string sSubAddress, string sServiceReference, string sChannelPackageListUnformatedXml)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = new IPTVAccountType();

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            Get the existing subscriber info.
//            SubscriberServiceSubscriberTypeV3 mySubscriberServiceSubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sSubAddress);
//            if (mySubscriberServiceSubscriberType == null)
//                throw new Exception("Unable to find subscriber account with this SubAddress: " + sSubAddress + ".\r\n");

//            This casts at SubscriberServiceSubscriberTypeV3 to a IPTVSubscriberType.
//            IPTVSubscriberType myIPTVSubscriberType = (IPTVSubscriberType)mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(IPTVSubscriberType));

//            <?xml version="1.0"?>
//            <ArrayOfChannelPackageType xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
//              <ChannelPackageType>
//                <PackageID>test</PackageID>
//                <PackageName>test</PackageName>
//              </ChannelPackageType>
//              <ChannelPackageType>
//                <PackageID>test4</PackageID>
//                <PackageName>test4</PackageName>
//              </ChannelPackageType>
//            </ArrayOfChannelPackageType>
//            myCurrentIPTVAccountType.ChannelPackageList = (ChannelPackageType[])sChannelPackageListUnformatedXml.DeSerializeStringToObject(typeof(ChannelPackageType[]));
//            myCurrentIPTVAccountType.SetTopBoxList = null;

//            myIPTVService.SetIPTVAccount(myLoginInformation.LoginToken, myCurrentIPTVAccountType, myIPTVSubscriberType);
//        }


//ported
//        [ComVisible(true)]
//        public void DisableIPTVAccount(string sSubAddress, string sServiceReference)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = new IPTVAccountType();

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            Get the existing subscriber info.
//            SubscriberServiceSubscriberTypeV3 mySubscriberServiceSubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sSubAddress);
//            if (mySubscriberServiceSubscriberType == null)
//                throw new Exception("Unable to find subscriber account with this SubAddress: " + sSubAddress + ".\r\n");

//            This casts at SubscriberServiceSubscriberTypeV3 to a IPTVSubscriberType.
//            IPTVSubscriberType myIPTVSubscriberType = (IPTVSubscriberType)mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(IPTVSubscriberType));

//            myCurrentIPTVAccountType.Active = false;
//            myCurrentIPTVAccountType.SetTopBoxList = null;

//            if (myCurrentIPTVAccountType.PurchasePIN.Length < 4)
//                myCurrentIPTVAccountType.PurchasePIN = "0000";

//            if (myCurrentIPTVAccountType.RatingPIN.Length < 4)
//                myCurrentIPTVAccountType.RatingPIN = "0000";

//            myIPTVService.SetIPTVAccount(myLoginInformation.LoginToken, myCurrentIPTVAccountType, myIPTVSubscriberType);
//        }


//ported
//        [ComVisible(true)]
//        public void EnableIPTVAccount(string sSubAddress, string sServiceReference)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = new IPTVAccountType();

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTX Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            Get the existing subscriber info.
//            SubscriberServiceSubscriberTypeV3 mySubscriberServiceSubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sSubAddress);
//            if (mySubscriberServiceSubscriberType == null)
//                throw new Exception("Unable to find subscriber account with this SubAddress: " + sSubAddress + ".\r\n");

//            This casts at SubscriberServiceSubscriberTypeV3 to a IPTVSubscriberType.
//            IPTVSubscriberType myIPTVSubscriberType = (IPTVSubscriberType)mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(IPTVSubscriberType));

//            myCurrentIPTVAccountType.Active = true;
//            myCurrentIPTVAccountType.SetTopBoxList = null;

//            if (myCurrentIPTVAccountType.PurchasePIN.Length < 4)
//                myCurrentIPTVAccountType.PurchasePIN = "0000";

//            if (myCurrentIPTVAccountType.PurchasePIN.Length < 4)
//                myCurrentIPTVAccountType.RatingPIN = "0000";

//            myIPTVService.SetIPTVAccount(myLoginInformation.LoginToken, myCurrentIPTVAccountType, myIPTVSubscriberType);
//        }

//ported
//        [ComVisible(true)]
//        public void RemoveSTBFromIPTVAccount(string sSubAddress, string sServiceReference, string sMacAddress)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = null;

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");


//            myIPTVService.DeleteStb(myLoginInformation.LoginToken, myCurrentIPTVAccountType.SubscriberID, sMacAddress);
//        }

//ported
//        [ComVisible(true)]
//        public void DeauthorizeSTB(string sSubAddress, string sServiceReference, string sMacAddress)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = null;

//            if (myIPTVAccountTypeArray == null)
//                throw new Exception("Unable to find IPTV accounts with this SubAddress: " + sSubAddress + ".\r\n");

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            if (myCurrentIPTVAccountType == null)
//                throw new Exception("Error IPTV Account null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");

//            if (myCurrentIPTVAccountType.SubscriberID.Length == 0)
//                throw new Exception("Error IPTVAccount.SubscriberID is null. Unable to find IPTV account with this SubAddress: " + sSubAddress + " and this ServiceReference: " + sServiceReference + ".\r\n");


//            myIPTVService.DeauthorizeStb(myLoginInformation.LoginToken, myCurrentIPTVAccountType.SubscriberID, sMacAddress);
//        }

//PORTED
//        [ComVisible(true)]
//        public string GetIPTVSubscribersByMAC(string sMacAddress)
//        {
//            string sError = "";
//            string sResult = XmlHelper.SerializeObject(myIPTVService.FindSubscribersByMAC(myLoginInformation.LoginToken, sMacAddress), ref sError);
//            if (sError != "SUCCESS")
//                throw new Exception("Method GetIPTVSubscribersByMAC() encountered an Outer Exception: Unable to SerializeObject:\"IPTVAccountType\". Inner Exception: " + sError);

//            return XmlHelper.ValidateXml(sResult) ? XmlHelper.XmlFormater(sResult) : sResult;
//        }

//PORTED
//        [ComVisible(true)]
//        public string GetIPTVSubscribersBySerialNumber(string sSerialNumber)
//        {
//            string sError = "";
//            string sResult = XmlHelper.SerializeObject(myIPTVService.FindSubscribersBySerialNumber(myLoginInformation.LoginToken, sSerialNumber), ref sError);
//            if (sError != "SUCCESS")
//                throw new Exception("Method GetIPTVSubscribersBySerialNumber() encountered an Outer Exception: Unable to SerializeObject:\"IPTVAccountType\". Inner Exception: " + sError);

//            return XmlHelper.ValidateXml(sResult) ? XmlHelper.XmlFormater(sResult) : sResult;
//        }

//PORTED
//        [ComVisible(true)]
//        public string GetIPTVAccountsBySubAddress(string sSubAddress)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            string sError = "";
//            string sResult = XmlHelper.SerializeObject(myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress), ref sError);
//            if (sError != "SUCCESS")
//                throw new Exception("Method GetIPTVSubscribersBySubAddress() encountered an Outer Exception: Unable to SerializeObject:\"IPTVAccountType\". Inner Exception: " + sError);

//            return XmlHelper.ValidateXml(sResult) ? XmlHelper.XmlFormater(sResult) : sResult;
//        }

//PORTED
//        [ComVisible(true)]
//        public string GetIPTVAccountBySubAddressAndServiceRef(string sSubAddress, string sServiceReference)
//        {
//            sSubAddress = sSubAddress.PadLeft(10, '0');

//            IPTVAccountType[] myIPTVAccountTypeArray = myIPTVService.GetAccountsBySubAddress(myLoginInformation.LoginToken, sSubAddress);
//            IPTVAccountType myCurrentIPTVAccountType = new IPTVAccountType();

//            foreach (IPTVAccountType myIPTVAccountType in myIPTVAccountTypeArray)
//            {
//                if (myIPTVAccountType.ServiceReference == sServiceReference)
//                {
//                    myCurrentIPTVAccountType = myIPTVAccountType;
//                    break;
//                }
//            }

//            string sError = "";
//            string sResult = XmlHelper.SerializeObject(myCurrentIPTVAccountType, ref sError);
//            if (sError != "SUCCESS")
//                throw new Exception("Method GetIPTVSubscriberBySubAddressAndServiceRef() encountered an Outer Exception: Unable to SerializeObject:\"IPTVAccountType\". Inner Exception: " + sError);

//            return XmlHelper.ValidateXml(sResult) ? XmlHelper.XmlFormater(sResult) : sResult;
//        }


//PORTED
//        [ComVisible(true)]
//        public string GetAllChannelLineups()
//        {
//            string sError = "";
//            string sResult = XmlHelper.SerializeObject(myIPTVService.GetAllChannelLineups(myLoginInformation.LoginToken), ref sError);
//            if (sError != "SUCCESS")
//                throw new Exception("Method GetAllChannelLineups() encountered an Outer Exception: Unable to SerializeObject:\"IPTVAccountType\". Inner Exception: " + sError);

//            return XmlHelper.ValidateXml(sResult) ? XmlHelper.XmlFormater(sResult) : sResult;
//        }
//        #endregion

//PORTED
//        #region ************************** LargeScaleConference *******************************
//        [ComVisible(true)]
//        public void InsertFreeConference()
//        {
//            myLargeScaleConference.InsertFreeConference():
//        }
//        #endregion

//        #region ************************** TCMService *******************************

//PORTED
//        [ComVisible(true)]
//        public void AddTCM(string sPhoneNumber, bool blnEnableTCM, bool blnEnableDND, string sInternetAccessTypeXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            This casts at SubscriberServiceSubscriberTypeV3 to a TCMInternetAccessType.
//            TCMSubscriberType myTCMSubscriberType = mySubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(TCMSubscriberType)) as TCMSubscriberType;
//            myTCMSubscriberType.InternetAccess = sInternetAccessTypeXml.DeSerializeStringToObject(typeof(TCMInternetAccessType)) as TCMInternetAccessType;

//            try
//            {
//                myTCMService.AddTCMSubscriber(myLoginInformation.LoginToken, sPhoneNumber, blnEnableTCM, blnEnableDND, myTCMSubscriberType);
//            }
//            catch (Exception ex)
//            {
//                if (!ex.ToString().Contains("Error calling Add Terminating Call Management Subscriber Method, details: Terminating Call Management Subscriber already exists with this number."))
//                    throw new Exception(ex.ToString());
//            }
//        }

//PORTED
//        [ComVisible(true)]
//        public void UpdateTCM(string sSubscriberAddress, string sTCMTypeXml, string sInternetAccessTypeXml)
//        {
//            TCMType myCurrentTCMType = myTCMService.GetTCMSubscriber(myLoginInformation.LoginToken, sSubscriberAddress);

//            TCMType myTCMType = sTCMTypeXml.DeSerializeStringToObject(typeof(TCMType)) as TCMType;
//            if (myTCMType == null)
//            {
//                throw new Exception("Method UpdateTCM encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sTCMTypeXml + Environment.NewLine);
//            }

//            myTCMType.SubscriberGuid = myCurrentTCMType.SubscriberGuid;
//            myTCMService.UpdateTCMSubscriber(myLoginInformation.LoginToken, myTCMType, sInternetAccessTypeXml.DeSerializeStringToObject(typeof(TCMInternetAccessType)) as TCMInternetAccessType);
//        }

//PORTED
//        [ComVisible(true)]
//        public void DeleteTCM(string sSubscriberAddress)
//        {
//            myTCMService.DeleteTCMSubscriber(myLoginInformation.LoginToken, sSubscriberAddress);
//        }

//PORTED
//        [ComVisible(true)]
//        public void SetDNDFeature(string sSubscriberAddress, bool blnEnableStatus)
//        {
//            myTCMService.DNDFeature(myLoginInformation.LoginToken, sSubscriberAddress, blnEnableStatus);
//        }

//PORTED
//        [ComVisible(true)]
//        public void SetTCMFeature(string sSubscriberAddress, bool blnEnableStatus)
//        {
//            myTCMService.TCMFeature(myLoginInformation.LoginToken, sSubscriberAddress, blnEnableStatus);
//        }

//PORTED
//        [ComVisible(true)]
//        public string GetTCMInfo(string sSubscriberAddress)
//        {
//            TCMType myTCMType = myTCMService.GetTCMSubscriber(myLoginInformation.LoginToken, sSubscriberAddress);
//            if (myTCMType == null)
//                return "A subscriber with this subscriber address does not exist." + Environment.NewLine;

//            DNDContactListType[] DNDContactListType = myTCMService.GetSubscribersContacts(myLoginInformation.LoginToken, sSubscriberAddress);
//            if (DNDContactListType == null)
//                return "No DND Contact List Found." + Environment.NewLine + myTCMType.SerializeObjectToString() + Environment.NewLine;

//            return DNDContactListType.SerializeObjectToString() + Environment.NewLine + myTCMType.SerializeObjectToString() + Environment.NewLine;
//        }

//        #endregion

//        #region ************************** Call Logging *******************************

//        [ComVisible(true)]
//        public void AddCPL(string sPhoneNumber, string sEmailAddress, bool bWebPortalEnabled)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            This casts at SubscriberServiceSubscriberTypeV3 to a TCMInternetAccessType.
//            CPLSubscriberType myCPLSubscriberType = mySubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(CPLSubscriberType)) as CPLSubscriberType;

//            myCLPService.AddClpSubscriberRecord(myLoginInformation.LoginToken,
//                new CallLoggingType { EmailAddress = sEmailAddress, SubscriberDn = sPhoneNumber, WebPortalEnabled = bWebPortalEnabled }, myCPLSubscriberType);
//        }

//        [ComVisible(true)]
//        public void UpdateCPL(string sPhoneNumber, string sEmailAddress, bool bWebPortalEnabled)
//        {
//            CallLoggingType[] myCallLoggingTypeArray = myCLPService.FindCallLoggingSubscribers(myLoginInformation.LoginToken, sPhoneNumber);
//            myCallLoggingTypeArray[0].WebPortalEnabled = bWebPortalEnabled;
//            myCallLoggingTypeArray[0].EmailAddress = sEmailAddress;

//            CPLInternetAccessType myCPLInternetAccessType = new CPLInternetAccessType() { Password = "", UserName = sEmailAddress, ServiceEnabled = bWebPortalEnabled };
//            myCLPService.UpdateClpSubscriberRecord(myLoginInformation.LoginToken, myCallLoggingTypeArray[0], myCPLInternetAccessType);
//        }

//        [ComVisible(true)]
//        public void DeleteCPL(string sPhoneNumber)
//        {
//            myCLPService.RemoveClpSubscriberRecord(myLoginInformation.LoginToken, sPhoneNumber);
//        }

//        [ComVisible(true)]
//        public string GetCPL(string sPhoneNumber)
//        {
//            return myCLPService.FindCallLoggingSubscribers(myLoginInformation.LoginToken, sPhoneNumber).SerializeObjectToString();
//        }
//        #endregion

//        #region ************************** Local Number Portability *******************************

//        [ComVisible(true)]
//        public void GetAllPortabilityEntries()
//        {
//            myLocalNumberPortabilityService.GetAllPortabilityEntries(myLoginInformation.LoginToken);
//        }

//        #endregion

//        #region ************************** Orginating Call Management Service Vars *******************************

//        [ComVisible(true)]
//        public void AddOCM(string sPhoneNumber, string sOCMTypeXml, string sInternetAccessTypeXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            This casts at SubscriberServiceSubscriberTypeV3 to a OCMSubscriberType.
//            OCMSubscriberType myOCMSubscriberType = mySubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(OCMSubscriberType)) as OCMSubscriberType;
//            myOCMSubscriberType.InternetAccess = sInternetAccessTypeXml.DeSerializeStringToObject(typeof(OCMInternetAccessType)) as OCMInternetAccessType;

//            OCMType myUCMType = sOCMTypeXml.DeSerializeStringToObject(typeof(OCMType)) as OCMType;
//            if (myUCMType == null)
//            {
//                throw new Exception("Method AddOCM encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sOCMTypeXml + Environment.NewLine);
//            }

//            myOCMService.AddSubscriberRecord(myLoginInformation.LoginToken, myUCMType, myOCMSubscriberType);
//        }

//        [ComVisible(true)]
//        public void UpdateOCM(string sPhoneNumber, string sOCMTypeXml, string sInternetAccessTypeXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            OCMType myCurrentUCMType = myOCMService.GetSubscriberBySubGuid(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);

//            OCMType myUCMType = sOCMTypeXml.DeSerializeStringToObject(typeof(OCMType)) as OCMType;
//            if (myUCMType == null)
//            {
//                throw new Exception("Method UpdateOCM encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sOCMTypeXml + Environment.NewLine);
//            }

//            myUCMType.SubscriberGuid = myCurrentUCMType.SubscriberGuid;
//            myOCMService.UpdateSubscriberRecord(myLoginInformation.LoginToken, myUCMType,
//                sInternetAccessTypeXml.DeSerializeStringToObject(typeof(OCMInternetAccessType)) as OCMInternetAccessType);
//        }

//        [ComVisible(true)]
//        public void DeleteOCM(string sPhoneNumber)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            myOCMService.DeleteSubscriberRecord(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//        }

//        [ComVisible(true)]
//        public string GetOCM(string sPhoneNumber)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            return myOCMService.GetSubscriberBySubGuid(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid).SerializeObjectToString();
//        }

//        #endregion

//        #region ************************** Universal Call Management *******************************

//        [ComVisible(true)]
//        public void AddUCM(string sPhoneNumber, string sUCMTypeXml, string sInternetAccessTypeXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            This casts at SubscriberServiceSubscriberTypeV3 to a UCMSubscriberType.
//            UCMSubscriberType myUCMSubscriberType = mySubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(UCMSubscriberType)) as UCMSubscriberType;
//            myUCMSubscriberType.InternetAccess = sInternetAccessTypeXml.DeSerializeStringToObject(typeof(UCMInternetAccessType)) as UCMInternetAccessType;

//            UCMType myUCMType = sUCMTypeXml.DeSerializeStringToObject(typeof(UCMType)) as UCMType;
//            if (myUCMType == null)
//            {
//                throw new Exception("Method AddUCM encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sUCMTypeXml + Environment.NewLine);
//            }

//            myUCMService.AddUniversalCallManagerSubscriber(myLoginInformation.LoginToken, myUCMType, myUCMSubscriberType);
//        }

//        [ComVisible(true)]
//        public void UpdateUCM(string sPhoneNumber, string sUCMTypeXml, string sInternetAccessTypeXml)
//        {
//            UCMType myCurrentUCMType = myUCMService.GetUniversalCallManagerNumber(myLoginInformation.LoginToken, sPhoneNumber);

//            UCMType myUCMType = sUCMTypeXml.DeSerializeStringToObject(typeof(UCMType)) as UCMType;
//            if (myUCMType == null)
//            {
//                throw new Exception("Method UpdateUCM encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sUCMTypeXml + Environment.NewLine);
//            }
//            myUCMType.SubscriberID = myCurrentUCMType.SubscriberID;
//            myUCMService.UpdateUniversalCallManagerSubscriber(myLoginInformation.LoginToken, myUCMType,
//                sInternetAccessTypeXml.DeSerializeStringToObject(typeof(UCMInternetAccessType)) as UCMInternetAccessType);
//        }

//        [ComVisible(true)]
//        public void DeleteUCM(string sPhoneNumber)
//        {
//            myUCMService.DeleteUniversalCallManagerSubscriber(myLoginInformation.LoginToken, sPhoneNumber);
//        }

//        [ComVisible(true)]
//        public string GetUCM(string sPhoneNumber)
//        {
//            return myUCMService.GetUniversalCallManagerNumber(myLoginInformation.LoginToken, sPhoneNumber).SerializeObjectToString();
//        }

//        #endregion

//        #region ************************** ODConferencing *******************************

//        [ComVisible(true)]
//        public void AddODConferencing(string sPhoneNumber, string sODCTypeXml, string sInternetAccessTypeXml)
//        {
//            SubscriberServiceSubscriberTypeV3 mySubscriberServiceSubscriberType = mySubscriberServiceV3.GetSubscriberByNumberProv(myLoginInformation.LoginToken, sPhoneNumber);
//            if (mySubscriberServiceSubscriberType == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            This casts at SubscriberServiceSubscriberTypeV3 to a ODSubscriberType.
//            ODSubscriberType myODSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(ODSubscriberType)) as ODSubscriberType;
//            myODSubscriberType.InternetAccess = sInternetAccessTypeXml.DeSerializeStringToObject(typeof(ODCInternetAccessType)) as ODCInternetAccessType;

//            ConferenceType myODCType = sODCTypeXml.DeSerializeStringToObject(typeof(ConferenceType)) as ConferenceType;
//            if (myODCType == null)
//            {
//                throw new Exception("Method AddODConferencing encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sODCTypeXml + Environment.NewLine);
//            }
//            Random myRandomNum = new Random();

//            if (myODCType.AdministratorID.Length == 0)
//                myODCType.AdministratorID = myRandomNum.Next(10000, 99999).ToString();

//            if (myODCType.ConferenceID.Length == 0)
//                myODCType.ConferenceID = myRandomNum.Next(10000, 99999).ToString();

//            string sError = "";
//            bool RetryAdd = true;
//            while (RetryAdd)
//            {
//                if (AddConferenceSub(myODCType, myODSubscriberType, myRandomNum, out sError, out RetryAdd))
//                    break;
//            }

//            if (sError.Length > 0)
//                throw new Exception(sError);
//        }

//        [ComVisible(false)]
//        private bool AddConferenceSub(ConferenceType myConferenceType, ODSubscriberType myODSubscriberType, Random myRandomNum, out string sError, out bool RetryAdd)
//        {
//            try
//            {
//                myODConferencingService.AddConferenceSub(myLoginInformation.LoginToken, myConferenceType, myODSubscriberType);
//            }
//            catch (Exception ex)
//            {
//                if (ex.ToString().Contains("Conference Subscriber already exists with this Conference ID."))
//                {
//                    myConferenceType.ConferenceID = myRandomNum.Next(10000, 99999).ToString();
//                    myODConferencingService.AddConferenceSub(myLoginInformation.LoginToken, myConferenceType, myODSubscriberType);
//                    RetryAdd = true;
//                    sError = "";
//                    return false;
//                }

//                RetryAdd = false;
//                sError = ex.ToString();
//                return false;
//            }

//            RetryAdd = false;
//            sError = "";
//            return true;
//        }

//        [ComVisible(true)]
//        public void DeleteODConferencing(string sSubscriberAddress)
//        {
//            myODConferencingService.DeleteConferenceBySubAddress(myLoginInformation.LoginToken, sSubscriberAddress);
//        }

//        [ComVisible(true)]
//        public void UpdateODConferencing(string sSubscriberAddress, string sODCTypeXml, string sInternetAccessTypeXml)
//        {
//            ConferenceType myCurrentODCType = myODConferencingService.GetConferenceSubByAddress(myLoginInformation.LoginToken, sSubscriberAddress);

//            ConferenceType myODCType = sODCTypeXml.DeSerializeStringToObject(typeof(ConferenceType)) as ConferenceType;
//            if (myODCType == null)
//            {
//                throw new Exception("Method UpdateODConferencing encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sODCTypeXml + Environment.NewLine);
//            }
//            myODCType.SubscriberGuid = myCurrentODCType.SubscriberGuid;
//            myODConferencingService.UpdateConferenceSub(myLoginInformation.LoginToken, myODCType,
//                sInternetAccessTypeXml.DeSerializeStringToObject(typeof(ODCInternetAccessType)) as ODCInternetAccessType);
//        }

//        [ComVisible(true)]
//        public string GetODConferencing(string sSubscriberAddress)
//        {
//            return myODConferencingService.GetConferenceSubByAddress(myLoginInformation.LoginToken, sSubscriberAddress).SerializeObjectToString();
//        }

//        #endregion

//        [ComVisible(true)]
//        public string FormatXML(string sUnformatedXml)
//        {
//            if (!XmlHelper.ValidateXml(sUnformatedXml))
//                return sUnformatedXml;

//            return XmlHelper.XmlFormater2(sUnformatedXml);
//        }

//        [ComVisible(true)]
//        public bool ParseXML(string sUnformatedXml)
//        {
//            return XmlHelper.ValidateXml(sUnformatedXml);
//        }

//        [ComVisible(false)]
//        private static List<int> GetAvailableDigitFieldsFromChildList(IEnumerable<ChildInfoType> myChildInfoTypeArray, int iMaxNumberOfSubMailboxesAllowed)
//        {
//            List<int> AvailableDigitFieldList = new List<int>();

//            for (int count = 1; count <= iMaxNumberOfSubMailboxesAllowed; count++)
//            {
//                bool blnFoundMatch = false;
//                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
//                {
//                    if (Convert.ToInt32(myChildInfoType.DigitField) == count)
//                        blnFoundMatch = true;
//                }

//                if (!blnFoundMatch)
//                    AvailableDigitFieldList.Add(count);
//            }

//            return AvailableDigitFieldList;
//        }
//    }

//    [ClassInterface(ClassInterfaceType.AutoDual)]
//    [ProgId("VoiceMail.OasisApMax")]
//    [ComVisible(true), Guid("7FF9DBF1-B5CA-4905-B976-8C6C032A4D23")]
//    public class OasisApMax
//    {

//        #region ************************** Global Vars *******************************
//        [ComVisible(false)]
//        private ApAdminClient myApAdmin;
//        [ComVisible(true)]
//        public LoginInformation myLoginInformation;
//        [ComVisible(true)]
//        public ServiceVersions myVersions;
//        [ComVisible(false)]
//        private ServiceReportClient myServiceReportV1;
//        [ComVisible(false)]
//        private object genVoiceMailService;
//        [ComVisible(false)]
//        private object genVoiceMailBoxType;
//        [ComVisible(false)]
//        private object genMailBoxType;
//        [ComVisible(false)]
//        private object genNotificationInfoType;
//        [ComVisible(false)]
//        private object genSubInfoType;
//        [ComVisible(false)]
//        private object genNotificationCenterTypeEmailType;
//        [ComVisible(false)]
//        private static object genVoicemailInternetAccessType;
//        [ComVisible(false)]
//        private static object genChildInfoType;
//        [ComVisible(false)]
//        private static object genNotificationCenterTypeEnum;
//        [ComVisible(false)]
//        private static object genVoiceMailServiceAddressTypeEnum;
//        [ComVisible(false)]
//        private static Type genNotificationInfoTypeType;

//        [ComVisible(false)]
//        private object genSubscriberService;
//        [ComVisible(false)]
//        private object genSubscriberSubType;
//        [ComVisible(false)]
//        private object genSubscriberInternetAccessType;
//        [ComVisible(false)]
//        private object genSubscriberType;
//        [ComVisible(false)]
//        private object genTimezone;
//        [ComVisible(false)]
//        private object genPlacementType;

//        [ComVisible(false)]
//        private object genCNAService;
//        [ComVisible(false)]
//        private object genCnaInfo;

//        [ComVisible(false)]
//        private object genCallingNameService;

//        [ComVisible(false)]
//        private object genIPTVService;
//        [ComVisible(false)]
//        private object genIPTVSubscriberType;
//        [ComVisible(false)]
//        private object genIPTVAccountType;




//        These do not have multiple versions. Yet...
//        Will make objects and use reflection if and when they get multiple version.
//        [ComVisible(false)]
//        private TCMServiceClient myTCMServiceV3;
//        [ComVisible(false)]
//        private LargeScaleConferenceClient myLargeScaleConferenceV1;
//        [ComVisible(false)]
//        private WirelessOtaClient myWirelessOTAServiceV1;
//        [ComVisible(false)]
//        private ODConferencingServiceClient myODConferencingServiceV3;
//        [ComVisible(false)]
//        private UCMServiceClient myUCMServiceV3;
//        [ComVisible(false)]
//        private CLPServiceClient myCLPServiceV3;
//        [ComVisible(false)]
//        private OCMServiceClient myOCMServiceV1;
//        [ComVisible(false)]
//        private LocalNumberPortabilityClient myLocalNumberPortabilityServiceV1;
//        #endregion


//         <summary> This method intializes the apmax interface.</summary>
//         <param name="IP">IP of the provisioning host.</param>
//         <param name="Port">Port of the provisioning host.</param>
//         <param name="UserName">User of the provisioning host.</param>
//         <param name="PassWord">Pasword of the provisioning host.</param>
//         <param name="SystemKey">SystemKey of the provisioning host.</param>
//        [ComVisible(true)]
//        public void Initialize(string IP, int Port, string UserName, string PassWord, string SystemKey)
//        {
//            Note: IMPORTANT anytime a reference is added, deleted, or changed you must copy the contents the app.config into the Oasis.exe.config!
//            Note: The reason for this is because anytime a DLL is called the calling application config file is the one that will be checked.
//            Note: Which means the configuration will not work unless you copy the DLLs app.config into the Oasis.exe.config.

//            myLoginInformation = new LoginInformation();
//            http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
//            myApAdmin = new ApAdminClient("WSHttpBinding_IApAdmin",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

//            http://localhost:8731/Design_Time_Addresses/ServiceReportV1/ServiceReport/
//            myServiceReportV1 = new ServiceReportClient("WSHttpBinding_IServiceReport",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/ServiceReportV1/ServiceReport/"));

//            http://localhost:8731/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/
//            myOCMServiceV1 = new OCMServiceClient("WSHttpBinding_IOCMService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/OriginatingCallManagementV1/OCMService/"));

//            http://localhost:8731/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/
//            myLocalNumberPortabilityServiceV1 = new LocalNumberPortabilityClient("WSHttpBinding_ILocalNumberPortability",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LocalNumberPortabilityV1/LocalNumberPortability/"));

//            http://localhost:8731/Design_Time_Addresses/WirelessOtaV1/WirelessOta/
//            myWirelessOTAServiceV1 = new WirelessOtaClient("WSHttpBinding_IWirelessOta",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/WirelessOtaV1/WirelessOta/"));

//            http://localhost:8731/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/
//            myLargeScaleConferenceV1 = new LargeScaleConferenceClient("WSHttpBinding_ILargeScaleConference",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/LargeScaleConferenceV1/LargeScaleConference/"));

//            http://localhost:8731/Design_Time_Addresses/IPTVServiceV3/IPTVService/
//            genIPTVService = new IPTVServiceClient("WSHttpBinding_IIPTVService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/IPTVServiceV3/IPTVService/"));

//            genIPTVSubscriberType = new IPTVSubscriberType();
//            genIPTVAccountType = new IPTVAccountType();

//            http://localhost:8731/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/
//            myODConferencingServiceV3 = new ODConferencingServiceClient("WSHttpBinding_IODConferencingService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/OnDemandConferencing/ODConferencingServiceV3/"));

//            http://localhost:8731/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/
//            myTCMServiceV3 = new TCMServiceClient("WSHttpBinding_ITCMService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/TerminatingCallManagementV3/TCMServiceV3/"));

//            http://localhost:8731/Design_Time_Addresses/CNAService/CNAServiceV3/
//            genCNAService = new CNAServiceClient("WSHttpBinding_ICNAService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/CNAService/CNAServiceV3/"));

//            genCnaInfo = new CnaInfo();

//            http://localhost:8731/Design_Time_Addresses/CallLoggingServiceV3/CLPService/
//            myCLPServiceV3 = new CLPServiceClient("WSHttpBinding_ICLPService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/CallLoggingServiceV3/CLPService/"));

//            http://localhost:8731/Design_Time_Addresses/UniveralCallManagementV3/UCMService/
//            myUCMServiceV3 = new UCMServiceClient("WSHttpBinding_IUCMService",
//                new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/UniveralCallManagementV3/UCMService/"));

//            myApAdmin.Login(SystemKey, UserName, PassWord);
//            myLoginInformation = myApAdmin.LoginAdv(SystemKey, UserName, PassWord);
//            myVersions = myServiceReportV1.GetAPmaxServiceVersions(myLoginInformation.LoginToken);

//            LogUtil.LogFileMode = LogMode.ALL;
//            LogUtil.Filepath = "c:\\OasisLog.txt";
//            LogUtil.WriteToFile("Apmax Versions: " + myVersions.SerializeObjectToString() + "\r\n", LogMode.DEBUG);

//            switch (myVersions.Subscriber)
//            {
//                case 3:
//                    http://localhost:8731/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/
//                    genSubscriberService = new SubscriberServiceV3.SubscriberServiceClient("WSHttpBinding_ISubscriberService3",
//                        new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/SubscriberServiceV3/SubscriberV3/"));

//                    genSubscriberSubType = new SubscriberServiceV3.SubscriberType();
//                    genSubscriberInternetAccessType = new SubscriberServiceV3.InternetAccessType();
//                    break;
//                case 4:
//                    http://localhost:8731/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4
//                    genSubscriberService = new SubscriberServiceV4.SubscriberServiceClient("WSHttpBinding_ISubscriberService4",
//                        new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4/"));

//                    genSubscriberSubType = new SubscriberServiceV4.SubscriberType();
//                    genSubscriberInternetAccessType = new SubscriberServiceV4.InternetAccessType();
//                    break;
//            }

//            switch (myVersions.Voicemail)
//            {
//                case 3:
//                    http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV3/
//                    genVoiceMailService = new VoicemailClient("WSHttpBinding_IVoicemail3",
//                        new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/VoicemailService/VoicemailV3/"));

//                    genVoiceMailBoxType = new VoiceMailServiceV3.VoiceMailBoxType();
//                    genMailBoxType = new VoiceMailServiceV3.MailboxType();
//                    genSubInfoType = new VoiceMailServiceV3.SubInfoType();
//                    genNotificationInfoType = new VoiceMailServiceV3.NotificationInfoType();
//                    genVoicemailInternetAccessType = new VoiceMailServiceV3.InternetAccessType();
//                    genChildInfoType = new VoiceMailServiceV3.ChildInfoType();
//                    genPackageType = new VoiceMailServiceV3.PackageType();  
//                    genSubscriberType = new VoiceMailServiceV3.SubscriberType();
//                    genTimezone = VoiceMailServiceV3.Timezone_e.ApDefault;
//                    genPlacementType = VoiceMailServiceV3.PlacementType_e.PlacementType_None;
//                    genNotificationCenterTypeEnum = VoiceMailServiceV3.NotificationCenterTypeEnum.typeEmail;
//                    genVoiceMailServiceAddressTypeEnum = VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber;

//                    genNotificationInfoTypeType = typeof(VoiceMailServiceV3.NotificationInfoType);
//                    break;
//                case 4:
//                    http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV4/
//                    genVoiceMailService = new VoiceMailServiceV4.VoicemailClient("WSHttpBinding_IVoicemail4",
//                        new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/VoicemailService/VoicemailV4/"));

//                    genVoiceMailBoxType = new VoiceMailServiceV4.VoiceMailBoxType();
//                    genMailBoxType = new VoiceMailServiceV4.MailboxType();
//                    genSubInfoType = new VoiceMailServiceV4.SubInfoType();
//                    genNotificationInfoType = new VoiceMailServiceV4.NotificationInfoType();
//                    genVoicemailInternetAccessType = new VoiceMailServiceV4.InternetAccessType();
//                    genChildInfoType = new VoiceMailServiceV4.ChildInfoType();
//                    genPackageType = new VoiceMailServiceV4.PackageType();
//                    genSubscriberType = new VoiceMailServiceV4.SubscriberType();
//                    genTimezone = VoiceMailServiceV4.Timezone_e.ApDefault;
//                    genPlacementType = VoiceMailServiceV4.PlacementType_e.PlacementType_None;
//                    genNotificationCenterTypeEnum = VoiceMailServiceV4.NotificationCenterTypeEnum.typeEmail;
//                    genVoiceMailServiceAddressTypeEnum = VoiceMailServiceV4.AddressType.AddressTypeMailboxNumber;

//                    genNotificationInfoTypeType = typeof(VoiceMailServiceV4.NotificationInfoType);
//                    break;
//                case 5:
//                    http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV5/
//                    genVoiceMailService = new VoiceMailServiceV5.VoicemailClient("WSHttpBinding_IVoicemail5",
//                        new EndpointAddress("http://" + IP + ":" + Port + "/Design_Time_Addresses/VoicemailService/VoicemailV5/"));

//                    genVoiceMailBoxType = new VoiceMailServiceV5.VoiceMailBoxType();
//                    genMailBoxType = new VoiceMailServiceV5.MailboxType();
//                    genSubInfoType = new VoiceMailServiceV5.SubInfoType();
//                    genNotificationInfoType = new VoiceMailServiceV5.NotificationInfoType();
//                    genVoicemailInternetAccessType = new VoiceMailServiceV5.InternetAccessType();
//                    genChildInfoType = new VoiceMailServiceV5.ChildInfoType();
//                    genPackageType = new VoiceMailServiceV5.PackageType();
//                    genSubscriberType = new VoiceMailServiceV5.SubscriberType();
//                    genTimezone = VoiceMailServiceV5.Timezone_e.ApDefault;
//                    genPlacementType = VoiceMailServiceV5.PlacementType_e.PlacementType_None;
//                    genNotificationCenterTypeEnum = VoiceMailServiceV5.NotificationCenterTypeEnum.typeEmail;
//                    genVoiceMailServiceAddressTypeEnum = VoiceMailServiceV5.AddressType.AddressTypeMailboxNumber;

//                    genNotificationInfoTypeType = typeof(VoiceMailServiceV5.NotificationInfoType);

//                    LogUtil.WriteToFile("VoiceMailService5 instantiated\r\n", LogMode.DEBUG);
//                    break;
//            }
//        }

//        [ComVisible(true)]
//        public void Debug(string logMode)
//        {
//            LogUtil.LogFileMode = (LogMode)Enum.Parse(typeof(LogMode), logMode);
//            LogUtil.Filepath = "c:\\OasisLog.txt";
//            LogUtil.WriteToFile("\r\n****\r\n", LogMode.ALL);
//            LogUtil.WriteToFile("Date:" + DateTime.Now + "\r\n", LogMode.ALL);
//        }

//        [ComVisible(true)]
//        public void Close()
//        {
//             Invoke the close methods using reflection.
//            genVoiceMailService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, genVoiceMailService, null);
//            genSubscriberService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, genSubscriberService, null);
//            genCallingNameService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, genCallingNameService, null);
//            genCNAService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, genCNAService, null);
//            genIPTVService.GetType().InvokeMember("Close", BindingFlags.InvokeMethod, null, genIPTVService, null);

//            Close all the other instantiations.
//            myLocalNumberPortabilityServiceV1.Close();
//            myOCMServiceV1.Close();
//            myLargeScaleConferenceV1.Close();
//            myCLPServiceV3.Close();
//            myUCMServiceV3.Close();
//            myODConferencingServiceV3.Close();
//            myTCMServiceV3.Close();
//            myServiceReportV1.Close();

//            myApAdmin.Logout(myLoginInformation.LoginToken);
//            myApAdmin.Close();
//        }

//        [ComVisible(true)]
//        public string GetApmaxVersions()
//        {
//            return myVersions.SerializeObjectToString();
//            <?xml version="1.0"?>
//            <ServiceVersions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
//              <ExtensionData />
//              <CallLogging>3</CallLogging>
//              <CallingName>4</CallingName>
//              <ChangeNumberAnnouncements>3</ChangeNumberAnnouncements>
//              <Iptv>3</Iptv>
//              <LargeScaleConference>-1</LargeScaleConference>
//              <LocalNumberPortability>-1</LocalNumberPortability>
//              <OnDemandConference>3</OnDemandConference>
//              <OriginatingCallManagement>1</OriginatingCallManagement>
//              <SipAcs>-1</SipAcs>
//              <Subscriber>4</Subscriber>
//              <TerminatingCallManagement>3</TerminatingCallManagement>
//              <UniversalCallManagement>3</UniversalCallManagement>
//              <Voicemail>3</Voicemail>
//              <WirelessOta>-1</WirelessOta>
//            </ServiceVersions>
//        }

//        #region ************************** VoiceMail *******************************

//        [ComVisible(true)]
//        public void AddNewVoiceMailBox(string sPhoneNumber, string sMailBoxDescription, string sVmPackageName, string sSubscriberName, string sMailBoxType, string sNotificationCenter, string BillingAccountNumber)
//        {
//            This will take string xml and deserialize to an object.
//            var myNewVoiceMailBoxTypeArray = sVoiceMailBoxUnformatedXml.DeSerializeStringToObject(genVoiceMailBoxType.GetType());
//            if (myNewVoiceMailBoxTypeArray == null)
//                throw new Exception("Method UpdateVoiceMailBoxFull encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sVoiceMailBoxUnformatedXml + Environment.NewLine);

//            Once you have an object find the property we need and get its value.
//            PropertyInfo test = myNewVoiceMailBoxTypeArray.GetType().GetProperty("DescriptionField");
//            string sPhoneNumber = test.GetValue(myNewVoiceMailBoxTypeArray, null).ToString();

//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currenly have any Voicemail setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            if (sPhoneNumber.Length < 10)
//                throw new Exception("PhoneNumber must be 10 digits." + Environment.NewLine);

//            string sPackageGuid = "";
//            Gets a list of all available vm packages
//            myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);
//            object result = genVoiceMailService.GetType().InvokeMember("GetAllPackages", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[] { myLoginInformation.LoginToken });
//            now cast as array so we can loop thorugh using the foreach loop.
//            Array genPackagesArray = result as Array;


//            object resultArray = Convert.ChangeType(result, genPackageTypeArray.GetType()); //Changes type from a generic object to an object of my specific type.
//            now cast as array so we can loop thorugh using the foreach loop.
//            Array array = resultArray as Array;

//            Here we are searching thorugh all the packages and matching up the description with what we want.
//            Because we need to send the guid of the description.
//            foreach (var myPackageType in genPackagesArray)
//            {
//                PropertyInfo pi = myPackageType.GetType().GetProperty("DescriptionField");
//                string description = pi.GetValue(myPackageType, null).ToString();

//                if (description.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
//                {
//                    pi = myPackageType.GetType().GetProperty("GuidField");
//                    sPackageGuid = pi.GetValue(myPackageType, null).ToString();
//                    break;
//                }
//            }

//            if (sPackageGuid.Length == 0)
//                throw new Exception("Could not find this Package in the ApMax system." + Environment.NewLine +
//                          "Note: Case sentivity does not matter." + Environment.NewLine +
//                          "Please check that the correct package was sent to this method." + Environment.NewLine);


//            int iNotificationCenterID = -1;

//            Gets all the notification centers
//            NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);
//            result = genVoiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[] { myLoginInformation.LoginToken });
//            Array genNotificationCentersArray = result as Array;

//            Here we are searching thorugh all the NotificationCenters and matching up the description with what we want.
//            Because we need to send the centerid.
//            foreach (var myNotificationType in genNotificationCentersArray)
//            {
//                PropertyInfo pi = myNotificationType.GetType().GetProperty("DescriptionField");
//                string description = pi.GetValue(myNotificationType, null).ToString();

//                if (description.Equals(sNotificationCenter, StringComparison.OrdinalIgnoreCase))
//                {
//                    pi = myNotificationType.GetType().GetProperty("CenterIdField");
//                    iNotificationCenterID = Convert.ToInt32(pi.GetValue(myNotificationType, null));
//                    break;
//                }
//            }

//            iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
//            if (iNotificationCenterID < 0)
//                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
//                          "Note: Case sentivity does not matter." + Environment.NewLine +
//                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

//            genMailBoxType = Enum.Parse(genMailBoxType.GetType(), sMailBoxType, true);

//            PropertyInfo pip = genSubscriberType.GetType().GetProperty("SubscriberName");
//            pip.SetValue(genSubscriberType, sSubscriberName, null);
//            pip = genSubscriberType.GetType().GetProperty("BillingAccountNumber");
//            pip.SetValue(genSubscriberType, BillingAccountNumber, null);
//            pip = genSubscriberType.GetType().GetProperty("SubscriberTimezone");
//            pip.SetValue(genSubscriberType, genTimezone, null);
//            pip = genSubscriberType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            pip.SetValue(genSubscriberType, sPhoneNumber, null);
//            pip = genSubscriberType.GetType().GetProperty("PlacementType");
//            pip.SetValue(genSubscriberType, genPlacementType, null);

//            object[] objParms = new[] {myLoginInformation.LoginToken, sPhoneNumber, sPhoneNumber, sPackageGuid, 
//                genMailBoxType, iNotificationCenterID, sPhoneNumber, genSubscriberType};

//            AddNewVoiceMailBox(myLoginInformation.LoginToken, sPhoneNumber, sMailBoxDescription, sPackageGuid,myMailboxType, iNotificationCenterID, sPhoneNumber, mySubscriberType);
//            result = genVoiceMailService.GetType().InvokeMember("AddNewVoiceMailBox", BindingFlags.InvokeMethod, null, genVoiceMailService, objParms);
//        }

//        [ComVisible(true)]
//        public void DeleteVoiceMailBox(string sVMPhone, bool DeleteSubscriber)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            This will get the current mailbox info before we delete the box.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sVMPhone });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            If this is not null mailbox is present so we will delete.
//            if (array == null || array.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            foreach (var VoiceMailBoxType in array)
//            {
//                PropertyInfo pi = VoiceMailBoxType.GetType().GetProperty("IdField");
//                string id = pi.GetValue(VoiceMailBoxType, null).ToString();

//                myVoiceMailServiceV3.DeleteVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField);
//                genVoiceMailService.GetType().InvokeMember("DeleteVoiceMailBox", BindingFlags.InvokeMethod, null,
//                    genVoiceMailService, new object[] { myLoginInformation.LoginToken, id });
//            }

//            if (DeleteSubscriber)
//                DeleteSubscriberByPhoneNumber(sVMPhone);
//        }

//        [ComVisible(true)]
//        public void UpdateVoiceMailBoxPackage(string sPhoneNumber, string sVmPackageName)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            string sPackageGuid = "";
//            Gets a list of all available vm packages
//            myVoiceMailServiceV3.GetAllPackages(myLoginInformation.LoginToken);
//            object result = genVoiceMailService.GetType().InvokeMember("GetAllPackages", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[] { myLoginInformation.LoginToken });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            object resultArray = Convert.ChangeType(result, genPackageTypeArray.GetType()); //Changes type from a generic object to an object of my specific type.
//            now cast as array so we can loop through using the foreach loop.
//            Array array = resultArray as Array;

//            if (array == null || array.Length == 0)
//                throw new Exception("Failed to return packages." + Environment.NewLine);

//            Here we are searching through all the packages and matching up the description with what we want.
//            Because we need to send the guid of the description.
//            foreach (var myPackageType in array)
//            {
//                PropertyInfo pi = myPackageType.GetType().GetProperty("DescriptionField");
//                string description = pi.GetValue(myPackageType, null).ToString();

//                if (description.Equals(sVmPackageName, StringComparison.OrdinalIgnoreCase))
//                {
//                    pi = myPackageType.GetType().GetProperty("GuidField");
//                    sPackageGuid = pi.GetValue(myPackageType, null).ToString();
//                    break;
//                }
//            }

//            if (sPackageGuid.Length == 0)
//                throw new Exception("Could not find this Package in the ApMax system." + Environment.NewLine +
//                          "Note: Case sentivity does not matter." + Environment.NewLine +
//                          "Please check that the correct package was sent to this method." + Environment.NewLine);

//            This will get the current mailbox info.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });
//            now cast as array so we can loop through using the foreach loop.
//            array = result as Array;

//            If this is not null mailbox is present so we will delete.
//            if (array == null || array.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            foreach (var vmbox in array)
//            {
//                foreach (PropertyInfo pi in vmbox.GetType().GetProperties())
//                {
//                    This will set all property fields to null except the Id field.
//                    if (!(pi.Name == "IdField" || pi.Name == "OptionsPackageField" || pi.Name == "SubscriberListField"))
//                        pi.SetValue(vmbox, null, null);

//                    Put in the new package guid.
//                    if (pi.Name == "OptionsPackageField")
//                        pi.SetValue(vmbox, sPackageGuid, null);

//                    Per Innovatives request set the guid to null on all subtypeguids.
//                    if (pi.Name == "SubscriberListField")
//                    {
//                        genSubInfoType = pi.GetValue(vmbox, null);
//                        Array subInfoTypeArray = genSubInfoType as Array;
//                        foreach (var subInfoType in subInfoTypeArray)
//                        {
//                            PropertyInfo pic = subInfoType.GetType().GetProperty("SubscriberGuidField");
//                            pic.SetValue(subInfoType, null, null);
//                        }
//                    }
//                }

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
//                genVoiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
//                    genVoiceMailService, new[] { myLoginInformation.LoginToken, vmbox, null });
//            }
//        }

//        [ComVisible(true)]
//        public void UpdateVoiceMailBoxType(string sPhoneNumber, string sMailBoxType, string sInternetPassword,
//            string sInternetUserName, bool blnInternetAccess)
//        {


//        }

//        [ComVisible(true)]
//        public void AddVoiceMailBoxInternetAccess(string sVoiceMailBoxUnformatedXml, string sInternetAccessUnformatedXml, string sSubscriberTypeUnformatedXml)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            This will take string xml and deserialize to an object.
//            genVoiceMailBoxType = sVoiceMailBoxUnformatedXml.DeSerializeStringToObject(genVoiceMailBoxType.GetType());
//            if (genVoiceMailBoxType == null)
//                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sVoiceMailBoxUnformatedXml + Environment.NewLine);

//            This will take string xml and deserialize to an object.
//            genVoicemailInternetAccessType = sInternetAccessUnformatedXml.DeSerializeStringToObject(genVoicemailInternetAccessType.GetType());
//            if (genVoicemailInternetAccessType == null)
//                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sInternetAccessUnformatedXml + Environment.NewLine);

//            This will take string xml and deserialize to an object.
//            genSubscriberSubType = sSubscriberTypeUnformatedXml.DeSerializeStringToObject(genSubscriberSubType.GetType());
//            if (genSubscriberSubType == null)
//                throw new Exception("Method AddVoiceMailBoxInternetAccess encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sSubscriberTypeUnformatedXml + Environment.NewLine);

//            Get the phone number.
//            PropertyInfo pi = genVoiceMailBoxType.GetType().GetProperty("DescriptionField");
//            string sPhoneNumber = pi.GetValue(genVoiceMailBoxType, null).ToString();

//            Get the email address.
//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberEmail");
//            string sEmailAddress = pi.GetValue(genSubscriberSubType, null).ToString();

//            This will get the current mailbox info.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });

//            object result = IVoiceMailBoxService.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sPhoneNumber);

//            now cast as array so we can loop through using the foreach loop.
//            Array genVoiceMailBoxTypeArray = result as Array;

//            If this is not null mailbox is present so we can add internet access.)
//            if (genVoiceMailBoxTypeArray == null || genVoiceMailBoxTypeArray.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            object notificationInfoTypeArray = null;
//            string sSubscriberGuid = "";
//            string sSubscriberName = "";

//            foreach (var vmbox in genVoiceMailBoxTypeArray)
//            {
//                foreach (PropertyInfo pinfo in vmbox.GetType().GetProperties())
//                {
//                    Need to keep the notification list.
//                    if (pinfo.Name == "NotificationListField")
//                        notificationInfoTypeArray = pinfo.GetValue(vmbox, null);

//                    This will set all property fields to null except the Id field.
//                    if (!(pinfo.Name == "IdField" || pinfo.Name == "SubscriberListField"))
//                        pinfo.SetValue(vmbox, null, null);

//                    Per Innovatives request set the guid to null on all subtypeguids.
//                    if (pinfo.Name == "SubscriberListField")
//                    {
//                        genSubInfoType = pinfo.GetValue(vmbox, null);
//                        Array subInfoTypeArray = genSubInfoType as Array;
//                        foreach (var subInfoType in subInfoTypeArray)
//                        {
//                            Need sSubscriberGuid later so store this.
//                            pi = subInfoType.GetType().GetProperty("SubscriberGuidField");
//                            sSubscriberGuid = pi.GetValue(subInfoType, null).ToString();

//                            pi = subInfoType.GetType().GetProperty("SubscriberGuidField");
//                            pi.SetValue(subInfoType, null, null);

//                            pi = subInfoType.GetType().GetProperty("SubscriberDefaultPhoneNumberField");
//                            string subscriberDefaultPhoneNumber = pi.GetValue(subInfoType, null).ToString();

//                            if (subscriberDefaultPhoneNumber == sPhoneNumber)
//                            {
//                                Need sSubscriberName later so store this.
//                                pi = subInfoType.GetType().GetProperty("SubscriberNameField");
//                                sSubscriberName = pi.GetValue(subInfoType, null).ToString();
//                            }
//                        }
//                    }
//                }
//            }

//            int iNotificationCenterID = -1;

//            Gets all the notification centers
//            NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);
//            result = genVoiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[] { myLoginInformation.LoginToken });
//            Array notificationTypeArray = result as Array;

//            if (notificationTypeArray == null || notificationTypeArray.Length == 0)
//                throw new Exception("Failed to return notification centers." + Environment.NewLine);

//            Here we are searching through all the NotificationCenters and matching up the description with what we want.
//            Because we need to send the centerid.
//            foreach (var notificationType in notificationTypeArray)
//            {
//                pi = notificationType.GetType().GetProperty("TypeField");
//                string notificationCenterTypeField = pi.GetValue(notificationType, null).ToString();

//                if (notificationCenterTypeField.Equals(genNotificationCenterTypeEnum.ToString(), StringComparison.OrdinalIgnoreCase))
//                {
//                    pi = notificationType.GetType().GetProperty("CenterIdField");
//                    iNotificationCenterID = Convert.ToInt32(pi.GetValue(notificationType, null));
//                    break;
//                }
//            }

//            iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
//            if (iNotificationCenterID < 0)
//                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
//                          "Note: Case sentivity does not matter." + Environment.NewLine +
//                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

//            Change our current array to a list of NotificationInfoType.
//            pi = genNotificationInfoType.GetType().GetProperty("AddressField");
//            pi.SetValue(genNotificationInfoType, sEmailAddress, null);
//            pi = genNotificationInfoType.GetType().GetProperty("CenterField");
//            pi.SetValue(genNotificationInfoType, iNotificationCenterID, null);
//            pi = genNotificationInfoType.GetType().GetProperty("EnabledField");
//            pi.SetValue(genNotificationInfoType, true, null);

//            int length = (notificationInfoTypeArray as Array).Length;
//            Create new Array with one additional element.
//            Array ArrayOfNotificationInfo = Array.CreateInstance(genNotificationInfoTypeType, length + 1);
//            Copy in old data into new array.
//            (notificationInfoTypeArray as Array).CopyTo(ArrayOfNotificationInfo, 0);
//            Set the value of that of the above created new notification and add to the last element in array.
//            ArrayOfNotificationInfo.SetValue(genNotificationInfoType, length);

//            Darrin from hood wants to allow email deletion and add the email notification as well; the below 2 lines do this.
//            pi = genVoiceMailBoxType.GetType().GetProperty("AllowEmailDeletionField");
//            pi.SetValue(genVoiceMailBoxType, true, null);
//            pi = genVoiceMailBoxType.GetType().GetProperty("NotificationListField");
//            pi.SetValue(genVoiceMailBoxType, ArrayOfNotificationInfo, null);

//            Get current InternetAccess Username information.
//            result = genSubscriberService.GetType().InvokeMember("GetSubscriberInternetAccess", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sSubscriberGuid });

//            if (result == null)
//            {
//                foreach (var voiceMailBoxType in genVoiceMailBoxTypeArray as Array)
//                {
//                    genVoiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[]
//                                                                    {
//                                                                        myLoginInformation.LoginToken, voiceMailBoxType,
//                                                                        genVoicemailInternetAccessType
//                                                                    });
//                }
//            }
//            else
//            {
//                pi = genVoicemailInternetAccessType.GetType().GetProperty("UserName");
//                string sInternetUserName = pi.GetValue(genVoicemailInternetAccessType, null).ToString();

//                pi = result.GetType().GetProperty("UserName");
//                string sSubscriberInternetUserName = pi.GetValue(result, null).ToString();

//                else if username are equal modify status
//                if (sInternetUserName == sSubscriberInternetUserName)
//                {
//                    This re-enables the internet access.
//                    foreach (var voiceMailBoxType in genVoiceMailBoxTypeArray)
//                    {
//                        genVoiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
//                                                                   genVoiceMailService, new object[]
//                                                                                            {
//                                                                                                myLoginInformation.LoginToken,
//                                                                                                voiceMailBoxType,
//                                                                                                genVoicemailInternetAccessType
//                                                                                            });
//                    }
//                }
//                else
//                {
//                    DeleteVoiceMailBoxInternetAccess();
//                    AddVoiceMailBoxInternetAccess();
//                    This means they have internet access but the username is not what we expect.
//                    throw new Exception("A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine);
//                }
//            }

//            Set the SubscriberName on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberName");
//            pi.SetValue(genSubscriberSubType, sSubscriberName, null);

//            Set the SubscriberDefaultPhoneNumber on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            pi.SetValue(genSubscriberSubType, sPhoneNumber, null);

//            Set the PlacementType on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("PlacementType");
//            pi.SetValue(genSubscriberSubType, genPlacementType, null);

//            Set the SubscriberGuid on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberGuid");
//            pi.SetValue(genSubscriberSubType, sSubscriberGuid, null);

//            Set the SubscriberEmail on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberEmail");
//            pi.SetValue(genSubscriberSubType, sEmailAddress, null);

//            enable service on subscriber for internet access.
//            pi = genSubscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
//            pi.SetValue(genSubscriberInternetAccessType, true, null);

//            Set the InternetAccess on the Subscriber.
//            pi = genSubscriberSubType.GetType().GetProperty("InternetAccess");
//            pi.SetValue(genSubscriberSubType, genSubscriberInternetAccessType, null);

//            This re-enables the internet access.
//            genSubscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null, genSubscriberService,
//                new object[] { myLoginInformation.LoginToken, genSubscriberSubType });
//        }

//        [ComVisible(true)]
//        public void DeleteVoiceMailBoxInternetAccess(string sPhoneNumber, bool RemoveSubscriberAccess)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");


//            This will get the current mailbox info.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });
//            now cast as array so we can loop through using the foreach loop.
//            Array genVoiceMailBoxArray = result as Array;

//            If this is not null mailbox is present so we can add internet access.
//            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            Gets all the notification centers
//            NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = myVoiceMailServiceV3.GetAllNotificationCenters(myLoginInformation.LoginToken);
//            result = genVoiceMailService.GetType().InvokeMember("GetAllNotificationCenters", BindingFlags.InvokeMethod, null, genVoiceMailService, new object[] { myLoginInformation.LoginToken });
//            Array genNotificationCentersArray = result as Array;

//            int notificationCenterID = -1;

//            Here we are searching thorugh all the NotificationCenters and matching up the description with what we want.
//            Because we need to send the remove the email notification.
//            foreach (var notificationType in genNotificationCentersArray)
//            {
//                PropertyInfo pi = notificationType.GetType().GetProperty("TypeField");
//                string notificationCenterTypeField = pi.GetValue(notificationType, null).ToString();

//                if (notificationCenterTypeField.Equals(genNotificationCenterTypeEnum.ToString(), StringComparison.OrdinalIgnoreCase))
//                {
//                    pi = notificationType.GetType().GetProperty("CenterIdField");
//                    notificationCenterID = Convert.ToInt32(pi.GetValue(notificationType, null));
//                    break;
//                }
//            }

//            iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
//            if (notificationCenterID < 0)
//                throw new Exception("Could not find this notification center in the ApMax system." + Environment.NewLine +
//                          "Note: Case sentivity does not matter." + Environment.NewLine +
//                          "Please check that the correct notification was sent to this method." + Environment.NewLine);

//            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
//            {
//                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
//                {
//                    This will set all property fields to null except the Id field.
//                    if (!(pi.Name == "IdField" || pi.Name == "SubscriberListField" || pi.Name == "NotificationListField"))
//                        pi.SetValue(genVoiceMailBox, null, null);

//                    Darrin from hood wants to allow email deletion and add the email notification as well; the below 2 lines do this.
//                    if (pi.Name == "AllowEmailDeletionField")
//                        pi.SetValue(genVoiceMailBox, false, null);

//                    if (pi.Name == "NotificationListField")
//                    {
//                        object genNotificationArray = pi.GetValue(genVoiceMailBox, null);

//                        IList genNotificationCenterIList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(genNotificationInfoTypeType));
//                        foreach (var notificationType in genNotificationArray as Array)
//                        {
//                            PropertyInfo prinfo = notificationType.GetType().GetProperty("CenterField");
//                            int centerField = Convert.ToInt32(prinfo.GetValue(notificationType, null));

//                            if (centerField != notificationCenterID)
//                                genNotificationCenterIList.Add(notificationType);
//                        }

//                        int length = genNotificationCenterIList.Count;
//                        Create new Array.
//                        Array genNotificationInfoArray = Array.CreateInstance(genNotificationInfoTypeType, length);
//                        Copy in old data into new array.
//                        genNotificationCenterIList.CopyTo(genNotificationInfoArray, 0);

//                        pi.SetValue(genVoiceMailBox, genNotificationInfoArray, null);
//                    }

//                    if (pi.Name == "SubscriberListField")
//                    {
//                        genSubInfoType = pi.GetValue(genVoiceMailBox, null);
//                        foreach (var info in genSubInfoType as Array)
//                        {
//                            PropertyInfo pic = info.GetType().GetProperty("SubscriberGuidField");
//                            pic.SetValue(info, null, null);
//                        }
//                    }
//                }

//                disable service on voicemail for internet access.
//                PropertyInfo pinfo = genVoicemailInternetAccessType.GetType().GetProperty("ServiceEnabled");
//                pinfo.SetValue(genVoicemailInternetAccessType, false, null);

//                myVoiceMailServiceV3.UpdateVoiceMailBox(myLoginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
//                genVoiceMailService.GetType().InvokeMember("UpdateVoiceMailBox", BindingFlags.InvokeMethod, null,
//                    genVoiceMailService, new[] { myLoginInformation.LoginToken, genVoiceMailBox, genVoicemailInternetAccessType });

//                if (RemoveSubscriberAccess)
//                {
//                    Set the SubscriberName on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("SubscriberName");
//                    pinfo.SetValue(genSubscriberSubType, null, null);

//                    Set the SubscriberDefaultPhoneNumber on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//                    pinfo.SetValue(genSubscriberSubType, sPhoneNumber, null);

//                    Set the PlacementType on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("PlacementType");
//                    pinfo.SetValue(genSubscriberSubType, genPlacementType, null);

//                    Set the SubscriberGuid on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("SubscriberGuid");
//                    pinfo.SetValue(genSubscriberSubType, null, null);

//                    Set the SubscriberEmail on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("SubscriberEmail");
//                    pinfo.SetValue(genSubscriberSubType, "", null);

//                    enable service on subscriber for internet access.
//                    pinfo = genSubscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
//                    pinfo.SetValue(genSubscriberInternetAccessType, false, null);

//                    Set the InternetAccess on the Subscriber.
//                    pinfo = genSubscriberSubType.GetType().GetProperty("InternetAccess");
//                    pinfo.SetValue(genSubscriberSubType, genSubscriberInternetAccessType, null);

//                    This disables the internet access.
//                    genSubscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod,
//                                                                null, genSubscriberService,
//                                                                new object[]
//                                                                    {
//                                                                        myLoginInformation.LoginToken, genSubscriberSubType
//                                                                    });
//                }
//            }
//        }

//        [ComVisible(true)]
//        public void AddVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            This will get the current mailbox info.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });
//            now cast as array so we can loop through using the foreach loop.
//            Array genVoiceMailBoxArray = result as Array;

//            If this is not null mailbox is present so we can add internet access.
//            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            string parentId = "";

//            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
//            {
//                Submailboxes  will change per returned mailbox.
//                int iCurrentNumberOfSubBoxes = 0;

//                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
//                {
//                    if (pi.Name == "IdField")
//                        parentId = pi.GetValue(genVoiceMailBox, null).ToString();

//                    Only look for submailbox types.  Ignore outdial types etc...  
//                    also when you delete a child it does not get rid of this it only clears the NameField...
//                    Dont ask me why... This is the most redicules API I have had to deal with.
//                    if (pi.Name == "ChildListField")
//                    {
//                        genSubInfoType = pi.GetValue(genVoiceMailBox, null);
//                        foreach (var info in genSubInfoType as Array)
//                        {
//                            PropertyInfo pic = info.GetType().GetProperty("TypeField");
//                            object typeField = pi.GetValue(genVoiceMailBox, null);

//                            pic = info.GetType().GetProperty("NameField");
//                            string nameField = pic.GetValue(genSubInfoType, null).ToString();

//                            if (typeField.GetType().Equals(genVoiceMailServiceAddressTypeEnum) && nameField.Length > 0)
//                                iCurrentNumberOfSubBoxes++;
//                        }
//                    }
//                }

//                myVoiceMailServiceV3.AddNewChildMailBox(myLoginInformation.LoginToken, parentId, new ChildInfoType
//                {
//                    //We have to get the next available digit field. The array is pre-populated and sorted with available fields.
//                    DigitField = sDigitField,
//                    NameField = sPhoneNumber + (iCurrentNumberOfSubBoxes + 1),
//                    DescriptionField = "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")",
//                    TypeField = VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber
//                }, MailboxType.FAMILY_CHILD);

//                genMailBoxType = Enum.Parse(genMailBoxType.GetType(), "FAMILY_CHILD", true);

//                Set the DigitField on the Child Info Type.
//                PropertyInfo pinfo = genChildInfoType.GetType().GetProperty("DigitField");
//                pinfo.SetValue(genChildInfoType, sDigitField, null);

//                Set the NameField on the Child Info Type.
//                pinfo = genChildInfoType.GetType().GetProperty("NameField");
//                pinfo.SetValue(genChildInfoType, sPhoneNumber + (iCurrentNumberOfSubBoxes + 1), null);

//                Set the DescriptionField on the Child Info Type.
//                pinfo = genChildInfoType.GetType().GetProperty("DescriptionField");
//                pinfo.SetValue(genChildInfoType, "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")", null);

//                Set the TypeField on the Child Info Type.
//                pinfo = genChildInfoType.GetType().GetProperty("TypeField");
//                pinfo.SetValue(genChildInfoType, genVoiceMailServiceAddressTypeEnum, null);

//                This disables the internet access.
//                genVoiceMailService.GetType().InvokeMember("AddNewChildMailBox", BindingFlags.InvokeMethod,
//                                                            null, genVoiceMailService,
//                                                            new object[]
//                                                                    {
//                                                                        myLoginInformation.LoginToken, 
//                                                                        parentId,
//                                                                        genChildInfoType,
//                                                                        genMailBoxType
//                                                                    });
//            }
//        }

//        [ComVisible(true)]
//        public void DeleteVoiceSubMailbox(string sPhoneNumber, string sDigitField, ref string sError)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            This will get the current mailbox info.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray = myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });
//            now cast as array so we can loop through using the foreach loop.
//            Array genVoiceMailBoxArray = result as Array;

//            If this is not null mailbox is present so we can add internet access.
//            if (genVoiceMailBoxArray == null || genVoiceMailBoxArray.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            string parentId = "";

//            foreach (var genVoiceMailBox in genVoiceMailBoxArray)
//            {
//                object genChildInfoArray = null;
//                foreach (PropertyInfo pi in genVoiceMailBox.GetType().GetProperties())
//                {
//                    if (pi.Name == "IdField")
//                        parentId = pi.GetValue(genVoiceMailBox, null).ToString();

//                    if (pi.Name == "ChildListField")
//                        genChildInfoArray = pi.GetValue(genVoiceMailBox, null);
//                }

//                genChildInfoType = (genChildInfoArray as Array).GetAddressFieldFromDigitField(sDigitField);

//                if (genChildInfoType == null)
//                    throw new Exception("Cannot find submailbox with that DigitField ID." + Environment.NewLine);

//                myVoiceMailServiceV3.DeleteChildMailBox(myLoginInformation.LoginToken, sParentID, myChildInfoType);
//                genVoiceMailService.GetType().InvokeMember("DeleteChildMailBox", BindingFlags.InvokeMethod, null,
//                    genVoiceMailService, new object[] { myLoginInformation.LoginToken, parentId, genChildInfoType });
//            }
//        }

//        [ComVisible(true)]
//        public void UpdateVoiceSubMailbox(string sPhoneNumber, int iNumberOfSubMailboxes, int iMaxNumberOfSubMailboxesAllowed)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public void UpdateVoiceMailBox(string sVoiceMailBoxUnformatedXml, string sInternetAccessUnformatedXml)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public void AddOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber, string sOutDialRoutingNumber)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public void DeleteOutDialNumber(string sPhoneNumber, string sOutDialPhoneNumber)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public void ReassignVMBoxNumber(string sOldPhoneNumber, string sNewPhoneNumber)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public void InternetPasswordReset(string sPhoneNumber, string sInternetPassword)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            PropertyInfo pi = genSubscriberInternetAccessType.GetType().GetProperty("ServiceEnabled");
//            pi.SetValue(genSubscriberInternetAccessType, true, null);
//            pi = genSubscriberInternetAccessType.GetType().GetProperty("Password");
//            pi.SetValue(genSubscriberInternetAccessType, sInternetPassword, null);

//            pi = genSubscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            pi.SetValue(genSubscriberSubType, sPhoneNumber, null);
//            pi = genSubscriberSubType.GetType().GetProperty("PlacementType");
//            pi.SetValue(genSubscriberSubType, genPlacementType, null);
//            pi = genSubscriberSubType.GetType().GetProperty("InternetAccess");
//            pi.SetValue(genSubscriberSubType, genSubscriberInternetAccessType, null);

//            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberType);
//            genSubscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new[] { myLoginInformation.LoginToken, genSubscriberSubType });
//        }

//        [ComVisible(true)]
//        public void VMPasswordReset(string sPhoneNumber, string sNewPin)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");
//        }

//        [ComVisible(true)]
//        public string GetVoiceMailBox(string sVMPhone)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            This will get the current mailbox.
//            VoiceMailBoxType[] myVoiceMailBoxTypeArray =
//                myVoiceMailServiceV3.GetVoiceMailBoxesByVoiceMailBoxNumber(myLoginInformation.LoginToken, sVMPhone);
//            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
//                return "No Mail Box found.";

//            object result = genVoiceMailService.GetType().InvokeMember("GetVoiceMailBoxesByVoiceMailBoxNumber", BindingFlags.InvokeMethod, null,
//                genVoiceMailService, new object[] { myLoginInformation.LoginToken, sVMPhone });
//            now cast as array so we can loop thorugh using the foreach loop.
//            Array genVoiceMailBoxesArray = result as Array;

//            If this is not null mailbox is present so we will delete.
//            if (genVoiceMailBoxesArray == null || genVoiceMailBoxesArray.Length == 0)
//                throw new Exception("Mailbox does not exist." + Environment.NewLine);

//            return genVoiceMailBoxesArray.SerializeObjectToString();
//        }

//        [ComVisible(true)]
//        public string GetVoiceMailBoxByID(string sMailboxID)
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            return "";
//        }

//        [ComVisible(true)]
//        public string GetAllPackages()
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            return "";
//        }

//        [ComVisible(true)]
//        public string GetAllNotificationCenters()
//        {
//            if (myVersions.Voicemail < 1)
//                throw new Exception("Apmax does not currently have any Voicemail Service setup.");

//            if (genVoiceMailService == null)
//                throw new Exception("This version (" + myVersions.Voicemail + ") of voicemail has not been implemented by Oasis.");

//            return "";
//        }
//        #endregion

//        #region ************************** CallingName *******************************
//        [ComVisible(true)]
//        public void AddCallerName(string sDN, string sCallerName, string sPresentation)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            myCallingNameServiceV3.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);
//            genCallingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName });
//        }

//        [ComVisible(true)]
//        public void DeleteCallerName(string sDN)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            myCallingNameServiceV3.DeleteCallingEntry(myLoginInformation.LoginToken, sDN, 0);
//            genCallingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, 0 });
//        }

//        [ComVisible(true)]
//        public void ModifyCallerName(string sDN, string sCallerName, string sPresentation)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            myCallingNameServiceV3.DeleteCallingEntry(myLoginInformation.LoginToken, sDN, 0);
//            genCallingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, 0 });

//            myCallingNameServiceV3.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName);
//            genCallingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, 0, sPresentation, sCallerName });
//        }

//        [ComVisible(true)]
//        public void ReassignCallerName(string sDN, string sOldDN, ref string sResponse)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            object result = genCallingNameService.GetType().InvokeMember("GetCallingEntries", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sOldDN });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            if (array == null || array.Length == 0)
//                throw new Exception("Failed to return any caller name entries." + Environment.NewLine);

//            foreach (var callerNameEntry in array)
//            {
//                PropertyInfo pi = callerNameEntry.GetType().GetProperty("Presentation");
//                string presentation = pi.GetValue(callerNameEntry, null).ToString();
//                pi = callerNameEntry.GetType().GetProperty("CName");
//                string cName = pi.GetValue(callerNameEntry, null).ToString();

//                myCallingNameServiceV3.InsertCallingEntry(myLoginInformation.LoginToken, sDN, 0, myCallingNameType.Presentation, myCallingNameType.CName);
//                genCallingNameService.GetType().InvokeMember("InsertCallingEntry", BindingFlags.InvokeMethod, null,
//                    genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, 0, presentation, cName });
//            }

//            myCallingNameServiceV3.DeleteCallingEntry(myLoginInformation.LoginToken, sOldDN, 0);
//            genCallingNameService.GetType().InvokeMember("DeleteCallingEntry", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sOldDN, 0 });
//        }

//        [ComVisible(true)]
//        public string GetCallerName(string sDN)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            CallingNameType[] myCallingNameTypeArray = myCallingNameServiceV3.GetCallingEntries(myLoginInformation.LoginToken, sDN);
//            sResponse = myCallingNameTypeArray.SerializeObjectToString();

//            return genCallingNameService.GetType().InvokeMember("GetCallingEntries", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN }).SerializeObjectToString();
//        }

//        #endregion

//        #region ************************** Subscriber Mangement *******************************

//        [ComVisible(true)]
//        public string GetSubscriber(string sPhoneNumber)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });

//            if (result == null)
//                return "A subscriber with the default phone number does not exist." + Environment.NewLine;

//            return result.SerializeObjectToString();
//        }

//        [ComVisible(true)]
//        public string GetSubscriberServices(string sPhoneNumber)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });

//            if (result == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            If this is not null mailbox is present so we will delete.
//            PropertyInfo pi = result.GetType().GetProperty("SubscriberGuid");
//            string id = pi.GetValue(result, null).ToString();

//            mySubscriberServiceV4.GetSubscriberServices(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//            result = genSubscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, id });
//            now cast as array so we can loop through using the foreach loop.
//            Array genSubscriberServicesArray = result as Array;

//            If there are no services than we can delete else throw error.
//            if (genSubscriberServicesArray != null)
//            {
//                if (genSubscriberServicesArray.Length > 0)
//                    return result.SerializeObjectToString() + Environment.NewLine;
//            }

//            return "There are no services on this subscriber.";
//        }

//        [ComVisible(true)]
//        public void DeleteSubscriberByGuid(string sGuid)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            mySubscriberServiceV4.GetSubscriberServices(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sGuid });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            If there are no services than we can delete else throw error.
//            if (array != null)
//            {
//                if (array.Length > 0)
//                    throw new Exception("This subscriber still has services connect. " + Environment.NewLine +
//                        result.SerializeObjectToString() + Environment.NewLine);
//            }

//            PropertyInfo pip = genSubscriberSubType.GetType().GetProperty("SubscriberGuid");
//            pip.SetValue(genSubscriberSubType, sGuid, null);
//            pip = genSubscriberSubType.GetType().GetProperty("PlacementType");
//            pip.SetValue(genSubscriberSubType, genPlacementType, null);

//            object[] objParms = new[] { myLoginInformation.LoginToken, genSubscriberSubType };
//            mySubscriberServiceV4.RemoveSubscriberProv(myLoginInformation.LoginToken,
//                new SubscriberServiceSubscriberTypeV4
//                {
//                    SubscriberGuid = sGuid,
//                    PlacementType = SubscriberPlacementType.PlacementType_None
//                });
//            genSubscriberService.GetType().InvokeMember("RemoveSubscriberProv", BindingFlags.InvokeMethod, null, genSubscriberService, objParms);
//        }

//        [ComVisible(true)]
//        public void DeleteSubscriberByPhoneNumber(string sPhoneNumber)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            mySubscriberServiceV4.GetSubscribersByBillingAccountNumberProv(myLoginInformation.LoginToken, accnm);
//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sPhoneNumber });

//            if (result == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            If this is not null mailbox is present so we will delete.
//            PropertyInfo pi = result.GetType().GetProperty("SubscriberGuid");
//            string id = pi.GetValue(result, null).ToString();

//            mySubscriberServiceV4.GetSubscriberServices(myLoginInformation.LoginToken, mySubscriberType.SubscriberGuid);
//            result = genSubscriberService.GetType().InvokeMember("GetSubscriberServices", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, id });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            If there are no services than we can delete else throw error.
//            if (array != null)
//            {
//                if (array.Length > 0)
//                    throw new Exception("This subscriber still has services connect. " + Environment.NewLine +
//                        result.SerializeObjectToString() + Environment.NewLine);
//            }

//            PropertyInfo pip = genSubscriberSubType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            pip.SetValue(genSubscriberSubType, sPhoneNumber, null);
//            pip = genSubscriberSubType.GetType().GetProperty("PlacementType");
//            pip.SetValue(genSubscriberSubType, genPlacementType, null);

//            object[] objParms = new[] { myLoginInformation.LoginToken, genSubscriberSubType };
//            mySubscriberServiceV4.RemoveSubscriberProv(myLoginInformation.LoginToken,
//                new SubscriberServiceSubscriberTypeV4
//                {
//                    SubscriberGuid = sGuid,
//                    PlacementType = SubscriberPlacementType.PlacementType_None
//                });
//            genSubscriberService.GetType().InvokeMember("RemoveSubscriberProv", BindingFlags.InvokeMethod, null, genSubscriberService, objParms);
//        }

//        [ComVisible(true)]
//        public void ReassignSubscriber(string sNewPhoneNumber, string sOldPhoneNumber)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            Attempts to changethe defaultsubscriberPhonenumber.
//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, sOldPhoneNumber });

//            if (result == null)
//                throw new Exception("A subscriber with the default phone number does not exist." + Environment.NewLine);

//            PropertyInfo pi = result.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            pi.SetValue(result, sNewPhoneNumber, null);

//            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberType);
//            genSubscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new[] { myLoginInformation.LoginToken, result });
//        }

//        [ComVisible(true)]
//        public void AddorUpdateSubscriber(string sSubscriberTypeUnformatedXml, string sInternetAccessTypeUnformatedXml)
//        {
//            if (myVersions.Subscriber < 1)
//                throw new Exception("Apmax does not currently have any Subscriber Service setup.");

//            if (genSubscriberService == null)
//                throw new Exception("This version (" + myVersions.Subscriber + ") of Subscriber Service has not been implemented by Oasis.");

//            This will take string xml and deserialize to an object.
//            genSubscriberSubType = sSubscriberTypeUnformatedXml.DeSerializeStringToObject(genSubscriberSubType.GetType());
//            if (genSubscriberSubType == null)
//                throw new Exception("Method AddorUpdateSubscriber encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sSubscriberTypeUnformatedXml + Environment.NewLine);

//            var internetAccess = sInternetAccessTypeUnformatedXml.DeSerializeStringToObject(genSubscriberInternetAccessType.GetType());

//            PropertyInfo pip = genSubscriberSubType.GetType().GetProperty("InternetAccess");
//            pip.SetValue(genSubscriberSubType, internetAccess, null);

//            SubscriberServiceSubscriberTypeV3 mySubscriberType = (SubscriberServiceSubscriberTypeV3)sSubscriberTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberServiceSubscriberTypeV3));
//            mySubscriberType.InternetAccess = (SubscriberInternetAccessType)sInternetAccessTypeUnformatedXml.DeSerializeStringToObject(typeof(SubscriberInternetAccessType));
//            mySubscriberServiceV3.AddOrUpdateSubscriberProv(myLoginInformation.LoginToken, mySubscriberType);
//            genSubscriberService.GetType().InvokeMember("AddOrUpdateSubscriberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new[] { myLoginInformation.LoginToken, genSubscriberSubType });
//        }

//        #endregion

//        #region ************************** ScreenPop *******************************
//        [ComVisible(true)]
//        public void AddScreenPop(string sDN)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            myCallingNameServiceV3.InsertScreenPopSubscriber(myLoginInformation.LoginToken, sDN, true);
//            genCallingNameService.GetType().InvokeMember("InsertScreenPopSubscriber", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new object[] { myLoginInformation.LoginToken, sDN, true });
//        }

//        [ComVisible(true)]
//        public void DeleteScreenPop(string sDN)
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            myCallingNameServiceV3.DeleteScreenPopSubscriber(myLoginInformation.LoginToken, sDN);
//            genCallingNameService.GetType().InvokeMember("DeleteScreenPopSubscriber", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new[] { myLoginInformation.LoginToken, sDN });
//        }

//        [ComVisible(true)]
//        public string GetAllScreenPopEntries()
//        {
//            if (myVersions.CallingName < 1)
//                throw new Exception("Apmax does not currently have any Caller Name Service setup.");

//            if (genCallingNameService == null)
//                throw new Exception("This version (" + myVersions.CallingName + ") of Caller Name Service has not been implemented by Oasis.");

//            ScreenPopType[] myScreenPopType = myCallingNameServiceV3.GetAllScreenPopEntries(myLoginInformation.LoginToken);
//            sResponse = myScreenPopType.SerializeObjectToString();
//            return genCallingNameService.GetType().InvokeMember("GetAllScreenPopEntries", BindingFlags.InvokeMethod, null,
//                genCallingNameService, new[] { myLoginInformation.LoginToken }).SerializeObjectToString();
//        }
//        #endregion

//        #region ************************** Calling Number Announcement CNA *******************************
//        [ComVisible(true)]
//        public void AddCnaNumber(string sXmlCnaInfo)
//        {
//            if (myVersions.ChangeNumberAnnouncements < 1)
//                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

//            if (genCNAService == null)
//                throw new Exception("This version (" + myVersions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

//            This will take string xml and deserialize to an object.
//            genCnaInfo = sXmlCnaInfo.DeSerializeStringToObject(genCnaInfo.GetType());
//            if (genCnaInfo == null)
//                throw new Exception("Method AddCnaNumber encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sXmlCnaInfo + Environment.NewLine);

//            if (myVersions.ChangeNumberAnnouncements < 1)
//                throw new Exception("Apmax does not currently have any Change Number Announcements setup.");

//            if (genCNAService == null)
//                throw new Exception("This version (" + myVersions.ChangeNumberAnnouncements + ") of CNA has not been implemented by Oasis.");

//            DateTime currentDate = DateTime.Now;
//            TimeZone localZone = TimeZone.CurrentTimeZone;
//            TimeSpan currentOffset = localZone.GetUtcOffset( currentDate );
//            DateTime.Now + currentOffset;
//            DateTime.UtcNow;
//            DateTime myDateTime = currentDate.ToLocalTime();
//            DateTime.Parse("2009-10-28T16:32:19.1217873-05:00");
//            currentDate.Add();
//            currentDate.GetDateTimeFormats

//            PropertyInfo pi = genCnaInfo.GetType().GetProperty("FromNumber");
//            string fromNumber = pi.GetValue(genCnaInfo, null).ToString();
//            pi = genCnaInfo.GetType().GetProperty("ToNumber");
//            string toNumber = pi.GetValue(genCnaInfo, null).ToString();

//            CnaInfo[] myCnaInfoArray = myCNAServiceV3.GetAllCnaAnnouncements(myLoginInformation.LoginToken);
//            object result = genCNAService.GetType().InvokeMember("GetAllCnaAnnouncements", BindingFlags.InvokeMethod, null,
//                genCNAService, new[] { myLoginInformation.LoginToken });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            if (array != null)
//            {
//                foreach (var oldCnaInfo in array)
//                {
//                    pi = oldCnaInfo.GetType().GetProperty("FromNumber");
//                    string oldFromNumber = pi.GetValue(oldCnaInfo, null).ToString();
//                    pi = oldCnaInfo.GetType().GetProperty("toNumber");
//                    string oldToNumber = pi.GetValue(oldCnaInfo, null).ToString();

//                    check and see if this CNA number already is in the system if it is remove all occurences.
//                    if (oldFromNumber.Equals(fromNumber, StringComparison.OrdinalIgnoreCase))
//                    {
//                        myCNAServiceV3.DeleteCnaNumber(myLoginInformation.LoginToken, myCnaInfo.FromNumber, myCnaInfo.ToNumber);
//                        genCNAService.GetType().InvokeMember("DeleteCnaNumber", BindingFlags.InvokeMethod, null,
//                            genCNAService, new object[] { myLoginInformation.LoginToken, oldFromNumber, oldToNumber });
//                    }
//                }
//            }

//            Now once we no there are not other occurences add this CNA number.
//            myCNAServiceV3.SetCnaNumber(myLoginInformation.LoginToken, Object);
//            genCNAService.GetType().InvokeMember("SetCnaNumber", BindingFlags.InvokeMethod, null,
//                    genCNAService, new object[] { myLoginInformation.LoginToken, fromNumber, toNumber });
//        }

//        [ComVisible(true)]
//        public void DeleteCnaNumber(string sFromPhoneNumber, string sToPhoneNumber)
//        {
//            if (myVersions.ChangeNumberAnnouncements < 1)
//                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

//            if (genCNAService == null)
//                throw new Exception("This version (" + myVersions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

//            myCNAServiceV3.DeleteCnaNumber(myLoginInformation.LoginToken, sFromPhoneNumber, sToPhoneNumber);
//            genCNAService.GetType().InvokeMember("DeleteCnaNumber", BindingFlags.InvokeMethod, null,
//                genCNAService, new[] { myLoginInformation.LoginToken, sFromPhoneNumber, sToPhoneNumber });
//        }

//        [ComVisible(true)]
//        public string GetCnaAnnouncement(string sFromPhoneNumber, string sToPhoneNumber)
//        {
//            if (myVersions.ChangeNumberAnnouncements < 1)
//                throw new Exception("Apmax does not currently have any Change Number Announcements Service setup.");

//            if (genCNAService == null)
//                throw new Exception("This version (" + myVersions.ChangeNumberAnnouncements + ") of CNA Service has not been implemented by Oasis.");

//            object result = genCNAService.GetType().InvokeMember("GetAllCnaAnnouncements", BindingFlags.InvokeMethod, null,
//                genCNAService, new[] { myLoginInformation.LoginToken });
//            now cast as array so we can loop through using the foreach loop.
//            Array array = result as Array;

//            if (array != null)
//            {
//                foreach (var oldCnaInfo in array)
//                {
//                    PropertyInfo pi = oldCnaInfo.GetType().GetProperty("FromNumber");
//                    string oldFromNumber = pi.GetValue(oldCnaInfo, null).ToString();
//                    pi = oldCnaInfo.GetType().GetProperty("toNumber");
//                    string oldToNumber = pi.GetValue(oldCnaInfo, null).ToString();

//                    check and see if this CNA number already is in the system if it is remove all occurences.
//                    if (oldFromNumber.Equals(sFromPhoneNumber, StringComparison.OrdinalIgnoreCase) &&
//                        oldToNumber.Equals(sToPhoneNumber, StringComparison.OrdinalIgnoreCase))
//                    {
//                        return oldCnaInfo.SerializeObjectToString();
//                    }
//                }
//            }

//            return "No matching To and From numbers found.";
//        }
//        #endregion

//        #region ************************** IPTV *******************************

//        [ComVisible(true)]
//        public void SetIPTVAccount(string sIPTVAccountTypeUnformatedXml, string sSubscriberTypeUnformatedXml)
//        {
//            if (myVersions.Iptv < 1)
//                throw new Exception("Apmax does not currently have any IPTV Service setup.");

//            if (genIPTVService == null)
//                throw new Exception("This version (" + myVersions.Iptv + ") of IPTV Service has not been implemented by Oasis.");

//              <IPTVAccountType>
//                <ExtensionData />
//                <AccountDescription />
//                <Active>true</Active>
//                <ChannelPackageList>
//                  <ChannelPackageType>
//                    <ExtensionData />
//                    <PackageID>1b1cb36c-15f8-4970-b5b9-b8f329aee7ad</PackageID>
//                    <PackageName>Test</PackageName>
//                  </ChannelPackageType>
//                </ChannelPackageList>
//                <CurrentAmountCharged>0</CurrentAmountCharged>
//                <DeactivateReason />
//                <FIPSCountyCode>35</FIPSCountyCode>
//                <FIPSStateCode>46</FIPSStateCode>
//                <MaxBandwidthKbps>0</MaxBandwidthKbps>
//                <MaxChargingLimit>0</MaxChargingLimit>
//                <PurchasePIN />
//                <RatingPIN />
//                <ServiceAreaID>754cd53a-30c7-4130-8fa7-0a4d53f0b1be</ServiceAreaID>
//                <ServiceReference>6059974200</ServiceReference>
//                <SubscriberID>73cd8543-2c99-44c6-ac87-8702c3174e49</SubscriberID>
//                <SubscriberName>CHR Solutions</SubscriberName>
//              </IPTVAccountType>

//            This will take string xml and deserialize to an object.
//            genIPTVAccountType = sIPTVAccountTypeUnformatedXml.DeSerializeStringToObject(genIPTVAccountType.GetType());
//            if (genIPTVAccountType == null)
//                throw new Exception("Method SetIPTVAccount encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sIPTVAccountTypeUnformatedXml + Environment.NewLine);

//            genIPTVSubscriberType = sIPTVAccountTypeUnformatedXml.DeSerializeStringToObject(genIPTVSubscriberType.GetType());
//            if (genIPTVSubscriberType == null)
//                throw new Exception("Method SetIPTVAccount encountered exception: Unable to parse xml or invalid xml." +
//                    Environment.NewLine + "XML recieved: " + sIPTVAccountTypeUnformatedXml + Environment.NewLine);

//            May need this if the subscriber already exists.
//            PropertyInfo pi = genIPTVSubscriberType.GetType().GetProperty("BillingServiceAddress");
//            string billingServiceAddress = pi.GetValue(genIPTVSubscriberType, null).ToString();
//            pi = genIPTVSubscriberType.GetType().GetProperty("SubscriberDefaultPhoneNumber");
//            string subscriberDefaultPhoneNumber = pi.GetValue(genIPTVSubscriberType, null).ToString();

//            Get the existing subscriber info.
//            object result = genSubscriberService.GetType().InvokeMember("GetSubscriberByNumberProv", BindingFlags.InvokeMethod, null,
//                genSubscriberService, new object[] { myLoginInformation.LoginToken, subscriberDefaultPhoneNumber });
//            if (result != null)
//            {
//                Take what we got from the GetSubscriberByNumberProv and send everything back as is.
//                We must
//                genIPTVSubscriberType = result.SerializeObjectToString().DeSerializeStringToObject(genIPTVSubscriberType.GetType());

//                Always get the guid on the account if the subscriber already exists.
//                pi = result.GetType().GetProperty("SubscriberGuid");
//                string SubscriberGuid = pi.GetValue(result, null).ToString();
//                Then set the guid on the iptvaccount.
//                pi = genIPTVSubscriberType.GetType().GetProperty("SubscriberID");
//                pi.SetValue(genIPTVSubscriberType, SubscriberGuid, null);

//                Always set the Billing Service Address on the account if the subscriber already exists.
//                pi = genIPTVSubscriberType.GetType().GetProperty("BillingServiceAddress");
//                pi.SetValue(genIPTVSubscriberType, billingServiceAddress, null);
//            }

//            myIPTVServiceClientV3.SetIPTVAccount(myLoginInformation.LoginToken, myIPTVAccountType, myIPTVSubscriberType);
//            genIPTVService.GetType().InvokeMember("SetIPTVAccount", BindingFlags.InvokeMethod, null,
//                genCNAService, new object[] { myLoginInformation.LoginToken, genIPTVAccountType, result });
//        }

//        [ComVisible(true)]
//        public void DeleteIPTVAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
//        {

//        }

//        [ComVisible(true)]
//        public void ForceDeleteIPTVAccount(string sSubAddress, string sServiceReference, bool blnDeleteSubscriber)
//        {

//        }

//        [ComVisible(true)]
//        public void SetIPTVChannelPackageList(string sSubAddress, string sServiceReference, string sChannelPackageListUnformatedXml)
//        {

//        }

//        [ComVisible(true)]
//        public void DisableIPTVAccount(string sSubAddress, string sServiceReference)
//        {

//        }

//        [ComVisible(true)]
//        public void EnableIPTVAccount(string sSubAddress, string sServiceReference)
//        {

//        }

//        [ComVisible(true)]
//        public void RemoveSTBFromIPTVAccount(string sSubAddress, string sServiceReference, string sMacAddress)
//        {

//        }

//        [ComVisible(true)]
//        public void DeauthorizeSTB(string sSubAddress, string sServiceReference, string sMacAddress)
//        {

//        }

//        [ComVisible(true)]
//        public string GetIPTVSubscribersByMAC(string sMacAddress)
//        {
//            return "";
//        }

//        [ComVisible(true)]
//        public string GetIPTVSubscribersBySerialNumber(string sSerialNumber)
//        {
//            return "";
//        }

//        [ComVisible(true)]
//        public string GetIPTVAccountsBySubAddress(string sSubAddress)
//        {
//            return "";
//        }

//        [ComVisible(true)]
//        public string GetIPTVAccountBySubAddressAndServiceRef(string sSubAddress, string sServiceReference)
//        {
//            return "";
//        }

//        [ComVisible(true)]
//        public string GetAllChannelLineups()
//        {
//            return "";
//        }
//        #endregion

//        #region ************************** LargeScaleConference *******************************
//        [ComVisible(true)]
//        public void InsertFreeConference()
//        {
//            myLargeScaleConference.InsertFreeConference():
//        }
//        #endregion

//        #region ************************** TCMService *******************************

//        [ComVisible(true)]
//        public void AddTCM(string sPhoneNumber, bool blnEnableTCM, bool blnEnableDND, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void UpdateTCM(string sSubscriberAddress, string sTCMTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void DeleteTCM(string sSubscriberAddress)
//        {

//        }

//        [ComVisible(true)]
//        public void SetDNDFeature(string sSubscriberAddress, bool blnEnableStatus)
//        {

//        }

//        [ComVisible(true)]
//        public void SetTCMFeature(string sSubscriberAddress, bool blnEnableStatus)
//        {

//        }

//        [ComVisible(true)]
//        public string GetTCMInfo(string sSubscriberAddress)
//        {
//            return "";
//        }

//        #endregion

//        #region ************************** Call Logging *******************************

//        [ComVisible(true)]
//        public void AddCPL(string sPhoneNumber, string sEmailAddress, bool bWebPortalEnabled)
//        {

//        }

//        [ComVisible(true)]
//        public void UpdateCPL(string sPhoneNumber, string sEmailAddress, bool bWebPortalEnabled)
//        {

//        }

//        [ComVisible(true)]
//        public void DeleteCPL(string sPhoneNumber)
//        {

//        }

//        [ComVisible(true)]
//        public string GetCPL(string sPhoneNumber)
//        {
//            return "";
//        }
//        #endregion

//        #region ************************** Local Number Portability *******************************

//        [ComVisible(true)]
//        public void GetAllPortabilityEntries()
//        {

//        }

//        #endregion

//        #region ************************** Orginating Call Management Service Vars *******************************

//        [ComVisible(true)]
//        public void AddOCM(string sPhoneNumber, string sOCMTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void UpdateOCM(string sPhoneNumber, string sOCMTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void DeleteOCM(string sPhoneNumber)
//        {

//        }

//        [ComVisible(true)]
//        public string GetOCM(string sPhoneNumber)
//        {
//            return "";
//        }

//        #endregion

//        #region ************************** Universal Call Management *******************************

//        [ComVisible(true)]
//        public void AddUCM(string sPhoneNumber, string sUCMTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void UpdateUCM(string sPhoneNumber, string sUCMTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void DeleteUCM(string sPhoneNumber)
//        {

//        }

//        [ComVisible(true)]
//        public string GetUCM(string sPhoneNumber)
//        {
//            return "";
//        }

//        #endregion

//        #region ************************** ODConferencing *******************************

//        [ComVisible(true)]
//        public void AddODConferencing(string sPhoneNumber, string sODCTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public void DeleteODConferencing(string sSubscriberAddress)
//        {

//        }

//        [ComVisible(true)]
//        public void UpdateODConferencing(string sSubscriberAddress, string sODCTypeXml, string sInternetAccessTypeXml)
//        {

//        }

//        [ComVisible(true)]
//        public string GetODConferencing(string sSubscriberAddress)
//        {
//            return "";
//        }

//        #endregion

//        [ComVisible(true)]
//        public string FormatXML(string sUnformatedXml)
//        {
//            if (!XmlHelper.ValidateXml(sUnformatedXml))
//                return sUnformatedXml;

//            return XmlHelper.XmlFormater2(sUnformatedXml);
//        }

//        [ComVisible(true)]
//        public bool ParseXML(string sUnformatedXml)
//        {
//            return XmlHelper.ValidateXml(sUnformatedXml);
//        }

//        [ComVisible(false)]
//        private static List<int> GetAvailableDigitFieldsFromChildList(IEnumerable<ChildInfoType> myChildInfoTypeArray, int iMaxNumberOfSubMailboxesAllowed)
//        {
//            List<int> AvailableDigitFieldList = new List<int>();

//            for (int count = 1; count <= iMaxNumberOfSubMailboxesAllowed; count++)
//            {
//                bool blnFoundMatch = false;
//                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
//                {
//                    if (Convert.ToInt32(myChildInfoType.DigitField) == count)
//                        blnFoundMatch = true;
//                }

//                if (!blnFoundMatch)
//                    AvailableDigitFieldList.Add(count);
//            }

//            return AvailableDigitFieldList;
//        }
//    }

//    Extension methods must be defined in a static class
//    public static class ExtensionClass
//    {
//        1. Shows up in intellisense
//        2. add new methods to objects that were already defined, especially when you don't own/control the source to the original object
//        3. The only advantage of extension methods is code readability

//         This is the extension method.
//         The first parameter takes the "this" modifier
//         and specifies the type for which the method is defined.
//        public static ChildInfoType GetAddressFieldFromDigitField(this ChildInfoType[] myChildInfoTypeArray, string sDigitField)
//        {
//            foreach (var myChild in myChildInfoTypeArray)
//            {
//                if (myChild.DigitField.Equals(sDigitField, StringComparison.OrdinalIgnoreCase))
//                {
//                    return myChild;
//                }
//            }

//            return null;
//        }

//        public static object GetAddressFieldFromDigitField(this Array childInfoArray, string sDigitField)
//        {
//            foreach (var child in childInfoArray)
//            {
//                PropertyInfo pi = child.GetType().GetProperty("DigitField");
//                string digitField = pi.GetValue(child, null).ToString();

//                if (digitField.Equals(sDigitField, StringComparison.OrdinalIgnoreCase))
//                {
//                    return child;
//                }
//            }

//            return null;
//        }

//        public static ChildInfoType GetChildInfoByHighestDigitField(this ChildInfoType[] myChildInfoTypeArray)
//        {
//            ChildInfoType CurrentChildInfoType = new ChildInfoType() { DigitField = "-1" };
//            foreach (var myChild in myChildInfoTypeArray)
//            {
//                Submailboxes are always AddressType of AddressTypeMailboxNumber.  Outdialnumbers are AddressTypeDN.
//                if (Convert.ToInt32(myChild.DigitField) > Convert.ToInt32(CurrentChildInfoType.DigitField)
//                    && myChild.TypeField == VoiceMailServiceV3.AddressType.AddressTypeMailboxNumber)
//                    CurrentChildInfoType = myChild;
//            }

//            return CurrentChildInfoType.DigitField == "-1" ? null : CurrentChildInfoType;
//        }

//        public static int WordCount(this String str)
//        {
//            return str.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
//        }

//        public static string HtmlDecode(this string str)
//        {
//            return HttpUtility.HtmlDecode(str);
//        }

//        public static object DeSerializeStringToObject(this string sxml, Type type)
//        {
//            using (XmlTextReader xreader = new XmlTextReader(new StringReader(sxml.Replace("&", "&amp;"))))
//            {
//                XmlSerializer xs = new XmlSerializer(type);
//                return xs.Deserialize(xreader);
//            }
//        }

//        public static string SerializeObjectToString(this object obj)
//        {
//            using (MemoryStream stream = new MemoryStream())
//            {
//                XmlSerializer x = new XmlSerializer(obj.GetType());
//                x.Serialize(stream, obj);
//                return Encoding.Default.GetString(stream.ToArray());
//            }
//        }
//    }
//}
