using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Infrastructure.CustomConfigSections
{
    public class PathSection : ConfigurationSection
    {
        private PathSection() { }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path => this["path"] as string;
    }
}
