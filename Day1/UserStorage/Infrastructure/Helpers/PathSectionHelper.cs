using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Infrastructure.CustomConfigSections;

namespace UserStorage.Infrastructure.Helpers
{
    public static class PathSectionHelper
    {
        public static string GetPath()
        {
            PathSection section = ConfigurationManager.GetSection("PathSection") as PathSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Path section not found");
            }
            return section.Path;
        }
    }
}
