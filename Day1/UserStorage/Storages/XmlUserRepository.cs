using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UserStorage.Entities;

namespace UserStorage.Storages
{
    [Serializable]
    public class XmlUserRepository : IRepository<User>
    {
        private readonly string filePath;
        private readonly List<User> users;

        public XmlUserRepository(string path)
        {
            filePath = path;
            users = LoadUsers();
        }

        public void Add(User user)
        {
            if (user != null)
            {
                users.Add(user);
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
            }
        }

        public void Save()
        {
            var formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, users);
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
    }
}