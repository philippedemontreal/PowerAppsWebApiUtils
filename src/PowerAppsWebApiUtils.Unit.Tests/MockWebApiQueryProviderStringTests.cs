using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using PawauBeta01.Data;
using PowerAppsWebApiUtils.Linq;
using PowerAppsWebApiUtils.Repositories;
using Xunit;

namespace PowerAppsWebApiUtils.Tests
{
    public class MockWebApiQueryProviderStringTests
    {    
        [Fact]
        public async Task MockWebApiQueryProviderStartWithTest1()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Contact>(provider)
                .Where(p => p.LastName.StartsWith("test"))
                .Select(p => new { Id = p.Id })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Contact.CollectionName}?$select=contactid&$filter=(startswith(lastname,'test'))");
        }   
        
        [Fact]
        public async Task MockWebApiQueryProviderEndsWithTest1()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Contact>(provider)
                .Where(p => p.LastName.EndsWith("test"))
                .Select(p => new { Id = p.Id })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Contact.CollectionName}?$select=contactid&$filter=(endswith(lastname,'test'))");
        }  
        [Fact]
        public async Task MockWebApiQueryProviderContainsTest1()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Contact>(provider)
                .Where(p => p.LastName.Contains("test"))
                .Select(p => new { Id = p.Id })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Contact.CollectionName}?$select=contactid&$filter=(contains(lastname,'test'))");
        }   
           
                
        [Fact]
        public async Task MockWebApiQueryProviderStartWithTest2()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Contact>(provider)
                .Where(p => p.FirstName == "John" && p.LastName.StartsWith("Doe"))
                .Select(p => new { Id = p.Id })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Contact.CollectionName}?$select=contactid&$filter=(firstname eq 'John' and startswith(lastname,'Doe'))");
        }    
                
        [Fact]
        public async Task MockWebApiQueryProviderStartWithTest3()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Contact>(provider)
                .Where(p => p.FirstName == "John" || p.LastName.StartsWith("Doe"))
                .Select(p => new { Id = p.Id })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Contact.CollectionName}?$select=contactid&$filter=(firstname eq 'John' or startswith(lastname,'Doe'))");
        }  
    }
}
