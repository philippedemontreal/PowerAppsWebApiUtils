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
            public async Task ScenariosTest1()
            {
                var context = serviceProvider.GetService<WebApiContext>();
                context.MSCRMCallerID = Guid.Parse("BFC5064F-C69E-42B4-884D-83E3A9900945");

                {
                    var accounts = context.CreateQuery<Account>().Where(p => p.Address1_City == "Montreal").Select(p => p.Id).ToList();
                    Assert.NotNull(accounts);
                    Assert.True(accounts.Count > 0);
                    await context.Update(new Account(accounts[0]) { Address1_City  = "San Francisco", Address1_Country = "USA" });

                    var account = context.CreateQuery<Account>().Where(p => p.Address1_City != "Montreal").FirstOrDefault();
                    await context.Update(new Account(account.Id) { Address1_City  = "Montreal", Address1_Country = "Canada" });

                    var id = account.Id;

                    var query = context.GetQueryText(context.CreateQuery<Account>().Where(p => p.Id == account.Id).Select(p => new Account(p.Id){ Address1_City = p.Address1_City, Address1_Country = p.Address1_Country }).Expression);
                    Assert.Equal($"accounts?$select=accountid,address1_city,address1_country&$filter=(accountid eq '{id}')", query);
                    account = context.CreateQuery<Account>().Where(p => p.Id == account.Id).Select(p => new Account(p.Id){ Address1_City = p.Address1_City, Address1_Country = p.Address1_Country }).FirstOrDefault();

                    Assert.NotNull(account);
                    Assert.Equal(id, account.Id);
                    Assert.Equal("Montreal", account.Address1_City);               
                    Assert.Equal("Canada", account.Address1_Country);               
                }
            }


            [Fact]
            public async Task ScenariosTest2()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    context.MSCRMCallerID = Guid.Parse("BFC5064F-C69E-42B4-884D-83E3A9900945");
                    var account =  new Account{ Name  = $"John Doe Ltd {Guid.NewGuid()}" };
                    account.Id = await context.Create(account);

                    var contact = new Contact { LastName = "John", FirstName = "Doe", ParentCustomerId = account.ToNavigationProperty() };
                    contact.Id = await context.Create(contact);

                    var parentaccount = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => p.ParentCustomerId).FirstOrDefault();

                    await context.Delete(contact);
                    await context.Delete(account);

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
                        
            [Fact]
            public  async Task ScenariosTest5()
            {

                var context = serviceProvider.GetService<WebApiContext>();
                {
                    context.CallerObjectId = Guid.Parse("cabfad08-d910-4c38-9873-ee9bec791238");
                   //var account = context.CreateQuery<Account>().FirstOrDefault();
                    //await context.Create(new CustomerAddress{ ParentId = account.ToNavigationProperty() });
                     
                    var parent = new new_parent{ new_name = "Test"};
                    parent.Id = await context.Create(parent);
                    var child = new new_child { new_name = "Test", new_parentId = parent.ToNavigationProperty(), new_parent = parent.ToNavigationProperty() };
                    child.Id = await context.Create(child);
                    var result = context.CreateQuery<new_child>().Where(p => p.new_parentId == parent.ToNavigationProperty()).FirstOrDefault();
                    await context.Diassociate<new_child>(result, p => p.new_parentId);
                    Assert.NotNull(result);
                    Assert.NotNull(result.new_parentId);
                }
            }


            [Fact]
            public  async Task ScenariosTest6()
            {

                using (var context = serviceProvider.GetService<WebApiContext>())
                {                   
                    context.CallerObjectId = Guid.Parse("cabfad08-d910-4c38-9873-ee9bec791238");
                    var account = 
                        new Account { 
                        Name = "JB Petroleum Inc.", 
                        NumberOfEmployees = 6, 
                        IndustryCode = account_industrycode.PetrochemicalExtractionandDistribution, 
                        Revenue = 100000m,
                        CreditLimit  = 250000m,
                        CustomerTypeCode = account_customertypecode.Reseller,
                        };

                    
                    account.Id = await context.Create(account);
                    var contact = 
                        new Contact {
                            FirstName = "J",
                            LastName = "B",
                            ParentCustomerId = null,
                            AccountRoleCode = contact_accountrolecode.DecisionMaker,
                            Address1_Line1 = "1450 JB Street",
                            Address1_Country = "Canada",
                        };
                    await context.Create(contact);  

                    contact = 
                        new Contact {
                            FirstName = "J",
                            LastName = "B",
                            ParentCustomerId = account.ToNavigationProperty(),
                            AccountRoleCode = contact_accountrolecode.DecisionMaker,
                            Address1_Line1 = "1450 JB Street",
                            Address1_Country = "Canada",
                        };
                    contact.Id = await context.Create(contact);
                    var parentcustomerid = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => p.ParentCustomerId).FirstOrDefault();
                    Assert.NotNull(parentcustomerid);
                    Assert.Equal(account.Id, parentcustomerid.Id);

                    await context.Update(new Contact(contact.Id){ ParentCustomerId = null });
                    contact = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => new Contact(p.Id) { ParentCustomerId = p.ParentCustomerId  }).FirstOrDefault();
                    Assert.NotNull(contact);
                    Assert.Null(contact.ParentCustomerId);



                    await context.Update(new Contact(contact.Id){ ParentCustomerId = account.ToNavigationProperty() });
                    contact = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => new Contact(p.Id) { ParentCustomerId = p.ParentCustomerId  }).FirstOrDefault();
                    Assert.NotNull(contact);
                    Assert.NotNull(contact.ParentCustomerId);
                    Assert.Equal(account.Id, contact.ParentCustomerId.Id);



                    // await context.Diassociate<Contact>(contact, p => p.ParentCustomerId);
                    // parentcustomerid = context.CreateQuery<Contact>().Where(p => p.Id == contact.Id).Select(p => p.ParentCustomerId).FirstOrDefault();
                    // Assert.Null(parentcustomerid);
                    
                }
            }


        }
    }
}