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
using PowerAppsWebApiUtils.Security;
using Xunit;

namespace PowerAppsWebApiUtils.Tests
{
    public class GenericRepositoryTests
    {
        private static ServiceProvider serviceProvider;
        
        static GenericRepositoryTests()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();
            serviceProvider = 
                new ServiceCollection()
                .AddWebApiContext(config)
                .BuildServiceProvider();
        }

        [Fact]
        public async Task GetOneTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();
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
                    Name = p.Name,
                });

            Assert.NotNull(account);
            Assert.NotNull(account.Name);
            Assert.NotNull(account.CreatedBy);
            Assert.NotNull(account.OwnerId);
            Assert.Equal(entityId, account.Id);
            Assert.Equal<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.Equal<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.NotNull(account.LastOnHoldTime);
            Assert.NotNull(account.ModifiedOn);
            Assert.NotNull(account.CreatedOn);

            var json = JObject.FromObject(account, new JsonSerializer{ ContractResolver = new NavigationPropertyContractResolver() });
        }

        [Fact]
        public async Task GetMultipleTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = serviceProvider.GetService<GenericRepository<Account>>())
            {
                var accounts = await repo.GetList();
                Assert.NotNull(accounts);
            }
        }

        
        [Fact]
        public async Task CreateAccountTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = serviceProvider.GetService<GenericRepository<Account>>())
            {
                var account = new Account 
                {
                    Name = Guid.NewGuid().ToString(),
                    AccountCategoryCode = account_accountcategorycode.Standard,
                    AccountClassificationCode = account_accountclassificationcode.DefaultValue,
                    AccountRatingCode = account_accountratingcode.DefaultValue,
                    AccountNumber = "11111111",
                    Address1_AddressTypeCode = account_address1_addresstypecode.Primary,
                    Address1_City = "Montreal",
                    Address1_Country = "Canada",
                    Address1_PostalCode = "H1H 1H1",
                    Address1_StateOrProvince = "QC",
                    DoNotEMail = true,
                    DoNotPhone = false,
                    CreditLimit = 500000.99m,
                    EMailAddress1 = string.Empty,
                    Telephone1 = "Telephone1",
                    Fax = "Fax",
                    WebSiteURL = "WebSiteURL",
                    LastOnHoldTime = new DateTime(2019, 1, 1, 0, 0, 0)
                };  

                var accountid = await repo.Create(account);

                Assert.NotEqual(Guid.Empty, accountid);
                account = 
                    await repo.GetById(
                        accountid,
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
                        });

                var owner = account.OwnerId;
            }

        }

         [Fact]
        public async Task UpdateAdressParentAccountTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = serviceProvider.GetService<GenericRepository<Account>>())
            {
                var address = 
                    new CustomerAddress(Guid.Parse("83ca70b4-0d9a-e911-a98c-000d3af49373"))
                    {
                        City = "Montreal",
                        AddressTypeCode = customeraddress_addresstypecode.BillTo,
                        ParentId = new Account(Guid.Parse("48cf55d9-6e9f-e911-a982-000d3af3b3af")).ToNavigationProperty(),
                    };

                 await repo.Update(address);
            }
        }
    
        [Fact]
        public async Task GetcustomerAddressesTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = serviceProvider.GetService<GenericRepository<Account>>())
            {
                var addresses = await repo.GetList();
            }
        }

    }
}
