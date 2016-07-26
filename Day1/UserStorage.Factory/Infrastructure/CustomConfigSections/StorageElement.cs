using System.Configuration;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class StorageElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("isMaster", DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool IsMaster
        {
            get { return bool.Parse(base["isMaster"].ToString()); }
            set { base["isMaster"] = value; }
        }

        [ConfigurationProperty("domainName", IsKey = false, IsRequired = true)]
        public string DomainName
        {
            get { return (string)base["domainName"]; }
            set { base["domainName"] = value; }
        }

        [ConfigurationProperty("Repository", IsKey = false, IsRequired = true)]
        public RepositoryElement Repository
        {
            get { return (RepositoryElement)base["Repository"]; }
            set { base["Repository"] = value; }
        }

        [ConfigurationProperty("Validator", IsKey = false, IsRequired = false)]
        public ValidatorElement Validator
        {
            get { return (ValidatorElement)base["Validator"]; }
            set { base["Validator"] = value; }
        }

        [ConfigurationProperty("Generator", IsKey = false, IsRequired = false)]
        public GeneratorElement Generator
        {
            get { return (GeneratorElement)base["Generator"]; }
            set { base["Generator"] = value; }
        }

        [ConfigurationProperty("MessageSender", IsKey = false, IsRequired = false)]
        public MessageSenderElement MessageSender
        {
            get { return (MessageSenderElement)base["MessageSender"]; }
            set { base["MessageSender"] = value; }
        }

        [ConfigurationProperty("TcpInfo", IsKey = false, IsRequired = false)]
        public TcpInfoElement TcpInfo
        {
            get { return (TcpInfoElement)base["TcpInfo"]; }
            set { base["TcpInfo"] = value; }
        }
    }
}