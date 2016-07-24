using System.Configuration;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class RepositoryElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string RepositoryType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("path", IsRequired = false, DefaultValue = "")]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
    }
}