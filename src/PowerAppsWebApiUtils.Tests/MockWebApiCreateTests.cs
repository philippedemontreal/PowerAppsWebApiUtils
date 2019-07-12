using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Entities;
using PowerAppsWebApiUtils.Repositories;

namespace PowerAppsWebApiUtils.Tests
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        public string Content { get; private set; }
        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; set; }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            Content = await request.Content.ReadAsStringAsync();
            return await Task.FromResult(Response);
        }
    }

    [TestClass]
    public class MockWebApiCreateTests
    {       
        private static FakeHttpMessageHandler fakeHttpMessageHandler;
        private static IHttpClientFactory factory;
        
        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            factory = Substitute.For<IHttpClientFactory>();

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Add("OData-EntityId", $"/customeraddresses({Guid.NewGuid()})");

            fakeHttpMessageHandler = new FakeHttpMessageHandler { Response = response };
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://good.uri1")};
            
            factory.CreateClient(Arg.Any<string>()).Returns(fakeHttpClient);
        }

        [TestMethod]
        public async Task MockWebApiCreateTest1()
        {
            
            var repository =new GenericRepository(factory);
            var guid = Guid.NewGuid();

            await repository.Create(new CustomerAddress{ ParentId = new Account(guid).ToNavigationProperty() });
            
            Assert.AreEqual(HttpMethod.Post, fakeHttpMessageHandler.Request.Method);
            Assert.AreEqual($"{{\"parentid_account@odata.bind\":\"/accounts({guid})\"}}",  fakeHttpMessageHandler.Content);
        }

        [TestMethod]
        public async Task MockWebApiCreateTest2()
        {
            var repository =new GenericRepository(factory);
            var guid = Guid.NewGuid();

            await repository.Create(new CustomerAddress{ ParentId = new Contact(guid).ToNavigationProperty() });
            
            Assert.AreEqual(HttpMethod.Post, fakeHttpMessageHandler.Request.Method);            
            Assert.AreEqual($"{{\"parentid_contact@odata.bind\":\"/contacts({guid})\"}}", fakeHttpMessageHandler.Content);
        }   

        
        [TestMethod]
        public async Task MockWebApiCreateTest3()
        {
           
            var repository =new GenericRepository(factory);
            var parentcustomerid = Guid.NewGuid();
            var ownerid = Guid.NewGuid();

            await repository.Create(
                new Contact 
                { 
                    FirstName = "First Name", 
                    LastName="LastName", 
                    OwnerId = new NavigationProperty { LogicalCollectionName = "systemusers", EntityLogicalName = "systemuser", Id = ownerid },  
                    ParentCustomerId = new Account(parentcustomerid).ToNavigationProperty() 
                });
            
            Assert.AreEqual(HttpMethod.Post, fakeHttpMessageHandler.Request.Method);
            Assert.AreEqual($"{{\"firstname\":\"First Name\",\"lastname\":\"LastName\",\"ownerid_systemuser@odata.bind\":\"/systemusers({ownerid})\",\"parentcustomerid_account@odata.bind\":\"/accounts({parentcustomerid})\"}}", fakeHttpMessageHandler.Content);
        }       
   
    }
}
