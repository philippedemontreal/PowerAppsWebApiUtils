using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerAppsWebApiUtils.Linq;
using webapi.entities;

namespace PowerAppsWebApiUtils.Tests
{
    namespace PowerAppsWebApiUtils.Tests
    {
        [TestClass]
        public class LinqProviderQueryTextTests
        {
            [TestMethod]
            public void Test1()
            {
                var context = new WebApiContext(null);
                var query = new Query<Account>(context);
                var command = context.GetQueryText(query.Expression);

                Assert.AreEqual("accounts", command);               
            }

            [TestMethod]
            public void Test2()
            {
                var context = new WebApiContext(null);
                var query = new Query<Account>(context).Where(p => p.Name == "Test");
                var command = context.GetQueryText(query.Expression);

                Assert.AreEqual("accounts?$filter=name eq 'Test'", command);
            }
        }
    }
}