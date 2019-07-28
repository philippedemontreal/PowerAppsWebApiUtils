# This is an API to work with PowerApps and Dynamics 365 Web API

Design goals include

Early bound classes for web api
Simple Linq provider for web api.

Targets .netcore 2.0 and (soon) .net framework 4.7
Integrate well with Azure Active Directory 

Feedback and contributions welcome...


Examples of supported Linq queries:
=================================================================

To get the service context:
var config = 
    new PowerAppsAuthenticationSettings                     
    {
        ClientId = configuration["clientId"],
        ClientSecret = configuration["secret"],
        DirectoryId = configuration["directory"],
        ApiUrl = configuration["powerappsApiUrl"],
        ResourceUrl = configuration["powerappsUrl"],  
    };

serviceProvider = 
    new ServiceCollection()
    .AddPowerAppsWebApiConfiguration(config)
    .BuildServiceProvider();

var context = serviceProvider.GetService<WebApiContext>();


To create a new query:

To create an entity:
///
await context.Create(new CustomerAddress{ ParentId = new Account(Guid.NewGuid()).ToNavigationProperty() });
///
await context.Create(
    new Contact 
    { 
        FirstName = "First Name", 
        LastName="LastName", 
        OwnerId = new NavigationProperty { LogicalCollectionName = "systemusers", EntityLogicalName = "systemuser", Id = ownerid },  
        ParentCustomerId = new Account(parentcustomerid).ToNavigationProperty() 
    });

To delete an entity:
contact = new Contact(Guid.NewGuid());
await context.Delete(contact);

To select, filter and order:

context.CreateQuery<Account>().Where(p => p.Address1_Country  == "Canada").Select(p => new Account(p.Id) { Name = p.Name}).OrderBy(p => p.Name).ToList();
context.CreateQuery<Account>().ToList(); 
context.CreateQuery<Account>().FirstOrDefault(); 
context.CreateQuery<Account>().Where(p => p.Name == "Test").ToList(); 
context.CreateQuery<Account>().Where(p => p.Name == "Test").FirstOrDefault();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).FirstOrDefault();
context.CreateQuery<Account>().Where(p => p.Name == "Test").Where(p => p.StateCode == account_statecode.Active);
context.CreateQuery<Account>().Where(p => p.Name == "Toto" || p.Name == "Tata").Where(p => p.StateCode == account_statecode.Active).ToList();
context.CreateQuery<Account>().Where(p => p.Name == "Toto" || p.Name == "Tata").Where(p => p.StateCode == account_statecode.Active).FirstOrDefault();

context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Where(p => p.ShippingMethodCode == customeraddress_shippingmethodcode.Airborne && p.Country == "Canada").ToList();
context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Where(p => p.ShippingMethodCode == customeraddress_shippingmethodcode.Airborne && p.Country == "Canada").FirstOrDefault();
context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Select(p => new { Id = p.Id, OwnerId = p.OwnerId }).ToList();
context.CreateQuery<CustomerAddress>().Where(p => p.ParentId == new Account(Guid.NewGuid()).ToNavigationProperty()).Select(p => new { Id = p.Id, OwnerId = p.OwnerId }).FirstOrDefault();

context.CreateQuery<Account>().OrderByDescending(p => p.Name).ToList();
context.CreateQuery<Account>().OrderByDescending(p => new {p.Address1_City, p.Name} ).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderBy(p => p.Name).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderBy(p => new {p.Address1_City, p.Name}).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => new {p.Address1_City, p.Name}).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).OrderBy(p => p.Name).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).OrderByDescending(p => p.Name).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy }).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode }).OrderByDescending(p => p.Name).ToList();
context.CreateQuery<Account>().Where(p => p.Name == "John").Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode }).ToList();
context.CreateQuery<Account>().Where(p => p.Name == "John" || p.Name == "Doe").Where(p => p.StateCode == account_statecode.Active).OrderByDescending(p => p.Name).Select(p => new { Id = p.Id, CustomerTypeCode = p.CustomerTypeCode, ExchangeRate = p.ExchangeRate }).ToList();
context.CreateQuery<Account>().Where(p => p.StateCode == account_statecode.Active).Select(p => new { Id = p.Id, CreatedBy = p.CreatedBy }).ToList();


=================================================================
You can still override the webapi request using the following method for operation not supported by webapi or use the async/await pattern:

var repository = serviceProvider.GetRequiredService<GenericRepository<Contact>>();
var querystring = $"{config.ApiUrl}{Contact.CollectionName}?$select=firstname,lastname,_parentcustomerid_value,address1_city&$filter=address1_city eq 'Redmond'&$orderby=lastname";
var contacts = await repository.RetrieveMultiple(querystring);
      

