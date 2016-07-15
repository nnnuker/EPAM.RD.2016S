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
    public class Storage : IStorage<User>, IMaster<User>
    {
        private readonly IRepository<User> repository;
        private readonly IValidator<User> validator;
        private readonly IGenerator generator;

        private readonly List<ISlave<User>> slaves;

        public Storage(IRepository<User> repository, IValidator<User> validator, IGenerator generator)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            this.repository = repository;
            this.validator = validator;
            this.generator = generator;
            slaves = new List<ISlave<User>>();
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!validator.IsValid(user))
                throw new ArgumentException(nameof(user));

            var findResult = repository.Get(u => u.Equals(user));

            if (!findResult.Any())
            {
                user.Id = generator.Get();
                repository.Add(user);
                slaves.ForEach(s => s.OnAdd(repository.GetAll()));
                return user.Id;
            }

            return findResult.First().Id;
        }

        public void Delete(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            repository.Delete(userId);
            slaves.ForEach(s => s.OnDelete(repository.GetAll()));
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            repository.Delete(user.Id);
            slaves.ForEach(s => s.OnDelete(repository.GetAll()));
        }

        public int[] Search(Predicate<User> predicate)
        {
            var result = repository.Get(predicate);

            return result?.Select(u => u.Id).ToArray();
        }

        public void Subscribe(ISlave<User> slave)
        {
            if (slave == null) throw new ArgumentNullException(nameof(slave));

            slaves.Add(slave);
        }
    }
}