using System.ComponentModel;

namespace Attributes
{
    public class AdvancedUser : User
    {
        private int externalId;

        [DefaultValue(3443454)]
        public int ExternalId
        {
            get { return externalId; }
            set { externalId = value; }
        }

        [MatchParameterWithProperty("id", "Id")]
        [MatchParameterWithProperty("externalId", "ExternalId")]
        public AdvancedUser(int id, int externalId) : base(id)
        {
            this.externalId = externalId;
        }
    }
}
