using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ANDP.Lib.Domain.Models;
using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.MappingProfiles;
using ANDP.Lib.Infrastructure;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Common.Lib.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainAccount = ANDP.Lib.Domain.Models.Account;
using DomainOrder = ANDP.Lib.Domain.Models.Order;
using Service = ANDP.Lib.Domain.Models.Service;
using StatusType = ANDP.Lib.Domain.Models.StatusType;

namespace ANDP.Lib.Data.Tests.Repositories
{
    [TestClass]
    public class OrderRepositoryFixture
    {
        private ICommonMapper _commonMapper;
        private IANDP_Order_Entities _iAndOrderEntities;
        private IOrderRepository _iOrderRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            var settings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                    {
                        AssemblyName = Assembly.GetAssembly(typeof(OrderProfile)).GetName().Name,
                        Namespace = new List<string> {typeof(OrderProfile).Namespace}
                    }
            };

            _commonMapper = new CommonMapper(settings);

            _iAndOrderEntities = new ANDP_Order_Entities(BootStrapper.AndpEntitiesBootstrapper().ToString(), "test");
            _iOrderRepository = new OrderRepository(_iAndOrderEntities);
        }

        [TestMethod]
        public void Can_Create_Order_Add_Phone_And_Video()
        {
            //*** Arrange ***
            #region Building domain order model

            string externalAccountId = Guid.NewGuid().ToString();

            var order = new DomainOrder
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                ExternalCompanyId = "3", //required by database
                ExternalAccountId = externalAccountId,
                Priority = 1, //required by database
                ProvisionDate = DateTime.Now,
                CreatedByUser = "Unit Test",
                ModifiedByUser = "Unit Test",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Version = 0,
                ActionType = ANDP.Lib.Domain.Models.ActionType.Add,
                StatusType = StatusType.Pending,
                NewNetCaseId = "30194091753e4244f75eb34087632097",
                NewNetRouteIndex = "2",
                NewNetProcessId = "1",
                NewNetTaskId = "1",
                NewNetUserId = "1",
                Services = new List<Service>
                {
                    new Service
                    {
                        CreatedByUser = "Unit Test",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = Guid.NewGuid().ToString(),
                        ModifiedByUser = "Unit Test",
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        Version = 0,
                        StatusType = StatusType.Pending,
                        ActionType = ActionType.Add,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "hi"
                                },
                                VideoItems = new List<VideoItem>
                                {
                                    new VideoItem
                                    {
                                        CreatedByUser = "Unit Test",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = Guid.NewGuid().ToString(),
                                        ModifiedByUser = "Unit Test",
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        ProvisionSequence = 1,
                                        Version = 0,
                                        ActionType = ActionType.Add,
                                        StatusType = StatusType.Pending,
                                        Plant = new SerializableDictionary<string, object>
                                        {
                                            {"Settops", new List<object>
                                            {
                                                new SerializableDictionary<string, object>  
                                                {
                                                    {"Name","DVR"},
                                                    {"Number", 5},
                                                    {"Status", ""},
                                                    {"DataServiceNumber", 1},
                                                    {"Type", "RCA"}
                                                }
                                            }}
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                Code = "HBO",
                                                Description = "HBO"
                                            },new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                Code = "Starz",
                                                Description = "Starz"
                                            }
                                        }
                                    }
                                },
                                PhoneItems = new List<PhoneItem>
                                {
                                    new PhoneItem
                                    {
                                        CreatedByUser = "Unit Test",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = Guid.NewGuid().ToString(),
                                        ModifiedByUser = "Unit Test",
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        ProvisionSequence = 1,
                                        Version = 0,
                                        ActionType = ActionType.Add,
                                        StatusType = StatusType.Pending,
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Add,
                                            PhoneNumber = "6051119999",
                                            CallerName = "QS Solutions",
                                            DirectoryPublish = "Yes"
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Add,
                                                Code = "VoiceMail",
                                                Description = "Gold Voice mail"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Account = new DomainAccount
                {
                    Name = "Brent",
                    CreatedByUser = "Unit Test",
                    ModifiedByUser = "Unit Test",
                    ExternalAccountId = externalAccountId,
                    ExternalAccountGroupId = "test",
                    Contact = new Contact
                    {
                        Address = new Address
                        {
                            Attention = "",
                                
                        }
                    }
                }

            };
            #endregion

            try
            {
                var daoOrder = ObjectFactory.CreateInstanceAndMap<ANDP.Lib.Domain.Models.Order, ANDP.Lib.Data.Repositories.Order.Order>(_commonMapper, order);

                //*** Act ***
                var newDomainOrder = _iOrderRepository.CreateOrUpdateOrder(daoOrder, "UnitTest");
                //*** Assert ***
                Assert.IsNotNull(newDomainOrder);
                Assert.IsNotNull(newDomainOrder.Id);
                Assert.AreNotEqual(0, newDomainOrder.Id);
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
                throw;
            }
        }

        [TestMethod]
        public void Can_Create_Order_Delete_Phone_And_Video()
        {
            //*** Arrange ***
            #region Building domain order model

            string externalAccountId = Guid.NewGuid().ToString();

            var order = new DomainOrder
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                ExternalCompanyId = "3", //required by database
                ExternalAccountId = externalAccountId,
                Priority = 1, //required by database
                ProvisionDate = DateTime.Now,
                CreatedByUser = "Unit Test",
                ModifiedByUser = "Unit Test",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Version = 0,
                ActionType = ANDP.Lib.Domain.Models.ActionType.Delete,
                StatusType = StatusType.Pending,
                NewNetCaseId = "30194091753e4244f75eb34087632097",
                NewNetRouteIndex = "1",
                NewNetProcessId = "1",
                NewNetTaskId = "1",
                NewNetUserId = "1",
                Services = new List<Service>
                {
                    new Service
                    {
                        CreatedByUser = "Unit Test",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = Guid.NewGuid().ToString(),
                        ModifiedByUser = "Unit Test",
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        Version = 0,
                        StatusType = StatusType.Pending,
                        ActionType = ActionType.Delete,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "hi"
                                },
                                VideoItems = new List<VideoItem>
                                {
                                    new VideoItem
                                    {
                                        CreatedByUser = "Unit Test",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = Guid.NewGuid().ToString(),
                                        ModifiedByUser = "Unit Test",
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        ProvisionSequence = 1,
                                        Version = 0,
                                        ActionType = ActionType.Delete,
                                        StatusType = StatusType.Pending,
                                        Plant = new SerializableDictionary<string, object>
                                        {
                                            {"Settops", new List<object>
                                            {
                                                new SerializableDictionary<string, object>  
                                                {
                                                    {"Name","DVR"},
                                                    {"Number", 5},
                                                    {"Status", ""},
                                                    {"DataServiceNumber", 1},
                                                    {"Type", "RCA"}
                                                }
                                            }}
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Delete,
                                                Code = "HBO",
                                                Description = "HBO"
                                            },new Feature
                                            {
                                                ActionType = ActionType.Delete,
                                                Code = "Starz",
                                                Description = "Starz"
                                            }
                                        }
                                    }
                                },
                                PhoneItems = new List<PhoneItem>
                                {
                                    new PhoneItem
                                    {
                                        CreatedByUser = "Unit Test",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        ExternalItemId = Guid.NewGuid().ToString(),
                                        ModifiedByUser = "Unit Test",
                                        Priority = 1,
                                        ProvisionDate = DateTime.Now,
                                        ProvisionSequence = 1,
                                        Version = 0,
                                        ActionType = ActionType.Delete,
                                        StatusType = StatusType.Pending,
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Delete,
                                            PhoneNumber = "6051119999",
                                            CallerName = "QS Solutions",
                                            DirectoryPublish = "Yes"
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Delete,
                                                Code = "VoiceMail",
                                                Description = "Gold Voice mail"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Account = new DomainAccount
                {
                    Name = "Brent",
                    CreatedByUser = "Unit Test",
                    ModifiedByUser = "Unit Test",
                    ExternalAccountId = externalAccountId,
                    ExternalAccountGroupId = "test",
                    Contact = new Contact
                    {
                        Address = new Address
                        {
                            Attention = ""
                        }
                    }
                }

            };
            #endregion

            try
            {
                var daoOrder = ObjectFactory.CreateInstanceAndMap<ANDP.Lib.Domain.Models.Order, ANDP.Lib.Data.Repositories.Order.Order>(_commonMapper, order);

                //*** Act ***
                var newDomainOrder = _iOrderRepository.CreateOrUpdateOrder(daoOrder, "UnitTest");
                //*** Assert ***
                Assert.IsNotNull(newDomainOrder);
                Assert.IsNotNull(newDomainOrder.Id);
                Assert.AreNotEqual(0, newDomainOrder.Id);
            }
            catch (Exception ex)
            {
                var t = ex.ToString();
                throw;
            }
        }

        [TestMethod]
        public void Can_Create_Delete_Voicemail_Order()
        {
            //*** Arrange ***
            #region Building domain order model
            var order = new DomainOrder
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                ExternalCompanyId = "3",
                ExternalAccountId = Guid.NewGuid().ToString(),
                Priority = 1, //required by database
                ProvisionDate = DateTime.Now,
                //CreatedByUser = "UnitTest",
                //ModifiedByUser = "UnitTest",
                //DateCreated = DateTime.Now,
                //DateModified = DateTime.Now,
                //Version = 0,
                ActionType = ActionType.Change,
                StatusType = StatusType.Pending,
                NewNetCaseId = "30194091753e4244f75eb34087632097",
                NewNetRouteIndex = "1",
                NewNetProcessId = "1",
                NewNetTaskId = "1",
                NewNetUserId = "1",
                Services = new List<Service>
                {
                    new Service
                    {
                        CreatedByUser = "UnitTest",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = Guid.NewGuid().ToString(),
                        ModifiedByUser = "UnitTest",
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        //Version = 0,
                        StatusType = StatusType.Pending,
                        ActionType = ActionType.Change,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                Address = new Address
                                {
                                    Attention = "UnitTest",
                                    Municipality = "Sioux Falls",
                                    PostalCode = "57064",
                                    StreetLine1 = "111 S Some Street"
                                },
                                PhoneItems = new List<PhoneItem>
                                {
                                    new PhoneItem
                                    {
                                        ExternalItemId = Guid.NewGuid().ToString(),
                                        ActionType = ActionType.Change,
                                        StatusType = StatusType.Pending,
                                        CreatedByUser = "UnitTest",
                                        ModifiedByUser = "UnitTest",
                                        DateCreated = DateTime.Now,
                                        DateModified = DateTime.Now,
                                        LineInformation = new LineInformation
                                        {
                                            ActionType = ActionType.Unchanged,
                                            PhoneNumber = "6051119999",
                                            CallerName = "QS Solutions",
                                            DirectoryPublish = "Yes"
                                        },
                                        Features = new List<Feature>
                                        {
                                            new Feature
                                            {
                                                ActionType = ActionType.Delete,
                                                Code = "VoiceMail",
                                                Description = "Gold Voice mail"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Account = new DomainAccount
                {
                    Name = "Brent",
                    //CreatedByUser = "Unit Test",
                    //ModifiedByUser = "Unit Test",
                    Contact = new Contact
                    {
                        Address = new Address
                        {
                            Attention = "UnitTest",
                            Municipality = "Sioux Falls",
                            PostalCode = "57064",
                            StreetLine1 = "111 S Some Street"
                        }
                    }
                }

            };
            #endregion

            var daoOrder = ObjectFactory.CreateInstanceAndMap<ANDP.Lib.Domain.Models.Order, ANDP.Lib.Data.Repositories.Order.Order>(_commonMapper, order);

            //*** Act ***
            var newDomainOrder = _iOrderRepository.CreateOrUpdateOrder(daoOrder, "UnitTest");

            //*** Assert ***
            Assert.IsNotNull(newDomainOrder);
            Assert.IsNotNull(newDomainOrder.Id);
            Assert.AreNotEqual(0, newDomainOrder.Id);
        }


        //[TestMethod]
        //public void Can_Delete_Order_By_Id()
        //{
        //    var deleteOrderResult = _iOrderRepository.DeleteOrderById(19);
        //}

        [TestMethod]
        public void Can_Delete_Order_By_Ext_Order_Id()
        {
            var externalOrderId = Guid.NewGuid().ToString();

            //*** Arrange *** 
            #region Building domain order model
            var order = new DomainOrder
            {
                ExternalOrderId = externalOrderId,
                ExternalCompanyId = "3", //required by database
                ExternalAccountId = Guid.NewGuid().ToString(), //required by the database
                Priority = 1, //required by database
                ProvisionDate = DateTime.Now,
                CreatedByUser = "Unit Test",
                ModifiedByUser = "Unit Test",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Version = 0,
                StatusType = StatusType.Pending,
                ActionType = ActionType.Add,
                Services = new List<Service>
                {
                    new Service
                    {
                        CreatedByUser = "Unit Test",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        ExternalServiceId = "3333",
                        ModifiedByUser = "Unit Test",
                        Priority = 1,
                        ProvisionDate = DateTime.Now,
                        ProvisionSequence = 1,
                        Version = 0,
                        StatusType = StatusType.Pending,
                        ActionType = ActionType.Add,
                        Locations = new List<Location>
                        {
                            new Location
                            {
                                
                                Address = new Address
                                {
                                    Attention = "hi"
                                }
                            }
                        }
                    }
                },
                Account = new DomainAccount
                {
                    Name = "Brent",
                    CreatedByUser = "Unit Test",
                    ModifiedByUser = "Unit Test",
                    Contact = new Contact
                    {
                        Address = new Address
                        {
                            Attention = ""
                        }
                    }
                }

            };
            #endregion

            var daoOrder = ObjectFactory.CreateInstanceAndMap<ANDP.Lib.Domain.Models.Order, ANDP.Lib.Data.Repositories.Order.Order>(_commonMapper, order);

            var initialOrderStatus = _iOrderRepository.CreateOrUpdateOrder(daoOrder, "UnitTest");
            //Making sure we have a order created to delete.
            Assert.IsNotNull(initialOrderStatus);
            Assert.IsNotNull(initialOrderStatus.Id);
            Assert.AreNotEqual(0,initialOrderStatus.Id);
            Assert.IsNotNull(initialOrderStatus.ExternalOrderId);
           
            //*** Act ***
            var deleteOrderResult = _iOrderRepository.DeleteOrderByExtId(externalOrderId, "");

            //*** Assert ***
            Assert.IsNull(deleteOrderResult);
        }

        [TestMethod]
        public void Can_Update_Order()
        {

            //*** Arrange ***
            //Making sure our test order is at a base status all the time.
            var existingDomainOrder = _iOrderRepository.RetrieveOrderById(25);
            existingDomainOrder.Priority = 1;
            existingDomainOrder.Services.First().Priority = 1;
            var existingDomainOrderStatus = _iOrderRepository.UpdateOrder(existingDomainOrder, "UnitTest");
            Assert.AreEqual(existingDomainOrder.Priority, 1);
            Assert.AreEqual(existingDomainOrder.Services.First().Priority, 1);

            //*** Act ***
            var existingActDomainOrder = _iOrderRepository.RetrieveOrderById(25);
            existingActDomainOrder.Priority = 2;
            existingActDomainOrder.Services.First().Priority = 2;
            var existingActDomainOrderStatus = _iOrderRepository.UpdateOrder(existingActDomainOrder, "UnitTest");

            //var updatedDomainOrderStatus = _iOrderRepository.UpdateOrder(existingActDomainOrder, "1234");

            //*** Assert ***
            Assert.IsNotNull(existingActDomainOrderStatus);
            Assert.IsNotNull(existingActDomainOrderStatus.Id);
            Assert.AreEqual(existingActDomainOrder.Priority, 2);
            Assert.AreEqual(existingActDomainOrder.Services.First().Priority, 2);
        }

        [TestMethod]
        public void Can_Retrieve_Order_By_Id()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrder = _iOrderRepository.RetrieveOrderById(6);

            //*** Assert ***
            Assert.IsNotNull(domainOrder);
        }

        [TestMethod]
        public void Can_Retrieve_Order_By_ExtId_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var orderId = _iOrderRepository.RetrieveOrderIdByExtIdAndCompanyId("2fe042f8-9a6b-4855-9214-209827362be3", 3);

            //*** Assert ***
            Assert.IsNotNull(orderId);
            Assert.AreEqual(6, orderId);
        }

        [TestMethod]
        public void Can_Retrieve_Next_Order_To_Provision()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrder = _iOrderRepository.RetrieveNextOrderToProvisionByCompanyIdAndActionType(6, null);

            //*** Assert ***
            Assert.IsNotNull(domainOrder);
        }

        [TestMethod]
        public void Can_Update_Order_Status()
        {
            //*** Arrange ***
            //Making sure our order isn't already at a completed status
            var initialOrderStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Pending.ToStatusTypeEnum(),0 , "I'm pending", "I'm pending", null, null, "UnitTest");
            Assert.AreNotEqual((int)Domain.Models.StatusType.Success, initialOrderStatus.StatusTypeId);

            //*** Act ***
            var orderStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Success.ToStatusTypeEnum(), 0, "worked!", "worked!", DateTime.Now.AddMinutes(-2), DateTime.Now, "UnitTest");

            //*** Assert ***
            Assert.IsNotNull(orderStatus);
            Assert.AreEqual((int)Domain.Models.StatusType.Success, orderStatus.StatusTypeId);

            //*** Cleanup ***
            var finalOrderStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Pending.ToStatusTypeEnum(), 0, "I'm Pending", "I'm Pending", null, null, "UnitTest");
        }

        [TestMethod]
        public void Can_Update_Service_Status()
        {
            //*** Arrange ***
            //Making sure our service isn't already at a completed status
            var initialServiceStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Pending.ToStatusTypeEnum(), 0, "I'm Pending", "I'm Pending", null, null, "UnitTest");
            Assert.AreNotEqual((int)Domain.Models.StatusType.Success, initialServiceStatus.StatusTypeId);

            //*** Act ***
            var serviceStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Success.ToStatusTypeEnum(), 0, "I'm a Success", "I'm a Success", DateTime.Now.AddMinutes(-6), DateTime.Now, "UnitTest");

            //*** Assert ***
            Assert.IsNotNull(serviceStatus);
            Assert.AreEqual((int)Domain.Models.StatusType.Success, serviceStatus.StatusTypeId);

            //*** Cleanup ***
            var finalServiceStatus = _iOrderRepository.UpdateOrderStatus(11, ANDP.Lib.Domain.Models.StatusType.Pending.ToStatusTypeEnum(), 0, "I'm Pending", "I'm Pending", null, null, "UnitTest");
        }

        [TestMethod]
        public void Can_Retrieve_Orders_By_StatusType_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrders = _iOrderRepository.RetrieveOrdersByStatusTypeAndCompanyId(StatusType.Pending.ToStatusTypeEnum(), 6);

            //*** Assert ***
            Assert.IsNotNull(domainOrders);
            Assert.AreNotEqual(0, domainOrders.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Orders_By_Priority_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrders = _iOrderRepository.RetrieveOrdersByPriorityAndCompanyId(1, 6);

            //*** Assert ***
            Assert.IsNotNull(domainOrders);
            Assert.AreNotEqual(0, domainOrders.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Services_Left_On_Order()
        {
            //*** Arrange ***

            //*** Act ***
            var domainServices = _iOrderRepository.AnyServicesLeftOnOrder(67);

            //*** Assert ***
            Assert.IsNotNull(domainServices);
            Assert.AreNotEqual(0, domainServices.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Orders_By_ActionType_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrders = _iOrderRepository.RetrieveOrdersByActionTypeAndCompanyId(ActionType.Add.ToActionTypeEnum(), 6);

            //*** Assert ***
            Assert.IsNotNull(domainOrders);
            Assert.AreNotEqual(0, domainOrders.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Orders_By_Provision_Date_And_Company_Id()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrders = _iOrderRepository.RetrieveOrdersByProvisionDateAndCompanyId(new DateTime(2014, 06, 25), 6);

            //*** Assert ***
            Assert.IsNotNull(domainOrders);
            Assert.AreNotEqual(0, domainOrders.Count());
        }


        [TestMethod]
        public void Can_Retrieve__Orders_By_CreateByUser_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var domainOrders = _iOrderRepository.RetrieveOrdersByCreateByUserAndCompanyId("bhs", 6);

            //*** Assert ***
            Assert.IsNotNull(domainOrders);
            Assert.AreNotEqual(0, domainOrders.Count());
        }

        [TestMethod]
        public void Can_Retrieve_Service_Id_By_ExtId_And_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var serviceId = _iOrderRepository.RetrieveServiceIdByOrderIdAndExtServiceIdAndCompanyId(70, "3333", 6);

            //*** Assert ***
            Assert.IsNotNull(serviceId);
            Assert.AreNotEqual(0, serviceId);
        }

        [TestMethod]
        public void Can_Retrieve_Service_By_OrderId_ServiceExtId_CompanyId()
        {
            //*** Arrange ***

            //*** Act ***
            var service = _iOrderRepository.RetrieveServiceByOrderIdAndExtServiceIdAndCompanyId(70, "3333", 6);

            //*** Assert ***
            Assert.IsNotNull(service);
            Assert.AreNotEqual(0, service.Id);
        }

        [TestMethod]
        public void Can_Retrieve_Service_By_OrderId_And_ServiceId()
        {
            
            //*** Arrange ***

            //*** Act ***  
            var service = _iOrderRepository.RetrieveServiceById(1);

            //*** Assert ***
            Assert.IsNotNull(service);
        }
    }
}
