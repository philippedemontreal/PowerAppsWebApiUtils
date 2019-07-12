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
    public class GenericRepositoryTests
    {
        private static ServiceProvider serviceProvider;
        
        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();
            serviceProvider = 
                new ServiceCollection()
                .AddWebApiContext(config)
                .BuildServiceProvider();
        }

        [TestMethod]
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

            Assert.IsNotNull(account);
            Assert.IsNotNull(account.Name);
            Assert.IsNotNull(account.CreatedBy);
            Assert.IsNotNull(account.OwnerId);
            Assert.AreEqual(entityId, account.Id);
            Assert.AreEqual<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.AreEqual<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.IsNotNull(account.LastOnHoldTime);
            Assert.IsNotNull(account.ModifiedOn);
            Assert.IsNotNull(account.CreatedOn);

            var json = JObject.FromObject(account, new JsonSerializer{ ContractResolver = new NavigationPropertyContractResolver() });
        }

        [TestMethod]
        public async Task GetMultipleTest()
        {
            var config = PowerAppsConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = serviceProvider.GetService<GenericRepository<Account>>())
            {
                var accounts = await repo.GetList();
                Assert.IsNotNull(accounts);
            }
        }

        
        [TestMethod]
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

                Assert.IsNotNull(accountid);
                //var 
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

         [TestMethod]
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
    
        [TestMethod]
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
