using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Factory.Infrastructure.CustomConfigSections;

namespace UserStorage.Factory.Infrastructure.Helpers
{
    public static class ReplicationSectionHelper
    {
        public static StorageElement GetMasterSection()
        {
            var section = ConfigurationManager.GetSection("Replication") as ReplicationSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Replication section not found");
            }

            var master = section.Storages.OfType<StorageElement>().Single(s => s.IsMaster);
            
            return master;
        }

        public static IEnumerable<StorageElement> GetSlaveSections()
        {
            var section = ConfigurationManager.GetSection("Replication") as ReplicationSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Replication section not found");
            }

            var slaves = section.Storages.OfType<StorageElement>().Where(s => !s.IsMaster);

            return slaves;
        }
    }
}
