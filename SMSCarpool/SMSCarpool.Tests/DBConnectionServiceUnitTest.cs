using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMSCarpool.Services;

namespace SMSCarpool.Tests
{
    [TestClass]
    [TestCategory("DBConnectionServiceUnitTest")]
    public class DBConnectionServiceUnitTest
    {
        [TestMethod]
        public void ObjectInitializationTest()
        {
            DBConnectionService service = new Services.DBConnectionService("");

            IDbConnection conn = service.getConnection();

            Assert.IsNotNull(conn);
        }

        [TestMethod]
        public void IsConectionOpen()
        {
            DBConnectionService service = new Services.DBConnectionService("");

            IDbConnection conn = service.getConnection();

            Assert.IsNotNull(conn);
        }
    }
}
