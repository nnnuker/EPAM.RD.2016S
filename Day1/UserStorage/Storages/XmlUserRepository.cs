using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserStorage.Entities;
using UserStorage.Infrastructure.CustomConfigSections;

namespace UserStorage.Storages
{
    public class XmlUserRepository : IRepository<User>
    {
        private string filePath;
        private List<User> users;

        public XmlUserRepository()
        {
            filePath = GetFilePath();
            users = LoadUsers();
        }

        public void Add(User user)
        {
            if (user != null)
            {
                users.Add(user);
                SaveUser(user);
            }
        }

        public User Get(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return users.Find(predicate);
        }

        public void Delete(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            var findResult = users.Find(user => user.Id == userId);
            if (findResult != null)
            {
                users.Remove(findResult);
            }
        }

        private string GetFilePath()
        {
            Configuration config =
    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            CustomSection section = (CustomSection)config.Sections["CustomSection"];
            return section.Path;
        }

        private List<User> LoadUsers()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                return (List<User>)formatter.Deserialize(fs);
            }
        }

        private void SaveUser(User user)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(User));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, user);
            }
        }
    }
}
