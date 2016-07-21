using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using UserStorage.Entities;
using UserStorage.Replication;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class SlaveStorage : IStorage<User>, ISlave<User>
    {
        private readonly IRepository<User> repository;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static BooleanSwitch BooleanSwitch { get; set; } = new BooleanSwitch("switch", "Logger switcher");

        public SlaveStorage(IRepository<User> repository, IMaster<User> master)
        {
            if (repository == null)
            {
                var exception = new ArgumentNullException(nameof(repository) + " is null ref object.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            this.repository = repository;
            master.Subscribe(this);
        }

        public int Add(User user)
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to add user in slave storage");
            }
            throw new NotSupportedException();
        }

        public IEnumerable<int> SearchByName(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                var exception = new ArgumentException("Null or empty firstName search in slave storage.", nameof(firstName));
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            if (string.IsNullOrEmpty(lastName))
            {
                var exception = new ArgumentException("Null or empty lastName search in slave storage.", nameof(lastName));
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
                var exception = new ArgumentException("Null or empty personalId search in slave storage.", nameof(personalId));
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
                var exception = new ArgumentException("Null or empty country search in slave storage.", nameof(country));
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
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to delete user from slave storage");
            }
            throw new NotSupportedException();
        }

        public void Save()
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to save users in slave storage");
            }
            throw new NotSupportedException();
        }

        public void Delete(User user)
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to delete user from slave storage");
            }
            throw new NotSupportedException();
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
            if (BooleanSwitch.Enabled)
            {
                logger.Trace("Slave storage got update.");
            }
            repository.UpdateRepository(entities);
        }
    }
}
