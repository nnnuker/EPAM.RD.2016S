using System;
using System.ComponentModel;
using System.Linq;

namespace Attributes
{
    // Should be applied to classes only.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InstantiateUserAttribute : Attribute
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public InstantiateUserAttribute(int id, string firstName, string lastName)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public InstantiateUserAttribute(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;

            this.Id = GetId() ?? 0;
        }

        private int? GetId()
        {
            var type = typeof(User);
            var property = type.GetProperty(GetProperty());
            var defaultAttribute = (DefaultValueAttribute[])property.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            return defaultAttribute.FirstOrDefault()?.Value as int?;
        }

        private string GetProperty()
        {
            var type = typeof(User);

            var ctor = type.GetConstructors().FirstOrDefault();

            var matchParameter = (MatchParameterWithPropertyAttribute)ctor?
                .GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute), false).FirstOrDefault();

            return matchParameter?.Property;
        }
    }
}
