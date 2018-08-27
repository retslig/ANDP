using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using ANDP.Provisioning.API.Rest.Models.MetaSwitch;
using Common.Lib.Extensions;
using Common.Lib.Security;
using Common.MetaSwitch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Test
{
    [TestClass]
    public class MetaSwitchFixture
    {
        //private IUnityContainer _container;
        private const string BaseRemoteUrl = "https://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/metaswitch/";
        private const string BaseLocalUrl = "https://localhost:56198/api/metaswitch/";
        private const string dn = "6055554444";
        private const int EquipmentId = 1;
        private WebRequestHandler _handler;
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new WebRequestHandler();
            _handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallback;
            _client = new HttpClient(_handler);
            _client.BaseAddress = new Uri(BaseRemoteUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.SetBasicAuthentication("srtc", "srtc");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _handler.Dispose();
        }

        [TestMethod]
        public void Can_Call_Test_Controller()
        {
            HttpResponseMessage response = _client.GetAsync("test").Result;

            var result = response.Content.ReadAsStringAsync();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("OK", response.ReasonPhrase);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("\"OK\"", result.Result);
            Assert.AreEqual(null, result.Exception);
        }

        [TestMethod]
        public void Can_Retrieve_Subscriber()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var httpResponseMessage = _client.GetAsync("pull/0123456795/equipment/7").Result;
            var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //result = result.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", "").Replace("</string>", ""); //Removes the extras if recieved xml
            //result = result.Trim('"').Replace("\\", ""); //removes the extras if json is recieved.

            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(result);

            //*** ASSERT ***
            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsNotNull(userData);
        }

        [TestMethod]
        public void Can_Create_Subscriber()
        {
            //*** ARRANGE ***

            //*** ACT ***
            //var httpResponseMessage = _client.GetAsync("subscriber/dn/0833019517/equipment/7").Result;
            //var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            //var result = myResponse.Result;

            //result = result.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", "").Replace("</string>", ""); //Removes the extras if recieved xml
            //result = result.Trim('"').Replace("\\", ""); //removes the extras if json is recieved.

            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 0,
                        ServiceIndication = "Meta_Subscriber_BaseInformation",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_BaseInformation,
                                Item = new tMeta_Subscriber_BaseInformation
                                {
                                    Action = tMeta_Subscriber_BaseInformationAction.apply,
                                    ActionSpecified = true,
                                    PersistentProfile = "None",
                                    MetaSwitchName = "Meribel",
                                    SubscriberGroup = "Subscribers in Guernsey, NJ",
                                    SignalingType = tMeta_Subscriber_BaseInformation_SignalingType.AnalogLineT1CAS,
                                    SignalingTypeSpecified = true,
                                    NumberStatus = tMeta_Subscriber_BaseInformation_NumberStatus.Normal,
                                    NumberStatusSpecified = true,
                                    SignalFunctionCode = tMeta_Subscriber_BaseInformation_SignalFunctionCode.Loopstart,
                                    AccessLineNumber = 7,
                                    AccessLineNumberSpecified = true,
                                    Locale = tMeta_Subscriber_BaseInformation_Locale.EnglishUS,
                                    LocaleSpecified = true,
                                    LineUsage = tMeta_Subscriber_BaseInformation_LineUsage.Voiceandfax,
                                    AccessDevice = new tAccessDeviceReference
                                    {
                                        AccessDeviceName = "Sample IDT",
                                        GatewayName = "MetaSwitch Local Media Gateway"
                                    },
                                    SubMediaGatewayModel = new tSwitchableDefaultString
                                    {
                                        Default = "Derived from SIP User Agent",
                                        UseDefault = tTrueFalse.False,
                                        Value = "MyMediaGatewayModel"
                                    },
                                    BillingType = new tMeta_Subscriber_BaseInformation_BillingType
                                    {
                                        Default = tMeta_Subscriber_BaseInformation_BillingType_Value.Messagerate,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_BaseInformation_BillingType_Value.Messagerate,
                                        ValueSpecified = true
                                    },
                                    RoutingAttributes = new tMeta_Subscriber_BaseInformation_RoutingAttributes
                                    {
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = new tMeta_Subscriber_BaseInformation_RoutingAttributes_Value
                                        {
                                            PrepaidOffswitchCallingCardSubscriber = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            },
                                            FaxModemSubscriber = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            },
                                            NomadicSubscriber = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            }
                                        }
                                    },
                                    DenyAllUsageSensitiveFeatures = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    },
                                    //ForceLNPLookup = new tSwitchableDefaultTrueFalse
                                    //{
                                    //    ValueSpecified = reyw
                                    //}
                                    Timezone = new tMeta_Subscriber_BaseInformation_Timezone
                                    {
                                        Default = tMeta_Subscriber_BaseInformation_Timezone_Value.CST6,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_BaseInformation_Timezone_Value.CST6,
                                        ValueSpecified = true
                                    },
                                    AdjustForDaylightSavings = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    LongDistanceCarrier = new tSwitchableDefaultString
                                    {
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = "InterLataPic"
                                    },
                                    IntraLATACarrier = new tSwitchableDefaultString
                                    {
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = "IntraLataPic"
                                    },
                                    InternationalCarrier = new tSwitchableDefaultString
                                    {
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = "InternationalPic"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            //tUserData userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(userData);

            //baseInformation.DirectoryNumber = "0833019599";
            //userData.ShData.RepositoryData.ServiceData.Item.Item = baseInformation;

            var addUpdate = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0833019599",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(addUpdate);

            var updateHttpResponse = _client.PostAsync("subscriber/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;

            //*** ASSERT ***
            Assert.IsTrue(true);
            Assert.IsNotNull(updateResult);
        }

        [TestMethod]
        public void Can_Retrieve_Calling_Name_Delivery()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var httpResponseMessage = _client.GetAsync("pull/useridentity/0123456795/serviceindication/Meta_Subscriber_CallingNameDelivery/equipment/7").Result;
            var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(result);

            //*** ASSERT ***
            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsNotNull(userData);
        }

        [TestMethod]
        public void Can_Add_Calling_Name_Delivery()
        {

            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallingNameDelivery",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallingNameDelivery,
                                Item = new tMeta_Subscriber_CallingNameDelivery
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        //UseDefault = tTrueFalse.False,
                                        //UseDefaultSpecified = true,
                                        //Default = tTrueFalse.False,
                                        //DefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    LocalName = "hello",
                                    LocalNameBusLine = null,
                                    UseLocalNameForIntercomCallsOnlyBusLine = null,
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    UsageSensitiveBilling = null,
                                    //Action = tMeta_Subscriber_CallingNameDeliveryAction.apply,
                                    //ActionSpecified = true,
                                    ActionSpecified = false
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add calling name delivery
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallingNameDelivery",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefaultSpecified": false,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "DefaultSpecified": false
            //              },
            //              "LocalName": "hello",
            //              "LocalNameBusLine": null,
            //              "UseLocalNameForIntercomCallsOnlyBusLine": null,
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 420,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}

            //pull of subscriber with calling name delivery
            //            {
            //  "$id": "1",
            //  "ShData": {
            //    "$id": "2",
            //    "RepositoryData": {
            //      "$id": "3",
            //      "ServiceIndication": "Meta_Subscriber_CallingNameDelivery",
            //      "SequenceNumber": 42480,
            //      "ServiceData": {
            //        "$id": "4",
            //        "Item": {
            //          "$id": "5",
            //          "Item": {
            //            "$id": "6",
            //            "Subscribed": {
            //              "$id": "7",
            //              "UseDefault": "false",
            //              "UseDefaultSpecified": true,
            //              "Value": "true",
            //              "ValueSpecified": true,
            //              "Default": "false",
            //              "DefaultSpecified": true
            //            },
            //            "LocalName": "hello",
            //            "LocalNameBusLine": null,
            //            "UseLocalNameForIntercomCallsOnlyBusLine": null,
            //            "Enabled": "true",
            //            "EnabledSpecified": true,
            //            "UsageSensitiveBilling": null,
            //            "ActionSpecified": false
            //          },
            //          "ItemElementName": "meta_Subscriber_CallingNameDelivery",
            //          "IgnoreSequenceNumber": "false",
            //          "MetaSwitchVersion": "9.0"
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Remove_Calling_Name_Delivery()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallingNameDelivery",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallingNameDelivery,
                                Item = new tMeta_Subscriber_CallingNameDelivery
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        //UseDefault = tTrueFalse.False,
                                        //UseDefaultSpecified = true,
                                        //Default = tTrueFalse.False,
                                        //DefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    },
                                    LocalName = "",
                                    LocalNameBusLine = null,
                                    UseLocalNameForIntercomCallsOnlyBusLine = null,
                                    //Enabled = tTrueFalse.True,
                                    EnabledSpecified = false,
                                    UsageSensitiveBilling = null,
                                    //Action = tMeta_Subscriber_CallingNameDeliveryAction.apply,
                                    //ActionSpecified = true,
                                    ActionSpecified = false
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to remove calling name delivery
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallingNameDelivery",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "LocalName": "",
            //              "LocalNameBusLine": null,
            //              "UseLocalNameForIntercomCallsOnlyBusLine": null,
            //              "EnabledSpecified": false,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 420,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}

            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallingNameDelivery",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefaultSpecified": false,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "DefaultSpecified": false
            //              },
            //              "LocalName": "",
            //              "LocalNameBusLine": null,
            //              "UseLocalNameForIntercomCallsOnlyBusLine": null,
            //              "EnabledSpecified": false,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 420,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}

            //pull of subscriber with calling name delivery
            //{
            //  "$id": "1",
            //  "ShData": {
            //    "$id": "2",
            //    "RepositoryData": {
            //      "$id": "3",
            //      "ServiceIndication": "Meta_Subscriber_CallingNameDelivery",
            //      "SequenceNumber": 9894,
            //      "ServiceData": {
            //        "$id": "4",
            //        "Item": {
            //          "$id": "5",
            //          "Item": {
            //            "$id": "6",
            //            "Subscribed": {
            //              "$id": "7",
            //              "UseDefault": "false",
            //              "UseDefaultSpecified": true,
            //              "Value": "false",
            //              "ValueSpecified": true,
            //              "Default": "false",
            //              "DefaultSpecified": true
            //            },
            //            "LocalName": "",
            //            "LocalNameBusLine": null,
            //            "UseLocalNameForIntercomCallsOnlyBusLine": null,
            //            "EnabledSpecified": false,
            //            "UsageSensitiveBilling": null,
            //            "ActionSpecified": false
            //          },
            //          "ItemElementName": "meta_Subscriber_CallingNameDelivery",
            //          "IgnoreSequenceNumber": "false",
            //          "MetaSwitchVersion": "9.0"
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Delete_Subscriber()
        {
            //*** ARRANGE ***

            //*** ACT ***
            //var httpResponseMessage = _client.GetAsync("subscriber/dn/0833019502/equipment/7").Result;
            //var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            //var result = myResponse.Result;

            //var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(result);
            //Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            //Assert.IsNotNull(userData);

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(userData);
            var deleteHttpResponse = _client.DeleteAsync("subscriber/dn/0833019502/equipment/7").Result;
            var deleteResponse = deleteHttpResponse.Content.ReadAsStringAsync();
            var deleteResult = deleteResponse.Result;


            var httpResponseMessage2 = _client.GetAsync("subscriber/dn/0833019502/equipment/7").Result;
            var myResponse2 = httpResponseMessage2.Content.ReadAsStringAsync();
            var result2 = myResponse2.Result;

            //*** ASSERT ***


            //*** ASSERT ***
            Assert.IsTrue(true);

        }

        [TestMethod]
        public void Can_Retrieve_LCCs()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var httpResponseMessage = _client.GetAsync("pull/useridentity/0123456795/serviceindication/Meta_Subscriber_LineClassCodes/equipment/7").Result;
            var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(result);

            #region pull json example
            //            {
            //  "$id": "1",
            //  "ShData": {
            //    "$id": "2",
            //    "RepositoryData": {
            //      "$id": "3",
            //      "ServiceIndication": "Meta_Subscriber_LineClassCodes",
            //      "SequenceNumber": 30235,
            //      "ServiceData": {
            //        "$id": "4",
            //        "Item": {
            //          "$id": "5",
            //          "Item": {
            //            "$id": "6",
            //            "LineClassCode1": {
            //              "$id": "7",
            //              "UseDefault": "true",
            //              "UseDefaultSpecified": true,
            //              "Value": "0 Default",
            //              "Default": "0 Default"
            //            },
            //            "LineClassCode2": {
            //              "$id": "8",
            //              "UseDefault": "true",
            //              "UseDefaultSpecified": true,
            //              "Value": "0",
            //              "Default": "0"
            //            },
            //            "LineClassCode3": {
            //              "$id": "9",
            //              "UseDefault": "true",
            //              "UseDefaultSpecified": true,
            //              "Value": "0",
            //              "Default": "0"
            //            },
            //            "LineClassCode4": {
            //              "$id": "10",
            //              "UseDefault": "true",
            //              "UseDefaultSpecified": true,
            //              "Value": "0 Default",
            //              "Default": "0 Default"
            //            },
            //            "LineClassCode5": null,
            //            "LineClassCode6": null,
            //            "LineClassCode7": null,
            //            "LineClassCode8": null,
            //            "LineClassCode9": null,
            //            "LineClassCode10": null,
            //            "LineClassCode11": null,
            //            "LineClassCode12": null,
            //            "LineClassCode13": null,
            //            "LineClassCode14": null,
            //            "LineClassCode15": null,
            //            "LineClassCode16": null,
            //            "LineClassCode17": null,
            //            "LineClassCode18": null,
            //            "LineClassCode19": null,
            //            "LineClassCode20": null,
            //            "ActionSpecified": false
            //          },
            //          "ItemElementName": "meta_Subscriber_LineClassCodes",
            //          "IgnoreSequenceNumber": "false",
            //          "MetaSwitchVersion": "9.0"
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            //*** ASSERT ***
            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsNotNull(userData);
        }

        [TestMethod]
        public void Can_Update_LCCs()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_LineClassCodes",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_LineClassCodes,
                                Item = new tMeta_Subscriber_LineClassCodes
                                {
                                    LineClassCode1 = new tSwitchableDefaultString
                                    {
                                        Value = "0 Default"
                                    },
                                    LineClassCode2 = new tSwitchableDefaultString
                                    {
                                        Value = "0"
                                    },
                                    LineClassCode3 = new tSwitchableDefaultString
                                    {
                                        Value = "0"
                                    },
                                    LineClassCode4 = new tSwitchableDefaultString
                                    {
                                        Value = "0"
                                    }
                                    //LineClassCode5 = new tSwitchableDefaultString
                                    //{
                                    //    Value = "0"
                                    //},
                                    //LineClassCode8 = new tSwitchableDefaultString
                                    //{
                                    //    Value = "0"
                                    //},
                                    //LineClassCode13 = new tSwitchableDefaultString
                                    //{
                                    //    Value = "0"
                                    //},
                                    //LineClassCode14 = new tSwitchableDefaultString
                                    //{
                                    //    Value = "0"
                                    //}
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;

            Assert.IsNotNull(updateResult);

        }

        [TestMethod]
        public void Can_Retrieve_SelectiveCallForwarding()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var httpResponseMessage = _client.GetAsync("pull/useridentity/0123456795/serviceindication/Meta_Subscriber_SelectiveCallForwarding/equipment/7").Result;
            var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<tUserData>(result);

            //*** ASSERT ***
            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsNotNull(userData);
        }

        [TestMethod]
        public void Can_Add_SelectiveCallForwarding()
        {

            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_SelectiveCallForwarding",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_SelectiveCallForwarding,
                                Item = new tMeta_Subscriber_SelectiveCallForwarding
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.False,
                                    EnabledSpecified = true,
                                    NumberToForwardTo = null,
                                    SingleRing = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    },
                                    NumberOfAnonymousNumbersForwarded = 0,
                                    NumberOfAnonymousNumbersForwardedSpecified = true,
                                    //Action = tMeta_Subscriber_SelectiveCallForwardingAction.apply,
                                    ActionSpecified = false,
                                    UsageSensitiveBilling = null
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_SelectiveCallForwarding",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 1,
            //              "EnabledSpecified": true,
            //              "NumberToForwardTo": null,
            //              "SingleRing": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "NumberOfAnonymousNumbersForwarded": 0,
            //              "NumberOfAnonymousNumbersForwardedSpecified": true,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 469,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}

            //pull of subscriber with SelectiveCallForwarding
            //{
            //  "$id": "1",
            //  "ShData": {
            //    "$id": "2",
            //    "RepositoryData": {
            //      "$id": "3",
            //      "ServiceIndication": "Meta_Subscriber_SelectiveCallForwarding",
            //      "SequenceNumber": 40819,
            //      "ServiceData": {
            //        "$id": "4",
            //        "Item": {
            //          "$id": "5",
            //          "Item": {
            //            "$id": "6",
            //            "Subscribed": {
            //              "$id": "7",
            //              "UseDefault": "false",
            //              "UseDefaultSpecified": true,
            //              "Value": "true",
            //              "ValueSpecified": true,
            //              "Default": "false",
            //              "DefaultSpecified": true
            //            },
            //            "Enabled": "false",
            //            "EnabledSpecified": true,
            //            "NumberToForwardTo": "",
            //            "SingleRing": {
            //              "$id": "8",
            //              "UseDefault": "true",
            //              "UseDefaultSpecified": true,
            //              "Value": "false",
            //              "ValueSpecified": true,
            //              "Default": "false",
            //              "DefaultSpecified": true
            //            },
            //            "NumberOfAnonymousNumbersForwarded": 0,
            //            "NumberOfAnonymousNumbersForwardedSpecified": true,
            //            "UsageSensitiveBilling": null,
            //            "ActionSpecified": false
            //          },
            //          "ItemElementName": "meta_Subscriber_SelectiveCallForwarding",
            //          "IgnoreSequenceNumber": "false",
            //          "MetaSwitchVersion": "9.0"
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_SelectiveCallRejection()
        {

            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_SelectiveCallRejection",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_SelectiveCallRejection,
                                Item = new tMeta_Subscriber_SelectiveCallRejection
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.False,
                                    EnabledSpecified = true,
                                    NumberOfAnonymousNumbers = 0,
                                    NumberOfAnonymousNumbersSpecified = true,
                                    //Action = tMeta_Subscriber_SelectiveCallRejectionAction.apply,
                                    ActionSpecified = false,
                                    UsageSensitiveBilling = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_SelectiveCallRejection",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 1,
            //              "EnabledSpecified": true,
            //              "NumberOfAnonymousNumbers": 0,
            //              "NumberOfAnonymousNumbersSpecified": true,
            //              "UsageSensitiveBilling": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 471,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallingNumberDelivery()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallingNumberDelivery",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallingNumberDelivery,
                                Item = new tMeta_Subscriber_CallingNumberDelivery
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.False,
                                    EnabledSpecified = true,
                                    //Action = tMeta_Subscriber_SelectiveCallRejectionAction.apply,
                                    ActionSpecified = false,
                                    //UsageSensitiveBilling = null
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallingNumberDelivery",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 1,
            //              "EnabledSpecified": true,
            //              "UsageSensitiveBilling": null,
            //              "PreferredSIPFormat": null,
            //              "OverridePrivacySettingOfCallingSubscriber": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 421,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }


        [TestMethod]
        public void Can_Add_BusyCallForwarding()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_BusyCallForwarding",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_BusyCallForwarding,
                                Item = new tMeta_Subscriber_BusyCallForwarding
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Variant = new tMeta_Subscriber_BusyCallForwarding_Variant
                                    {
                                        Default = tMeta_Subscriber_BusyCallForwarding_Variant_Value.Fixednumber,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_BusyCallForwarding_Variant_Value.Fixednumber,
                                        ValueSpecified = true,
                                    },

                                    Enabled = tTrueFalse.False,
                                    EnabledSpecified = true,
                                    //Action = tMeta_Subscriber_SelectiveCallRejectionAction.apply,
                                    ActionSpecified = false,
                                    //UsageSensitiveBilling = null
                                    Number = "99999"
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_BusyCallForwarding",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Variant": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 1,
            //              "EnabledSpecified": true,
            //              "Number": "99999",
            //              "UnconditionalBusyAndDelayCallForwardingUsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 406,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_DelayedCallForwarding()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_DelayedCallForwarding",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_DelayedCallForwarding,
                                Item = new tMeta_Subscriber_DelayedCallForwarding
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Variant = new tMeta_Subscriber_DelayedCallForwarding_Variant
                                    {
                                        Default = tMeta_Subscriber_DelayedCallForwarding_Variant_Value.Fixednumber,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_DelayedCallForwarding_Variant_Value.Fixednumber,
                                        ValueSpecified = true,
                                    },

                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    //Action = tMeta_Subscriber_SelectiveCallRejectionAction.apply,
                                    ActionSpecified = false,
                                    //UsageSensitiveBilling = null
                                    Number = "99999",
                                    NoReplyTime = new tMeta_Subscriber_DelayedCallForwarding_NoReplyTime
                                    {
                                        Default = 24,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = 24,
                                        ValueSpecified = true
                                    }

                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_DelayedCallForwarding",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Variant": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "Number": "99999",
            //              "NoReplyTime": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 24,
            //                "ValueSpecified": true,
            //                "Default": 24,
            //                "DefaultSpecified": true
            //              },
            //              "UnconditionalBusyAndDelayCallForwardingUsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 428,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_UnconditionalCallForwarding()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_UnconditionalCallForwarding",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_UnconditionalCallForwarding,
                                Item = new tMeta_Subscriber_UnconditionalCallForwarding
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Variant = new tMeta_Subscriber_UnconditionalCallForwarding_Variant
                                    {
                                        Default = tMeta_Subscriber_UnconditionalCallForwarding_Variant_Value.Fixednumber,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_UnconditionalCallForwarding_Variant_Value.Fixednumber,
                                        ValueSpecified = true,
                                    },
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    ActionSpecified = false,
                                    Number = "999999"
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_UnconditionalCallForwarding",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Variant": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "Number": "999999",
            //              "SingleRing": null,
            //              "UnconditionalBusyAndDelayCallForwardingUsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 483,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_DoNotDisturb()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_DoNotDisturb",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_DoNotDisturb,
                                Item = new tMeta_Subscriber_DoNotDisturb
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    ActionSpecified = false,
                                    SingleRing = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_DoNotDisturb",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "ServiceLevel": null,
            //              "SingleRing": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "IntegratedDND": null,
            //              "SelectiveCallAcceptanceNumberOfAnonymousNumbersSpecified": false,
            //              "SelectiveCallAcceptanceUsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 433,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_Voicemail()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_Voicemail",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_Voicemail,
                                Item = new tMeta_Subscriber_Voicemail
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    CallDeliveryMethod = new tMeta_Subscriber_Voicemail_CallDeliveryMethod
                                    {
                                        Default = tMeta_Subscriber_Voicemail_CallDeliveryMethod_Value.None,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_Voicemail_CallDeliveryMethod_Value.SIP,
                                        ValueSpecified = true
                                    },
                                    IndicatorNotificationMethod = new tMeta_Subscriber_Voicemail_IndicatorNotificationMethod
                                    {
                                        Default = tMeta_Subscriber_Voicemail_IndicatorNotificationMethod_Value.SMDI,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_Voicemail_IndicatorNotificationMethod_Value.SIP,
                                        ValueSpecified = true
                                    },
                                    SMDILink = new tSwitchableDefaultString
                                    {
                                        Default = "Metasphere EAS",
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = "Metasphere EAS"
                                    },
                                    VisualMessageWaitingIndicator = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    AudibleMessageWaitingIndicator = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    CallTransferTime = new tMeta_Subscriber_Voicemail_CallTransferTime
                                    {
                                        Default = 36,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = 24,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_Voicemail",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "SharePrimaryLineVoicemailMailboxSpecified": false,
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "CallDeliveryMethod": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 3,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "VoicemailSystemLineGroup": null,
            //              "RetrievalNumber": null,
            //              "IncomingNumber": null,
            //              "ApplicationServer": null,
            //              "IndicatorNotificationMethod": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 3,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "SMDILink": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": "Metasphere EAS",
            //                "Default": "Metasphere EAS"
            //              },
            //              "AuthorizedIDForIndicatorControl": null,
            //              "IndicatorNotificationApplicationServer": null,
            //              "VisualMessageWaitingIndicator": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "AudibleMessageWaitingIndicator": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "SIPMessageWaitingIndicator": null,
            //              "MessageWaitingIndicatorStatusSpecified": false,
            //              "CallTransferTime": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 24,
            //                "ValueSpecified": true,
            //                "Default": 36,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 485,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_SpeedCalling()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_SpeedCalling",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_SpeedCalling,
                                Item = new tMeta_Subscriber_SpeedCalling
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    ActionSpecified = false,
                                    AllowedTypes = new tMeta_Subscriber_SpeedCalling_AllowedTypes
                                    {
                                        Default = tMeta_Subscriber_SpeedCalling_AllowedTypes_Value.Oneandtwodigit,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_SpeedCalling_AllowedTypes_Value.Onedigit,
                                        ValueSpecified = true
                                    },
                                    HandsetAccessAllowed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_SpeedCalling",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "AllowedTypes": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 2,
            //                "DefaultSpecified": true
            //              },
            //              "HandsetAccessAllowed": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "ValueSpecified": false,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 473,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_TerminationAttemptTrigger()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_TerminationAttemptTrigger",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_TerminationAttemptTrigger,
                                Item = new tMeta_Subscriber_TerminationAttemptTrigger
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Trigger = new tSwitchableDefaultString
                                    {
                                        Default = "Reference not valid: Intelligent Networking Services Trigger 0",
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = "AP Screen Pop",
                                    },
                                    ActionSpecified = false,
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_TerminationAttemptTrigger",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Trigger": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": "AP Screen Pop",
            //                "Default": "Reference not valid: Intelligent Networking Services Trigger 0"
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 480,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_OffHookDelaySubscriberTrigger()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_OffHookDelaySubscriberTrigger",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_OffHookDelaySubscriberTrigger,
                                Item = new tMeta_Subscriber_OffHookDelaySubscriberTrigger
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Trigger = new tSwitchableDefaultString
                                    {
                                        Default = "Reference not valid: Intelligent Networking Services Trigger 0",
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = "AP OCM",
                                    },
                                    ActionSpecified = false,
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //            {
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_OffHookDelaySubscriberTrigger",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Trigger": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": "AP OCM",
            //                "Default": "Reference not valid: Intelligent Networking Services Trigger 0"
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 453,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_3WayCalling()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_3WayCalling",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_3WayCalling,
                                Item = new tMeta_Subscriber_3WayCalling()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Action =  tMeta_Subscriber_3WayCallingAction.apply,
                                    ActionSpecified = false,
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_3WayCalling",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 391,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallWaiting()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallWaiting",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallWaiting,
                                Item = new tMeta_Subscriber_CallWaiting()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    ActionSpecified = false,
                                    Action = tMeta_Subscriber_CallWaitingAction.apply,
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallWaiting",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "EnabledSpecified": false,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 415,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_AutomaticRecall()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_AutomaticRecall",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_AutomaticRecall,
                                Item = new tMeta_Subscriber_AutomaticRecall()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    UsageSensitiveBilling = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    },
                                    ActionSpecified = false
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_AutomaticRecall",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "UsageSensitiveBilling": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 400,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_TeenServiceLine1()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_TeenServiceLine1",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_TeenServiceLine1,
                                Item = new tMeta_Subscriber_TeenServiceLine1()
                                {
                                    Action = tMeta_Subscriber_TeenServiceLine1Action.apply,
                                    ActionSpecified = true,
                                    Configured = tTrueFalse.True,
                                    ConfiguredSpecified = true,
                                    DirectoryNumber = "6059998888",
                                    RingCadence = tMeta_Subscriber_TeenServiceLine1_RingPattern.LongLong,
                                    RingCadenceSpecified = true,
                                    NumberStatus = tMeta_Subscriber_TeenServiceLine1_NumberStatus.Normal,
                                    NumberStatusSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding

            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_TeenServiceLine2()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_TeenServiceLine2",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_TeenServiceLine2,
                                Item = new tMeta_Subscriber_TeenServiceLine2()
                                {
                                    Action = tMeta_Subscriber_TeenServiceLine2Action.apply,
                                    ActionSpecified = true,
                                    Configured = tTrueFalse.True,
                                    ConfiguredSpecified = true,
                                    DirectoryNumber = "6059998888",
                                    RingCadence = tMeta_Subscriber_TeenServiceLine2_RingPattern.ShortLongShort,
                                    RingCadenceSpecified = true,
                                    NumberStatus = tMeta_Subscriber_TeenServiceLine2_NumberStatus.Normal,
                                    NumberStatusSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_TeenServiceLine2",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Configured": 0,
            //              "ConfiguredSpecified": true,
            //              "DirectoryNumber": "6059998888",
            //              "RingPatternSpecified": false,
            //              "RingCadence": 1,
            //              "RingCadenceSpecified": true,
            //              "NumberStatus": 0,
            //              "NumberStatusSpecified": true,
            //              "Action": 0,
            //              "ActionSpecified": true
            //            },
            //            "ItemElementName": 477,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_TeenServiceLine3()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_TeenServiceLine3",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_TeenServiceLine3,
                                Item = new tMeta_Subscriber_TeenServiceLine3()
                                {
                                    Action = tMeta_Subscriber_TeenServiceLine3Action.apply,
                                    ActionSpecified = true,
                                    Configured = tTrueFalse.True,
                                    ConfiguredSpecified = true,
                                    DirectoryNumber = "6059998888",
                                    RingCadence = tMeta_Subscriber_TeenServiceLine3_RingPattern.ShortShortLong,
                                    RingCadenceSpecified = true,
                                    NumberStatus = tMeta_Subscriber_TeenServiceLine3_NumberStatus.Normal,
                                    NumberStatusSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_TeenServiceLine3",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Configured": 0,
            //              "ConfiguredSpecified": true,
            //              "DirectoryNumber": "6059998888",
            //              "RingPatternSpecified": false,
            //              "RingCadence": 2,
            //              "RingCadenceSpecified": true,
            //              "NumberStatus": 0,
            //              "NumberStatusSpecified": true,
            //              "Action": 0,
            //              "ActionSpecified": true
            //            },
            //            "ItemElementName": 478,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_RemoteAccessToCallForwarding()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_RemoteAccessToCallForwarding",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_RemoteAccessToCallForwarding,
                                Item = new tMeta_Subscriber_RemoteAccessToCallForwarding()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Blocked = tTrueFalse.False,
                                    BlockedSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_RemoteAccessToCallForwarding",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Blocked": 1,
            //              "BlockedSpecified": true,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 468,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallTransfer()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallTransfer",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallTransfer,
                                Item = new tMeta_Subscriber_CallTransfer()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallTransfer",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 414,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallHold()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallHold",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallHold,
                                Item = new tMeta_Subscriber_CallHold()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallHold",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 409,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallWaitingWithCallerID()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallWaitingWithCallerID",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallWaitingWithCallerID,
                                Item = new tMeta_Subscriber_CallWaitingWithCallerID()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallWaitingWithCallerID",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 416,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallTrace()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallTrace",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallTrace,
                                Item = new tMeta_Subscriber_CallTrace()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallTrace",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 413,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_HomeIntercom()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_HomeIntercom",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_HomeIntercom,
                                Item = new tMeta_Subscriber_HomeIntercom()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_HomeIntercom",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "DistinctiveRinging": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 441,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_AnonymousCallRejection()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_AnonymousCallRejection",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_AnonymousCallRejection,
                                Item = new tMeta_Subscriber_AnonymousCallRejection()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_AnonymousCallRejection",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "EnabledSpecified": false,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 396,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_AutomaticCallback()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_AutomaticCallback",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_AutomaticCallback,
                                Item = new tMeta_Subscriber_AutomaticCallback()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    UsageSensitiveBilling = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.False,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_AutomaticCallback",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "UsageSensitiveBilling": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 1,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 399,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_PriorityCall()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_PriorityCall",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_PriorityCall,
                                Item = new tMeta_Subscriber_PriorityCall()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    NumberOfAnonymousNumbers = 0,
                                    NumberOfAnonymousNumbersSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_PriorityCall",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "EnabledSpecified": false,
            //              "NumberOfAnonymousNumbers": 0,
            //              "NumberOfAnonymousNumbersSpecified": true,
            //              "UsageSensitiveBilling": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 462,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallerIDPresentation()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallerIDPresentation",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallerIDPresentation,
                                Item = new tMeta_Subscriber_CallerIDPresentation()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.True,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    WithholdDirectoryNumber = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    NumberWithholdRejectionReason = new tMeta_Subscriber_CallerIDPresentation_NumberWithholdRejectionReason
                                    {
                                        Default = tMeta_Subscriber_CallerIDPresentation_NumberWithholdRejectionReason_Value.Blocked,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_CallerIDPresentation_NumberWithholdRejectionReason_Value.Blocked,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallerIDPresentation",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "WithholdNumberByDefault": null,
            //              "NumberWithholdRejectionReason": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "PresentNumberByDefault": null,
            //              "WithholdDirectoryNumber": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "AlwaysPresentNumberForIntercomCalls": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 417,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_MandatoryAccountCodes()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_MandatoryAccountCodes",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_MandatoryAccountCodes,
                                Item = new tMeta_Subscriber_MandatoryAccountCodes()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Variant = new tMeta_Subscriber_MandatoryAccountCodes_Variant
                                    {
                                        Default = tMeta_Subscriber_MandatoryAccountCodes_Variant_Value.Nonvalidated,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_MandatoryAccountCodes_Variant_Value.Nonvalidated,
                                        ValueSpecified = true
                                    },
                                    USCallTypes = new tMeta_Subscriber_MandatoryAccountCodes_USCallTypes
                                    {
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = new tMeta_Subscriber_MandatoryAccountCodes_USCallTypes_Value
                                        {
                                            Local = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            },
                                            Operator = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Premium = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Directory = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            LocalBusinessGroup = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            },
                                            OtherBusinessGroup = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.False
                                            },
                                            National = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Regional = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            CarrierDialed = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            International = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.True,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            }
                                        }
                                    },
                                    CodeLength = new tMeta_Subscriber_MandatoryAccountCodes_CodeLength
                                    {
                                        Default = 4,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = 4,
                                        ValueSpecified = true
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_MandatoryAccountCodes",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Variant": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "USCallTypes_Old": null,
            //              "USCallTypes": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": {
            //                  "Local": {
            //                    "Default": 1,
            //                    "DefaultSpecified": true,
            //                    "Value": 1
            //                  },
            //                  "Operator": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "Premium": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "Directory": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "LocalBusinessGroup": {
            //                    "Default": 1,
            //                    "DefaultSpecified": true,
            //                    "Value": 1
            //                  },
            //                  "OtherBusinessGroup": {
            //                    "Default": 1,
            //                    "DefaultSpecified": true,
            //                    "Value": 1
            //                  },
            //                  "National": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "Regional": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "CarrierDialed": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  },
            //                  "International": {
            //                    "Default": 0,
            //                    "DefaultSpecified": true,
            //                    "Value": 0
            //                  }
            //                }
            //              },
            //              "UKCallTypes": null,
            //              "InheritCodesAndCodeLengthSpecified": false,
            //              "CodeLength": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 4,
            //                "ValueSpecified": true,
            //                "Default": 4,
            //                "DefaultSpecified": true
            //              },
            //              "MaxIncorrectCodeAttemptsPerCall": null,
            //              "MaxIncorrectCodeAttempts": null,
            //              "BlockedSpecified": false,
            //              "ValidAccountCodes": null,
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 451,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_CallingNameAndNumberDeliveryOverIP()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallingNameAndNumberDeliveryOverIP",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallingNameAndNumberDeliveryOverIP,
                                Item = new tMeta_Subscriber_CallingNameAndNumberDeliveryOverIP()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    Destination = new tSwitchableDefaultString
                                    {
                                        Default = "Myrio Ap Server",
                                        UseDefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        Value = "Myrio Ap Server"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_CallingNameAndNumberDeliveryOverIP",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "Destination": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": "Myrio Ap Server",
            //                "Default": "Myrio Ap Server"
            //              },
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 419,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_WarmHotLine()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_WarmHotLine",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_WarmHotLine,
                                Item = new tMeta_Subscriber_WarmHotLine()
                                {
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    ServiceLevel = new tMeta_Subscriber_WarmHotLine_ServiceLevel
                                    {
                                        Default = tMeta_Subscriber_WarmHotLine_ServiceLevel_Value.WarmLine,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = tMeta_Subscriber_WarmHotLine_ServiceLevel_Value.WarmLine,
                                        ValueSpecified = true
                                    },
                                    Timeout = new tMeta_Subscriber_WarmHotLine_Timeout
                                    {
                                        Default = 16,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.True,
                                        UseDefaultSpecified = true,
                                        Value = 16,
                                        ValueSpecified = true
                                    },
                                    Enabled = tTrueFalse.True,
                                    EnabledSpecified = true,
                                    Number = "9995554444"
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json
            //json sent to add SelectiveCallForwarding
            //{
            //  "EquipmentId": 7,
            //  "Dn": "0123456795",
            //  "UserData": {
            //    "ShData": {
            //      "RepositoryData": {
            //        "ServiceIndication": "Meta_Subscriber_WarmHotLine",
            //        "SequenceNumber": 12345,
            //        "ServiceData": {
            //          "Item": {
            //            "Item": {
            //              "Subscribed": {
            //                "UseDefault": 1,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 1,
            //                "DefaultSpecified": true
            //              },
            //              "ServiceLevel": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 0,
            //                "ValueSpecified": true,
            //                "Default": 0,
            //                "DefaultSpecified": true
            //              },
            //              "Timeout": {
            //                "UseDefault": 0,
            //                "UseDefaultSpecified": true,
            //                "Value": 16,
            //                "ValueSpecified": true,
            //                "Default": 16,
            //                "DefaultSpecified": true
            //              },
            //              "Enabled": 0,
            //              "EnabledSpecified": true,
            //              "Number": "9995554444",
            //              "ActionSpecified": false
            //            },
            //            "ItemElementName": 486,
            //            "IgnoreSequenceNumber": 0,
            //            "MetaSwitchVersion": "9.0"
            //          }
            //        }
            //      }
            //    }
            //  }
            //}
            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }

        [TestMethod]
        public void Can_Add_Call_Barring()
        {
            var userData = new tUserData()
            {
                ShData = new tShData()
                {
                    RepositoryData = new tTransparentData()
                    {
                        SequenceNumber = 12345,
                        ServiceIndication = "Meta_Subscriber_CallBarring",
                        ServiceData = new tServiceData()
                        {
                            Item = new tMetaSwitchData()
                            {
                                MetaSwitchVersion = "9.0",
                                IgnoreSequenceNumber = tTrueFalse.True,
                                ItemElementName = ItemChoiceType.Meta_Subscriber_CallBarring,
                                Item = new tMeta_Subscriber_CallBarring()
                                {
                                    Action = tMeta_Subscriber_CallBarringAction.apply,
                                    ActionSpecified = true,
                                    Subscribed = new tSwitchableDefaultTrueFalse
                                    {
                                        Default = tTrueFalse.False,
                                        DefaultSpecified = true,
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = tTrueFalse.True,
                                        ValueSpecified = true
                                    },
                                    CurrentSubscriberBarredCallTypes = new tMeta_Subscriber_CallBarring_CurrentSubscriberBarredCallTypes
                                    {
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = new tMeta_Subscriber_CallBarring_CurrentSubscriberBarredCallTypes_Value
                                        {
                                            International = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            NationalAndMobile = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Local = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Operator = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            AccessCodes = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Premium = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            AccessCodesThatChangeConfiguration = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            DirectoryAssistance = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Mobile = null,
                                            National = null
                                        }
                                    },
                                    CurrentOperatorBarredCallTypes = new tMeta_Subscriber_CallBarring_CurrentOperatorBarredCallTypes
                                    {
                                        UseDefault = tTrueFalse.False,
                                        UseDefaultSpecified = true,
                                        Value = new tMeta_Subscriber_CallBarring_CurrentOperatorBarredCallTypes_Value
                                        {
                                            International = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            NationalAndMobile = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Local = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Operator = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            AccessCodes = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Premium = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            AccessCodesThatChangeConfiguration = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            DirectoryAssistance = new tSwitchableDefaultBitCombinationValue
                                            {
                                                Default = tTrueFalse.False,
                                                DefaultSpecified = true,
                                                Value = tTrueFalse.True
                                            },
                                            Mobile = null,
                                            National = null
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var update = new MetaSwitchUpdate
            {
                EquipmentId = 7,
                Dn = "0123456795",
                UserData = userData
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(update);

            #region Example Json

            #endregion

            var updateHttpResponse = _client.PutAsync("update/", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;
            var updateResponse = updateHttpResponse.Content.ReadAsStringAsync();
            var updateResult = updateResponse.Result;
        }
    }
}
