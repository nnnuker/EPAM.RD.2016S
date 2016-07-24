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

        [ConfigurationProperty("Storages")]
        [ConfigurationCollection(typeof(StorageElement), AddItemName = "Storage")]
        public StoragesCollection Storages => (StoragesCollection)base["Storages"];
    }
}
