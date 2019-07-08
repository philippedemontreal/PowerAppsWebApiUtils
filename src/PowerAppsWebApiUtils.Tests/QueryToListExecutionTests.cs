using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {
        [TestClass]
        public class QueryToListExecutionTests
        {
            [TestMethod]
            public void ToListTest1()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config.AuthenticationSettings))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var accounts = context.CreateQuery<Account>().ToList();
                    Assert.IsNotNull(accounts);
                }
            }

            [TestMethod]
            public void ToListTest2()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config.AuthenticationSettings))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .ToList();
                    Assert.IsNotNull(accounts);
                }
            }

            [TestMethod]
            public void ToListTest3()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config.AuthenticationSettings))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.IsNotNull(accounts);
                }                    
            }

            [TestMethod]
            public void ToListTest4()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config.AuthenticationSettings))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var query = context.CreateQuery<Account>();
                    var accounts = 
                        query                        
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.IsNotNull(accounts);
                    Assert.AreNotEqual(0, accounts.Count);
                }                    
            }

            [TestMethod]
            public void ToListTest5()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();

                using (var tokenProvider = new AuthenticationMessageHandler(config.AuthenticationSettings))
                using(var context = new WebApiContext(tokenProvider))
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new Account { Id = p.Id, Address1_City = p.Address1_City })
                        .ToList();

                    Assert.IsNotNull(accounts);
                }
            }
        }
    }
}