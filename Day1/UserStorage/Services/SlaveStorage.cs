using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;
using UserStorage.Infrastructure;
using UserStorage.Replication;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class SlaveStorage : IStorage<User>, ISlave<User>
    {
        private readonly IRepository<User> repository;

        public SlaveStorage(IRepository<User> repository)
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

        public int[] Search(Predicate<User> predicate)
        {
            var result = repository.Get(predicate);

            return result?.Select(u => u.Id).ToArray();
        }

        public void OnAdd(IEnumerable<User> entities)
        {
            UpdateRepository(entities);
        }

        public void OnDelete(IEnumerable<User> entities)
        {
            UpdateRepository(entities);
        }

        private void UpdateRepository(IEnumerable<User> entities)
        {
            throw new NotImplementedException();
        }
    }
}
