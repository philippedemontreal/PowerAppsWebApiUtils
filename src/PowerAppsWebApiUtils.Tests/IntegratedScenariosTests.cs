using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Security;
using webapi.entities;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {
        [TestClass]
        public class IntegratedScenariosTests
        {
            [TestMethod]
            public void ScenariosTest1()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var accounts = context.CreateQuery<Account>().Where(p => p.Address1_City == "Montreal").Select(p => p.Id).ToList();
                    Assert.IsNotNull(accounts);
                    Assert.IsTrue(accounts.Count > 0);

                    var account = context.CreateQuery<Account>().Where(p => p.Address1_City == "Montreal").FirstOrDefault();
                    var updatedAccount = new Account(account.Id) { Address1_City  = "Montréal" };
                    context.Update(updatedAccount);

                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).Select(p => new Account{ Address1_City = p.Address1_City }).FirstOrDefault();
                    Assert.AreEqual("Montréal", account.Address1_City);               
                }
            }


            [TestMethod]
            public async Task ScenariosTest12()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account =  new Account{ Name  = $"John Doe Ltd {Guid.NewGuid()}" };
                    account.Id = await context.Create(account);

                    var contact = new Contact { LastName = "John", FirstName = "Doe", ParentCustomerId = account.ToNavigationProperty() };
                    contact.Id = await context.Create(contact);

                    var parentaccount = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => p.ParentCustomerId).FirstOrDefault();

                    await context.Delete(contact);
                    await context.Delete(account);

                    Assert.AreEqual(account.Name, parentaccount.Name);
                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).FirstOrDefault();
                    Assert.IsNull(account);
                    contact = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).FirstOrDefault();
                    Assert.IsNull(contact);

                }
            }

        }
    }
}