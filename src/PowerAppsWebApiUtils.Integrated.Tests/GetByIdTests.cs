using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;
using PowerAppsWebApiUtils.Json;
using PowerAppsWebApiUtils.Repositories;
using Xunit;

namespace PowerAppsWebApiUtils.Tests
{
    public class GetByIdTests
    {
        [Fact]
        public async Task GetOneTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();
            var serviceProvider = 
                new ServiceCollection()
                .AddPowerAppsWebApiConfiguration(config)
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<GenericRepository<Account>>();
            var list = await repo.GetList();
            if (list.Count == 0)
                return;

            var entityId =list[0].Id;
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

            Assert.NotNull(account);
            Assert.NotNull(account.Name);
            Assert.NotNull(account.CreatedBy);
            Assert.NotNull(account.OwnerId);
            Assert.Equal(entityId, account.Id);
            Assert.Equal<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.Equal<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.NotNull(account.Address1_Composite);
            Assert.NotNull(account.LastOnHoldTime);
            Assert.NotNull(account.ModifiedOn);
            Assert.NotNull(account.CreatedOn);

            var json = JObject.FromObject(account, new JsonSerializer{ ContractResolver = new NavigationPropertyContractResolver() });
        }


    }
}
