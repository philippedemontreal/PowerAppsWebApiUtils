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
        public class QueryToListExecutionTests
        {
            private static ServiceProvider serviceProvider;
                        
            static QueryToListExecutionTests()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();
                serviceProvider = 
                    new ServiceCollection()
                    .AddPowerAppsWebApiConfiguration(config)
                    .BuildServiceProvider();
            } 

            [Fact]
            public void ToListTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = context.CreateQuery<Account>().ToList();
                    Assert.NotNull(accounts);
                }
            }

            [Fact]
            public void ToListTest2()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .ToList();
                    Assert.NotNull(accounts);
                }
            }

            [Fact]
            public void ToListTest3()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.NotNull(accounts);
                }                    
            }

            [Fact]
            public void ToListTest4()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var query = context.CreateQuery<Account>();
                    var accounts = 
                        query                        
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.NotNull(accounts);
                    Assert.NotEmpty(accounts);
                }                    
            }

            [Fact]
            public void ToListContainsTest4()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var query = context.CreateQuery<Account>();
                    var accounts = 
                        query                        
                        .Where(p => p.Address1_City.Contains("Mont"))
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.NotNull(accounts);
                    Assert.NotEmpty(accounts);
                }                    
            }

            [Fact]
            public void ToListStartWithsTest4()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var query = context.CreateQuery<Account>();
                    var accounts = 
                        query                        
                        .Where(p => p.Address1_City.StartsWith("Mont"))
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.NotNull(accounts);
                    Assert.NotEmpty(accounts);
                }                    
            }

            [Fact]
            public void ToListEndsWithTest4()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var query = context.CreateQuery<Account>();
                    var accounts = 
                        query                        
                        .Where(p => p.Address1_City.EndsWith("al"))
                        .Select(p => new { Id = p.Id, Address1_Composite = p.Address1_Composite, Address1_Fax = p.Address1_Fax })
                        .ToList();

                    Assert.NotNull(accounts);
                    Assert.NotEmpty(accounts);
                }                    
            }
            [Fact]
            public void ToListTest5()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = 
                        context
                        .CreateQuery<Account>()
                        .Where(p => p.Address1_City == "Montreal")
                        .Select(p => new Account { Id = p.Id, Address1_City = p.Address1_City })
                        .ToList();

                    Assert.NotNull(accounts);
                }
            }
        }
    }
}