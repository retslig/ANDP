using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Integration;
using Common.ServiceReport;
using Common.SubscriberV4;
using InternetAccessType = Common.SubscriberV4.InternetAccessType;
using PlacementType_e = Common.SubscriberV4.PlacementType_e;
using SubscriberType = Common.SubscriberV4.SubscriberType;

namespace Common.ApMax
{
    public class SubscriberV4Service
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly ServiceVersions _versions;
        private readonly LoginInformation _loginInformation;
        private readonly SubscriberServiceClient _subscriberService;

        public SubscriberV4Service(EquipmentConnectionSetting setting, ServiceVersions serviceVersions)
        {
            _setting = setting;
            _versions = serviceVersions;

            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var admin = new ApAdminClient("WSHttpBinding_IApAdmin", 
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4/
            _subscriberService = new SubscriberServiceClient("WSHttpBinding_ISubscriberService4",
                new EndpointAddress("http://" + setting.Ip + ":" + setting.Port + "/Design_Time_Addresses/SubscriberServiceV4/SubscriberV4/"));

            _loginInformation = admin.LoginAdv(setting.CustomString1, setting.Username, setting.Password);
        }

        public SubscriberType RetrieveSubscriberByDefaultPhoneNumber(string defaultPhoneNumber)
        {
            defaultPhoneNumber = defaultPhoneNumber.PadLeft(10, '0');

            SubscriberType mySubscriberType = _subscriberService.GetSubscriberByNumberProv(_loginInformation.LoginToken, defaultPhoneNumber);

            return mySubscriberType;
        }

        public SubscriberType RetrieveSubscriberByGuid(Guid guid)
        {
            var subscriberType = _subscriberService.GetSubscriberByGuidProv(_loginInformation.LoginToken, guid.ToString());
            return subscriberType;
        }

        public SubscriberType RetrieveSubscriberByBillingAccount(string billingAccount, string billingServiceId)
        {
            var subscriberType = _subscriberService.GetSubscriberByBillingProv(_loginInformation.LoginToken, billingAccount, billingServiceId);
            return subscriberType;
        }

        public List<ServiceInfoType> RetrieveSubscriberServicesByGuid(Guid guid)
        {
            var serviceInfoTypes = _subscriberService.GetSubscriberServices(_loginInformation.LoginToken, guid.ToString()).ToList();
            return serviceInfoTypes;
        }

        public List<ServiceInfoType> RetrieveSubscriberServicesByPhoneNumber(string defaultPhoneNumber)
        {
            var subscriberType = RetrieveSubscriberByDefaultPhoneNumber(defaultPhoneNumber);

            if (subscriberType == null || subscriberType.SubscriberGuid == null)
                return null;

            var guid = Guid.Parse(subscriberType.SubscriberGuid);

            var serviceInfoTypes = RetrieveSubscriberServicesByGuid(guid);
            return serviceInfoTypes;
        }

        public List<ServiceInfoType> RetrieveSubscriberServicesByBillingAccount(string billingAccount, string billingServiceId)
        {
            var subscriberType = RetrieveSubscriberByBillingAccount(billingAccount, billingServiceId);

            if (subscriberType == null || subscriberType.SubscriberGuid == null)
                return null;

            var guid = Guid.Parse(subscriberType.SubscriberGuid);

            var serviceInfoTypes = RetrieveSubscriberServicesByGuid(guid);
            return serviceInfoTypes;
        }

        public List<string> RetrieveServiceGuidsByServiceTypeForAudit(int offSet, int numberOfAccounts, ServiceType_e typeOfAccountToAudit)
        {
            var auditList = new List<string>();
            //var addresses = _subscriberService.AuditSubscriberAddresses(_loginInformation.LoginToken, startingAddress, numberOfAccounts);
            var addresses = _subscriberService.AuditSubscriberGuids(_loginInformation.LoginToken, numberOfAccounts, offSet);
            foreach (var address in addresses)
            {
                var services = _subscriberService.GetSubscriberServices(_loginInformation.LoginToken, address);
                var service = services.FirstOrDefault(p => p.ServiceType == typeOfAccountToAudit);
                if (service != null)
                    auditList.Add(service.ServiceGuid);
            }

            return auditList;
            //return (from address in _subscriberService.AuditSubscriberAddresses(_loginInformation.LoginToken, startingAddress, numberOfAccounts)
            //        select _subscriberService.GetSubscriberServices(_loginInformation.LoginToken, address)
            //            into services
            //            select services.FirstOrDefault(p => p.ServiceType == typeOfAccountToAudit)
            //                into service
            //                where service != null
            //                select service.BillingServiceID).ToList();
        }

        public SubscriberInternetAccessType RetrieveSubscriberInternetAccess(Guid subscriberGuid)
        {
            return _subscriberService.GetSubscriberInternetAccess(_loginInformation.LoginToken, subscriberGuid.ToString());
        }

        public void AddOrUpdateSubscriberProv(SubscriberType subscriberType)
        {
            _subscriberService.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, subscriberType);
        }

