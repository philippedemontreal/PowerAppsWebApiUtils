using System;
using System.Linq;
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
        public class QueryToFirstOrDefaultExecutionTests
        {
            [TestMethod]
            public void ToFirstAndDefaultTest1()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account = context.CreateQuery<Account>().FirstOrDefault();
                    Assert.IsNotNull(account);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest2()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .FirstOrDefault();
                    Assert.IsNotNull(account);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest2bis()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Where(p => p.StateCode == account_statecode.Active)
                        .FirstOrDefault();
                    
                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.AreEqual("Montreal", account.Address1_City);
                    Assert.AreEqual(account_statecode.Active, account.StateCode);
                }
            }

            [TestMethod]
            public void ToFirstAndDefaultTest3()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.IsNotNull(account.Address1_Composite);
                    Assert.IsTrue(account.Address1_Composite.Contains("Montreal"));
                }                    
            }

            [TestMethod]
            public void ToFirstAndDefaultTest4()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var query = context.CreateQuery<Account>();
                    var account = 
                        query                        
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.IsNotNull(account.Address1_Composite);
                    Assert.IsTrue(account.Address1_Composite.Contains("Montreal"));
                }                    
            }

            [TestMethod]
            public void ToFirstAndDefaultTest5()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new Account { Id = p.Id, Address1_City = p.Address1_City })
                        .FirstOrDefault();

                    Assert.IsNotNull(account);
                    Assert.IsNotNull(account.Id);
                    Assert.AreEqual("Montreal", account.Address1_City);
                    Assert.IsNotNull(account.Address1_Composite);
                    Assert.IsTrue(account.Address1_Composite.Contains("Montreal"));
                }
            }
        }
    }
}