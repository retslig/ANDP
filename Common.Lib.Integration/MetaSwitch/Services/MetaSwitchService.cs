using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.ServiceModel;
using ANDP.Lib.Domain.Models;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Interfaces;
using Common.Lib.Utility;

namespace Common.MetaSwitch.Services
{
    public class MetaSwitchService
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly ShClient _sh;
        private readonly string _originHost;
        private MetaSphereShTypedUtilities _utilities;
        private string _metaSwitchVersion;
        private readonly ILogger _logger;


        public MetaSwitchService(EquipmentConnectionSetting setting, ILogger logger)
        {
            var binding = new BasicHttpBinding
            {
                MaxBufferSize = Int32.MaxValue,//2147483647
                MaxReceivedMessageSize = Int32.MaxValue
            };

            //string url = "http://mvs.sandbox.innovators.metaswitch.com:8087/mvweb/services/ShService";
            //string url = "http://eas.sandbox.innovators.metaswitch.com:8086/wsd/services/ShService";
            //string url = "http://mvs.sandbox.innovators.metaswitch.com:8087/mvweb/services/ShService";
            //const string url = "http://mvs.sandbox.innovators.metaswitch.com:8080/services/ShService";
            _setting = setting;
            _sh = new ShClient(binding, new EndpointAddress(_setting.Url));
            _originHost = "ANDP?clientVersion=" + setting.CustomString1;
            _utilities = new MetaSphereShTypedUtilities();
            _metaSwitchVersion = setting.CustomString1;
            _logger = logger;
        }

        public int RetrieveCurrentSequenceNumber(string dn, string serviceIndication)
        {
            tUserData userData;
            var resultCode = ShPull(dn, serviceIndication, out userData);

            if (userData == null || userData.ShData == null || userData.ShData.RepositoryData == null)
            {
                return 0;
            }

            var currentSequenceNumber = userData.ShData.RepositoryData.SequenceNumber;
            return currentSequenceNumber;
        }

        public int ShPull(string userIdentity, string serviceIndication, out tUserData userData)
        {
            tExtendedResult extendedResult;
            var resultCode = _sh.ShPull(userIdentity, 0, serviceIndication, _originHost, out extendedResult, out userData);

            //Checks to make sure the request was a success and if not throws an exception and tacks on the extended result to the exception.
            _utilities.CheckResultCode(resultCode, extendedResult, null);

            return resultCode;
        }

        public void DeleteSubscriber(string dn)
        {
            #region example xml
            //"<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            //"<tUserData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
            //"<Sh-Data xmlns=\"http://www.metaswitch.com/ems/soap/sh/userdata\">" +
            //"<RepositoryData>" +
            //"<ServiceIndication>" + strServiceIndication + "</ServiceIndication>" +
            //"<SequenceNumber>1</SequenceNumber>" +
            //"<ServiceData>" +
            //"<MetaSwitchData MetaSwitchVersion=\"6.0\" xmlns=\"http://www.metaswitch.com/ems/soap/sh/servicedata\">" +
            //"< ServiceIndication Action=\"delete\">" +
            //"</ ServiceIndication>" +
            //"</MetaSwitchData>" +
            //"</ServiceData>" +
            //"</RepositoryData>" +
            //"</Sh-Data>" +
            //"</tUserData>";
            #endregion

            //var baseInformation = (tMeta_Subscriber_BaseInformation)userData.ShData.RepositoryData.ServiceData.Item.Item;
            //baseInformation.Action = tMeta_Subscriber_BaseInformationAction.delete;

            //userData.ShData.RepositoryData.ServiceData.Item.Item = baseInformation;

            var userData = new tUserData
            {
                ShData = new tShData
                {
                    RepositoryData = new tTransparentData
                    {
                        ServiceIndication = "Meta_Subscriber_BaseInformation",
                        SequenceNumber = 1, //0 indicates a new subscriber so setting to 1 to avoid confusion.
                        ServiceData = new tServiceData
                        {
                            Item = new tMetaSwitchData
                            {
                                MetaSwitchVersion = _metaSwitchVersion,
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_BaseInformation,
                                Item = new tMeta_Subscriber_BaseInformation
                                {
                                    Action = tMeta_Subscriber_BaseInformationAction.delete,
                                    ActionSpecified = true
                                }
                            }
                        }

                    }
                }
            };

            // Send it in as an Sh-Update request, and make sure it succeeded.
            tExtendedResult extendedResult;
            ShUpdate(dn, userData, out extendedResult);
        }

