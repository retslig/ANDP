using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ANDP.Lib.Domain.Models;
using Common.Lib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Attribute = ANDP.Lib.Domain.Models.Attribute;

namespace ANDP.API.Rest.Test.Controllers
{
    [TestClass]
    public class OrderControllerFixture
    {


        private const string BaseRemoteUrl = "http://andpserver.cloudapp.net/ANDP.API.Rest.v3000/";
        private const string BaseLocalUrl = "http://localhost:56198/api/order/";
        private HttpClient _client;


        [TestInitialize]
        public void TestInitialize()
        {
            //_container = BootStrapper.Initialize();
            //_iProvisioningEngineService = _container.Resolve<IProvisioningEngineService>();
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseRemoteUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //making sure to accept json
        }

        [TestMethod] //currently bhs is using this for emp.
        public void Create_Basic_Phone_Order()
        {
            //*** Arrange ***
            var order = new Order
            {
                Account = new Account
                {
                    Name = "",
                    CompanyId = 1,
                    ExternalAccountId = "myExternalAccountId",
                    Contact = new Contact
                    {
                        Address = null,
                        FirstName = "billy",
                        LastName = "bob",
                        Corporation = "myCorp",
                        PhoneNumber = "9374843002",
                        Type = "mytype"
                    },
                    StatusType = StatusType.Pending,
                    CreatedByUser = "bob",
                    ModifiedByUser = "bob"
                },
                ActionType = ActionType.Add,
                CSR = null,
                ClassOfService = "myclass",
                CompletionDate = DateTime.Now,
                Configuration = "myConfiguration",
                CreatedByUser = "bob",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ExternalAccountId = "12345",
                ExternalCompanyId = "12345",
                ExternalOrderId = "123245",
                Id = 0,
                Log = null,
                ModifiedByUser = "bob",
                NewNetCaseId = null,
                NewNetProcessId = null,
                NewNetRouteIndex = null,
                NewNetTaskId = null,
                NewNetTriggerId = null,
                NewNetUserId = null,
                OrginatingIp = null,
                Priority = 1,
                Product = null,
                ProvisionDate = DateTime.Now.AddYears(100),
                ResponseSent = false,
                ResultMessage = null,
                ServiceProvider = null,
                Services = new List<Service>
                {
                    new Service
                    {
                        ActionType = ActionType.Add,
                        CompletionDate = null,
                        CreatedByUser = "bob",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = "ExternalServiceId",
                        Id = 0,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "",
                                    StreetLine1 = "435 E 6th St",
                                    StreetLine2 = null,
                                    Municipality = "Sioux Falls",
                                    SubAdministrativeArea = "SD",
                                    PostalCode = "57107"
                                },
                                CountyJurisdiction = null,
                                DistrictJurisdiction = null,
                                ExternalLocationId = null,
                                LocationInfo = null,
                                MsaGesn = 0,
                                MsagExchange = null,
                                TaxArea = null,
                                ValidationErrors = null,
                                InternetItems = new List<InternetItem>
                                {
                                    new InternetItem
                                    {
                                        ActionType = ActionType.Add,
                                        CompletionDate = null,
                                        CreatedByUser = "bob",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = "876543456",
                                        Id = 0,
                                        Log = null,
                                        ModifiedByUser = "bob",
                                        ServiceId = 0,
                                        ResultMessage = null,
                                        StartDate = DateTime.Now,
                                        ValidationErrors = null,
                                        Version = 0,
                                        Xml = null,
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        StatusType = StatusType.Pending,
                                        Plant = new SerializableDictionary<string, object>
                                        {
                                            {"CentralOffice", "C7Calix"},
                                            {"TransportType", "GPON"},
                                            {"OntNumber", 5499100},
                                            {"Coid", "RSVL"},
                                            {
                                                "Ports", new List<object>
                                                {
                                                    new SerializableDictionary<string, object>
                                                    {
                                                        {"Name", ""},
                                                        {"Number", 5},
                                                        {"Status", ""},
                                                        {"DataServiceNumber", 1},
                                                        {"Type", "Ont"}
                                                    }
                                                }
                                            }
                                        },
                                        ProvisionSequence = 1
                                    }
                                },
                                //PhoneItems = new List<PhoneItem>
                                //{
                                //    new PhoneItem
                                //    {
                                //        ActionType = ActionType.Add,
                                //        CompletionDate = null,
                                //        CreatedByUser = "bob",
                                //        DateCreated = DateTime.Now,
                                //        DateModified = DateTime.Now,
                                //        Directory = null,
                                //        ExternalItemId = "876543456",
                                //        Id = 0,
                                //        Log = null,
                                //        ModifiedByUser = "bob",
                                //        ServiceId = 0,
                                //        ResultMessage = null,
                                //        StartDate = DateTime.Now,
                                //        ValidationErrors = null,
                                //        Version = 0,
                                //        Xml = null,
                                //        Priority = 1,
                                //        ProvisionDate = DateTime.Now,
                                //        StatusType = StatusType.Pending,
                                //        Plant = new PhonePlant
                                //        {
                                //            AccessDevice = new AccessDevice
                                //            {
                                //                AccessLine = 491,
                                //                Name = "C024"
                                //            }
                                //        },
                                //        LineInformation = new LineInformation
                                //        {
                                //            ActionType = ActionType.Unchanged,
                                //            PhoneNumber  = "9374843002",
                                //            //OldPhoneNumber = "9374843000",
                                //            CallerName = "Bob",
                                //            PrivacyIndicator = "",
                                //            InterLataPic = new InterLataPic
                                //            {
                                //                ActionType = ActionType.Unchanged,
                                //                Cic = "123",
                                //                PicEffectiveDate = DateTime.Now
                                //            },
                                //            IntraLataPic = new IntraLataPic
                                //            {
                                //                ActionType = ActionType.Unchanged,
                                //                Cic = "123",
                                //               PicEffectiveDate = DateTime.Now
                                //            },
                                //            InterNationalPic = new InterNationalPic
                                //            {
                                //                ActionType = ActionType.Unchanged,
                                //                Cic = "123",
                                //                PicEffectiveDate = DateTime.Now
                                //            }
                                //        },
                                //        ProvisionSequence = 1
                                //    }
                                //}
                            }
                        },
                        Log = null,
                        ModifiedByUser = "bhs",
                        OrderId = 0,
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        ResultMessage = null,
                        StartDate = DateTime.Now,
                        StatusType = StatusType.Pending,
                        ValidationErrors = null,
                        Version = 0,
                        Xml = null
                    }
                },
                StartDate = DateTime.Now,
                StatusType = StatusType.Pending,
                ValidationErrors = null,
                Version = 0,
                Xml = null
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(order);

