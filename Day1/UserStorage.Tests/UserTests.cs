using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class UserTests
    {
        private User user = new User
        {
            Id = 10,
            DateOfBirth = new DateTime(1994, 9, 25),
            FirstName = "My",
            LastName = "Name",
            Gender = GenderEnum.Male,
            PersonalId = "AB231223"
        };

        private User user1 = new User
        {
            Id = 10,
            DateOfBirth = new DateTime(1994, 9, 25),
            FirstName = "My",
            LastName = "Name",
            Gender = GenderEnum.Male,
            PersonalId = "AB231223"
        };

        [TestMethod]
        public void GetHashCode_TwoUsersHashCompare_ReturnIsValid()
        {
            user1.Gender = GenderEnum.Male;

            Assert.IsTrue(user.GetHashCode() == user1.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_TwoDiffUsersHashCompare_ReturnIsNotValid()
        {
            user1.Gender = GenderEnum.Female;

            Assert.IsFalse(user.GetHashCode() == user1.GetHashCode());
        }
    }
}
