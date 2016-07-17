using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attributes.Tests
{
    [TestClass]
    public class InstantiateUserAttributeTests
    {
        private readonly User user = new User(1)
        {
            FirstName = "Ivan",
            LastName = "Ivanov"
        };

        [TestMethod]
        public void InstantiateUserAttribute_GetId_ReturnDefaultValue()
        {
            var instantiateAttribute = new InstantiateUserAttribute("Ivan", "Ivanov");

            var id = instantiateAttribute.Id;

            Assert.AreEqual(1, id);
        }

        [TestMethod]
        public void InstantiateUserAttribute_CreateUser_ReturnUser()
        {
            var type = typeof(User);

            var attributes = type.GetCustomAttributes(typeof(InstantiateUserAttribute), false) as InstantiateUserAttribute[];

            var users = attributes?.Select(attribute => new User(attribute.Id)
            {
                FirstName = attribute.FirstName, LastName = attribute.LastName
            });

            Assert.AreEqual(3, users?.Count());
        }

        [TestMethod]
        public void InstantiateUserAttribute_IntValidatorUserFields_UserIsValid()
        {
            var fields = typeof(User).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var fieldAttribute
                    = field.GetCustomAttributes(typeof(IntValidatorAttribute), false).FirstOrDefault() 
                    as IntValidatorAttribute;

                if (fieldAttribute == null) continue;

                int lower = fieldAttribute.Lower;
                int upper = fieldAttribute.Upper;

                var flag = user.Id >= lower || user.Id <= upper;

                Assert.IsTrue(flag);
            }
        }

        [TestMethod]
        public void InstantiateUserAttribute_IntValidatorUserProperties_UserIsValid()
        {
            var properties = typeof(User).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var propertyAttribute
                    = property.GetCustomAttributes(typeof(IntValidatorAttribute), false).FirstOrDefault()
                    as IntValidatorAttribute;

                if (propertyAttribute == null) continue;

                int lower = propertyAttribute.Lower;
                int upper = propertyAttribute.Upper;

                var flag = user.Id >= lower || user.Id <= upper;

                Assert.IsTrue(flag);
            }
        }

        [TestMethod]
        public void InstantiateUserAttribute_StringValidatorUserFields_UserIsValid()
        {
            var fields = typeof(User).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var fieldAttribute
                    = field.GetCustomAttributes(typeof(StringValidatorAttribute), false).FirstOrDefault()
                    as StringValidatorAttribute;

                if (fieldAttribute == null) continue;

                int maxLength = fieldAttribute.MaxLength;

                var value = field.GetValue(user) as string;
                var flag = value?.Length <= maxLength;

                Assert.IsTrue(flag);
            }
        }

        [TestMethod]
        public void InstantiateUserAttribute_StringValidatorUserProperties_UserIsValid()
        {
            var properties = typeof(User).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var propertyAttribute
                    = property.GetCustomAttributes(typeof(StringValidatorAttribute), false).FirstOrDefault()
                    as StringValidatorAttribute;

                if (propertyAttribute == null) continue;

                int maxLength = propertyAttribute.MaxLength;

                var value = property.GetValue(user) as string;
                var flag = value?.Length <= maxLength;

                Assert.IsTrue(flag);
            }
        }
    }
}
