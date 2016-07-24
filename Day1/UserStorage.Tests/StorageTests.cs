using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Services;
using UserStorage.Entities;
using UserStorage.Infrastructure;
using UserStorage.Storages;

namespace UserStorage.Tests
{
    [TestClass]
    public class StorageTests
    {
        private IUserStorage storage;
        private IUserStorage storageXml;
        private User user = new User
        {
            Id = 1,
            DateOfBirth = new DateTime(1994, 9, 25),
            FirstName = "My",
            LastName = "Name",
            Gender = GenderEnum.Male,
            PersonalId = "AB231223",
            Visa = new VisaRecord { Country = "Belarus" }
        };

        [TestInitialize]
        public void Initialize()
        {
            storage = new Storage(new MemoryUserRepository(), new ValidatorUsers(), new GeneratorIds());
            storageXml = new Storage(new XmlUserRepository(@"d:\Projects\EPAM.RD.2016S.Larkovich\Day1\UserStorage.Tests\bin\Debug\UserDataBase.xml"), new ValidatorUsers(), new GeneratorIds());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Add_NotValidUser_ThrowsArgumentException()
        {
            var user = new User();

            storage.Add(user);
        }

        [TestMethod]
        public void Add_ValidUser_ReturnsUserId()
        {
            var result = storage.Add(user);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void AddXmlRepo_ValidUser_ReturnsUserId()
        {
            var result = storageXml.Add(user);

            Assert.IsTrue(storageXml.Search(user.Id) != null);
        }

        [TestMethod]
        public void Add_TwoUsersWithSameNames_ReturnsUserId()
        {
            storage.Add(user);
            var result = storage.Add(user);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Add_TwoUsers_ReturnsUserId()
        {
            storage.Add(user);

            User user1 = new User
            {
                FirstName = "His",
                LastName = "Name",
                PersonalId = "AB231223",
                Visa = new VisaRecord { Country = "Belarus" }
            };

            var result = storage.Add(user1);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Delete_ByCorrectId_ExpectDelete()
        {
            storage.Add(user);
            storage.Delete(user.Id);

            Assert.AreEqual(null, storage.Search(1));
        }

        [TestMethod]
        public void Delete_ByUser_ExpectDelete()
        {
            storage.Add(user);
            storage.Delete(user.Id);

            Assert.AreEqual(null, storage.Search(1));
        }

        [TestMethod]
        public void Search_ByName_ReturnOne()
        {
            storage.Add(user);

            var result = storage.SearchByName("My", "Name");

            Assert.AreEqual(1, result.Count());
        }
    }
}