        public void DeleteSubscriberByGuid(Guid guid)
        {
            var serviceInfoTypes = RetrieveSubscriberServicesByGuid(guid);

            if (serviceInfoTypes != null && serviceInfoTypes.Any())
                throw new Exception("Subscriber still has services. Unable to delete.");

            _subscriberService.RemoveSubscriberProv(_loginInformation.LoginToken,
                new SubscriberType
                {
                    SubscriberGuid = guid.ToString(),
                    PlacementType = PlacementType_e.PlacementType_None
                });
        }

        public void DeleteSubscriberByDefaultPhoneNumber(string defaultPhoneNumber)
        {
            var serviceInfoTypes = RetrieveSubscriberServicesByPhoneNumber(defaultPhoneNumber);

            if (serviceInfoTypes != null && serviceInfoTypes.Any())
                throw new Exception("Subscriber still has services. Unable to delete.");

            _subscriberService.RemoveSubscriberProv(_loginInformation.LoginToken, new SubscriberType
                    {
                        SubscriberDefaultPhoneNumber = defaultPhoneNumber,
                        PlacementType = PlacementType_e.PlacementType_None
                    });
        }

        public void ReassignSubscriber(string newPhoneNumber, string oldPhoneNumber, string internetPassword)
        {
            var mySubscriberServiceSubscriberType = _subscriberService.GetSubscriberByNumberProv(_loginInformation.LoginToken, oldPhoneNumber);

            if (mySubscriberServiceSubscriberType == null)
                throw new Exception("Subscriber does not exist for DN " + oldPhoneNumber + ".");

            var mySubscriberInternetAccessType = _subscriberService.GetSubscriberInternetAccess(_loginInformation.LoginToken,
                mySubscriberServiceSubscriberType.SubscriberGuid);

            if (mySubscriberInternetAccessType != null)
            {
                //internet access is not returned from get subscriber
                mySubscriberServiceSubscriberType.InternetAccess = new InternetAccessType
                {
                    ServiceEnabled = mySubscriberInternetAccessType.InternetAccessEnabled,
                    Password = internetPassword,
                    UserName = mySubscriberInternetAccessType.UserName
                };
            }

            //Creates new subscriber.
            mySubscriberServiceSubscriberType.SubscriberGuid = "";
            mySubscriberServiceSubscriberType.SubscriberDefaultPhoneNumber = newPhoneNumber;
            mySubscriberServiceSubscriberType.ServiceInformation = null;
            _subscriberService.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, mySubscriberServiceSubscriberType);

            //Reassigning Voicemail
            switch (_versions.Voicemail)
            {
                case 3:
                {
                    var apMaxVoicemailV3Service = new VoicemailV3Service(_setting, _versions);

                    //This will get the current mailbox info before we delete the box.
                    VoicemailV3.VoiceMailBoxType[] myVoiceMailBoxTypeArray = apMaxVoicemailV3Service.GetVoiceMailBoxesByVoiceMailBoxNumber(oldPhoneNumber);

                    //ToDo: need to go through what's being passed in. such as internet access
                    apMaxVoicemailV3Service.ReassignVmBoxNumber(oldPhoneNumber, newPhoneNumber,
                        myVoiceMailBoxTypeArray[0].DescriptionField, myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField,
                        internetPassword, mySubscriberInternetAccessType.UserName,
                        mySubscriberInternetAccessType != null, false);

                    break;
                }
                default:
                    throw new NotImplementedException();

            }

            //Reassigning On Demand Conferencing
            switch (_versions.OnDemandConference)
            {
                case 3:
                {
                    var apMaxOdV3Service = new OnDemandConferencingV3Service(_setting);
                    var myConferenceType = apMaxOdV3Service.RetrieveConferenceSubByAddress(oldPhoneNumber);

                    if (myConferenceType != null)
                    {
                        var myOdSubscriberType =
                            mySubscriberServiceSubscriberType.SerializeObjectToString()
                                .DeSerializeStringToObject(typeof (OnDemandConferencingV3.SubscriberType)) as
                                OnDemandConferencingV3.SubscriberType;

                        myConferenceType.SubscriberAddress = newPhoneNumber;
                        myConferenceType.SubscriberGuid = "";

                        //Add Conference
                        apMaxOdV3Service.AddConferencingSub(myConferenceType, myOdSubscriberType);

                        //Delete Conference
                        apMaxOdV3Service.DeleteConferenceBySubAddress(oldPhoneNumber);
                    }
                    break;
                }
                default:
                    throw new NotImplementedException();
            }

