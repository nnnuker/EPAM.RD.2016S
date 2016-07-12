using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;

namespace UserStorage.Storages
{
    public class MemoryRepository : IRepository
    {
        private List<IEntity> users;

        public MemoryRepository()
        {
            users = new List<IEntity>();
        }

        public void Add(IEntity user)
        {
            if (user != null)
                users.Add(user);
        }

        public IEntity Get(Predicate<IEntity> predicate)
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
