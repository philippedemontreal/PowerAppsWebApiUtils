using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;
using PowerAppsWebApiUtils.Json;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Tests
{
    [TestClass]
    public class GetByIdTests
    {
        [TestMethod]
        public async Task GetOneTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();
            var serviceProvider = 
                new ServiceCollection()
                .AddWebApiContext(config)
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<GenericRepository<Account>>();
            var entityId = Guid.Parse("48cf55d9-6e9f-e911-a982-000d3af3b3af");
            var account = 
                await repo.GetById(entityId, 
                p => 
                new
                {
                    Id = p.Id, 
                    StateCode = p.StateCode, 
                    StatusCode = p.StatusCode,
                    LastOnHoldTime = p.LastOnHoldTime,
                    ModifiedOn = p.ModifiedOn,
                    CreatedOn = p.CreatedOn,
                    CreatedBy = p.CreatedBy,
                    OwnerId = p.OwnerId,
                    ParentAccountId = p.ParentAccountId,
                    Telephone1 = p.Telephone1,
                    Address1_Composite = p.Address1_Composite,
                    Name = p.Name,
                });

            Assert.IsNotNull(account);
            Assert.IsNotNull(account.Name);
            Assert.IsNotNull(account.CreatedBy);
            Assert.IsNotNull(account.OwnerId);
            Assert.AreEqual(entityId, account.Id);
            Assert.AreEqual<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.AreEqual<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.IsNotNull(account.Address1_Composite);
            Assert.IsNotNull(account.LastOnHoldTime);
            Assert.IsNotNull(account.ModifiedOn);
            Assert.IsNotNull(account.CreatedOn);

            var json = JObject.FromObject(account, new JsonSerializer{ ContractResolver = new NavigationPropertyContractResolver() });
        }


    }
}
