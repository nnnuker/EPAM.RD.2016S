using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void GetHashCode_TwoUsersHashCompare_ReturnIsValid()
        {
            User user = new User
            {
                Id = 10,
                DateOfBirth = new DateTime(1994, 9, 25),
                FirstName = "My",
                LastName = "Name",
                Gender = GenderEnum.Male,
                PersonalId = "AB231223"
            };

            User user1 = new User
            {
                Id = 10,
                DateOfBirth = new DateTime(1994, 9, 25),
                FirstName = "My",
                LastName = "Name",
                Gender = GenderEnum.Male,
                PersonalId = "AB231223"
            };

            Assert.IsTrue(user.GetHashCode() == user1.GetHashCode());
        }
    }
}