        public int ShUpdate(string dn, tUserData userData, out tExtendedResult extendedResult, bool updateExisting = false)
        {
            if (string.IsNullOrEmpty(dn))
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name + ": dn is required and was not supplied.");
            }

            if (userData == null)
            {
                throw new Exception(MethodBase.GetCurrentMethod().Name + ": userData is required and was not supplied.");
            }

            if (updateExisting)
            {
                var serviceIndication = userData.ShData.RepositoryData.ServiceIndication;
                var sequenceNumber = RetrieveCurrentSequenceNumber(dn, serviceIndication);
                if (sequenceNumber > 0)
                {
                    userData.ShData.RepositoryData.SequenceNumber = _utilities.IncrementSequenceNumber(sequenceNumber);
                    userData.ShData.RepositoryData.ServiceData.Item.IgnoreSequenceNumber = tTrueFalse.False;
                }
                else
                {
                    //if the subscriber doesn't exist then forcing a sequence number of 0 and forcing not to ignore sequence number.
                    userData.ShData.RepositoryData.SequenceNumber = 0;
                    userData.ShData.RepositoryData.ServiceData.Item.IgnoreSequenceNumber = tTrueFalse.False;
                }
            }

            //tMeta_Subscriber_Alarms alarms;
            var resultCode = _sh.ShUpdate(dn, 0, userData, _originHost, out extendedResult);

            //Checks to make sure the request was a success and if not throws an exception and tacks on the extended result to the exception.
            _utilities.CheckResultCode(resultCode, extendedResult, null);

            return resultCode;

            //alarms = RetrieveAlarms(dn);
        }

        private string RetrieveFormattedAlarms(string dn)
        {
            var alarms = RetrieveAlarms(dn);
            return "Alarm state: \"" + new MetaSphereShTypedUtilities().ConvertToString(alarms.AlarmState) + "\"";
        }

        private tMeta_Subscriber_Alarms RetrieveAlarms(string dn)
        {
            // Send an ShPull to get the new subscriber's alarm state.  Extract and
            // print the alarm state.
            tExtendedResult extendedResult;
            tUserData userData;
            var resultCode = _sh.ShPull(dn, 0, "Meta_Subscriber_Alarms", _originHost, out extendedResult, out userData);

            new MetaSphereShTypedUtilities().CheckResultCode(resultCode, extendedResult, null);

            Object alarmsObject = ((tMetaSwitchData)userData.ShData.RepositoryData.ServiceData.Item).Item;

            tMeta_Subscriber_Alarms subscriberAlarms = (tMeta_Subscriber_Alarms)alarmsObject;

            return subscriberAlarms;
        }

        public SerializableDictionary<string, string> Validate(tUserData userData)
        {
            var validationErrors = new SerializableDictionary<string, string>();

            if (userData == null)
            {
                validationErrors.Add(LambdaHelper<tUserData>.GetPropertyName(x => x), "UserData is a mandatory field.");
            }
            else
            {
                if (userData.ShData == null)
                {
                    validationErrors.Add(LambdaHelper<tShData>.GetPropertyName(x => x), "ShData is a mandatory field.");
                }
                else
                {
                    if (userData.ShData.RepositoryData == null)
                    {
                        validationErrors.Add(LambdaHelper<tTransparentData>.GetPropertyName(x => x), "RepositoryData is a mandatory field.");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(userData.ShData.RepositoryData.ServiceIndication))
                        {
                            validationErrors.Add("ServiceIndication", "ServiceIndication is a mandatory field.");
                        }

                        if (userData.ShData.RepositoryData.ServiceData == null)
                        {
                            ItemChoiceType gggg = userData.ShData.RepositoryData.ServiceData.Item.ItemElementName;
                            validationErrors.Add(LambdaHelper<tServiceData>.GetPropertyName(x => x), "ServiceData is a mandatory field.");
                        }
                        else
                        {
                            if (userData.ShData.RepositoryData.ServiceData.Item == null)
                            {
                                validationErrors.Add(LambdaHelper<tMetaSwitchData>.GetPropertyName(x => x), "MetaSwitchData is a mandatory field.");
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            return validationErrors;
        }

        /**
         * Parse the command line arguments and extract the necessary information.
         * See the associated CreateSubscriber.bat script for parameter details.
         *
         * @returns           Information about the subscriber to be created.
         *
         * @param args        The arguments provided at the command line.
         * @param emsAddress
         *                    The URL to which to send SOAP messages.
         * @param dN          The directory number to update.
         */
        //private static SubscriberInfo getParameters(string[] args, out String emsAddress, out String dN)
        //{
        //    string emsAddressParameter;
        //    string dNParameter;

        //    SubscriberInfo fields = new SubscriberInfo();
        //    string currentArg = "";

        //    foreach (string arg in args)
        //    {
        //        //-----------------------------------------------------------------------
        //        // Parse each parameter one by one (ignores anything before the first
        //        // flag).
        //        //-----------------------------------------------------------------------
        //        if (arg.StartsWith("-"))
        //        {
        //            currentArg = arg.ToLower();
        //        }
        //        else
        //        {
        //            switch (currentArg)
        //            {
        //                case "-ems":
        //                    emsAddressParameter.append(arg);
        //                    break;
        //                case "-dn":
        //                    dNParameter.append(arg);
        //                    break;
        //                case "-metaswitch":
        //                    fields.metaSwitchName.append(arg);
        //                    break;
        //                case "-businessgroup":
        //                    fields.businessGroupName.append(arg);
        //                    break;
        //                case "-subscribergroup":
        //                    fields.subscriberGroup.append(arg);
        //                    break;
        //                case "-profile":
        //                    fields.persistentProfile.append(arg);
        //                    break;
        //                case "-username":
        //                    fields.sipUserName.append(arg);
        //                    break;
        //                case "-domain":
        //                    fields.sipDomainName.append(arg);
        //                    break;
        //                case "-password":
        //                    fields.sipPassword.append(arg);
        //                    break;
        //                default:
        //                    throw new WrongParametersException("Unrecognised argument: " + currentArg);
        //            }
        //        }
        //    }

        //    if (emsAddressParameter == null)
        //    {
        //        emsAddress = null;
        //    }
        //    else
        //    {
        //        emsAddress = "http://" + emsAddressParameter + ":8080/services/ShService";
        //    }

        //    if (dNParameter == null)
        //    {
        //        //throw new WrongParametersException("No directory number was provided");
        //    }
        //    else
        //    {
        //        dN = dNParameter;
        //    }

        //    return fields;
        //}

        //---------------------------------------------------------------------------
        // INNER CLASS: SubscriberInfo
        //
        // Holds information about the subscriber to be created.
        //---------------------------------------------------------------------------
    }

}