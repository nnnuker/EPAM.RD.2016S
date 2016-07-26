using System.Configuration;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class MessageSenderElement:ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string MessageSenderType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}