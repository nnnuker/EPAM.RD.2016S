using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Infrastructure;

namespace UserStorage.Tests
{
    [TestClass]
    public class GeneratorIdsTests
    {
        [TestMethod]
        public void GetId_TryGetFirstId_ReturnOne()
        {
            IGenerator generator = new GeneratorIds();
            var result = generator.GetId();

            Assert.AreEqual(1, result);
        }
    }
}
