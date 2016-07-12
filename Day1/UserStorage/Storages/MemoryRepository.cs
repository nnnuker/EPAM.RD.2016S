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

        public void Save(IEntity user)
        {
            if (user != null)
                users.Add(user);
        }
    }
}
