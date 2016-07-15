using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserStorage.Factory.Tests
{
    [TestClass]
    public class UserStorageFactoryTests
    {
        [TestMethod]
        public void GetMaster_GetMaster_ReturnsNotNull()
        {
            var result = UserStorageFactory.GetMaster;

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        public void GetSlave_GetSlave_ReturnsNotNull()
        {
            var result = UserStorageFactory.GetSlave;

            Assert.AreNotEqual(null, result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetSlave_GetTwoSlaves_ThrowsNotAllowedException()
        {
            var result = UserStorageFactory.GetSlave;
            result = UserStorageFactory.GetSlave;
        }
    }
}
