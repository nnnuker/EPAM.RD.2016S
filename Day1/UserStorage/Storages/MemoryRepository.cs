using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Storages
{
    public class MemoryRepository : IRepository<User>
    {
        private List<User> users;

        public MemoryRepository()
        {
            users = new List<User>();
        }

        public void Add(User user)
        {
            if (user != null)
                users.Add(user);
        }

        public User Get(Predicate<User> predicate)
        {
            return users.Find(predicate);
        }

        public void Delete(int userId)
        {
            var findResult = users.Find(user => user.Id == userId);
            if (findResult != null)
            {
                users.Remove(findResult);
            }
        }
    }
}
