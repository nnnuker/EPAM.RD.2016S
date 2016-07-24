using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class ValidatorElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string ValidatorType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}
