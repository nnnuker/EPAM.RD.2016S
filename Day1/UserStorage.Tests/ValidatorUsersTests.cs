using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserStorage.Infrastructure;
using UserStorage.Entities;

namespace UserStorage.Tests
{
    [TestClass]
    public class ValidatorUsersTests
    {
        private User user = new User
        {
            Id = 10,
            DateOfBirth = new DateTime(1994, 9, 25),
            FirstName = "My",
            LastName = "Name",
            PersonalId = "AB231223",
            Visa = new VisaRecord { Country = "Belarus" }
        };

        [TestMethod]
        public void IsValid_WrongGender_ReturnNotValid()
        {
            IValidator<User> validator = new ValidatorUsers();

            user.Gender = (GenderEnum)3;
            
            Assert.IsFalse(validator.IsValid(user));
        }

        [TestMethod]
        public void IsValid_CorrectGender_ReturnIsValid()
        {
            IValidator<User> validator = new ValidatorUsers();

            user.Gender = GenderEnum.Male;

            Assert.IsTrue(validator.IsValid(user));
        }
    }
}
