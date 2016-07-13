using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;
using UserStorage.Infrastructure;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class Storage : IStorage<User>
    {
        private IRepository<User> repository;
        private IValidator<User> validator;
        private IGenerator generator;

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
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!validator.IsValid(user))
                throw new ArgumentException(nameof(user));

            var findResult = repository.Get(u => u.Equals(user));

            if (findResult == null)
            {
                user.Id = generator.GetId();
                repository.Add(user);
                return user.Id;
            }

            return findResult.Id;
        }

        public void Delete(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            repository.Delete(userId);
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            repository.Delete(user.Id);
        }

        public int Search(Predicate<User> predicate)
        {
            var result = repository.Get(predicate);

            if (result == null)
                return 0;

            return result.Id;
        }
    }
}
