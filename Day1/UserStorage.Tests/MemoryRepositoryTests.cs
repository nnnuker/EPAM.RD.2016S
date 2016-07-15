using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Storages;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class MemoryRepository
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_NullRefPredicate_ThrowsAnException()
        {
            IRepository<User> repository = new MemoryUserRepository();

            repository.Get(null);
        }

        [TestMethod]
        public void Get_WrongPredicate_ReturnNull()
        {
            IRepository<User> repository = new MemoryUserRepository();

            var result = repository.Get(user=>user.Id==0);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Get_CorrectPredicate_ReturnCorrectUser()
        {
            IRepository<User> repository = new MemoryUserRepository();
            repository.Add(new User { FirstName = "Name"});

            var result = repository.Get(user => user.FirstName == "Name");

            Assert.AreEqual("Name", result?.First().FirstName);
        }

        [TestMethod]
        public void Delete_ExistentUser_SuccessDeleting()
        {
            IRepository<User> repository = new MemoryUserRepository();
            repository.Add(new User { Id = 10 });

            repository.Delete(10);

            Assert.AreEqual(0, repository.Get(u => u.Id == 10).Length);
        }
    }
}