            //Reassigning Call Logging
            switch (_versions.CallLogging)
            {
                case 3:
                {
                    var apMaxCallLoggingService = new CallLoggingV3Service(_setting);
                    var myCallLoggingTypeArray = apMaxCallLoggingService.FindCallLoggingSubscribers(oldPhoneNumber);


                    if (myCallLoggingTypeArray.Length > 0)
                    {
                        var myCPLSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(Common.CallLoggingV3.SubscriberType)) as CallLoggingV3.SubscriberType;
                        myCallLoggingTypeArray[0].SubscriberDn = newPhoneNumber;
                        myCallLoggingTypeArray[0].SubscriberID = "";
                        
                        //Add CLP
                        apMaxCallLoggingService.AddClpSubscriberRecord(
                            new CallLoggingV3.CallLoggingType
                            {
                                EmailAddress = myCallLoggingTypeArray[0].EmailAddress,
                                SubscriberDn = newPhoneNumber,
                                WebPortalEnabled = myCallLoggingTypeArray[0].WebPortalEnabled
                            },
                            myCPLSubscriberType);

                        //Delete Conference
                        apMaxCallLoggingService.RemoveClpSubscriberRecord(oldPhoneNumber);
                    }

                    break;
                }
                default:
                    throw new NotImplementedException();
            }

            //Reassigning Originating Call Management
            switch (_versions.OriginatingCallManagement)
            {
                case 1:
                {
                    var myOcmService = new OriginatingCallManagmentV1Service(_setting);

                    var myOcmType = myOcmService.GetSubscriberBySubGuid(mySubscriberServiceSubscriberType.SubscriberGuid);

                    if (myOcmType != null)
                    {
                        var myOcmSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(OriginatingCallManagementV1.SubscriberType)) as OriginatingCallManagementV1.SubscriberType;
                        myOcmType.SubscriberGuid = "";

                        //Add OCM
                        myOcmService.AddSubscriberRecord(myOcmType, myOcmSubscriberType);

                        //Delete OCM
                        myOcmService.DeleteSubscriberRecord(mySubscriberServiceSubscriberType.SubscriberGuid);
                    }

                    break;
                }
                default:
                    throw new NotImplementedException();
            }

            //Reassign UniversalCallManagement
            switch (_versions.UniversalCallManagement)
            {
                case 3:
                {
                    var myUcmService = new UniversalCallManagementV3(_setting);

                    var myUcmType = myUcmService.GetUniversalCallManagerNumber(oldPhoneNumber); 

                    if (myUcmType != null)
                    {
                        var myUcmSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(UniveralCallManagementV3.SubscriberType)) as UniveralCallManagementV3.SubscriberType;
                        myUcmType.SubscriberID = "";

                        //Add OCM
                        myUcmService.AddUniversalCallManagerSubscriber(myUcmType, myUcmSubscriberType);

                        //Delete OCM
                        myUcmService.DeleteUniversalCallManagerSubscriber(oldPhoneNumber);
                    }

                    break;
                }
                default:
                    throw new NotImplementedException();
            }

            //Reassigning Terminating Call Management
            switch (_versions.UniversalCallManagement)
            {
                case 3:
                {
                    var myTcmService = new TerminatingCallManagementV3Service(_setting);
                    var myTcmType = myTcmService.RetrieveTcmSubscriber(oldPhoneNumber);
                    if (myTcmType != null)
                    {
                        var myTcmSubscriberType = mySubscriberServiceSubscriberType.SerializeObjectToString().DeSerializeStringToObject(typeof(TerminatingCallManagementV3.SubscriberType)) as TerminatingCallManagementV3.SubscriberType;
                        myTcmType.SubscriberDn = newPhoneNumber;
                        myTcmType.SubscriberGuid = "";

                        //Add TCM
                        myTcmService.AddTcmSubscriber(newPhoneNumber, (bool)myTcmType.TcmFeatureEnabled, (bool)myTcmType.DndFeatureEnabled, myTcmSubscriberType);

                        //Delete TCM
                        myTcmService.DeleteTcmSubscriber(oldPhoneNumber);
                    }

                    //Removes the old number.
                    _subscriberService.RemoveSubscriberProv(_loginInformation.LoginToken, new SubscriberType
                    {
                        SubscriberDefaultPhoneNumber = oldPhoneNumber,
                        PlacementType = PlacementType_e.PlacementType_None
                    });
                    break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        public void InternetPasswordReset(string phoneNumber, string internetPassword)
        {
            if (phoneNumber.Length != 0)
            {
                SubscriberType mySubscriberType = _subscriberService.GetSubscriberByNumberProv(_loginInformation.LoginToken, phoneNumber);

                if (mySubscriberType == null)
                {
                    throw new Exception("A subscriber with the default phone number does not exist. Phone Number = " +
                                        phoneNumber + Environment.NewLine);
                }

                SubscriberInternetAccessType mySubscriberInternetAccessType = _subscriberService.GetSubscriberInternetAccess(_loginInformation.LoginToken, mySubscriberType.SubscriberGuid);

                if (mySubscriberInternetAccessType == null)
                {
                    throw new Exception("A subscriber with this guid does not exist. Guid = " + mySubscriberType.SubscriberGuid + Environment.NewLine);
                }

                //internet access is not returned from get subscriber
                mySubscriberType.InternetAccess = new InternetAccessType
                {
                    ServiceEnabled = mySubscriberInternetAccessType.InternetAccessEnabled,
                    Password = internetPassword
                };

                _subscriberService.AddOrUpdateSubscriberProv(_loginInformation.LoginToken, mySubscriberType);
            }
        }
    }
}
