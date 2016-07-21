using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
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

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static BooleanSwitch BooleanSwitch { get; set; } = new BooleanSwitch("switch", "Logger switcher");

        public Storage(IRepository<User> repository, IValidator<User> validator, IGenerator generator)
        {
            if (repository == null || validator == null || generator == null)
            {
                var exception = new ArgumentNullException("Error while creating master storage.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            this.repository = repository;
            this.validator = validator;
            this.generator = generator;

            this.generator.Initialize(repository.GetAll().LastOrDefault()?.Id);

            slaves = new List<ISlave<User>>();
        }

        public int Add(User user)
        {
            if (user == null)
            {
                var exception = new ArgumentNullException(nameof(user) + " is null ref object.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            if (!validator.IsValid(user))
            {
                var exception = new ArgumentException(nameof(user) + " is not valid.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            var findResult = repository.Get(u => u.Equals(user));

            if (!findResult.Any())
            {
                user.Id = generator.Get();

                repository.Add(user);
                slaves.ForEach(s => s.OnAdd(repository.GetAll()));

                if (BooleanSwitch.Enabled)
                    logger.Trace("Added new user with id " + user.Id.ToString());

                return user.Id;
            }

            return findResult.First().Id;
        }

        public IEnumerable<int> SearchByName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(firstName));
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(lastName));
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            return repository.Get(u => u.FirstName == firstName && u.LastName == lastName).Select(u => u.Id);
        }

        public IEnumerable<int> SearchByPersonalId(string personalId)
        {
            if (string.IsNullOrEmpty(personalId))
            {
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(personalId));
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            return repository.Get(u => u.PersonalId == personalId).Select(u => u.Id);
        }

        public IEnumerable<int> SearchByVisaCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(country));
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            return repository.Get(u => u.Visa.Country == country).Select(u => u.Id);
        }

        public void Delete(int userId)
        {
            if (userId <= 0)
            {
                var exception = new ArgumentOutOfRangeException(nameof(userId) + " is less zero.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            repository.Delete(userId);
            slaves.ForEach(s => s.OnDelete(repository.GetAll()));

            if (BooleanSwitch.Enabled)
                logger.Trace("User is deleted " + userId);
        }

        public void Save()
        {
            repository.Save();
        }

        public void Delete(User user)
        {
            if (user == null)
            {
                var exception = new ArgumentNullException(nameof(user) + " is null ref object.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            repository.Delete(user.Id);
            slaves.ForEach(s => s.OnDelete(repository.GetAll()));

            if (BooleanSwitch.Enabled)
                logger.Trace("User is deleted " + user.Id);
        }

        public void Subscribe(ISlave<User> slave)
        {
            if (slave == null) throw new ArgumentNullException(nameof(slave));

            slaves.Add(slave);

            if (BooleanSwitch.Enabled)
                logger.Trace("Slave is subscribed to master");

            slave.OnAdd(repository.GetAll());
        }
    }
}