using System;
using System.Collections.Generic;
using System.Reflection;
using ANDP.Lib.Domain.Models;
using ANDP.Lib.Domain.MappingProfiles;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Account = ANDP.Lib.Domain.Models.Account;
using DomainOrder = ANDP.Lib.Domain.Models.Order;
using DaoOrder = ANDP.Lib.Data.Repositories.Order.Order;
using StatusTypeEnum = ANDP.Lib.Data.Repositories.Order.StatusTypeEnum;

namespace ANDP.Lib.Data.Tests.MappingProfiles
{
    [TestClass]
    public class EngineRepositoryFixture
    {
        private ICommonMapper _commonMapper;

        //static readonly Mock<IOrder> mock = new Mock<IOrder>();
        //static readonly Mock<Order> mock = new Mock<Order>();
        //readonly Order _order = new Order(mock.Object);

        [TestInitialize]
        public void OrderProfileFixture()
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
        }

        [TestMethod]
        public void Can_Map_DomainOrder_To_DaoOrder()
        {
            //*** Arrange ***
            #region *** building xml ***
            var order = new DomainOrder
            {
                ExternalCompanyId = "333333", //requiared by database
                ExternalOrderId = Guid.NewGuid().ToString(),
                Priority = 1, //required by database
                ProvisionDate = DateTime.Now,
                CreatedByUser = "bhs",
                ModifiedByUser = "bhs",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Version = 1,
                StatusType = ANDP.Lib.Domain.Models.StatusType.Pending,
                ActionType = ActionType.Add,
                Account = new Account
                {
                    Name = "Brent",
                    Contact = new Contact
                       {
                           Address = new Address
                           {
                               Attention = "",
                                
                           },
                       }
                }

            };
            #endregion

            //*** Act ***
            var daoOrder = ObjectFactory.CreateInstanceAndMap<DomainOrder, DaoOrder>(_commonMapper, order);
            Assert.IsNotNull(daoOrder);

            daoOrder.StatusTypeId = (int) StatusTypeEnum.Pending;

            var mappedDomainOrder = ObjectFactory.CreateInstanceAndMap<DaoOrder, DomainOrder>(_commonMapper, daoOrder);
            Assert.IsNotNull(mappedDomainOrder);
        }
    }
}
