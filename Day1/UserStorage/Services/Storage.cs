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
    public class Storage : IStorage
    {
        private IRepository repository;
        private IValidator<User> validator;
        private IGenerator generator;

        public Storage(IRepository repository, IValidator<User> validator, IGenerator generator)
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

        public int Add(IEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!validator.IsValid(user))
                throw new ArgumentException(nameof(user));

            user.Id = generator.GetId();

            var findResult = repository.Get(u => u.Equals(user));

            if (findResult == null)
            {
                repository.Add(user);
                return user.Id;
            }

            return findResult.Id;
        }

        public void Delete(int userId)
        {
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
            repository.Get(predicate);
        }
    }
}
