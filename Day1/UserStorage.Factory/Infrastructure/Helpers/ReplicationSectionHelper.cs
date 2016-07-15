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
        public static int GetSlavesNumber()
        {
            ReplicationSection section = ConfigurationManager.GetSection("ReplicationSection") as ReplicationSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Path section not found");
            }
            return section.SlavesNumber;
        }
    }
}
