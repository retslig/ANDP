using System;
using System.Collections.Generic;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Models;
using Common.Lib.Utility;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Account = ANDP.Lib.Domain.Models.Account;
using BootStrapper = ANDP.Domain.Test.Infrastructure.BootStrapper;
using Order = ANDP.Lib.Domain.Models.Order;
using Service = ANDP.Lib.Domain.Models.Service;
using Newtonsoft.Json;

namespace ANDP.Domain.Test.Services
{
    [TestClass]
    public class OrderServiceFixture
    {
        private IUnityContainer _container;
        private IOrderService _iOrderService;
        [TestInitialize]
        public void TestInitialize()
        {
            _container = BootStrapper.Initialize();
            _iOrderService = _container.Resolve<IOrderService>();
        }

        [TestMethod]
        public void Can_Provision_Order()
        {
            Order order = new Order
            {
                Account = new Account
                {
                    CompanyId = 7,
                    ExternalAccountId = "myExternalAccountId",
                    Contact = new Contact
                    {
                        Address = null,
                        FirstName = "billy",
                        LastName = "bob",
                        PhoneNumber = "6541231234",
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
                ExternalAccountId = "s6767d96s78d",
                ExternalCompanyId = "4",
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
                                    
                                },
                                CountyJurisdiction = null,
                                DistrictJurisdiction = null,
                                ExternalLocationId = null,
                                LocationInfo = null,
                                MsaGesn = 0,
                                MsagExchange = null,
                                TaxArea = null,
                                ValidationErrors = null,
                                //InternetItems = null,
                                //VideoItems = new List<VideoItem>
                                //{
                                //    new VideoItem
                                //    {
                                //        StatusType = StatusType.Pending
                                //    }
                                //},
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
                                        ExternalItemId = "ExternalItemId",
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
                                        //Features = null,
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Add,
                                            PhoneNumber  = "6541231234",
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

            order.Services[0].Locations[0].PhoneItems[0].Plant = new SerializableDictionary<string, object>();
            var accessDevice = new SerializableDictionary<string, object> {{"AccessLine", 484}, {"Name", "C024"}};

            order.Services[0].Locations[0].PhoneItems[0].Plant.Add("AccessDevice", accessDevice);

            string json = JsonConvert.SerializeObject(order);

            var orderStatus = _iOrderService.CreateOrUpdateOrder(order, "hi");
            //Assert.AreEqual(orderStatus.Id, 0);
        }

        [TestMethod]
        public void Can_Provision_Emp_Order()
        {
            Order order = new Order
            {
                Account = new Account
                {
                    CompanyId = 7,
                    ExternalAccountId = "myExternalAccountId",
                    Contact = new Contact
                    {
                        Address = null,
                        FirstName = "billy",
                        LastName = "bob",
                        PhoneNumber = "6541231234",
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
                ExternalAccountId = "s6767d96s78d",
                ExternalCompanyId = "4",
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
                                        Plant = new SerializableDictionary<string, object>
                                        {
                                            {"CentralOffice", "CalixE7"},
                                            {"TransportType", "GPON"},
                                            {"OntNumber", 5499100},
                                            {"Coid", "RSVL"},
                                            {"Ports", new List<object>
                                            {
                                                new SerializableDictionary<string, object>  
                                                {
                                                    {"Name",""},
                                                    {"Number", 5},
                                                    {"Status", ""},
                                                    {"DataServiceNumber",1},
                                                    {"Type", "Ont"}
                                                }
                                            }}
                                        }
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


            string json = JsonConvert.SerializeObject(order);

            //var doc = new XmlDocument();
            //doc.LoadXml(xml);
            //string jsonText = JsonConvert.SerializeXmlNode(doc);


            //var orderStatus = _iOrderService.CreateOrUpdateOrder(order, "hi");
            //Assert.IsNotNull(json);
            //Assert.AreEqual(orderStatus.Id, 0);
        }


    }
}
