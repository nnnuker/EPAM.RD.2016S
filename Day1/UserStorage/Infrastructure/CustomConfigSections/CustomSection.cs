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

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return this["path"] as string;
            }
        }
    }
}
