using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Storages;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class XmlUserRepositoryTests
    {
        private IRepository<User> repository;

        [TestInitialize]
        public void Initialize()
        {
            repository = new XmlUserRepository(@"d:\Projects\EPAM.RD.2016S.Larkovich\Day1\UserStorage.Tests\bin\Debug\UserDataBase.xml");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_NullRefPredicate_ThrowsAnException()
        {
            repository.Get(null);
        }

        [TestMethod]
        public void Get_WrongPredicate_ReturnNull()
        {
            var result = repository.Get(user => user.Id == 500);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Get_CorrectPredicate_ReturnCorrectUser()
        {
            repository.Add(new User { FirstName = "Name" });

            var result = repository.Get(user => user.FirstName == "Name");

            Assert.AreEqual("Name", result?.First().FirstName);
        }

        [TestMethod]
        public void Delete_ExistentUser_SuccessDeleting()
        {
            repository.Add(new User { Id = 100 });

            repository.Delete(100);

            Assert.AreEqual(0, repository.Get(u => u.Id == 100).Count());
        }
    }
}
