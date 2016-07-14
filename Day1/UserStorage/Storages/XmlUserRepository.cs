using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
                DeleteUser();
            }
        }

        private string GetFilePath()
        {
            CustomSection section = ConfigurationManager.GetSection("CustomSection") as CustomSection;
            if (section == null)
            {
                throw new ConfigurationErrorsException("Custom section not found");
            }
            return section.Path;
        }

        private List<User> LoadUsers()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (XmlTextReader reader = new XmlTextReader(fs))
                {
                    //if (formatter.CanDeserialize(reader))
                    //{
                    //    return formatter.Deserialize(fs) as List<User>;
                    //}

                    return new List<User>();
                }
            }
        }

        private void SaveUser(User user)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(User), new XmlRootAttribute("Users"));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, user);
            }
        }

        private void DeleteUser()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                formatter.Serialize(fs, users);
            }
        }
    }
}