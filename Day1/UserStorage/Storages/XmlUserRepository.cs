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
using UserStorage.Infrastructure.Helpers;

namespace UserStorage.Storages
{
    public class XmlUserRepository : IRepository<User>
    {
        private readonly string filePath;
        private readonly List<User> users;

        public XmlUserRepository()
        {
            filePath = PathSectionHelper.GetPath();
            users = LoadUsers();
        }

        public void Add(User user)
        {
            if (user != null)
            {
                users.Add(user);
                SaveUsers();
            }
        }

        public IEnumerable<User> Get(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return users.FindAll(predicate);
        }

        public IEnumerable<User> GetAll()
        {
            return users;
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

        private List<User> LoadUsers()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                XmlTextReader reader = new XmlTextReader(fs);

                if (formatter.CanDeserialize(reader))
                {
                    return formatter.Deserialize(reader) as List<User>;
                }

                return new List<User>();
            }
        }

        private void SaveUsers()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, users);
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