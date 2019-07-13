using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Client;
using PowerAppsWebApiUtils.Configuration;
using PowerAppsWebApiUtils.Extensions;
using Xunit;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {
        public class QueryToFirstOrDefaultExecutionTests
        {
            private static ServiceProvider serviceProvider;
                        
            static QueryToFirstOrDefaultExecutionTests()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();
                serviceProvider = 
                    new ServiceCollection()
                    .AddWebApiContext(config)
                    .BuildServiceProvider();
            } 

            [Fact]
            public void ToFirstAndDefaultTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = context.CreateQuery<Account>().FirstOrDefault();
                    Assert.NotNull(account);
                }
            }

            [Fact]
            public void ToFirstAndDefaultTest2()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var account = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montréal")
                        .FirstOrDefault();
                    Assert.NotNull(account);
                }
            }

            [Fact]
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
                    
                    Assert.NotNull(account);
                    Assert.NotEqual(Guid.Empty, account.Id);
                    Assert.Equal("Montréal", account.Address1_City);
                    Assert.Equal(account_statecode.Active, account.StateCode);
                }
            }

            [Fact]
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

                    Assert.NotNull(account);
                    Assert.NotEqual(Guid.Empty, account.Id);
                    Assert.NotNull(account.Address1_Composite);
                    Assert.Contains("Montréal", account.Address1_Composite);
                }                    
            }

            [Fact]
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

                    Assert.NotNull(account);
                    Assert.NotEqual(Guid.Empty, account.Id);
                    Assert.NotNull(account.AddressComposite);
                    Assert.Contains("Montréal", account.City);
                }                    
            }

            [Fact]
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

                    Assert.NotNull(account);
                    Assert.NotEqual(Guid.Empty, account.Id);
                    Assert.Equal("Montréal", account.Address1_City);
                    Assert.NotNull(account.Address1_Composite);
                    Assert.Contains("Montréal", account.Address1_Composite);
                }
            }
        }
    }
}