using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.Lib.Extensions;
using Common.Lib.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Test
{
    [TestClass]
    public class ApmaxFixture
    {
        //private IUnityContainer _container;
        private const string BaseRemoteUrl = "https://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/";
        private const string BaseLocalUrl = "https://localhost:56198/api/apmax/";
        private const string SubscriberNumber = "6055554444";
        private const int EquipmentId = 1;
        private WebRequestHandler _handler;
        private HttpClient _client;

        [TestInitialize]
        public void TestInitialize()
        {
            //_container = BootStrapper.Initialize();
            //_iProvisioningEngineService = _container.Resolve<IProvisioningEngineService>();
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
        public void Can_Retrieve_Equipment_Information()
        {
            var httpResponseMessage = _client.GetAsync("equipment/1").Result;
            var myResponse = httpResponseMessage.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var equipmentConnectionSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<Common.Lib.Domain.Common.Models.EquipmentConnectionSetting>(result);

            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsNotNull(equipmentConnectionSetting);
        }

        [TestMethod]
        public void Can_Retrieve_Subscriber_Version()
        {
            //version/service/{service}/equipment/{equipmentId}
            //http://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/version/service/Voicemail/equipment/1
            //http://localhost/ANDP.Provisioning.API.Rest/api/apmax/version/service/Voicemail/equipment/1

            var httpResponseMessage = _client.GetAsync("version/service/Subscriber/equipment/1").Result;
            var task = httpResponseMessage.Content.ReadAsStringAsync();
            var result = task.Result;
            var myResult = result.Replace("\"", "");
            var version = int.Parse(myResult);

            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsTrue(version > 0); //currently Subscriber is at version 4 on test system.
        }

        [TestMethod]
        public void Can_Retrieve_Voicemail_Version()
        {
            //http://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/version/service/Voicemail/equipment/1
            //http://localhost/ANDP.Provisioning.API.Rest/api/apmax/version/service/Voicemail/equipment/1

            var httpResponseMessage = _client.GetAsync("version/service/Voicemail/equipment/1").Result;
            var task = httpResponseMessage.Content.ReadAsStringAsync();
            var result = int.Parse(task.Result.Replace("\"", ""));

            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsTrue(result > 0); //currently voicemail is at version 7 on test system.
        }


        [TestMethod]
        public void Can_Retrieve_Iptv_Version()
        {
            //http://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/version/service/Iptv/equipment/1
            //http://localhost/ANDP.Provisioning.API.Rest/api/apmax/version/service/Iptv/equipment/1

            var httpResponseMessage = _client.GetAsync("version/service/Iptv/equipment/1").Result;
            var task = httpResponseMessage.Content.ReadAsStringAsync();
            var result = int.Parse(task.Result.Replace("\"", ""));

            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
            Assert.IsTrue(result > 0); //currently I'm recieving a -1 back
        }


        [TestMethod]
        public void Can_Retrieve_Subscriber_By_Phone_Number_And_Equipment_Id()
        {
            //http://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/subscriber/phone/6051231234/equipment/1
            //http://localhost/ANDP.Provisioning.API.Rest/api/apmax/subscriber/phone/6051231234/equipment/1

            HttpResponseMessage response = _client.GetAsync("subscriber/phone/6053511186/equipment/1").Result;
            var result = response.Content.ReadAsStringAsync().Result;

            SubscriberType subscriber = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscriberType>(result);

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("Nathan Test", subscriber.SubscriberName);
            Assert.AreEqual("6053511186", subscriber.SubscriberDefaultPhoneNumber);
            Assert.AreEqual("7dd60aa3-4552-4d03-95b6-78891a052f9c", subscriber.SubscriberGuid);
        }


        [TestMethod]
        public void Can_Create_Subscriber()
        {
            var provisioningSubscriberType = new ProvisioningAddUpdateSubscriber
            {
                EquipmentId = 1,
                SubscriberType = new SubscriberType
                {
                    SubscriberName = "Bob Bobby",
                    SubscriberDefaultPhoneNumber = "6478921546",
                    PlacementType = PlacementType.PlacementType_None,
                    BillingAccountNumber = "6478921546"
                }
            };

            //var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(provisioningSubscriberType);

            var jsonContent = provisioningSubscriberType.SerializeObjectToJsonString();

            HttpResponseMessage response = _client.PostAsync("subscriber/", new StringContent(jsonContent, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Can_Delete_Subscriber_By_Phone_Number()
        {
            //*** ARRANGE ***
            ////1. Make sure subscriber either already exists and if not create it.
            //HttpResponseMessage queryResponse = _client.GetAsync("subscriber/phone/5879954555/equipment/1").Result;
            //var queryReasonPhrase = queryResponse.ReasonPhrase;
            //var queryContent = queryResponse.Content.ReadAsStringAsync();
            //var queryResult = queryContent.Result;
            //Assert.IsTrue(queryResponse.IsSuccessStatusCode, "setup failed");

            //var subscriber = Newtonsoft.Json.JsonConvert.DeserializeObject<SubscriberType>(queryResult);

            //if (subscriber==null)
            //{
                //we don't have a subscriber to delete so we need to create one.

                var provisioningAddUpdateSubscriber = new ProvisioningAddUpdateSubscriber
                {
                    EquipmentId = 1,
                    SubscriberType = new SubscriberType
                    {
                        SubscriberName = "sdfdfadsfds sfd",
                        SubscriberDefaultPhoneNumber = "5879954555",
                        PlacementType = PlacementType.PlacementType_None,
                        BillingAccountNumber = "5879954555"
                    }
                };

                var jsonContent = provisioningAddUpdateSubscriber.SerializeObjectToJsonString();
                HttpResponseMessage addResponse = _client.PostAsync("subscriber/", new StringContent(jsonContent, Encoding.UTF8, "application/json")).Result;
                var addReasonPhrase = addResponse.ReasonPhrase;
                var addContent = addResponse.Content.ReadAsStringAsync();
                var addResult = addContent.Result;
                //Assert.IsTrue(addResponse.IsSuccessStatusCode, "setup failed");
            //}

            //*** ACT ***
            HttpResponseMessage response = _client.DeleteAsync("subscriber/phone/5879954555/equipment/1").Result;
            //var result = response.Content.ReadAsStringAsync();

            //*** ASSERT ***
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Can_Retrieve_All_Voicemail_Packages()
        {
            //voicemail/retrieveallvoicemailpackages/equipment/1

            HttpResponseMessage response = _client.GetAsync("voicemail/retrieveallvoicemailpackages/equipment/1").Result;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var packageType = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PackageType>>(result).ToList();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(packageType);
            Assert.IsTrue(packageType.Any());
            Assert.IsNotNull(packageType[0].Description);

            #region Voicemail packages on test system
            //[
            //  {
            //    "$id": "1",
            //    "Description": "ApConversionDefaultNone"
            //  },
            //  {
            //    "$id": "2",
            //    "Description": "Basic Bus"
            //  },
            //  {
            //    "$id": "3",
            //    "Description": "Silver"
            //  },
            //  {
            //    "$id": "4",
            //    "Description": "Gold"
            //  },
            //  {
            //    "$id": "5",
            //    "Description": "Apple VVM"
            //  },
            //  {
            //    "$id": "6",
            //    "Description": "Basic Res"
            //  },
            //  {
            //    "$id": "7",
            //    "Description": "Test Greg"
            //  },
            //  {
            //    "$id": "8",
            //    "Description": "AAwoRecording"
            //  },
            //  {
            //    "$id": "9",
            //    "Description": "Voicemail"
            //  },
            //  {
            //    "$id": "10",
            //    "Description": "GoldwithEmail greg"
            //  },
            //  {
            //    "$id": "11",
            //    "Description": "AAwRecording"
            //  },
            //  {
            //    "$id": "12",
            //    "Description": "Mobile Gold"
            //  },
            //  {
            //    "$id": "13",
            //    "Description": "Bronze"
            //  },
            //  {
            //    "$id": "14",
            //    "Description": "chrisp"
            //  }
            //]
            #endregion
        }

        [TestMethod]
        public void Can_Retrieve_All_Notification_Centers()
        {
            //voicemail/retrieveallnotificationcenters/equipment/{equipmentId}

            HttpResponseMessage response = _client.GetAsync("voicemail/retrieveallnotificationcenters/equipment/1").Result;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var notificationCenterInfoType = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<NotificationCenterInfoType>>(result).ToList();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(notificationCenterInfoType);
            Assert.IsTrue(notificationCenterInfoType.Any());
            Assert.IsNotNull(notificationCenterInfoType[0].Description);

            #region Notification Centers on test system
            //[
            //  {
            //    "$id": "1",
            //    "Description": "Shared"
            //  },
            //  {
            //    "$id": "2",
            //    "Description": "Unique"
            //  },
            //  {
            //    "$id": "3",
            //    "Description": "Menu"
            //  },
            //  {
            //    "$id": "4",
            //    "Description": "CS1500"
            //  },
            //  {
            //    "$id": "5",
            //    "Description": "CS1500 417776"
            //  },
            //  {
            //    "$id": "6",
            //    "Description": "email"
            //  },
            //  {
            //    "$id": "7",
            //    "Description": "DMS100 712999"
            //  },
            //  {
            //    "$id": "8",
            //    "Description": "DMS100 712943"
            //  },
            //  {
            //    "$id": "9",
            //    "Description": "Taqua"
            //  },
            //  {
            //    "$id": "10",
            //    "Description": "Phone"
            //  },
            //  {
            //    "$id": "11",
            //    "Description": "spatch"
            //  },
            //  {
            //    "$id": "12",
            //    "Description": "Test SMDI MWI"
            //  },
            //  {
            //    "$id": "13",
            //    "Description": "interop"
            //  },
            //  {
            //    "$id": "14",
            //    "Description": "High Path"
            //  },
            //  {
            //    "$id": "15",
            //    "Description": "DMS100 000000"
            //  },
            //  {
            //    "$id": "16",
            //    "Description": "Apple Push Notification service"
            //  },
            //  {
            //    "$id": "17",
            //    "Description": "Google Cloud Messaging Notification service"
            //  }
            //]
            #endregion
        }

        [TestMethod]
        public void Can_Retrieve_Voicemail_By_Phone_Number_And_Equipment_Id()
        {
            //http://andpserver.cloudapp.net/ANDP.Provisioning.API.Rest/api/apmax/voicemail/phone/6051231234/equipment/1
            //http://localhost/ANDP.Provisioning.API.Rest/api/apmax/voicemail/phone/6051231234/equipment/1

            HttpResponseMessage response = _client.GetAsync("voicemail/phone/6053511186/equipment/1").Result;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            var voicemailBoxType = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<VoiceMailBoxType>>(result).ToList();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsNotNull(voicemailBoxType);
        }

        [TestMethod]
        public void Can_Delete_Voicemail()
        {
            //voicemail/phone/{phoneNumber}/deletesubscriber/{deleteSubscriber}/equipment/{equipmentId}

            HttpResponseMessage response = _client.DeleteAsync("voicemail/phone/6053511186/deletesubscriber/true/equipment/1").Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);

        }

        [TestMethod]
        public void Can_Create_Voicemail()
        {
            ////Checking to see if voicemail exists before trying to create it.
            //HttpResponseMessage originalResponse = _client.GetAsync("voicemail/phone/6059991234/equipment/1").Result;
            //var myOriginalResponse = originalResponse.Content.ReadAsStringAsync();
            //var myOriginalresult = myOriginalResponse.Result;
            //var voicemailBoxType = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<VoiceMailBoxType>>(myOriginalresult).ToList();
            //Assert.IsTrue(originalResponse.IsSuccessStatusCode, "Setup failed");

            //if (voicemailBoxType.FirstOrDefault() != null)
            //{
            //got a mailbox back and need to delete it for setup reasons.
            HttpResponseMessage deleteResponse = _client.DeleteAsync("voicemail/phone/6059991234/deletesubscriber/true/equipment/1").Result;
            Assert.IsTrue(deleteResponse.IsSuccessStatusCode, "Setup failed");
            var deleteRasonPhrase = deleteResponse.ReasonPhrase;
            var deleteContent = deleteResponse.Content.ReadAsStringAsync();
            var deleteResult = deleteContent.Result;
            //}

            //mailBoxType of 0 = 'normal'

                var voicemail = new ProvisioningCreateVoicemail
                {
                    EquipmentId = 1,
                    Description = "my description",
                    MailBoxType = MailboxType.Normal,
                    NotificationCenterDescription = "spatch",
                    PhoneNumber = "6059991234",
                    SubscriberBillingAccountNumber = "987654321",
                    SubscriberName = "Test",
                    VoiceMailPackageName = "Basic Res"
                };

                var json = voicemail.SerializeObjectToJsonString();

                HttpResponseMessage response = _client.PostAsync("voicemail/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

                var reasonPhrase = response.ReasonPhrase;
                var myResponse = response.Content.ReadAsStringAsync();
                var result = myResponse.Result;

                Assert.IsTrue(response.IsSuccessStatusCode);


                //Delete voicemail user and subscriber now that we're done with it.
                HttpResponseMessage teardownDeleteResponse = _client.DeleteAsync("voicemail/phone/6059991234/deletesubscriber/true/equipment/1").Result;
                Assert.IsTrue(teardownDeleteResponse.IsSuccessStatusCode, "Teardown failed");
                var teardownDeleteReasonPhrase = teardownDeleteResponse.ReasonPhrase;
                var teardownDeleteContent = teardownDeleteResponse.Content.ReadAsStringAsync();
                var teardownDeleteResults = teardownDeleteContent.Result;
        }

        [TestMethod]
        public void Can_Create_SubMailBoxes()
        {
            //voicemail/phone/{phoneNumber}/digitfield/{digitField}/equipment/{equipmentId}"

            HttpResponseMessage response =
                _client.PostAsync("voicemail/phone/6053511186/digitfield/1/equipment/1", new StringContent("", Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }


        [TestMethod]
        public void Can_Update_Voicemail_Simple()
        {
            //voicemail/simple/"

            var provVoicemail = new ProvisioningUpdateVoicemailSimple
            {
                EquipmentId = 1,
                PhoneNumber = "6053511186",
                MaxMailBoxtime = 23,
                MaxMessages = 12
            };

            var json = provVoicemail.SerializeObjectToJsonString();

            HttpResponseMessage response =
                _client.PutAsync("voicemail/simple/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);  
        }

        [TestMethod]
        public void Can_Update_Voicemail_Full()
        {

            //voicemail/"

            var provVoicemail = new ProvisioningUpdateVoicemail
            {
                EquipmentId = 1,
                PhoneNumber = "6053511186",
                InternetAccessType = new InternetAccessType
                {
                    MobileEnabled = true,
                    Password = "mypassword",
                    ServiceEnabled = true,
                    UserName = "myusername"
                },
                VoiceMailBoxType = new VoiceMailBoxType
                {
                    Description = "mydescription",
                    MailBoxType = MailboxType.Normal,
                    MaxMailBoxTime = 60,
                    MaxMessageLength = 60,
                    Notifications = new List<NotificationInfoType>
                    {
                        new NotificationInfoType
                        {
                            Enabled = true,
                            Address = "6053511186",
                            Center = 2
                        },
                        new NotificationInfoType
                        {
                            Enabled = false,
                            Address = "6053511186",
                            Center = 9
                        }
                    },
                    MessageCount = 2
                }
            };

            var json = provVoicemail.SerializeObjectToJsonString();

            HttpResponseMessage response = _client.PutAsync("voicemail/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);  

            //NotificationInfoType Center notes:
            //typeNone 0  
            //typeSerial 1  
            //typeSs7 2  
            //typeSmpp 3  
            //typeSip 4  
            //typeSipDirect 5  
            //typePager 6  
            //typePagerGroup 7  
            //typeScreenPop 8  
            //typeEmail 9  
            //typePcApplication 10  
            //typeVideoMwi 11  
            //typeLast 12 
        }

        [TestMethod]
        public void Can_Update_Voicemail_Box_Package()
        {
            //voicemail/phone/{phoneNumber}/packagename/{vmPackageName}/equipment/{equipmentId}

            HttpResponseMessage response = _client.PutAsync("voicemail/phone/6053511186/packagename/Gold/equipment/1", new StringContent("", Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);  
        }

        [TestMethod]
        public void Can_Add_Voice_Mail_Box_Internet_Access()
        {
            //voicemail/internetaccess/phone/{phoneNumber}/emailAddress/{emailAddress}/internetpassword/{internetPassword}/internetusername/{internetUserName}equipment/{equipmentId}

            HttpResponseMessage response = _client.PostAsync("voicemail/internetaccess/phone/6053511186/emailAddress/bob@bob.com/internetpassword/bobspassword/internetusername/bobsusername/equipment/1", 
                new StringContent("", Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Can_Delete_Voice_Mail_Box_Internet_Access()
        {
            //voicemail/internetaccess/phone/{phoneNumber}/equipment/{equipmentId}

            HttpResponseMessage response = _client.DeleteAsync("voicemail/internetaccess/phone/6053511186/equipment/1").Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Can_Reassign_VM_Box_Number()
        {
            //voicemail/reassignVmBoxNumber/

            var reassignVoicemail = new ProvisioningReassignVoicemail
            {
                EquipmentId = 1,
                OldPhoneNumber = "6053511186",
                NewPhoneNumber = "6053511187",
                DeleteOldSubscriber = false,
                InternetAccess = true,
                InternetUserName = "bobsusername",
                InternetPassword = "internetusername",
                MailBoxDescription = "my mail box description"
            };

            var json = reassignVoicemail.SerializeObjectToJsonString();

            HttpResponseMessage response = _client.PutAsync("voicemail/reassignVmBoxNumber/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

           Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public void Can_Reset_VM_Password()
        {
            //voicemail/phone/{phoneNumber}/newpin/{newPin}/equipment/{equipmentId}

            HttpResponseMessage response = _client.PutAsync("voicemail/phone/6053511186/newpin/1234/equipment/1", new StringContent("", Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;
        }

        [TestMethod]
        public void Can_Create_IPTV()
        {
            //iptv/

            ProvisioningSetIptvAccount account = new ProvisioningSetIptvAccount
            {
                EquipmentId = 1,
                IptvAccountType = new IptvAccountType
                {
                    AccountDescription = "QSS test account",
                    Active = true,
                    AdultChannelsState = AdultChannelState.useDefault,
                    ChannelPackageTypes = new List<ChannelPackageType>
                    {
                        new ChannelPackageType
                        {
                            PackageId = "10640a2f-67fd-497b-aae0-272504a5fb76",
                            PackageName = "Ultra"
                        },

                        new ChannelPackageType
                        {
                            PackageId = "74cfa799-fce7-4e8f-908e-2bb545638dd7",
                            PackageName = "SRTV WFL"
                        }
                    },
                    CurrentAmountCharged = 0,
                    DeactivateReason = "",
                    FipsCountyCode = 155,
                    FipsStateCode = 48,
                    MaxBandwidthKbs = 0,
                    MaxChargingLimit = -1,
                    ServiceAreaId = "a00d0d86-acac-48a3-b13f-8281dab95df8",
                    ServiceReference = "107715",
                    RatingPin = "",
                    PurchasePin = ""
                },
                SubscriberType = new SubscriberType()
                {
                    SubscriberName = "Bob Bobby",
                    SubscriberDefaultPhoneNumber = "6059991234",
                    PlacementType = PlacementType.PlacementType_None,
                    BillingAccountNumber = "6059991234"
                }
            };


            var json = account.SerializeObjectToJsonString();

            HttpResponseMessage response = _client.PutAsync("iptv/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

        }


        [TestMethod]
        public void Can_DeleteIptvAccountByServiceReference_Iptv()
        {
            //DeleteIptvAccountByServiceReference
            //iptv/servicereference/{serviceReference}/force/{force}/equipment/{equipmentId}
            HttpResponseMessage response = _client.PutAsync("iptv/servicereference/mycustomservicereference/false/equipment/3", new StringContent("", Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;
        }

        [TestMethod]
        public void Can_Disable_Iptv()
        {
            ProvisioningDisableIptvAccount foo = new ProvisioningDisableIptvAccount
            {
                EquipmentId = 1,
                ServiceReference = "6059991234",
                SubAddress = "6059991234"
            };

            var json = foo.SerializeObjectToJsonString();

            //HttpResponseMessage response = _client.PutAsync("iptv/disable/", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            //var reasonPhrase = response.ReasonPhrase;
            //var myResponse = response.Content.ReadAsStringAsync();
            //var result = myResponse.Result;

        }

        [TestMethod]
        public void Can_Create_Screen_Pop()
        {
            var provisioningScreenPop = new ProvisioningCreateScreenPop
            {
                EquipmentId = 1,
                Description = "My Description",
                NpaNxx = "605999",
                ScreenPopSubscriberType = new ScreenPopSubscriberType
                {
                    ScreenPopEnabled = true,
                    MacAddresses = new List<string>
                    {
                        ""
                    },
                    SubscriberPhoneNumber = "6059996666"
                }
            };

            var json = provisioningScreenPop.SerializeObjectToJsonString();
        }

        [TestMethod]
        public void Can_Add_Caller_Name()
        {
            var provisioningAddCallerName = new ProvisioningAddCallerName
            {
                EquipmentId = 1,
                CallerName = "Bob",
                PhoneNumber = "6059996666",
                Presentation = "MyPresentation"
            };

            var json = provisioningAddCallerName.SerializeObjectToJsonString();
        }

        [TestMethod]
        public void Can_Reassign_Caller_Name()
        {
            var provisioningAddCallerName = new ProvisioningReassignCallerName
            {
                EquipmentId = 1,
                OldPhoneNumber = "6059996666",
                NewPhoneNumber = "6059997777"
            };

            var json = provisioningAddCallerName.SerializeObjectToJsonString();
        }

        [TestMethod]
        public void Can_Suspend_IPTV()
        {
            var provisioningSuspendIptvAccount = new ProvisioningSuspendIptvAccount()
            {
                EquipmentId = 1,
                ServiceReference = "ssssssssss",
                SubscriberId = "ssssssssss",
                Suspend = true,
                SuspendReason = "myreason"
            };

            var json = provisioningSuspendIptvAccount.SerializeObjectToJsonString();

            //{
            //  "EquipmentId": 1,
            //  "ServiceReference": "ssssssssss",
            //  "SubscriberId": "ssssssssss",
            //  "Suspend": true,
            //  "SuspendReason": "myreason"
            //}
        }

        [TestMethod]
        public void Can_Suspend_Voicemail()
        {
            var provisioningDisableVoicemail = new ProvisioningDisableVoicemail()
            {
                EquipmentId = 1,
                Disable = true,
                PhoneNumber = "9999999999"
            };

            var json = provisioningDisableVoicemail.SerializeObjectToJsonString();

            //{
            //  "Disable": true,
            //  "EquipmentId": 1,
            //  "PhoneNumber": "9999999999"
            //}
        }


        [TestMethod]
        public void Can_Update_Channel_Packages_iptv()
        {
            //*** ARRANGE ***
            var provisioningDisableVoicemail = new ProvisioningSetIptvChannelPackageList()
            {
                EquipmentId = 6,
                ServiceReference = "ServiceReference",
                ChannelPackageTypes = new List<ChannelPackageType>
                {
                    new ChannelPackageType
                    {
                        PackageId = "PackageId1",
                        PackageName = "PackageName1"
                    },
                    new ChannelPackageType
                    {
                        PackageId = "PackageId2",
                        PackageName = "PackageName2"
                    }
                }
            };

            var json = provisioningDisableVoicemail.SerializeObjectToJsonString();
        }

        [TestMethod]
        public void Can_Retrieve_Subscriber_By_Phone_Number()
        {
            //*** ARRANGE ***

            //*** ACT ***
            var response = _client.GetAsync("subscriber/phone/716/equipment/6").Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** ASSERT ***
            Assert.IsTrue(response.IsSuccessStatusCode);  
        }
        


    }
}
