using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Storages
{
    public class MemoryUserRepository : IRepository<User>
    {
        private List<User> users;

        public MemoryUserRepository()
        {
            users = new List<User>();
        }

        public void Add(User user)
        {
            if (user != null)
                users.Add(user);
        }

        public User[] Get(Predicate<User> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return users.FindAll(predicate).ToArray();
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
    }
}
