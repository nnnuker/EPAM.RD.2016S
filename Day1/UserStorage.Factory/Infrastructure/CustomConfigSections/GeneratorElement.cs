using System.Configuration;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class GeneratorElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string GeneratorType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}