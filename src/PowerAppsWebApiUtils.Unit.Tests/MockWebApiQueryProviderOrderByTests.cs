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
    public class MockWebApiQueryProviderOrderByTests
    {
        [Fact]
        public async Task WebApiQueryProviderOrderByTest1()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).OrderBy(p => p.Name).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=name asc");
        }

        [Fact]
        public async Task WebApiQueryProviderOrderByTest1bis()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).OrderBy(p => new {p.Address1_City, p.Name} ).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=address1_city asc, name asc");
        }


        [Fact]
        public async Task WebApiQueryProviderOrderByTest2()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).Where(p => p.StateCode == account_statecode.Active).OrderBy(p => p.Name).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=name asc&$filter=(statecode eq 0)");
        }  

        [Fact]
        public async Task WebApiQueryProviderOrderByTest2bis()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);

            var provider = new WebApiQueryProvider(serviceProvider);
            var query = new Query<Account>(provider).Where(p => p.StateCode == account_statecode.Active).OrderBy(p => new {p.Address1_City, p.Name}).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=address1_city asc, name asc&$filter=(statecode eq 0)");
        }

        [Fact]
        public async Task WebApiQueryProviderOrderByTest3()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.StateCode == account_statecode.Active)
                .Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy })
                .OrderBy(p => p.Name).ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=name asc&$select=name,accountid,_createdby_value&$filter=(statecode eq 0)");
        }   
        [Fact]
        public async Task WebApiQueryProviderOrderByTest3bis()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.StateCode == account_statecode.Active)
                .OrderBy(p => p.Name)
                .Select(p => new { Name = p.Name, Id = p.Id, CreatedBy = p.CreatedBy })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=name,accountid,_createdby_value&$orderby=name asc&$filter=(statecode eq 0)");
        }   
        [Fact]
        public async Task WebApiQueryProviderOrderByTest4()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.StateCode == account_statecode.Active)
                .Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode })
                .OrderBy(p => p.Name)
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$orderby=name asc&$select=accountid,customertypecode&$filter=(statecode eq 0)");
        }        
        [Fact]
        public async Task WebApiQueryProviderOrderByTest4BIS()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var repository =Substitute.For<GenericRepository<Account>>();

            repository.RetrieveMultiple(Arg.Any<string>()).Returns(Task.FromResult(new List<Account>()));
            serviceProvider.GetService(Arg.Any<Type>()).Returns(repository);
            
            var provider = new WebApiQueryProvider(serviceProvider);
            var query = 
                new Query<Account>(provider)
                .Where(p => p.StateCode == account_statecode.Active)
                .OrderBy(p => p.Name)
                .Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode&$orderby=name asc&$filter=(statecode eq 0)");
        }   
        [Fact]
        public async Task WebApiQueryProviderOrderByTest5()
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
                .OrderBy(p => p.Name)                
                .Select(p => new Account{ Id = p.Id, CustomerTypeCode = p.CustomerTypeCode })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode&$orderby=name asc&$filter=(name eq 'John') and (statecode eq 0)");
        }  

        [Fact]
        public async Task WebApiQueryProviderOrderByTest6()
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
                .OrderBy(p => p.Name)                
                .Select(p => new { Id = p.Id, CustomerTypeCode = p.CustomerTypeCode, ExchangeRate = p.ExchangeRate })
                .ToList();

            await repository.Received().RetrieveMultiple($"{Account.CollectionName}?$select=accountid,customertypecode,exchangerate&$orderby=name asc&$filter=(name eq 'John' or name eq 'Doe') and (statecode eq 0)");
        }    
  
        [Fact]
        public async Task WebApiQueryProviderOrderByTest7()
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
                .OrderBy(p => p.Name)                
                .Select(p => new { Id = p.Id, ParentId = p.ParentId, ExchangeRate = p.ExchangeRate })
                .ToList();

            await repository.Received().RetrieveMultiple($"{CustomerAddress.CollectionName}?$select=customeraddressid,_parentid_value,exchangerate&$orderby=name asc&$filter=(_parentid_value eq '{guid}')");
        }    

    }
}
