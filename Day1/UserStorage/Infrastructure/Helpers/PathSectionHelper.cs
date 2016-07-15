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
        private static readonly PathSection section = ConfigurationManager.GetSection("PathSection") as PathSection;
        public static string GetPath()
        {
            if (section == null)
            {
                throw new ConfigurationErrorsException("Path section not found");
            }
            return section.Path;
        }

        public static string GetIdPath()
        {
            if (section == null || section.IdPath == null)
            {
                throw new ConfigurationErrorsException("Path section not found");
            }
            return section.IdPath.Path;
        }
    }
}
