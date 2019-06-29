using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using app.Configuration;
using app.entities;
using app.Repositories;
using app.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using webapi.entities;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetOneTest()
        {
            var config = ConfigurationReader.GetConfiguration();
            var tokenProvider = new AuthenticationMessageHandler(config);

            var repo = new GenericRepository<Account>(tokenProvider);
            var entityId = Guid.Parse("BB7F2EEC-A38C-E911-A985-000D3AF49637");
            var account = repo.GetById(entityId, null
            //new Expression<Func<Account, object>>[]
            // {
            //     p => p.AccountId, 
            //     p => p.StateCode, 
            //     p => p.StatusCode,
            //     p => p.LastOnHoldTime,
            //     p => p.ModifiedOn,
            //     p => p.CreatedOn,
            //     p => p.CreatedBy,
            //     p => p.OwnerId,
            //     p => p.ParentAccountId,
            //     p => p.Telephone1,
            // }
            ).Result;
            Assert.IsNotNull(account);
            Assert.IsNotNull(account.CreatedBy);
            Assert.IsNotNull(account.OwnerId);
            Assert.AreEqual(entityId, account.AccountId);
            Assert.AreEqual<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.AreEqual<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.IsNull(account.LastOnHoldTime);
            Assert.IsNotNull(account.ModifiedOn);
            Assert.IsNotNull(account.CreatedOn);

            var json = JObject.FromObject(account);
        }

        [TestMethod]
        public void GetMultipleTest()
        {
            var config = ConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = new GenericRepository<Account>(tokenProvider))
            {
                var accounts = repo.GetList().Result;
                Assert.IsNotNull(accounts);
            }
        }

        
        [TestMethod]
        public async Task CreateAccountTest()
        {
            var config = ConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = new GenericRepository<Account>(tokenProvider))
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

                account = 
                    await repo.GetById(
                        accountid, 
                        new Expression<Func<Account, object>>[]
                        {
                            p => p.AccountId, 
                            p => p.StateCode, 
                            p => p.StatusCode,
                            p => p.LastOnHoldTime,
                            p => p.ModifiedOn,
                            p => p.CreatedOn,
                            p => p.CreatedBy,
                            p => p.OwnerId,
                            p => p.ParentAccountId,
                            p => p.Telephone1,
                        });
            }

        }

         [TestMethod]
        public async Task UpdateAdressParentAccountTest()
        {
            var config = ConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = new GenericRepository<CustomerAddress>(tokenProvider))
            {
                var address = 
                    new CustomerAddress(Guid.Parse("83ca70b4-0d9a-e911-a98c-000d3af49373"))
                    {
                        City = "Montreal",
                        AddressTypeCode = customeraddress_addresstypecode.BillTo,
                        ParentId = new Account(Guid.Parse("72e4bfa0-836a-e911-a98a-000d3af49373")).ToNavigationProperty(),
                    };

                 await repo.Update(address);
            }
        }
    
        [TestMethod]
        public async Task GetcustomerAddressesTest()
        {
            var config = ConfigurationReader.GetConfiguration();

            using (var tokenProvider = new AuthenticationMessageHandler(config))
            using(var repo = new GenericRepository<Account>(tokenProvider))
            {
                var addresses = await repo.GetList();
            }
        }

    }
}
