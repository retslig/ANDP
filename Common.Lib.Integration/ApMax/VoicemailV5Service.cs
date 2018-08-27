using System;
using System.Collections.Generic;
using System.ServiceModel;
using Common.ApAdmin;
using Common.Lib.Domain.Common.Models;
using Common.ServiceReport;
using Common.VoicemailV5;

namespace Common.ApMax
{
    public class VoicemailV5Service
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly ServiceVersions _versions;
        private readonly LoginInformation _loginInformation;
        private readonly VoicemailClient _voiceMailService;

        public VoicemailV5Service(EquipmentConnectionSetting setting, ServiceVersions serviceVersions)
        {
            _setting = setting;
            _versions = serviceVersions;

            //http://localhost:8731/Design_Time_Addresses/LoginService/ApAdmin/
            var apAdmin = new ApAdminClient("WSHttpBinding_IApAdmin", new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/LoginService/ApAdmin/"));

            //http://localhost:8731/Design_Time_Addresses/VoicemailService/VoicemailV5/
            //http://ANDPSERVER:8731/Design_Time_Addresses/VoicemailService/VoicemailV5/
            _voiceMailService = new VoicemailClient("WSHttpBinding_IVoicemail2", new EndpointAddress("http://" + _setting.Ip + ":" + _setting.Port + "/Design_Time_Addresses/VoicemailService/VoicemailV5/"));

            _loginInformation = apAdmin.LoginAdv(_setting.CustomString1, _setting.Username, _setting.Password);
        }

        public IEnumerable<VoiceMailBoxType> RetrieveVoicemail(string phoneNumber)
        {
            //This will get the current mailbox.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
                return null;

            return myVoiceMailBoxTypeArray;
        }

        public VoiceMailBoxType[] GetVoiceMailBoxesByVoiceMailBoxNumber(string phoneNumber)
        {
            return _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);
        }

        public IEnumerable<PackageType> RetrieveAllPackages()
        {
            PackageType[] myPackageTypeArray = _voiceMailService.GetAllPackages(_loginInformation.LoginToken);
            return myPackageTypeArray;
        }