            //*** Act ***
            var response =
                _client.PostAsync("", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            //Assert.IsNotNull(json);
        }

        [TestMethod]
        public void Create_CTC_Order()
        {
            //*** Arrange ***
            var order = new Order
            {
                Account = new Account
                {
                    Name = "",
                    CompanyId = 1,
                    ExternalAccountId = "myExternalAccountId",
                    Contact = new Contact
                    {
                        Address = null,
                        FirstName = "billy",
                        LastName = "bob",
                        Corporation = "myCorp",
                        PhoneNumber = "9374843002",
                        Type = "mytype"
                    },
                    StatusType = StatusType.Pending,
                    CreatedByUser = "bob",
                    ModifiedByUser = "bob"
                },
                ActionType = ActionType.SuspendNonPay,
                CSR = null,
                ClassOfService = "myclass",
                CompletionDate = DateTime.Now,
                Configuration = "myConfiguration",
                CreatedByUser = "bob",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ExternalAccountId = "s6767d96s78d",
                ExternalCompanyId = "srv081",
                ExternalOrderId = "4664984964678",
                Id = 0,
                Log = null,
                ModifiedByUser = "bob",
                NewNetCaseId = null,
                NewNetProcessId = null,
                NewNetRouteIndex = null,
                NewNetTaskId = null,
                NewNetTriggerId = null,
                NewNetUserId = null,
                OrginatingIp = null,
                Priority = 1,
                Product = null,
                ProvisionDate = DateTime.Now,
                ResponseSent = false,
                ResultMessage = null,
                ServiceProvider = null,
                Services = new List<Service>
                {
                    new Service
                    {
                        ActionType = ActionType.SuspendNonPay,
                        CompletionDate = null,
                        CreatedByUser = "bob",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = "ExternalServiceId",
                        Id = 0,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "",
                                    StreetLine1 = "435 E 6th St",
                                    StreetLine2 = null,
                                    Municipality = "Sioux Falls",
                                    SubAdministrativeArea = "SD",
                                    PostalCode = "57107"
                                },
                                CountyJurisdiction = null,
                                DistrictJurisdiction = null,
                                ExternalLocationId = null,
                                LocationInfo = null,
                                MsaGesn = 0,
                                MsagExchange = null,
                                TaxArea = null,
                                ValidationErrors = null,
                                InternetItems = new List<InternetItem>
                                {
                                    new InternetItem
                                    {
                                        ActionType = ActionType.SuspendNonPay,
                                        CompletionDate = null,
                                        CreatedByUser = "bob",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = "123456587",
                                        Id = 0,
                                        Log = null,
                                        ModifiedByUser = "bob",
                                        ServiceId = 0,
                                        ResultMessage = null,
                                        StartDate = DateTime.Now,
                                        ValidationErrors = null,
                                        Version = 0,
                                        Xml = null,
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        StatusType = StatusType.Pending,
                                        ProvisionSequence = 1,
                                        Users = new List<InternetUser>
                                        {
                                            new InternetUser
                                            {
                                                ActionType = ActionType.Unchanged,
                                                Domain = null,
                                                EmailAddress = null,
                                                Password = "BobsUserPassword",
                                                Type = null,
                                                UserName = "BobsUserUserName",
                                                FirstName = "Bob",
                                                LastName = "BobsLastName"
                                            }
                                        },
                                        PrimaryUser = new InternetUser
                                        {
                                            ActionType = ActionType.Unchanged,
                                            Domain = null,
                                            EmailAddress = null,
                                            Password = "BobsPassword",
                                            Type = null,
                                            UserName = "BobsUserName"
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                Attributes = new List<Attribute>
                                                {
                                                    new Attribute
                                                    {
                                                        Name = "Quotaa",
                                                        Value1 = "1"
                                                    }
                                                },
                                                Code = "8660",
                                                //DeactivationDate = null,
                                                Description = "NeoNova_WebDrive"
                                            },
                                            //new Feature
                                            //{
                                            //    ActionType = ActionType.Add,
                                            //    ActivationDate = DateTime.Now,
                                            //    Attributes = new List<Attribute>
                                            //    {
                                            //        new Attribute
                                            //        {
                                            //            Name = "Quota",
                                            //            Value1 = "2"
                                            //        }
                                            //    },
                                            //    Code = "8660",
                                            //    //DeactivationDate = null,
                                            //    Description = "NeoNova_WebDrive"
                                            //},
                                            //new Feature
                                            //{
                                            //    ActionType = ActionType.Delete,
                                            //    ActivationDate = DateTime.Now,
                                            //    //Attributes = null,
                                            //    Code = "8660",
                                            //    //DeactivationDate = null,
                                            //    Description = "NeoNova_WebDrive"
                                            //}
                                        }
                                    }
                                },
                                PhoneItems = new List<PhoneItem>
                                {
                                    new PhoneItem
                                    {
                                        ActionType = ActionType.SuspendNonPay,
                                        CompletionDate = null,
                                        CreatedByUser = "bob",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        Directory = null,
                                        ExternalItemId = "876543456",
                                        Id = 0,
                                        Log = null,
                                        ModifiedByUser = "bob",
                                        ServiceId = 0,
                                        ResultMessage = null,
                                        StartDate = DateTime.Now,
                                        ValidationErrors = null,
                                        Version = 0,
                                        Xml = null,
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        StatusType = StatusType.Pending,
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "CMVM",
                                                //DeactivationDate = null,
                                                Description = "voicemail"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4911",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallWaitingWithCallerID"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4887",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallTrace"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4844",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AnonymousCallRejection"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4839",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_PriorityCall"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4833",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_DoNotDisturb"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4821",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SelectiveCallRejection"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4809",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AutomaticRecall"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4803",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AutomaticCallback"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4800",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallerIDPresentation"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4410",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_RemoteAccessToCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4148",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_MandatoryAccountCodes"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "9999",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallingNameDelivery"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4861",
                                                //DeactivationDate = null,
                                                Description = "CallingNumberDelivery"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4100",
                                                //DeactivationDate = null,
                                                Description = "UnconditionalCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4140",
                                                //DeactivationDate = null,
                                                Description = "CallWaiting"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4103",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallTransfer"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4110",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_3WayCalling"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "AddSpeedValue", Value1 = "8"}
                                                    },
                                                Code = "4120",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SpeedCalling"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "ForwardingNumber", Value1 = "9374843002"}
                                                    },
                                                Code = "4101",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_DelayedCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "ForwardingNumber", Value1 = "9374843002"}
                                                    },
                                                Code = "4102",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_BusyCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4827",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SelectiveCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Unchanged,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4886",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallingNameAndNumberDeliveryOverIP"
                                            },
                                        },
                                        //Equipments = new List<Equipment>
                                        //{
                                        //    new Equipment
                                        //    {
                                        //        CreatedByUser = "bob",
                                        //        ModifiedByUser = "bob",
                                        //        StatusType = StatusType.Pending,
                                        //        ActionType = ActionType.Add,
                                        //        ExternalEquipmentId = "6s54df6ds47fd",
                                        //        EquipmentSetupId = 10
                                        //    }
                                        //},
                                        //OldPlant = new JObject
                                        //{
                                        //    {
                                        //        "AccessDevice", new JObject
                                        //        {
                                        //            {"AccessLine", 484},
                                        //            {"Name", "C024"}
                                        //        }
                                        //    }
                                        //},
                                        //Plant = new JObject
                                        //{
                                        //    {
                                        //        "AccessDevice", new JObject
                                        //        {
                                        //            {"AccessLine", 484},
                                        //            {"Name", "C024"}
                                        //        }
                                        //    }
                                        //},
                                        Plant = new SerializableDictionary<string, object>()
                                        {
                                            {
                                                "AccessDevice", new
                                                {
                                                    AccessLine = 484,
                                                    Name = "C024"
                                                }
                                            }
                                        },
                                        //Plant = new JObject
                                        //{
                                        //    { "Cpu", "Intel" },
                                        //    { "Memory", 32 },
                                        //    {
                                        //        "AccessDevice", new JArray
                                        //        {
                                        //            "DVD",
                                        //            "SSD"
                                        //        }
                                        //    }
                                        //},
                                        //OldPlant = new PhonePlant
                                        //{
                                        //    AccessDevice = new AccessDevice
                                        //    {
                                        //        AccessLine = 484,
                                        //        Name = "C024"
                                        //    }
                                        //},
                                        //Plant = new PhonePlant
                                        //{
                                        //    AccessDevice = new AccessDevice
                                        //    {
                                        //        AccessLine = 491,
                                        //        Name = "C024"
                                        //    }
                                        //},
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Unchanged,
                                            PhoneNumber = "9374843002",
                                            //OldPhoneNumber = "9374843000",
                                            CallerName = "Bob",
                                            PrivacyIndicator = "",
                                            InterLataPic = new InterLataPic
                                            {
                                                ActionType = ActionType.Unchanged,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            },
                                            IntraLataPic = new IntraLataPic
                                            {
                                                ActionType = ActionType.Unchanged,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            },
                                            InterNationalPic = new InterNationalPic
                                            {
                                                ActionType = ActionType.Unchanged,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            }
                                        },
                                        ProvisionSequence = 1
                                    }
                                }
                            }
                        },
                        Log = null,
                        ModifiedByUser = "bhs",
                        OrderId = 0,
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        ResultMessage = null,
                        StartDate = DateTime.Now,
                        StatusType = StatusType.Pending,
                        ValidationErrors = null,
                        Version = 0,
                        Xml = null
                    }
                },
                StartDate = DateTime.Now,
                StatusType = StatusType.Pending,
                ValidationErrors = null,
                Version = 0,
                Xml = null
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(order);

            //*** Act ***
            var response =
                _client.PostAsync("", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            //Assert.IsNotNull(json);

        }

        [TestMethod]
        public void Create_EMP_Add_Phone_Order()
        {
            //*** Arrange ***
            var order = new Order
            {
                Account = new Account
                {
                    Name = "",
                    CompanyId = 1,
                    ExternalAccountId = "104494",
                    Contact = new Contact
                    {
                        Address = null,
                        FirstName = "billy",
                        LastName = "bob",
                        Corporation = "myCorp",
                        PhoneNumber = "6075225757",
                        Type = "mytype"
                    },
                    StatusType = StatusType.Pending,
                    CreatedByUser = "bob",
                    ModifiedByUser = "bob"
                },
                ActionType = ActionType.Add,
                CSR = null,
                ClassOfService = "R1",
                CompletionDate = DateTime.Now,
                Configuration = "myConfiguration",
                CreatedByUser = "bob",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ExternalAccountId = "104494",
                ExternalCompanyId = "SRV",
                ExternalOrderId = "4664984964678",
                Id = 0,
                Log = null,
                ModifiedByUser = "bob",
                NewNetCaseId = null,
                NewNetProcessId = null,
                NewNetRouteIndex = null,
                NewNetTaskId = null,
                NewNetTriggerId = null,
                NewNetUserId = null,
                OrginatingIp = null,
                Priority = 1,
                Product = null,
                ProvisionDate = DateTime.Now,
                ResponseSent = false,
                ResultMessage = null,
                ServiceProvider = null,
                Services = new List<Service>
                {
                    new Service
                    {
                        ActionType = ActionType.Add,
                        CompletionDate = null,
                        CreatedByUser = "bob",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = "ExternalServiceId",
                        Id = 0,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "",
                                    StreetLine1 = "435 E 6th St",
                                    StreetLine2 = null,
                                    Municipality = "Sioux Falls",
                                    SubAdministrativeArea = "SD",
                                    PostalCode = "57107"
                                },
                                CountyJurisdiction = null,
                                DistrictJurisdiction = null,
                                ExternalLocationId = null,
                                LocationInfo = null,
                                MsaGesn = 0,
                                MsagExchange = null,
                                TaxArea = null,
                                ValidationErrors = null,

                                PhoneItems = new List<PhoneItem>
                                {
                                    new PhoneItem
                                    {
                                        ActionType = ActionType.Add,
                                        CompletionDate = null,
                                        CreatedByUser = "bob",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        Directory = null,
                                        ExternalItemId = "876543456",
                                        Id = 0,
                                        Log = null,
                                        ModifiedByUser = "bob",
                                        ServiceId = 0,
                                        ResultMessage = null,
                                        StartDate = DateTime.Now,
                                        ValidationErrors = null,
                                        Version = 0,
                                        Xml = null,
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        StatusType = StatusType.Pending,
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "CMVM",
                                                //DeactivationDate = null,
                                                Description = "voicemail"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4911",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallWaitingWithCallerID"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4887",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallTrace"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4844",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AnonymousCallRejection"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4839",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_PriorityCall"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4833",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_DoNotDisturb"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4821",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SelectiveCallRejection"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4809",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AutomaticRecall"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4803",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_AutomaticCallback"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4800",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallerIDPresentation"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4410",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_RemoteAccessToCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4148",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_MandatoryAccountCodes"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "9999",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallingNameDelivery"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4861",
                                                //DeactivationDate = null,
                                                Description = "CallingNumberDelivery"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4100",
                                                //DeactivationDate = null,
                                                Description = "UnconditionalCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4140",
                                                //DeactivationDate = null,
                                                Description = "CallWaiting"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4103",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallTransfer"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4110",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_3WayCalling"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "AddSpeedValue", Value1 = "8"}
                                                    },
                                                Code = "4120",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SpeedCalling"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "ForwardingNumber", Value1 = "9374843002"}
                                                    },
                                                Code = "4101",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_DelayedCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                Attributes =
                                                    new List<Attribute>
                                                    {
                                                        new Attribute {Name = "ForwardingNumber", Value1 = "9374843002"}
                                                    },
                                                Code = "4102",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_BusyCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4827",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_SelectiveCallForwarding"
                                            },
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                ActivationDate = DateTime.Now,
                                                //Attributes = null,
                                                Code = "4886",
                                                //DeactivationDate = null,
                                                Description = "Meta_Subscriber_CallingNameAndNumberDeliveryOverIP"
                                            },
                                        },
                                        Plant = new SerializableDictionary<string, object>()
                                        {
                                            {
                                                "AccessDevice", new
                                                {
                                                    AccessLine = 484,
                                                    Name = "C024"
                                                }
                                            }
                                        },
                                        //Plant = new PhonePlant
                                        //{
                                        //    LineCard = new LineCard
                                        //    {
                                        //         = ,
                                        //         = "",
                                        //        Line = "",
                                        //         = "06",
                                        //         = "1",
                                        //         = "01"
                                        //    }
                                        //},
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Add,
                                            PhoneNumber = "6075225757",
                                            //OldPhoneNumber = "9374843000",
                                            CallerName = "Bob",
                                            PrivacyIndicator = "",
                                            InterLataPic = new InterLataPic
                                            {
                                                ActionType = ActionType.Add,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            },
                                            IntraLataPic = new IntraLataPic
                                            {
                                                ActionType = ActionType.Add,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            },
                                            InterNationalPic = new InterNationalPic
                                            {
                                                ActionType = ActionType.Add,
                                                Cic = "123",
                                                PicEffectiveDate = DateTime.Now
                                            }
                                        },
                                        ProvisionSequence = 1
                                    }
                                }
                            }
                        },
                        Log = null,
                        ModifiedByUser = "bhs",
                        OrderId = 0,
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        ResultMessage = null,
                        StartDate = DateTime.Now,
                        StatusType = StatusType.Pending,
                        ValidationErrors = null,
                        Version = 0,
                        Xml = null
                    }
                },
                StartDate = DateTime.Now,
                StatusType = StatusType.Pending,
                ValidationErrors = null,
                Version = 0,
                Xml = null
            };

            var updateJson = Newtonsoft.Json.JsonConvert.SerializeObject(order);

            //*** Act ***
            var response =
                _client.PostAsync("", new StringContent(updateJson, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            //Assert.IsNotNull(json);

        }

        [TestMethod]
        public void Create_Order_For_Dynamic_Plant_With_Arrays()
        {
            var json = "{\r\n  \"$id\": \"1\",\r\n  \"Id\": 10,\r\n  \"ExternalOrderId\": \"1245\",\r\n  \"ExternalAccountId\": \"96000666\",\r\n  \"ExternalCompanyId\": \"srv\",\r\n  \"Priority\": 3,\r\n  \"Xml\": \"<Order><Id>41</Id><ExternalOrderId>1245</ExternalOrderId><ExternalAccountId>96000666</ExternalAccountId><ExternalCompanyId>srv</ExternalCompanyId><Priority>3</Priority><Xml /><ProvisionDate>2016-02-12T00:00:00</ProvisionDate><StatusType>Pending</StatusType><ActionType>Add</ActionType><ResultMessage /><Log /><ResponseSent>false</ResponseSent><CompletionDate xmlns:p2=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p2:nil=\\\"true\\\" /><StartDate xmlns:p2=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p2:nil=\\\"true\\\" /><ServiceProvider /><Configuration /><Product /><ClassOfService /><CSR>EMPSMALLWOOD</CSR><Notes>INTERNET INSTALL</Notes><CreatedByUser>newnet_api_user</CreatedByUser><ModifiedByUser>newnet_api_user</ModifiedByUser><DateCreated>2016-02-05T17:05:56.0742793</DateCreated><DateModified>2016-02-10T12:08:58.2599534</DateModified><Version>26</Version><Account><Id>0</Id><Name>DONNA WILLIAMS</Name><MasterPhone>5705290568</MasterPhone><ExternalAccountId>96000666</ExternalAccountId><CompanyId>0</CompanyId><Type>R</Type><StatusType>Pending</StatusType><DateCreated>0001-01-01T00:00:00</DateCreated><DateModified>0001-01-01T00:00:00</DateModified><Version>0</Version><Contact><Type /><FirstName /><LastName /><Corporation /><PhoneNumber /><Address><Id>0</Id><Attention>DONNA WILLIAMS</Attention><StreetLine1>232  E MAIN ST</StreetLine1><StreetLine2 /><Municipality>TROY</Municipality><AdministrativeArea>PA</AdministrativeArea><SubAdministrativeArea>TROY BORO</SubAdministrativeArea><PostalCode>16947</PostalCode><Country /><CreatedById>0</CreatedById><ModifiedById>0</ModifiedById><DateCreated>0001-01-01T00:00:00</DateCreated><DateModified>0001-01-01T00:00:00</DateModified><Version>0</Version></Address></Contact><ValidationErrors /></Account><Services><Service><Locations><Location><ExternalLocationId>1245</ExternalLocationId><Address><Id>0</Id><Attention>DONNA WILLIAMS</Attention><StreetLine1>232  E MAIN ST</StreetLine1><StreetLine2 /><Municipality>TROY</Municipality><AdministrativeArea>PA</AdministrativeArea><SubAdministrativeArea>TROY BORO</SubAdministrativeArea><PostalCode>16947</PostalCode><Country /><CreatedById>0</CreatedById><ModifiedById>0</ModifiedById><DateCreated>0001-01-01T00:00:00</DateCreated><DateModified>0001-01-01T00:00:00</DateModified><Version>0</Version><ValidationErrors /></Address><PhoneItems /><InternetItems><InternetItem><Id>70</Id><ExternalItemId>97000896</ExternalItemId><ServiceId>70</ServiceId><Priority>1</Priority><ProvisionSequence>1</ProvisionSequence><ProvisionDate>2016-02-12T00:00:00</ProvisionDate><StatusType>Pending</StatusType><ActionType>Add</ActionType><ResultMessage /><Log /><CompletionDate xmlns:p8=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p8:nil=\\\"true\\\" /><StartDate xmlns:p8=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p8:nil=\\\"true\\\" /><Notes>INTERNET INSTALL</Notes><CustomId>97000896</CustomId><DateCreated>0001-01-01T00:00:00</DateCreated><DateModified>0001-01-01T00:00:00</DateModified><Version>0</Version><PrimaryUser><UserName /><EmailAddress /><Password /><Domain /><Type /><FirstName /><LastName /><ActionType>Add</ActionType></PrimaryUser><Users /><Features><Feature><ActionType>Add</ActionType><Code>Y1YRI</Code><Description>1 YEAR CONTRACT</Description><ActivationDate>2016-02-12T00:00:00</ActivationDate><DeactivationDate>2016-02-12T00:00:00</DeactivationDate><Quantity>0</Quantity><Attributes /></Feature><Feature><ActionType>Add</ActionType><Code>YRB100</Code><Description>RESIDENTIAL INTERNET</Description><ActivationDate>2016-02-12T00:00:00</ActivationDate><DeactivationDate>2016-02-12T00:00:00</DeactivationDate><Quantity>0</Quantity><Attributes /></Feature></Features><Plant><WirelessKey /><Ssid /><Ont /><NetworkIp /><NetworkPort /><NetworkName>NTWK-Troy E7</NetworkName><Vpi /><Vci /><Vlan /><SipUsername /><SipPassword /><TransportType>GPON</TransportType><Ports><Port><Name /><Number>1</Number><DataServiceNumber>1</DataServiceNumber><Status /><Type>Ont</Type></Port><Port><Name /><Number>2</Number><DataServiceNumber>1</DataServiceNumber><Status /><Type>Ont</Type></Port><Port><Name /><Number>3</Number><DataServiceNumber>1</DataServiceNumber><Status /><Type>Ont</Type></Port><Port><Name /><Number>4</Number><DataServiceNumber>1</DataServiceNumber><Status /><Type>Ont</Type></Port></Ports><OltPort>0</OltPort><PonPort>0</PonPort><OntNumber>5290568</OntNumber><OntModel>844G</OntModel><CentralOffice>CalixE7</CentralOffice><Coid /><DataServiceNumber>0</DataServiceNumber><CircuitName /><Node /><NodeIp /><Shelf /><Slot /><Card /><Port>0</Port><EquipmentType>OLTG-4E</EquipmentType><HasActiveInternetService>True</HasActiveInternetService><HasActivePhoneService>False</HasActivePhoneService><HasActiveVideoService>False</HasActiveVideoService><ServiceTagAction>34</ServiceTagAction></Plant><OldPlant><WirelessKey /><Ssid /><Ont /><NetworkIp /><NetworkPort /><NetworkName /><Vpi /><Vci /><Vlan /><SipUsername /><SipPassword /><TransportType /><Ports /><OltPort>0</OltPort><PonPort>0</PonPort><OntNumber>0</OntNumber><OntModel /><CentralOffice /><Coid /><DataServiceNumber>0</DataServiceNumber><CircuitName /><Node /><NodeIp /><Shelf /><Slot /><Card /><Port>0</Port><EquipmentType /><HasActiveInternetService>True</HasActiveInternetService><HasActivePhoneService>False</HasActivePhoneService><HasActiveVideoService>False</HasActiveVideoService><ServiceTagAction>0</ServiceTagAction></OldPlant><Equipments /><ValidationErrors /></InternetItem></InternetItems><VideoItems /><MsaGesn>0</MsaGesn><ValidationErrors /></Location></Locations><Id>41</Id><ExternalServiceId>97000896</ExternalServiceId><OrderId>41</OrderId><Priority>1</Priority><ProvisionSequence>1</ProvisionSequence><Xml /><ProvisionDate>2016-02-12T00:00:00</ProvisionDate><StatusType>Pending</StatusType><ActionType>Add</ActionType><ResultMessage /><Log /><CompletionDate xmlns:p4=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p4:nil=\\\"true\\\" /><StartDate xmlns:p4=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" p4:nil=\\\"true\\\" /><Notes>INTERNET INSTALL</Notes><CreatedByUser>newnet_api_user</CreatedByUser><ModifiedByUser>newnet_api_user</ModifiedByUser><DateCreated>2016-02-05T17:05:55.9462977</DateCreated><DateModified>2016-02-10T12:08:58.1825801</DateModified><Version>26</Version><ValidationErrors /></Service></Services><NewNetCaseId>36169433556b528c253b2e8067234080</NewNetCaseId><NewNetRouteIndex>2</NewNetRouteIndex><NewNetTriggerId /><NewNetProcessId /><NewNetTaskId>18347</NewNetTaskId><NewNetUserId>EMPSMALLWOOD</NewNetUserId><ValidationErrors /></Order>\",\r\n  \"ProvisionDate\": \"2016-02-12T00:00:00\",\r\n  \"OrginatingIp\": null,\r\n  \"StatusType\": \"pending\",\r\n  \"ActionType\": \"add\",\r\n  \"ResultMessage\": \"\",\r\n  \"Log\": \"\",\r\n  \"ResponseSent\": false,\r\n  \"CompletionDate\": null,\r\n  \"StartDate\": null,\r\n  \"ServiceProvider\": \"\",\r\n  \"Configuration\": \"\",\r\n  \"Product\": \"\",\r\n  \"ClassOfService\": \"\",\r\n  \"CSR\": \"EMPSMALLWOOD\",\r\n  \"Notes\": \"INTERNET INSTALL\",\r\n  \"CreatedByUser\": \"qss_apiuser\",\r\n  \"ModifiedByUser\": \"qss_apiuser\",\r\n  \"DateCreated\": \"2016-02-11T11:48:33.5973113\",\r\n  \"DateModified\": \"2016-02-11T13:33:14.0248243\",\r\n  \"Version\": 8,\r\n  \"Account\": {\r\n    \"$id\": \"2\",\r\n    \"Id\": 0,\r\n    \"Name\": \"DONNA WILLIAMS\",\r\n    \"MasterPhone\": \"5705290568\",\r\n    \"ExternalAccountId\": \"96000666\",\r\n    \"ExternalAccountGroupId\": null,\r\n    \"CompanyId\": 0,\r\n    \"Type\": \"R\",\r\n    \"StatusType\": \"pending\",\r\n    \"CreatedByUser\": null,\r\n    \"ModifiedByUser\": null,\r\n    \"DateCreated\": \"0001-01-01T00:00:00\",\r\n    \"DateModified\": \"0001-01-01T00:00:00\",\r\n    \"Version\": 0,\r\n    \"Contact\": {\r\n      \"$id\": \"3\",\r\n      \"Type\": \"\",\r\n      \"FirstName\": \"\",\r\n      \"LastName\": \"\",\r\n      \"Corporation\": \"\",\r\n      \"PhoneNumber\": \"\",\r\n      \"Address\": {\r\n        \"$id\": \"4\",\r\n        \"Id\": 0,\r\n        \"Attention\": \"DONNA WILLIAMS\",\r\n        \"StreetLine1\": \"232  E MAIN ST\",\r\n        \"StreetLine2\": \"\",\r\n        \"Municipality\": \"TROY\",\r\n        \"AdministrativeArea\": \"PA\",\r\n        \"SubAdministrativeArea\": \"TROY BORO\",\r\n        \"PostalCode\": \"16947\",\r\n        \"Country\": \"\",\r\n        \"CreatedById\": 0,\r\n        \"ModifiedById\": 0,\r\n        \"DateCreated\": \"0001-01-01T00:00:00\",\r\n        \"DateModified\": \"0001-01-01T00:00:00\",\r\n        \"Version\": 0,\r\n        \"ValidationErrors\": null\r\n      },\r\n      \"OldAddress\": null\r\n    },\r\n    \"ValidationErrors\": {\r\n      \"$id\": \"5\"\r\n    }\r\n  },\r\n  \"Services\": [\r\n    {\r\n      \"$id\": \"6\",\r\n      \"Locations\": [\r\n        {\r\n          \"$id\": \"7\",\r\n          \"ExternalLocationId\": \"1245\",\r\n          \"Address\": {\r\n            \"$id\": \"8\",\r\n            \"Id\": 0,\r\n            \"Attention\": \"DONNA WILLIAMS\",\r\n            \"StreetLine1\": \"232  E MAIN ST\",\r\n            \"StreetLine2\": \"\",\r\n            \"Municipality\": \"TROY\",\r\n            \"AdministrativeArea\": \"PA\",\r\n            \"SubAdministrativeArea\": \"TROY BORO\",\r\n            \"PostalCode\": \"16947\",\r\n            \"Country\": \"\",\r\n            \"CreatedById\": 0,\r\n            \"ModifiedById\": 0,\r\n            \"DateCreated\": \"0001-01-01T00:00:00\",\r\n            \"DateModified\": \"0001-01-01T00:00:00\",\r\n            \"Version\": 0,\r\n            \"ValidationErrors\": {\r\n              \"$id\": \"9\"\r\n            }\r\n          },\r\n          \"OldAddress\": null,\r\n          \"LocationInfo\": null,\r\n          \"PhoneItems\": [],\r\n          \"InternetItems\": [\r\n            {\r\n              \"$id\": \"10\",\r\n              \"Id\": 15,\r\n              \"ExternalItemId\": \"97000896\",\r\n              \"ServiceId\": 15,\r\n              \"Priority\": 1,\r\n              \"ProvisionSequence\": 1,\r\n              \"Xml\": null,\r\n              \"ProvisionDate\": \"2016-02-12T00:00:00\",\r\n              \"StatusType\": \"pending\",\r\n              \"ActionType\": \"add\",\r\n              \"ResultMessage\": \"\",\r\n              \"Log\": \"\",\r\n              \"CompletionDate\": null,\r\n              \"StartDate\": null,\r\n              \"Notes\": \"INTERNET INSTALL\",\r\n              \"CustomId\": \"97000896\",\r\n              \"CreatedByUser\": null,\r\n              \"ModifiedByUser\": null,\r\n              \"DateCreated\": \"0001-01-01T00:00:00\",\r\n              \"DateModified\": \"0001-01-01T00:00:00\",\r\n              \"Version\": 0,\r\n              \"PrimaryUser\": {\r\n                \"$id\": \"11\",\r\n                \"UserName\": \"\",\r\n                \"EmailAddress\": \"\",\r\n                \"Password\": \"\",\r\n                \"Domain\": \"\",\r\n                \"Type\": \"\",\r\n                \"FirstName\": \"\",\r\n                \"LastName\": \"\",\r\n                \"ActionType\": \"add\"\r\n              },\r\n              \"Users\": [],\r\n              \"Features\": [\r\n                {\r\n                  \"$id\": \"12\",\r\n                  \"ActionType\": \"add\",\r\n                  \"Code\": \"Y1YRI\",\r\n                  \"Description\": \"1 YEAR CONTRACT\",\r\n                  \"ActivationDate\": \"2016-02-12T00:00:00\",\r\n                  \"DeactivationDate\": \"2016-02-12T00:00:00\",\r\n                  \"Quantity\": 0,\r\n                  \"Attributes\": []\r\n                },\r\n                {\r\n                  \"$id\": \"13\",\r\n                  \"ActionType\": \"add\",\r\n                  \"Code\": \"YRB100\",\r\n                  \"Description\": \"RESIDENTIAL INTERNET\",\r\n                  \"ActivationDate\": \"2016-02-12T00:00:00\",\r\n                  \"DeactivationDate\": \"2016-02-12T00:00:00\",\r\n                  \"Quantity\": 0,\r\n                  \"Attributes\": []\r\n                }\r\n              ],\r\n              \"Plant\": {\r\n                \"$id\": \"14\",\r\n                \"WirelessKey\": \"\",\r\n                \"Ssid\": \"\",\r\n                \"Ont\": \"\",\r\n                \"NetworkIp\": \"\",\r\n                \"NetworkPort\": \"\",\r\n                \"NetworkName\": \"NTWK-Troy E7\",\r\n                \"Vpi\": \"\",\r\n                \"Vci\": \"\",\r\n                \"Vlan\": \"\",\r\n                \"SipUsername\": \"\",\r\n                \"SipPassword\": \"\",\r\n                \"TransportType\": \"GPON\",\r\n                \"Ports\": [\r\n                  {\r\n                    \"$id\": \"15\",\r\n                    \"Name\": \"\",\r\n                    \"Number\": 1,\r\n                    \"DataServiceNumber\": 1,\r\n                    \"Status\": \"\",\r\n                    \"Type\": \"Ont\"\r\n                  },\r\n                  {\r\n                    \"$id\": \"16\",\r\n                    \"Name\": \"\",\r\n                    \"Number\": 2,\r\n                    \"DataServiceNumber\": 1,\r\n                    \"Status\": \"\",\r\n                    \"Type\": \"Ont\"\r\n                  },\r\n                  {\r\n                    \"$id\": \"17\",\r\n                    \"Name\": \"\",\r\n                    \"Number\": 3,\r\n                    \"DataServiceNumber\": 1,\r\n                    \"Status\": \"\",\r\n                    \"Type\": \"Ont\"\r\n                  },\r\n                  {\r\n                    \"$id\": \"18\",\r\n                    \"Name\": \"\",\r\n                    \"Number\": 4,\r\n                    \"DataServiceNumber\": 1,\r\n                    \"Status\": \"\",\r\n                    \"Type\": \"Ont\"\r\n                  }\r\n                ],\r\n                \"OltPort\": 0,\r\n                \"PonPort\": 0,\r\n                \"OntNumber\": 5290568,\r\n                \"OntModel\": \"844G\",\r\n                \"CentralOffice\": \"CalixE7\",\r\n                \"Coid\": \"\",\r\n                \"DataServiceNumber\": 0,\r\n                \"CircuitName\": \"\",\r\n                \"Node\": \"\",\r\n                \"NodeIp\": \"\",\r\n                \"Shelf\": \"\",\r\n                \"Slot\": \"\",\r\n                \"Card\": \"\",\r\n                \"Port\": 0,\r\n                \"EquipmentType\": \"OLTG-4E\",\r\n                \"HasActiveInternetService\": true,\r\n                \"HasActivePhoneService\": false,\r\n                \"HasActiveVideoService\": false,\r\n                \"ServiceTagAction\": 34\r\n              },\r\n              \"OldPlant\": {\r\n                \"$id\": \"19\",\r\n                \"WirelessKey\": \"\",\r\n                \"Ssid\": \"\",\r\n                \"Ont\": \"\",\r\n                \"NetworkIp\": \"\",\r\n                \"NetworkPort\": \"\",\r\n                \"NetworkName\": \"\",\r\n                \"Vpi\": \"\",\r\n                \"Vci\": \"\",\r\n                \"Vlan\": \"\",\r\n                \"SipUsername\": \"\",\r\n                \"SipPassword\": \"\",\r\n                \"TransportType\": \"\",\r\n                \"Ports\": \"\",\r\n                \"OltPort\": 0,\r\n                \"PonPort\": 0,\r\n                \"OntNumber\": 0,\r\n                \"OntModel\": \"\",\r\n                \"CentralOffice\": \"\",\r\n                \"Coid\": \"\",\r\n                \"DataServiceNumber\": 0,\r\n                \"CircuitName\": \"\",\r\n                \"Node\": \"\",\r\n                \"NodeIp\": \"\",\r\n                \"Shelf\": \"\",\r\n                \"Slot\": \"\",\r\n                \"Card\": \"\",\r\n                \"Port\": 0,\r\n                \"EquipmentType\": \"\",\r\n                \"HasActiveInternetService\": true,\r\n                \"HasActivePhoneService\": false,\r\n                \"HasActiveVideoService\": false,\r\n                \"ServiceTagAction\": 0\r\n              },\r\n              \"Equipments\": [],\r\n              \"ValidationErrors\": {\r\n                \"$id\": \"20\"\r\n              }\r\n            }\r\n          ],\r\n          \"VideoItems\": [],\r\n          \"TaxArea\": null,\r\n          \"CountyJurisdiction\": null,\r\n          \"DistrictJurisdiction\": null,\r\n          \"MsagExchange\": null,\r\n          \"MsaGesn\": 0,\r\n          \"ValidationErrors\": {\r\n            \"$id\": \"21\"\r\n          }\r\n        }\r\n      ],\r\n      \"Id\": 10,\r\n      \"ExternalServiceId\": \"97000896\",\r\n      \"OrderId\": 10,\r\n      \"Priority\": 1,\r\n      \"ProvisionSequence\": 1,\r\n      \"Xml\": \"\",\r\n      \"ProvisionDate\": \"2016-02-12T00:00:00\",\r\n      \"StatusType\": \"pending\",\r\n      \"ActionType\": \"add\",\r\n      \"ResultMessage\": \"\",\r\n      \"Log\": \"\",\r\n      \"CompletionDate\": null,\r\n      \"StartDate\": null,\r\n      \"Notes\": \"INTERNET INSTALL\",\r\n      \"CreatedByUser\": \"qss_apiuser\",\r\n      \"ModifiedByUser\": \"qss_apiuser\",\r\n      \"DateCreated\": \"2016-02-11T11:48:33.5253021\",\r\n      \"DateModified\": \"2016-02-11T13:33:13.5402628\",\r\n      \"Version\": 8,\r\n      \"ValidationErrors\": {\r\n        \"$id\": \"22\"\r\n      }\r\n    }\r\n  ],\r\n  \"NewNetCaseId\": \"36169433556b528c253b2e8067234080\",\r\n  \"NewNetRouteIndex\": \"2\",\r\n  \"NewNetTriggerId\": \"\",\r\n  \"NewNetProcessId\": \"\",\r\n  \"NewNetTaskId\": \"18347\",\r\n  \"NewNetUserId\": \"EMPSMALLWOOD\",\r\n  \"ValidationErrors\": {\r\n    \"$id\": \"23\"\r\n  }\r\n}";
            
            //*** Act ***
            var response = _client.PostAsync("", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var reasonPhrase = response.ReasonPhrase;
            var myResponse = response.Content.ReadAsStringAsync();
            var result = myResponse.Result;

            //*** Assert ***
            //Assert.IsNotNull(json);
        }
    }
}
