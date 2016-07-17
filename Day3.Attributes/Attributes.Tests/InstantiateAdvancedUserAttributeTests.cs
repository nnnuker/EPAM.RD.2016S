using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Attributes.Tests
{
    [TestClass]
    public class InstantiateAdvancedUserAttributeTests
    {
        private readonly AdvancedUser user = new AdvancedUser(1, 15)
        {
            FirstName = "Ivan",
            LastName = "Ivanov"
        };

        [TestMethod]
        public void InstantiateAdvUser_GetExternalId_ReturnDefaultValue()
        {
            var instantiateAttribute = new InstantiateAdvancedUserAttribute(1, "Ivan", "Ivanov");

            var id = instantiateAttribute.ExternalId;

            Assert.AreEqual(3443454, id);
        }

        [TestMethod]
        public void InstantiateAdvUser_CreateUser_ReturnUser()
        {
            var type = typeof(AdvancedUser);

            var attributes = type.Assembly.GetCustomAttributes(typeof(InstantiateAdvancedUserAttribute), false)
                as InstantiateAdvancedUserAttribute[];

            var users = attributes?.Select(attribute => new AdvancedUser(attribute.Id, attribute.ExternalId)
            {
                FirstName = attribute.FirstName,
                LastName = attribute.LastName
            });

            Assert.AreEqual(1, users?.Count());
        }

        [TestMethod]
        public void InstantiateAdvUser_IntValidatorUserFields_UserIsValid()
        {
            var fields = typeof(AdvancedUser).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

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
        public void InstantiateAdvUser_IntValidatorUserProperties_UserIsValid()
        {
            var properties = typeof(AdvancedUser).GetProperties(BindingFlags.Instance | BindingFlags.Public);

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
        public void InstantiateAdvUser_StringValidatorUserFields_UserIsValid()
        {
            var fields = typeof(AdvancedUser).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

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
        public void InstantiateAdvUser_StringValidatorUserProperties_UserIsValid()
        {
            var properties = typeof(AdvancedUser).GetProperties(BindingFlags.Instance | BindingFlags.Public);

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
