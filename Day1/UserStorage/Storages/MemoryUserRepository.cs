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
        private readonly List<User> users;

        public MemoryUserRepository()
        {
            users = new List<User>();
        }

        public void Add(User user)
        {
            if (user != null)
                users.Add(user);
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
    }
}
