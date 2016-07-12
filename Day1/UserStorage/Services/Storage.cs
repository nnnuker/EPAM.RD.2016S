using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class Storage : IStorage
    {
        private IRepository repository;

        public Storage(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public int Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public int Search(Predicate<User> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
