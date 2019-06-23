using System;
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
            var repo = new GenericRepository<Account>(config, "accounts");
            var account = repo.GetById(Guid.Parse("BB7F2EEC-A38C-E911-A985-000D3AF49637")).Result;
            Assert.IsNotNull(account);
            Assert.AreEqual(account_statecode.Active, account.StateCode);            
            Assert.AreNotEqual(Guid.Empty, account.AccountId);
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
