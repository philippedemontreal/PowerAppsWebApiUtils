using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {

        [TestClass]
        public class IntegratedScenariosTests
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
            public void ScenariosTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();

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
            public async Task ScenariosTest2()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account =  new Account{ Name  = $"John Doe Ltd {Guid.NewGuid()}" };
                    account.Id = await context.Create(account);

                    var contact = new Contact { LastName = "John", FirstName = "Doe", ParentCustomerId = account.ToNavigationProperty() };
                    contact.Id = await context.Create(contact);

                    var parentaccount = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => p.ParentCustomerId).FirstOrDefault();

                    Task.WaitAll(context.Delete(contact), context.Delete(account));

                    Assert.AreEqual(account.Name, parentaccount.Name);
                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).FirstOrDefault();
                    Assert.IsNull(account);
                    contact = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).FirstOrDefault();
                    Assert.IsNull(contact);

                }
            }

            [TestMethod]
            public void ScenariosTest3()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = context.CreateQuery<Account>().Select(p => p.Id).ToList();
                }
            }

        }
    }
}