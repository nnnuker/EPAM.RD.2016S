using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Factory.Infrastructure.CustomConfigSections
{
    public class ReplicationSection : ConfigurationSection
    {
        private ReplicationSection() { }

        [ConfigurationProperty("slavesNumber", IsRequired = true)]
        public int SlavesNumber => (int)this["slavesNumber"];
    }
}
