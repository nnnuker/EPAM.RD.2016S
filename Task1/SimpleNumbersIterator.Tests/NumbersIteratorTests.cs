using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleNumbersIterator.Tests
{
    [TestClass]
    public class NumbersIteratorTests
    {
        [TestMethod]
        public void GetEnumerator_TakeTen_ReturnTen()
        {
            var result = new List<int>();
            var expected = new List<int> { 1, 2, 3, 5, 7, 11, 13, 17, 19, 23};

            var enumerator = new NumbersIterator();
            for (int i = 0; i < 10; i++)
            {
                if (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current);
                }
            }

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
