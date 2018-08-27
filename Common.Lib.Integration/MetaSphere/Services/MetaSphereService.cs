using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Interfaces;
using Common.Lib.Utility;
using Common.MetaSwitch;

namespace Common.MetaSphere.Services
{
    public class MetaSphereService
    {
        private readonly EquipmentConnectionSetting _setting;
        private readonly ShClient _sh;
        private readonly string _originHost;
        private MetaSphereShTypedUtilities _utilities;
        private readonly ILogger _logger;
        //private string _metaSphereVersion;

        public MetaSphereService(EquipmentConnectionSetting setting, ILogger logger)
        {
            var binding = new BasicHttpBinding
            {
                MaxBufferSize = Int32.MaxValue, //2147483647
                MaxReceivedMessageSize = Int32.MaxValue
            };

            _setting = setting;
            _sh = new ShClient(binding, new EndpointAddress(_setting.Url));
            //_originHost = "ANDP?clientVersion=" + setting.CustomString1;
            _originHost = setting.CustomString1;

            _utilities = new MetaSphereShTypedUtilities();
            //_metaSphereVersion = setting.CustomString1;
            _logger = logger;
        }

        public int RetrieveCurrentSequenceNumber(string dn, string serviceIndication)
        {
            tExtendedResult extendedResult;
            tUserData userData;
            var resultCode = ShPull(dn, serviceIndication, out extendedResult, out userData);

            if (userData == null || userData.ShData == null || !userData.ShData.Any())
            {
                return 0;
            }

            var currentSequenceNumber = userData.ShData[0].SequenceNumber;
            return currentSequenceNumber;
        }

        public int ShPull(string userIdentity, string serviceIndication, out tExtendedResult extendedResult, out tUserData userData)
        {
            var resultCode = _sh.ShPull(userIdentity, 0, serviceIndication, _originHost, out extendedResult, out userData);

            //Checks to make sure the request was a success and if not throws an exception and tacks on the extended result to the exception.
            _utilities.CheckResultCode(resultCode, extendedResult, null);

            return resultCode;
        }

        public int ShUpdate(string dn, tUserData userData, out tExtendedResult extendedResult, bool updateExisting = false)
        {
            //The apply action tells the web service to apply the settings supplied in the field elements, creating
            //or modifying the objects as required. This is the default action, and is the behavior if the Action
            //attribute is not specified explicitly
            //this is why you may not see action specified in the script or here.


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
                var serviceIndication = userData.ShData[0].ServiceIndication;

                var sequenceNumber = RetrieveCurrentSequenceNumber(dn, serviceIndication);
                if (sequenceNumber > 0)
                {
                    userData.ShData[0].SequenceNumber = _utilities.IncrementSequenceNumber(sequenceNumber);
                }
                else
                {
                    //if the subscriber doesn't exist then forcing a sequence number of 0 and forcing not to ignore sequence number.
                    userData.ShData[0].SequenceNumber = 0;
                }
            }

            //tMeta_Subscriber_Alarms alarms;
            var resultCode = _sh.ShUpdate(dn, 0, userData, _originHost, out extendedResult);

            //Checks to make sure the request was a success and if not throws an exception and tacks on the extended result to the exception.
            _utilities.CheckResultCode(resultCode, extendedResult, null);

            return resultCode;

            //alarms = RetrieveAlarms(dn);
        }

        //public void DeleteSubscriber(string dn)
        //{
        //    //When using Sh-Update to delete an object, you must specify a sequence number but it can be any
        //    //arbitrary non-zero value.
        //    //The only element that can be present in the <MetaSwitchData> or <MetaSphereData> element is
        //    //the service indication with Action="delete". Any other elements will cause an error.

        //    var userData = new MetaSphere.tUserData
        //    {
        //        ShData = new[]
        //        {
        //            new tTransparentData
        //            {
        //                ServiceIndication = "Msph_Subscriber_BaseInformation",
        //                SequenceNumber = 1, //0 indicates a new subscriber so setting to 1 to avoid confusion. When deleting any sequence number outside of 0 can be used.
        //                ServiceData = new tServiceData
        //                {
        //                    Item = new tMetaSphereData
        //                    {
        //                        ItemElementName = MetaSphere.ItemChoiceType1.Msph_Subscriber_BaseInformation,
        //                        Item = new tMsph_Subscriber_BaseInformation
        //                        {
        //                            PrimaryPhoneNumber = dn,
        //                            Action = "delete",
                                    
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    };

        //    // Send it in as an Sh-Update request, and make sure it succeeded.
        //    MetaSphere.tExtendedResult extendedResult;
        //    ShUpdate(dn, userData, out extendedResult);
        //}

        public SerializableDictionary<string, string> Validate(tUserData userData)
        {
            //update.UserData.ShData[0].ServiceData.Item.Item
            var validationErrors = new SerializableDictionary<string, string>();

            if (userData == null)
            {
                validationErrors.Add(LambdaHelper<tUserData>.GetPropertyName(x => x), "UserData is a mandatory field.");
            }
            else
            {
                if (userData.ShData == null)
                {
                    validationErrors.Add(LambdaHelper<tTransparentData>.GetPropertyName(x => x),
                        "TransparentData is a mandatory field.");
                }
                else
                {

                    if (string.IsNullOrWhiteSpace(userData.ShData[0].ServiceIndication))
                    {
                        validationErrors.Add("ServiceIndication", "ServiceIndication is a mandatory field.");
                    }

                    if (userData.ShData[0].ServiceData == null)
                    {
                        validationErrors.Add(LambdaHelper<tServiceData>.GetPropertyName(x => x),
                            "ServiceData is a mandatory field.");
                    }
                    else
                    {
                        if (userData.ShData[0].ServiceData.Item == null)
                        {
                            validationErrors.Add(LambdaHelper<tMetaSphereData>.GetPropertyName(x => x),
                                "MetaSphereData is a mandatory field.");
                        }
                        else
                        {
                        }
                    }
                }
            }

            return validationErrors;
        }
    }
}
