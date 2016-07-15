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
            var result = generator.Get();

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetId_GetIdFrom3_Return5()
        {
            IGenerator generator = new GeneratorIds();
            generator.Initialize(3);
            var result = generator.Get();

            Assert.AreEqual(5, result);
        }
    }
}
