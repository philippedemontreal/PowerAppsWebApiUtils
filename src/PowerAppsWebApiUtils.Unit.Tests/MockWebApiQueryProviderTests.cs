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
    public class MockWebApiQueryProviderTests
    {
        [Fact]
        public async Task WebApiQueryProviderTest1()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).ToList();

            await repository.Received().RetrieveMultiple(Account.CollectionName);
        }

        [Fact]
        public async Task WebApiQueryProviderTest2()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).Where(p => p.StateCode == account_statecode.Active).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$filter=(statecode eq 0)");
        }  

        [Fact]
        public async Task WebApiQueryProviderTest3()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).Where(p => p.StateCode == account_statecode.Active).Select(p => new { Id = p.Id, CreatedBy = p.CreatedBy }).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,_createdby_value&$filter=(statecode eq 0)");
        }   

        [Fact]
        public async Task WebApiQueryProviderTest4()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).Where(p => p.StateCode == account_statecode.Active).Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode }).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode&$filter=(statecode eq 0)");
        }        

        [Fact]
        public async Task WebApiQueryProviderTest5()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.Name == "John")
                .Where(p => p.StateCode == account_statecode.Active)
                .Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode&$filter=(name eq 'John') and (statecode eq 0)");
        }  

        [Fact]
        public async Task WebApiQueryProviderTest6()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.Name == "John" || p.Name == "Doe")
                .Where(p => p.StateCode == account_statecode.Active)
                .Select(p => new { Id = p.Id, CustomerTypeCode = p.CustomerTypeCode, ExchangeRate = p.ExchangeRate })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode,exchangerate&$filter=(name eq 'John' or name eq 'Doe') and (statecode eq 0)");
        }    
  
        [Fact]
        public async Task WebApiQueryProviderTest7()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository = Substitute.For<GenericRepository<CustomerAddress>>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<CustomerAddress>()));
            
            var guid = Guid.NewGuid();

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<CustomerAddress>(provider)
                .Where(p => p.ParentId == new Account(guid).ToNavigationProperty())
                .Select(p => new { Id = p.Id, ParentId = p.ParentId, ExchangeRate = p.ExchangeRate })
                .ToList();

            await repository.Received().RetrieveMultiple($"{CustomerAddress.CollectionName}?$select=customeraddressid,_parentid_value,exchangerate&$filter=(_parentid_value eq '{guid}')");
        }    
    }
}
