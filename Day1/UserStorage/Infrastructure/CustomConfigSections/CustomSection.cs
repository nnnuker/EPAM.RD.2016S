using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Infrastructure.CustomConfigSections
{
    public class CustomSection : ConfigurationSection
    {
        private CustomSection() { }

        [ConfigurationProperty("Path", DefaultValue = @"|DataDirectory|\database.xml")]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }
        }
    }
}
