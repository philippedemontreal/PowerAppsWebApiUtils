using System;
using System.Linq.Expressions;
using app.Configuration;
using app.Repositories;
using app.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using webapi.entities;

namespace tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetOneTest()
        {
            var config = ConfigurationReader.GetConfiguration();
            var repo = new GenericRepository<Account>(config, Account.LogicalCollectionName);
            var entityId = Guid.Parse("BB7F2EEC-A38C-E911-A985-000D3AF49637");
            var account = repo.GetById(entityId, 
            new Expression<Func<Account, object>>[]
            {
                p => p.AccountId, 
                p => p.StateCode, 
                p => p.StatusCode,
                p => p.LastOnHoldTime,
                p => p.ModifiedOn,
                p => p.CreatedOn,
                p => p.CreatedBy,
                p => p.OwnerId,
                //p => p.OwnerIdType,
                p => p.ParentAccountId,
                p => p.Telephone1,
            }
            ).Result;
            Assert.IsNotNull(account);
            Assert.IsNotNull(account.OwnerId);
            //Assert.IsNotNull(account.OwnerIdType);
            Assert.AreEqual(entityId, account.AccountId);
            Assert.AreEqual<account_statecode?>(account_statecode.Active, account.StateCode);            
            Assert.AreEqual<account_statuscode?>(account_statuscode.Active, account.StatusCode);            
            Assert.IsNull(account.LastOnHoldTime);
            Assert.IsNotNull(account.ModifiedOn);
            Assert.IsNotNull(account.CreatedOn);
        }

                [TestMethod]
        public void GetMultipleTest()
        {
            var config = ConfigurationReader.GetConfiguration();
            var repo = new GenericRepository<Account>(config, "accounts");
            var accounts = repo.GetList().Result;
            Assert.IsNotNull(accounts);
        }
    }
}
