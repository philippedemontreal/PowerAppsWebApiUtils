using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {
        [TestClass]
        public class QueryToFirstOrDefaultExecutionTests
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
            public void ToFirstAndDefaultTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = context.CreateQuery<Account>().FirstOrDefault();
                    Assert.IsNotNull(account);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest2()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montréal")
                        .FirstOrDefault();
                    Assert.IsNotNull(account);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest2bis()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Where(p => p.StateCode == account_statecode.Active)
                        .FirstOrDefault();
                    
                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.AreEqual("Montréal", account.Address1_City);
                    Assert.AreEqual(account_statecode.Active, account.StateCode);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest3()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montréal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.IsNotNull(account.Address1_Composite);
                    Assert.IsTrue(account.Address1_Composite.Contains("Montréal"));
                }                    
            }

            [TestMethod]
            public void ToFirstAndDefaultTest4()
            {
               var context = serviceProvider.GetService<WebApiContext>();
                {
                    var query = context.CreateQuery<Account>();
                    var account = 
                        query                        
                        .Where(p => p.Address1_City == "Montréal")
                        .Select(p => new { Id = p.Id, AddressComposite = p.Address1_Composite, City = p.Address1_City, Fax = p.Address1_Fax })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.IsNotNull(account.AddressComposite);
                    Assert.IsTrue(account.City.Contains("Montréal"));
                }                    
            }

            [TestMethod]
            public void ToFirstAndDefaultTest5()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montréal")
                        .Select(p => new Account { Id = p.Id, Address1_City = p.Address1_City })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.AreEqual("Montréal", account.Address1_City);
                    Assert.IsNotNull(account.Address1_Composite);
                    Assert.IsTrue(account.Address1_Composite.Contains("Montréal"));
                }
            }
        }
    }
}