        public IEnumerable<NotificationCenterInfoType> RetrieveAllNotifcationCenters()
        {
            NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);
            return myNotificationCenterInfoTypeArray;
        }


        public void SuspendVoicemail(string phoneNumber, bool suspend)
        {
            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox does not exist.");
            }

            myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
            myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
            myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
            myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
            myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
            myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
            myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
            myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
            myVoiceMailBoxTypeArray[0].CallingPartyField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningField = null;
            myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
            myVoiceMailBoxTypeArray[0].ChildListField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
            myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
            myVoiceMailBoxTypeArray[0].DescriptionField = null;
            myVoiceMailBoxTypeArray[0].DirectoryField = null;

            myVoiceMailBoxTypeArray[0].Disabled = suspend;
            
            myVoiceMailBoxTypeArray[0].DistributionListField = null;
            myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
            myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
            myVoiceMailBoxTypeArray[0].ExtensionData = null;
            myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
            myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
            myVoiceMailBoxTypeArray[0].ForwardingListField = null;
            myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
            myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
            myVoiceMailBoxTypeArray[0].GreetingListField = null;
            myVoiceMailBoxTypeArray[0].HolidayListField = null;
            //myVoiceMailBoxTypeArray[0].IdField = null;
            myVoiceMailBoxTypeArray[0].InitialActionListField = null;
            myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
            myVoiceMailBoxTypeArray[0].LanguageField = "english";
            myVoiceMailBoxTypeArray[0].LastAccessField = null;
            myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
            myVoiceMailBoxTypeArray[0].LoginField = null;
            myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
            myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
            myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
            myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
            myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
            myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
            myVoiceMailBoxTypeArray[0].MessageCountField = null;
            myVoiceMailBoxTypeArray[0].MessageListField = null;
            myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
            myVoiceMailBoxTypeArray[0].NameField = null;
            myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].NotificationListField = null;
            myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
            myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
            myVoiceMailBoxTypeArray[0].PagerListField = null;
            myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
            myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
            myVoiceMailBoxTypeArray[0].ParentListField = null;
            myVoiceMailBoxTypeArray[0].PasswordField = null;
            myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
            myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
            myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
            myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
            myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
            myVoiceMailBoxTypeArray[0].ScheduleListField = null;
            myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
            myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
            myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;
            myVoiceMailBoxTypeArray[0].OptionsPackageField = null;

            //Per Innovatives request set the guid to null.
            var mySubInfoTypeList = new List<SubInfoType>();
            foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
            {
                if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                    mySubInfoType.SubscriberGuidField = null;

                mySubInfoTypeList.Add(mySubInfoType);
            }

            myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
        }

        public void CreateVoicemail(string phoneNumber, string mailBoxDescription, string vmPackageName, string subscriberName, string subscriberBillingAccountNumber, MailboxType mailboxType, string notificationCenterTypeDescription)
        {
            if (phoneNumber.Length < 10)
            {
                throw new Exception("Phone number must be 10 digits" + Environment.NewLine);
            }

            string sPackageGuid = "";
            int iNotificationCenterID = -1;

            //Gets a list of all available vm packages
            PackageType[] myPackageTypeArray = _voiceMailService.GetAllPackages(_loginInformation.LoginToken);

            //Here we are searching through all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (PackageType myPackageType in myPackageTypeArray)
            {
                if (myPackageType.DescriptionField.Trim().Equals(vmPackageName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    sPackageGuid = myPackageType.GuidField;
                    break;
                }
            }

            if (sPackageGuid.Length == 0)
            {
                throw new Exception("Could not find this packagte in the ApMax system. Please check that the correct package was sent to this method." + Environment.NewLine);
            }

            //Gets all the notification centers
            NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);

            //Here we are searching through all the NotificationCenters and matching up the description with what we want.
            //Because we need to send the centerid.
            foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
            {
                if (myNotificationCenterInfoType.DescriptionField.Trim().Equals(notificationCenterTypeDescription.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
                    break;
                }
            }

            //iNotificationCenterID was intialized to -1 so if still below 0 we did not find a Notification Center.
            if (iNotificationCenterID < 0)
            {
                throw new Exception("Unable to determine notification center.");
            }

            var subscriberType = new SubscriberType
            {
                SubscriberName = subscriberName,
                SubscriberTimezone = Timezone_e.ApDefault,
                SubscriberDefaultPhoneNumber = phoneNumber,
                PlacementType = PlacementType_e.PlacementType_None,
                BillingAccountNumber = subscriberBillingAccountNumber
            };

            _voiceMailService.AddNewVoiceMailBox(_loginInformation.LoginToken, phoneNumber, mailBoxDescription, sPackageGuid, mailboxType, iNotificationCenterID, phoneNumber, subscriberType);
        }

        public void CreateVoiceMailSubMailbox(string phoneNumber, string digitField)
        {
            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox does not exist.");
            }

            int iCurrentNumberOfSubBoxes = 0;
            ChildInfoType[] myChildInfoTypeArray = myVoiceMailBoxTypeArray[0].ChildListField;
            foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
            {
                //Only look for submailbox types.  Ignore outdial types etc...  
                //also when you delete a child it does not get rid of this it only clears the NameField...
                //ont ask me why... This is the most redicules API I have had to deal with.
                if (myChildInfoType.TypeField.Equals(AddressType.AddressTypeMailboxNumber) &&
                    myChildInfoType.NameField.Length > 0)
                    iCurrentNumberOfSubBoxes++;
            }

            string sParentID = myVoiceMailBoxTypeArray[0].IdField;

            _voiceMailService.AddNewChildMailBox(_loginInformation.LoginToken, sParentID, new ChildInfoType
            {
                //We have to get the next available digit field. The array is pre-populated and sorted with available fields.
                DigitField = digitField,
                NameField = phoneNumber + (iCurrentNumberOfSubBoxes + 1),
                DescriptionField = "(Child " + (iCurrentNumberOfSubBoxes + 1) + ")",
                TypeField = AddressType.AddressTypeMailboxNumber
            }, MailboxType.FAMILY_CHILD);
        }

        public void UpdateVoiceMailBox(string phoneNumber, int maxMailBoxTime, int maxMessages)
        {
            //var returnError = new ReturnError {DidError = false, CustomErrorMessage = string.Empty, Exception = null};

            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                //returnError.DidError = true;
                //returnError.CustomErrorMessage = "Mailbox does not exist." + Environment.NewLine;
                //OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
                //return returnError;

                throw new Exception("Mailbox does not exist.");
            }

            //Per Hood wants max MaxSubmailboxesField set to zero by default.
            myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
            myVoiceMailBoxTypeArray[0].CallingPartyField = null;
            myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
            myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
            myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
            myVoiceMailBoxTypeArray[0].GreetingListField = null;
            myVoiceMailBoxTypeArray[0].HolidayListField = null;
            myVoiceMailBoxTypeArray[0].InitialActionListField = null;
            myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
            myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
            myVoiceMailBoxTypeArray[0].ScheduleListField = null;
            myVoiceMailBoxTypeArray[0].MaxMessagesField = maxMessages;
            myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = maxMailBoxTime;

            //Per Innovatives request set the guid to null.
            var mySubInfoTypeList = new List<SubInfoType>();
            foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
            {
                if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                    mySubInfoType.SubscriberGuidField = null;

                mySubInfoTypeList.Add(mySubInfoType);
            }

            myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);

            //return returnError;
        }

        public void UpdateVoiceMailBoxFull(string phoneNumber, VoiceMailBoxType voiceMailBoxType, InternetAccessType internetAccessType)
        {
            if (voiceMailBoxType == null)
            {
                throw new Exception("Didn't recieve a valid VoiceMailBoxType object.");
            }

            //Note: not sure the id field is the right value.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                //returnError.DidError = true;
                //returnError.CustomErrorMessage = "Mailbox does not exist." + Environment.NewLine;
                //OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
                throw new Exception("Mailbox does not exist.");
            }

            //Per Innovatives request set the guid to null.
            var mySubInfoTypeList = new List<SubInfoType>();
            foreach (var mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
            {
                if (mySubInfoType.SubscriberDefaultPhoneNumberField == "")
                {
                    mySubInfoType.SubscriberGuidField = null;
                }

                mySubInfoTypeList.Add(mySubInfoType);
            }

            voiceMailBoxType.IdField = myVoiceMailBoxTypeArray[0].IdField;
            voiceMailBoxType.SubscriberListField = mySubInfoTypeList.ToArray();

            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, voiceMailBoxType, internetAccessType);
        }

        public void UpdateVoiceSubMailbox(string phoneNumber, int numberOfSubMailboxes, int maxNumberOfSubMailboxesAllowed)
        {
            if (phoneNumber.Length != 0)
            {
                if (numberOfSubMailboxes > maxNumberOfSubMailboxesAllowed)
                {
                    throw new Exception("Cannot create more mailboxes then the max allowed.");
                }

                //This will get the current mailbox info.
                VoiceMailBoxType[] myVoiceMailBoxTypeArray =
                    _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken,
                        phoneNumber);
                //If this is not null mailbox is present.
                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
                {
                    throw new Exception("Mailbox does not exist.");
                }

                //Set some important values we will use later.
                //Need to know current submailboxes.  excludes outdial types etc...
                int iCurrentNumberOfSubBoxes = 0;
                ChildInfoType[] myChildInfoTypeArray = myVoiceMailBoxTypeArray[0].ChildListField;
                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
                {
                    //Only look for submailbox types.  Ignore outdial types etc...  
                    //also when you delete a child it does not get rid of this it only clears the NameField...
                    //Dont ask me why... This is the most redicules API I have had to deal with.
                    if (myChildInfoType.TypeField.Equals(AddressType.AddressTypeMailboxNumber) &&
                        myChildInfoType.NameField.Length > 0)
                        iCurrentNumberOfSubBoxes++;
                }

                string sParentID = myVoiceMailBoxTypeArray[0].IdField;
                //int iNumberOfChildBoxes = myVoiceMailBoxTypeArray[0].ChildListField.Length;

                if (numberOfSubMailboxes == iCurrentNumberOfSubBoxes)
                {
                    //sError = "No mailboxes to update. Account has " + numberOfSubMailboxes + Environment.NewLine;
                    //OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
                    //return false;
                    throw new Exception("No mailboxes to update. Account has " + numberOfSubMailboxes);
                }

                //Remove Mailboxes
                if (numberOfSubMailboxes < iCurrentNumberOfSubBoxes)
                {
                    //Takes the total mailboxes and minus current to find out how many to delete
                    int iNumberOfSubMailboxesToDelete = iCurrentNumberOfSubBoxes - numberOfSubMailboxes;
                    for (int count = 1; count <= iNumberOfSubMailboxesToDelete; count++)
                    {
                        ChildInfoType myChildInfoType = GetChildInfoByHighestDigitField(myChildInfoTypeArray);
                        List<ChildInfoType> myChildInfoTypeList = new List<ChildInfoType>();

                        //Must remove the child we found from this array so it is not selected again.
                        foreach (ChildInfoType myChildInfo in myChildInfoTypeArray)
                        {
                            //Adds all the childs that are not equal to the digitfield we are looking for.
                            if (!myChildInfoType.DigitField.Equals(myChildInfo.DigitField))
                                myChildInfoTypeList.Add(myChildInfo);
                        }
                        myChildInfoTypeArray = myChildInfoTypeList.ToArray();

                        if (myChildInfoType != null)
                            _voiceMailService.DeleteChildMailBox(_loginInformation.LoginToken, sParentID,
                                myChildInfoType);

                        iCurrentNumberOfSubBoxes--;
                    }
                }
                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
                myVoiceMailBoxTypeArray[0].GreetingListField = null;
                myVoiceMailBoxTypeArray[0].HolidayListField = null;
                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
                myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
                myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
                //Important step:  Must set children to null since we deleted we dont want to re-add them.
                myVoiceMailBoxTypeArray[0].ChildListField = null;
                myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = maxNumberOfSubMailboxesAllowed;

                string sDN = "";

                //Per Innovatives request set the guid to null.
                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
                {
                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                    {
                        mySubInfoType.SubscriberGuidField = null;
                        sDN = mySubInfoType.SubscriberDefaultPhoneNumberField;
                    }

                    mySubInfoTypeList.Add(mySubInfoType);
                }

                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

                _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);


                //iNumberOfChildBoxes = myVoiceMailBoxTypeArray[0].ChildListField.Length;
                iCurrentNumberOfSubBoxes = 0;
                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
                {
                    //Only look for submailbox types.  Ignore outdial types etc...
                    if (myChildInfoType.TypeField.Equals(AddressType.AddressTypeMailboxNumber))
                        iCurrentNumberOfSubBoxes++;
                }

                //Add Mailboxes
                if (numberOfSubMailboxes > iCurrentNumberOfSubBoxes)
                {
                    //Always add one right away so we goto next available mailbox.
                    iCurrentNumberOfSubBoxes++;

                    //GetAvailableDigitFieldsFromChildList method populate a integer array of avaible digitfields in a sorted order of lowest number first.
                    List<int> AvaiableDigitFiledNumbers = GetAvailableDigitFieldsFromChildList(
                        myChildInfoTypeArray, maxNumberOfSubMailboxesAllowed);

                    //Takes the total mailboxes and minus current to find out how many to add.
                    for (int count = iCurrentNumberOfSubBoxes; count <= numberOfSubMailboxes; count++)
                    {
                        //Example Description     <DescriptionField>(Child 1)</DescriptionField>
                        _voiceMailService.AddNewChildMailBox(_loginInformation.LoginToken, sParentID,
                            new ChildInfoType
                            {
                                //We have to get the next available digit field. The array is pre-populated and sorted with available fields.
                                DigitField = AvaiableDigitFiledNumbers[0].ToString(),
                                NameField = sDN + count,
                                DescriptionField = "(Child " + count + ")",
                                TypeField = AddressType.AddressTypeMailboxNumber
                            }, MailboxType.FAMILY_CHILD);

                        //Since we use this one out of the list we must remove it.
                        AvaiableDigitFiledNumbers.RemoveAt(0);
                    }
                }
            }
        }

        public void UpdateVoiceMailBoxPackage(string phoneNumber, string vmPackageName)
        {
            string packageGuid = "";

            //Gets a list of all available vm packages
            PackageType[] myPackageTypeArray = _voiceMailService.GetAllPackages(_loginInformation.LoginToken);

            //Here we are searching through all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (PackageType myPackageType in myPackageTypeArray)
            {
                if (myPackageType.DescriptionField.Equals(vmPackageName, StringComparison.OrdinalIgnoreCase))
                {
                    packageGuid = myPackageType.GuidField;
                    break;
                }
            }

            if (packageGuid.Length == 0)
            {
                throw new Exception("Could not find this package in the ApMax system.");
            }

            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox does not exist.");
            }

            myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
            myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
            myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
            myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
            myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
            myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
            myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
            myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
            myVoiceMailBoxTypeArray[0].CallingPartyField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningField = null;
            myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
            myVoiceMailBoxTypeArray[0].ChildListField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
            myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
            myVoiceMailBoxTypeArray[0].DescriptionField = null;
            myVoiceMailBoxTypeArray[0].DirectoryField = null;
            myVoiceMailBoxTypeArray[0].DistributionListField = null;
            myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
            myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
            myVoiceMailBoxTypeArray[0].ExtensionData = null;
            myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
            myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
            myVoiceMailBoxTypeArray[0].ForwardingListField = null;
            myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
            myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
            myVoiceMailBoxTypeArray[0].GreetingListField = null;
            myVoiceMailBoxTypeArray[0].HolidayListField = null;
            //myVoiceMailBoxTypeArray[0].IdField = null;
            myVoiceMailBoxTypeArray[0].InitialActionListField = null;
            myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
            myVoiceMailBoxTypeArray[0].LanguageField = null;
            myVoiceMailBoxTypeArray[0].LastAccessField = null;
            myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
            myVoiceMailBoxTypeArray[0].LoginField = null;
            myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
            myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
            myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
            myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
            myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
            myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
            myVoiceMailBoxTypeArray[0].MessageCountField = null;
            myVoiceMailBoxTypeArray[0].MessageListField = null;
            myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
            myVoiceMailBoxTypeArray[0].NameField = null;
            myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].NotificationListField = null;
            myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
            myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
            myVoiceMailBoxTypeArray[0].PagerListField = null;
            myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
            myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
            myVoiceMailBoxTypeArray[0].ParentListField = null;
            myVoiceMailBoxTypeArray[0].PasswordField = null;
            myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
            myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
            myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
            myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
            myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
            myVoiceMailBoxTypeArray[0].ScheduleListField = null;
            myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
            myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
            myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

            myVoiceMailBoxTypeArray[0].OptionsPackageField = packageGuid;

            //Per Innovatives request set the guid to null.
            List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
            foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
            {
                if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                {
                    mySubInfoType.SubscriberGuidField = null;
                }

                mySubInfoTypeList.Add(mySubInfoType);
            }

            myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
        }

        public void UpdateVoiceMailBoxType(string phoneNumber, MailboxType mailBoxType, string internetPassword, string internetUserName, bool internetAccess)
        {
            //Retrieve Subscriber info. Need billing account number.
            var subscriberBillingAccountNumber = new SubscriberV4Service(_setting, _versions).RetrieveSubscriberByDefaultPhoneNumber(phoneNumber).BillingAccountNumber;

            //This will get the current mailbox info before we delete the box.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present so we will delete.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox " + phoneNumber + " does not exist.");
            }

            DeleteVoiceMailBox(phoneNumber, false);

            string sEmailAddress = "";
            string sNotificationCenter = "";
            foreach (var type in myVoiceMailBoxTypeArray[0].NotificationListField)
            {
                if (type.AddressField.Equals(phoneNumber))
                {
                    //Gets all the notification centers
                    NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);

                    //Here we are searching through all the NotificationCenters and matching up the description with what we want.
                    //Because we need to send the centerid.
                    foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
                    {
                        if (myNotificationCenterInfoType.CenterIdField == type.CenterField)
                        {
                            sNotificationCenter = myNotificationCenterInfoType.DescriptionField;
                            break;
                        }
                    }
                }

                if (type.CenterField.Equals(3))
                    sEmailAddress = type.AddressField;
            }

            string sVmPackageName = "";
            //Gets a list of all available vm packages
            PackageType[] myPackageTypeArray = _voiceMailService.GetAllPackages(_loginInformation.LoginToken);

            //Here we are searching through all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (PackageType myPackageType in myPackageTypeArray)
            {
                if (myPackageType.GuidField.Equals(myVoiceMailBoxTypeArray[0].OptionsPackageField, StringComparison.OrdinalIgnoreCase))
                {
                    sVmPackageName = myPackageType.DescriptionField;
                    break;
                }
            }

            CreateVoicemail(phoneNumber, myVoiceMailBoxTypeArray[0].DescriptionField, sVmPackageName,
                myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField, subscriberBillingAccountNumber, mailBoxType, sNotificationCenter);

            if (myVoiceMailBoxTypeArray[0].ChildListField.Length > 0)
            {
                UpdateVoiceSubMailbox(phoneNumber, myVoiceMailBoxTypeArray[0].ChildListField.Length, (int)myVoiceMailBoxTypeArray[0].MaxSubmailboxesField);
            }

            if (internetAccess)
            {
                AddVoiceMailBoxInternetAccess(phoneNumber, sEmailAddress, internetPassword, internetUserName);
            }
        }

        public void DeleteVoiceMailBox(string phoneNumber, bool deleteSubscriber)
        {
            //Check if want to delete VMbox
            if (phoneNumber.Length != 0)
            {
                //This will get the current mailbox info before we delete the box.
                VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

                //If this is not null mailbox is present so we will delete.
                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
                {
                    //Note:Is it really an error if the box doesn't exist?  The end result is the desired result...
                    //sError = "Mailbox does not exist." + Environment.NewLine;
                    //throw new Exception("Mailbox does not exist.");
                }
                else
                {
                    _voiceMailService.DeleteVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField);

                    if (deleteSubscriber)
                    {
                        var subscriberVersion = _versions.Subscriber;

                        switch (subscriberVersion)
                        {
                            case 3:
                                var apMaxSubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);
                                apMaxSubscriberServiceV3.DeleteSubscriberByDefaultPhoneNumber(phoneNumber);
                                break;
                            case 4:
                                var apMaxSubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);
                                apMaxSubscriberServiceV4.DeleteSubscriberByDefaultPhoneNumber(phoneNumber);
                                break;
                            default:
                                throw new NotImplementedException("Subscriber Service version " + subscriberVersion + " is not implemented.");
                        }
                    }

                }
            }
        }

        public void DeleteVoiceMailBoxInternetAccess(string phoneNumber)
        {
            if (phoneNumber.Length != 0)
            {
                //This will get the current mailbox info.
                VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

                //If this is not null mailbox is present.
                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
                {
                    throw new Exception("Mailbox does not exist.");
                }

                int iNotificationCenterID = -1;

                //Gets all the notification centers
                NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);

                //Here we are searching through all the NotificationCenters and matching up the description with what we want.
                //Because we need to send the centerid.
                foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
                {
                    if (myNotificationCenterInfoType.DescriptionField.Equals("email", StringComparison.OrdinalIgnoreCase))
                    {
                        iNotificationCenterID = myNotificationCenterInfoType.CenterIdField;
                        break;
                    }
                }

                if (iNotificationCenterID < 0)
                {
                    throw new Exception("Unable to determine notification center from \"email\".");
                }

                List<NotificationInfoType> myNotificationInfoTypeList = new List<NotificationInfoType>();
                foreach (NotificationInfoType myNotificationInfoType in myVoiceMailBoxTypeArray[0].NotificationListField)
                {
                    if (myNotificationInfoType.CenterField != iNotificationCenterID)
                        myNotificationInfoTypeList.Add(myNotificationInfoType);
                }

                //Darrin from hood wants to allow email deletion and add the email notification as well the below 2 lines does this.
                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = false;
                myVoiceMailBoxTypeArray[0].NotificationListField = myNotificationInfoTypeList.ToArray();

                //Setting these to null will keep them from getting updated.
                myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
                myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
                myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
                myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
                myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
                myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
                myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
                myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
                myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
                myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
                myVoiceMailBoxTypeArray[0].CallScreeningField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
                myVoiceMailBoxTypeArray[0].ChildListField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
                myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
                myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
                myVoiceMailBoxTypeArray[0].DescriptionField = null;
                myVoiceMailBoxTypeArray[0].DirectoryField = null;
                myVoiceMailBoxTypeArray[0].DistributionListField = null;
                myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
                myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
                myVoiceMailBoxTypeArray[0].ExtensionData = null;
                myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
                myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
                myVoiceMailBoxTypeArray[0].ForwardingListField = null;
                myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].GreetingListField = null;
                myVoiceMailBoxTypeArray[0].HolidayListField = null;
                //myVoiceMailBoxTypeArray[0].IdField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
                //this one is important
                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
                myVoiceMailBoxTypeArray[0].LanguageField = null;
                myVoiceMailBoxTypeArray[0].LastAccessField = null;
                myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
                myVoiceMailBoxTypeArray[0].LoginField = null;
                myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
                myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
                myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
                myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
                myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
                myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
                myVoiceMailBoxTypeArray[0].MessageCountField = null;
                myVoiceMailBoxTypeArray[0].MessageListField = null;
                myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
                myVoiceMailBoxTypeArray[0].NameField = null;
                myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
                myVoiceMailBoxTypeArray[0].NotificationListField = null;
                myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
                myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
                myVoiceMailBoxTypeArray[0].PagerListField = null;
                myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
                myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
                myVoiceMailBoxTypeArray[0].ParentListField = null;
                myVoiceMailBoxTypeArray[0].PasswordField = null;
                myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
                myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
                myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
                myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
                myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
                myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
                myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;

                string sSubscriberGuid = "";
                string sSubscriberName = "";
                SubAddressInfoType[] SubscriberAddressArray;

                //Per Innovatives request set the guid to null.
                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
                {
                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                    {
                        SubscriberAddressArray = mySubInfoType.SubscriberAddressListField;
                        sSubscriberGuid = mySubInfoType.SubscriberGuidField;
                        sSubscriberName = mySubInfoType.SubscriberNameField;
                        mySubInfoType.SubscriberGuidField = null;
                    }

                    mySubInfoTypeList.Add(mySubInfoType);
                }

                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();
                myVoiceMailBoxTypeArray[0].TimezoneField = null;
                myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

                _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], new InternetAccessType { ServiceEnabled = false });

                var subscriberVersion = _versions.Subscriber;

                switch (subscriberVersion)
                {
                    case 3:
                        {
                            var mySubscriberServiceV3 = new SubscriberV3Service(_setting, _versions);

                            mySubscriberServiceV3.AddOrUpdateSubscriberProv(new SubscriberV3.SubscriberType
                            {
                                SubscriberName = sSubscriberName,
                                SubscriberDefaultPhoneNumber = phoneNumber,
                                PlacementType = SubscriberV3.PlacementType_e.PlacementType_None,
                                SubscriberGuid = sSubscriberGuid,
                                SubscriberEmail = "",
                                InternetAccess = new SubscriberV3.InternetAccessType { ServiceEnabled = false }
                            });
                            break;
                        }
                    case 4:
                        {
                            var mySubscriberServiceV4 = new SubscriberV4Service(_setting, _versions);

                            mySubscriberServiceV4.AddOrUpdateSubscriberProv(new SubscriberV4.SubscriberType
                            {
                                SubscriberName = sSubscriberName,
                                SubscriberDefaultPhoneNumber = phoneNumber,
                                PlacementType = SubscriberV4.PlacementType_e.PlacementType_None,
                                SubscriberGuid = sSubscriberGuid,
                                SubscriberEmail = "",
                                InternetAccess = new SubscriberV4.InternetAccessType { ServiceEnabled = false }
                            });
                            break;
                        }
                    default:
                        throw new NotImplementedException("Subscriber Service version " + subscriberVersion + " is not implemented.");
                }

            }
        }

        public void ReassignVmBoxNumber(string oldPhoneNumber, string newPhoneNumber, string mailboxDescription, string subscriberName, string internetPassword, string internetUserName,
            bool internetAccess, bool deleteSubscriber)
        {

            //Retrieve Subscriber info. Need billing account number.
            var subscriberBillingAccountNumber = new SubscriberV4Service(_setting, _versions).RetrieveSubscriberByDefaultPhoneNumber(oldPhoneNumber).BillingAccountNumber;

            //This will get the current mailbox info before we delete the box.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, oldPhoneNumber);

            //If this is not null mailbox is present so we will delete.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Old Mailbox " + oldPhoneNumber + " does not exist.");
            }

            DeleteVoiceMailBox(oldPhoneNumber, deleteSubscriber);

            string sEmailAddress = "";
            string sNotificationCenter = "";
            foreach (var type in myVoiceMailBoxTypeArray[0].NotificationListField)
            {
                if (type.AddressField.Equals(oldPhoneNumber))
                {
                    //Gets all the notification centers
                    NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);

                    //Here we are searching through all the NotificationCenters and matching up the description with what we want.
                    //Because we need to send the centerid.
                    foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
                    {
                        if (myNotificationCenterInfoType.CenterIdField == type.CenterField)
                        {
                            sNotificationCenter = myNotificationCenterInfoType.DescriptionField;
                            break;
                        }
                    }
                }

                if (type.CenterField.Equals(3))
                    sEmailAddress = type.AddressField;
            }

            string sVmPackageName = "";
            //Gets a list of all available vm packages
            PackageType[] myPackageTypeArray = _voiceMailService.GetAllPackages(_loginInformation.LoginToken);

            //Here we are searching through all the packages and matching up the description with what we want.
            //Because we need to send the guid of the description.
            foreach (PackageType myPackageType in myPackageTypeArray)
            {
                if (myPackageType.GuidField.Equals(myVoiceMailBoxTypeArray[0].OptionsPackageField, StringComparison.OrdinalIgnoreCase))
                {
                    sVmPackageName = myPackageType.DescriptionField;
                    break;
                }
            }

            CreateVoicemail(newPhoneNumber, mailboxDescription, sVmPackageName, myVoiceMailBoxTypeArray[0].SubscriberListField[0].SubscriberNameField, subscriberBillingAccountNumber, 
                myVoiceMailBoxTypeArray[0].MailboxTypeField.HasValue ? myVoiceMailBoxTypeArray[0].MailboxTypeField.Value : new MailboxType(), sNotificationCenter);

            if (myVoiceMailBoxTypeArray[0].ChildListField.Length > 0)
            {
                UpdateVoiceSubMailbox(newPhoneNumber, myVoiceMailBoxTypeArray[0].ChildListField.Length, (int)myVoiceMailBoxTypeArray[0].MaxSubmailboxesField);
            }

            if (internetAccess)
            {
                AddVoiceMailBoxInternetAccess(newPhoneNumber, sEmailAddress, internetPassword, internetUserName);
            }

        }

        public void AddOutDialNumber(string phoneNumber, string outDialPhoneNumber, string outDialRoutingNumber)
        {
            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox does not exist.");
            }

            _voiceMailService.AddNewChildMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField, new ChildInfoType
            {
                AddressField = outDialPhoneNumber,
                DigitField = outDialRoutingNumber,
                TypeField = AddressType.AddressTypeDN
            }, MailboxType.OUTDIAL);
        }

        public void DeleteOutDialNumber(string phoneNumber, string outDialPhoneNumber)
        {
            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                throw new Exception("Mailbox does not exist.");
            }

            //Loop through all the child mailboxs and to find the box we want to delete.
            foreach (ChildInfoType myChild in myVoiceMailBoxTypeArray[0].ChildListField)
            {
                if (myChild.AddressField == outDialPhoneNumber)
                {
                    _voiceMailService.DeleteChildMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0].IdField, myChild);
                    break;
                }
            }
        }

        public void AddVoiceMailBoxInternetAccess(string phoneNumber, string emailAddress, string internetPassword, string internetUserName)
        {
            if (phoneNumber.Length != 0)
            {
                //This will get the current mailbox info.
                VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

                //If this is not null mailbox is present.
                if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
                {
                    throw new Exception("Mailbox does not exist.");
                }

                //Setting these to null will keep them from getting updated.
                myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
                myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
                myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
                myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
                myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
                myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
                myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
                myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
                myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
                myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
                myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
                myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
                myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
                myVoiceMailBoxTypeArray[0].CallingPartyField = null;
                myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
                myVoiceMailBoxTypeArray[0].CallScreeningField = null;
                myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
                myVoiceMailBoxTypeArray[0].ChildListField = null;
                myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
                myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
                myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
                myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
                myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
                myVoiceMailBoxTypeArray[0].DescriptionField = null;
                myVoiceMailBoxTypeArray[0].DirectoryField = null;
                myVoiceMailBoxTypeArray[0].DistributionListField = null;
                myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
                myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
                myVoiceMailBoxTypeArray[0].ExtensionData = null;
                myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
                myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
                myVoiceMailBoxTypeArray[0].ForwardingListField = null;
                myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
                myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
                myVoiceMailBoxTypeArray[0].GreetingListField = null;
                myVoiceMailBoxTypeArray[0].HolidayListField = null;
                //Must have ID field.
                //myVoiceMailBoxTypeArray[0].IdField = null;
                myVoiceMailBoxTypeArray[0].InitialActionListField = null;
                myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
                myVoiceMailBoxTypeArray[0].LanguageField = null;
                myVoiceMailBoxTypeArray[0].LastAccessField = null;
                myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
                myVoiceMailBoxTypeArray[0].LoginField = null;
                myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
                myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
                myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
                myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
                myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
                myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
                myVoiceMailBoxTypeArray[0].MessageCountField = null;
                myVoiceMailBoxTypeArray[0].MessageListField = null;
                myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
                myVoiceMailBoxTypeArray[0].NameField = null;
                myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
                //myVoiceMailBoxTypeArray[0].NotificationListField = null;
                myVoiceMailBoxTypeArray[0].OptionsPackageField = null;
                myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
                myVoiceMailBoxTypeArray[0].PagerListField = null;
                myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
                myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
                myVoiceMailBoxTypeArray[0].ParentListField = null;
                myVoiceMailBoxTypeArray[0].PasswordField = null;
                myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
                myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
                myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
                myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
                myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
                myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
                myVoiceMailBoxTypeArray[0].ScheduleListField = null;
                myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
                myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
                //myVoiceMailBoxTypeArray[0].SubscriberListField = null;
                //myVoiceMailBoxTypeArray[0].TimezoneField = null;
                //myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

                int notificationCenterId = -1;

                //Gets all the notification centers
                NotificationCenterInfoType[] myNotificationCenterInfoTypeArray = _voiceMailService.GetAllNotificationCenters(_loginInformation.LoginToken);

                //Here we are searching through all the NotificationCenters and matching up the description with what we want.
                //Because we need to send the centerid.
                foreach (NotificationCenterInfoType myNotificationCenterInfoType in myNotificationCenterInfoTypeArray)
                {
                    if (myNotificationCenterInfoType.TypeField == NotificationCenterTypeEnum.typeEmail)
                    {
                        notificationCenterId = myNotificationCenterInfoType.CenterIdField;
                        break;
                    }
                }

                if (notificationCenterId < 0)
                {
                    throw new Exception("Unable to determine notification center from TypeField \"NotificationCenterType.typeemail\".");
                }

                List<NotificationInfoType> myNotificationInfoTypeList = new List<NotificationInfoType>();
                foreach (NotificationInfoType myNotificationInfoType in myVoiceMailBoxTypeArray[0].NotificationListField)
                {
                    myNotificationInfoTypeList.Add(myNotificationInfoType);
                }

                NotificationInfoType myNotificationInfoType2 = new NotificationInfoType
                {
                    AddressField = emailAddress,
                    CenterField = notificationCenterId,
                    EnabledField = true
                };
                myNotificationInfoTypeList.Add(myNotificationInfoType2);

                //Darrin from hood wants to allow email deletion and add the email notification as well the below 2 lines does this.
                myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = true;
                myVoiceMailBoxTypeArray[0].NotificationListField = myNotificationInfoTypeList.ToArray();

                string sSubscriberGuid = "";
                string sSubscriberName = "";

                //Per Innovatives request set the guid to null.
                List<SubInfoType> mySubInfoTypeList = new List<SubInfoType>();
                foreach (SubInfoType mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
                {
                    if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                    {
                        sSubscriberName = mySubInfoType.SubscriberNameField;
                        sSubscriberGuid = mySubInfoType.SubscriberGuidField;
                        mySubInfoType.SubscriberGuidField = null;
                    }

                    mySubInfoTypeList.Add(mySubInfoType);
                }

                myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();
                //myVoiceMailBoxTypeArray[0].TimezoneField = null;
                //myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

                switch (_versions.Subscriber)
                {
                    case 3:
                        var apMaxSubscriberService = new SubscriberV3Service(_setting, _versions);
                        SubscriberV3.SubscriberType mySubscriberType = apMaxSubscriberService.RetrieveSubscriberByDefaultPhoneNumber(phoneNumber);

                        if (mySubscriberType == null)
                        {
                            throw new Exception("A subscriber with the default phone number does not exist.");
                        }

                        //Get current InternetAccess Username information.
                        SubscriberV3.SubscriberInternetAccessType mySubscriberInternetAccessType = apMaxSubscriberService.RetrieveSubscriberInternetAccess(Guid.Parse(mySubscriberType.SubscriberGuid));

                        if (mySubscriberInternetAccessType == null)
                        {
                            //If the username did not exist we need to add email etc...
                            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken,
                                myVoiceMailBoxTypeArray[0],
                                new InternetAccessType
                                {
                                    Password = internetPassword,
                                    UserName = internetUserName,
                                    ServiceEnabled = true
                                });

                            apMaxSubscriberService.AddOrUpdateSubscriberProv(new Common.SubscriberV3.SubscriberType
                            {
                                SubscriberName = sSubscriberName,
                                SubscriberDefaultPhoneNumber = phoneNumber,
                                PlacementType = Common.SubscriberV3.PlacementType_e.PlacementType_None,
                                SubscriberGuid = sSubscriberGuid,
                                SubscriberEmail = emailAddress
                            });
                        }
                        else
                        {
                            //else if username are equal modify status
                            if (internetUserName == mySubscriberInternetAccessType.UserName)
                            {
                                //This re-enables the internet access.
                                _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], new InternetAccessType { ServiceEnabled = true });

                                //This re-enables the internet access.
                                apMaxSubscriberService.AddOrUpdateSubscriberProv(new Common.SubscriberV3.SubscriberType
                                {
                                    SubscriberName = sSubscriberName,
                                    SubscriberDefaultPhoneNumber = phoneNumber,
                                    PlacementType = Common.SubscriberV3.PlacementType_e.PlacementType_None,
                                    SubscriberGuid = sSubscriberGuid,
                                    SubscriberEmail = emailAddress,
                                    InternetAccess = new Common.SubscriberV3.InternetAccessType
                                    {
                                        ServiceEnabled = true
                                    }
                                });
                            }
                            else
                            {
                                //DeleteVoiceMailBoxInternetAccess();
                                //AddVoiceMailBoxInternetAccess();
                                //This means they have internet access but the username is not what we expect.
                                //OasisUtilitiesHelper.WriteToErrorFile("A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine, "ApMax");
                                //sError = "A subscriber with internet access does exist however the username does not match desired username." + Environment.NewLine;
                                //return false;
                                throw new Exception(
                                    "A subscriber with internet access does exist however the username does not match desired username.");
                            }
                        }
                        break;
                }
            }
        }

        public void VmPasswordReset(string phoneNumber, string newPin)
        {
            //This will get the current mailbox info.
            VoiceMailBoxType[] myVoiceMailBoxTypeArray = _voiceMailService.GetVoiceMailBoxesByVoiceMailBoxNumber(_loginInformation.LoginToken, phoneNumber);

            //If this is not null mailbox is present.
            if (myVoiceMailBoxTypeArray == null || myVoiceMailBoxTypeArray.Length == 0)
            {
                //sError = "Mailbox does not exist." + Environment.NewLine;
                //OasisUtilitiesHelper.WriteToErrorFile(sError, "ApMax");
                //return false;
                throw new Exception("Mailbox does not exist.");
            }

            // change what we need to change here to myVMBoxInfo.
            myVoiceMailBoxTypeArray[0].PasswordField = newPin;

            myVoiceMailBoxTypeArray[0].ActiveGreetingField = null;
            myVoiceMailBoxTypeArray[0].AdminEntryPointField = null;
            myVoiceMailBoxTypeArray[0].AllowBroadcastMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowDailyScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowEmailDeletionField = null;
            myVoiceMailBoxTypeArray[0].AllowForwardingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowFutureDeliveryField = null;
            myVoiceMailBoxTypeArray[0].AllowHolidayScheduleField = null;
            myVoiceMailBoxTypeArray[0].AllowInitialActionsField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialField = null;
            myVoiceMailBoxTypeArray[0].AllowOutdialListEditField = null;
            myVoiceMailBoxTypeArray[0].AllowPagerNotificationField = null;
            myVoiceMailBoxTypeArray[0].AllowRecordingField = null;
            myVoiceMailBoxTypeArray[0].AllowSendingMessagesField = null;
            myVoiceMailBoxTypeArray[0].AllowSpecificCallerField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardAddressField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardMessagesField = null;
            myVoiceMailBoxTypeArray[0].AutoForwardTypeField = null;
            myVoiceMailBoxTypeArray[0].CallingPartyField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningDelayField = null;
            myVoiceMailBoxTypeArray[0].CallScreeningField = null;
            myVoiceMailBoxTypeArray[0].ChargeNumberField = null;
            myVoiceMailBoxTypeArray[0].ChildListField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationStatusField = null;
            myVoiceMailBoxTypeArray[0].DailyNotificationTimeField = null;
            myVoiceMailBoxTypeArray[0].DefaultInitialActionField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortKnownField = null;
            myVoiceMailBoxTypeArray[0].DeleteShortUnknownField = null;
            myVoiceMailBoxTypeArray[0].DescriptionField = null;
            myVoiceMailBoxTypeArray[0].DirectoryField = null;
            myVoiceMailBoxTypeArray[0].DistributionListField = null;
            myVoiceMailBoxTypeArray[0].EmailcodecTypeField = null;
            myVoiceMailBoxTypeArray[0].EmailOnlyField = null;
            myVoiceMailBoxTypeArray[0].ExtensionData = null;
            myVoiceMailBoxTypeArray[0].ForceLanguageChoiceField = null;
            myVoiceMailBoxTypeArray[0].ForcePasswordChangeField = null;
            myVoiceMailBoxTypeArray[0].ForwardingListField = null;
            myVoiceMailBoxTypeArray[0].GreetingFilenameField = null;
            myVoiceMailBoxTypeArray[0].GreetingInterruptField = null;
            myVoiceMailBoxTypeArray[0].GreetingListField = null;
            myVoiceMailBoxTypeArray[0].HolidayListField = null;
            myVoiceMailBoxTypeArray[0].InitialActionListField = null;
            myVoiceMailBoxTypeArray[0].InterPageDelayField = null;
            myVoiceMailBoxTypeArray[0].LanguageField = null;
            myVoiceMailBoxTypeArray[0].LastAccessField = null;
            myVoiceMailBoxTypeArray[0].LoggingLevelField = null;
            myVoiceMailBoxTypeArray[0].LoginField = null;
            myVoiceMailBoxTypeArray[0].MailboxTypeField = null;
            myVoiceMailBoxTypeArray[0].MaxDistributionListsField = null;
            myVoiceMailBoxTypeArray[0].MaxMailboxTimeField = null;
            myVoiceMailBoxTypeArray[0].MaxMessageLengthField = null;
            myVoiceMailBoxTypeArray[0].MaxMessagesField = null;
            myVoiceMailBoxTypeArray[0].MaxSubmailboxesField = null;
            myVoiceMailBoxTypeArray[0].MessageCountField = null;
            myVoiceMailBoxTypeArray[0].MessageListField = null;
            myVoiceMailBoxTypeArray[0].MessagePlaybackOrderField = null;
            myVoiceMailBoxTypeArray[0].NameField = null;
            myVoiceMailBoxTypeArray[0].NewMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].NotificationListField = null;
            myVoiceMailBoxTypeArray[0].PagerAttemptsField = null;
            myVoiceMailBoxTypeArray[0].PagerListField = null;
            myVoiceMailBoxTypeArray[0].PageSeqentialField = null;
            myVoiceMailBoxTypeArray[0].PageUrgentOnlyField = null;
            myVoiceMailBoxTypeArray[0].ParentListField = null;
            myVoiceMailBoxTypeArray[0].PlayAfterPromptField = null;
            myVoiceMailBoxTypeArray[0].PlayBusyGreetingField = null;
            myVoiceMailBoxTypeArray[0].PrePageDelayField = null;
            myVoiceMailBoxTypeArray[0].ReplyTypeField = null;
            myVoiceMailBoxTypeArray[0].SavedMessageRetentionField = null;
            myVoiceMailBoxTypeArray[0].ScheduledDeletionDateField = null;
            myVoiceMailBoxTypeArray[0].ScheduleListField = null;
            myVoiceMailBoxTypeArray[0].ShortMessageCriteriaField = null;
            myVoiceMailBoxTypeArray[0].SpecificCallerListField = null;
            myVoiceMailBoxTypeArray[0].VideoMWIUpdateRateField = null;

            //Per Innovatives request set the guid to null.
            var mySubInfoTypeList = new List<SubInfoType>();

            foreach (var mySubInfoType in myVoiceMailBoxTypeArray[0].SubscriberListField)
            {
                if (mySubInfoType.SubscriberDefaultPhoneNumberField == phoneNumber)
                {
                    mySubInfoType.SubscriberGuidField = null;
                }

                mySubInfoTypeList.Add(mySubInfoType);
            }

            myVoiceMailBoxTypeArray[0].SubscriberListField = mySubInfoTypeList.ToArray();

            _voiceMailService.UpdateVoiceMailBox(_loginInformation.LoginToken, myVoiceMailBoxTypeArray[0], null);
        }

        public static ChildInfoType GetChildInfoByHighestDigitField(ChildInfoType[] myChildInfoTypeArray)
        {
            ChildInfoType CurrentChildInfoType = new ChildInfoType() { DigitField = "-1" };
            foreach (var myChild in myChildInfoTypeArray)
            {
                //Submailboxes are always AddressType of AddressTypeMailboxNumber.  Outdialnumbers are AddressTypeDN.
                if (Convert.ToInt32(myChild.DigitField) > Convert.ToInt32(CurrentChildInfoType.DigitField)
                    && myChild.TypeField == AddressType.AddressTypeMailboxNumber)
                    CurrentChildInfoType = myChild;
            }

            return CurrentChildInfoType.DigitField == "-1" ? null : CurrentChildInfoType;
        }

        private static List<int> GetAvailableDigitFieldsFromChildList(IEnumerable<ChildInfoType> myChildInfoTypeArray, int iMaxNumberOfSubMailboxesAllowed)
        {
            List<int> AvailableDigitFieldList = new List<int>();

            for (int count = 1; count <= iMaxNumberOfSubMailboxesAllowed; count++)
            {
                bool blnFoundMatch = false;
                foreach (ChildInfoType myChildInfoType in myChildInfoTypeArray)
                {
                    if (Convert.ToInt32(myChildInfoType.DigitField) == count)
                        blnFoundMatch = true;
                }

                if (!blnFoundMatch)
                    AvailableDigitFieldList.Add(count);
            }

            return AvailableDigitFieldList;
        }
    }

    //Extension methods must be defined in a static class
    public static class ApMaxVoicemailV5ExtensionClass
    {
        public static ChildInfoType GetChildInfoByHighestDigitField(ChildInfoType[] myChildInfoTypeArray)
        {
            ChildInfoType CurrentChildInfoType = new ChildInfoType() { DigitField = "-1" };
            foreach (var myChild in myChildInfoTypeArray)
            {
                //Submailboxes are always AddressType of AddressTypeMailboxNumber.  Outdialnumbers are AddressTypeDN.
                if (Convert.ToInt32(myChild.DigitField) > Convert.ToInt32(CurrentChildInfoType.DigitField)
                    && myChild.TypeField == AddressType.AddressTypeMailboxNumber)
                    CurrentChildInfoType = myChild;
            }

            return CurrentChildInfoType.DigitField == "-1" ? null : CurrentChildInfoType;
        }
    }
}
