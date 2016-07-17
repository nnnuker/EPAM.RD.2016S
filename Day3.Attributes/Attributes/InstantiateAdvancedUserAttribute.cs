using System;
using System.ComponentModel;
using System.Linq;

namespace Attributes
{
    // Should be applied to assembly only.
    [AttributeUsage(AttributeTargets.Assembly)]
    public class InstantiateAdvancedUserAttribute : InstantiateUserAttribute
    {
        public int ExternalId { get; }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName)
            : base(id, firstName, lastName)
        {
            this.ExternalId = GetId() ?? 0;
        }

        public InstantiateAdvancedUserAttribute(int id, string firstName, string lastName, int externalId)
            : base(id, firstName, lastName)
        {
            this.ExternalId = externalId;
        }

        private int? GetId()
        {
            var type = typeof(AdvancedUser);
            var property = type.GetProperty(GetProperty());
            var defaultAttribute = (DefaultValueAttribute[])property.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            return defaultAttribute.FirstOrDefault()?.Value as int?;
        }

        private string GetProperty()
        {
            var type = typeof(AdvancedUser);

            var ctor = type.GetConstructors().FirstOrDefault();

            var matchParameters = ctor?
                .GetCustomAttributes(typeof(MatchParameterWithPropertyAttribute), false) 
                as MatchParameterWithPropertyAttribute[];

            var matchParam = matchParameters?.FirstOrDefault(p => p.Parameter == "externalId");

            return matchParam?.Property;
        }
    }
}
