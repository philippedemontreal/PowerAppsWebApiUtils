using System;
using System.Linq;
using System.Threading.Tasks;
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

        public class IntegratedScenariosTests
        {
            private static ServiceProvider serviceProvider;
            
            static IntegratedScenariosTests()
            {
                var config = PowerAppsConfigurationReader.GetConfiguration();
                serviceProvider = 
                    new ServiceCollection()
                    .AddPowerAppsWebApiConfiguration(config)
                    .BuildServiceProvider();
            } 

            [Fact]
            public void ScenariosTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();

                {
                    var accounts = context.CreateQuery<Account>().Where(p => p.Address1_City == "Montreal").Select(p => p.Id).ToList();
                    Assert.NotNull(accounts);
                    Assert.True(accounts.Count > 0);

                    var account = context.CreateQuery<Account>().Where(p => p.Address1_City == "Montreal").FirstOrDefault();
                    var updatedAccount = new Account(account.Id) { Address1_Country  = "Canada" };
                    context.Update(updatedAccount);

                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).Select(p => new Account{ Address1_Country = p.Address1_Country }).FirstOrDefault();
                    Assert.Equal("Canada", account.Address1_Country);               
                }
            }


            [Fact]
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

                    Assert.Equal(account.Name, parentaccount.Name);
                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).FirstOrDefault();
                    Assert.Null(account);
                    contact = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).FirstOrDefault();
                    Assert.Null(contact);

                }
            }

            [Fact]
            public void ScenariosTest3()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    var accounts = context.CreateQuery<Account>().Select(p => p.Id).ToList();
                }
            }

            
            [Fact]
            public void ScenariosTest4()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    object result;
                    result = context.CreateQuery<Account>().Where(p => p.Address1_Country  == "Canada").Select(p => new Account(p.Id) { Name = p.Name}).OrderBy(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().ToList(); 
                    result = context.CreateQuery<Account>().FirstOrDefault(); 
                    result = context.CreateQuery<Account>().Where(p => p.Name == "Test").ToList(); 
                    result = context.CreateQuery<Account>().Where(p => p.Name == "Test").FirstOrDefault();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).FirstOrDefault();
                    result = context.CreateQuery<Account>().Where(p => p.Name == "Test").Where(p => p.StateCode == account_statecode.Active);
                    result = context.CreateQuery<Account>().Where(p => p.Name == "Toto" || p.Name == "Tata").Where(p => p.StateCode == account_statecode.Active).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.Name == "Toto" || p.Name == "Tata").Where(p => p.StateCode == account_statecode.Active).FirstOrDefault();

                    result = context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Where(p => p.ShippingMethodCode == customeraddress_shippingmethodcode.Airborne && p.Country == "Canada").ToList();
                    result = context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Where(p => p.ShippingMethodCode == customeraddress_shippingmethodcode.Airborne && p.Country == "Canada").FirstOrDefault();
                    result = context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Select(p => new { Id = p.Id, OwnerId = p.OwnerId }).ToList();
                    result = context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Select(p => new { Id = p.Id, OwnerId = p.OwnerId }).FirstOrDefault();

                    result = context.CreateQuery<Account>().OrderByDescending(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().OrderByDescending(p => new {p.Address1_City, p.Name} ).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderBy(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderBy(p => new {p.Address1_City, p.Name}).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => new {p.Address1_City, p.Name}).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).OrderBy(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).OrderByDescending(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode }).OrderByDescending(p => p.Name).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.Name == "John").Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode }).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.Name == "John" || p.Name == "Doe").Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new { Id = p.Id, CustomerTypeCode = p.CustomerTypeCode, ExchangeRate = p.ExchangeRate }).ToList();
                    result = context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Id = p.Id, CreatedBy = p.CreatedBy }).ToList();


                }
            }


        }
    }
}