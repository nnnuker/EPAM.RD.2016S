using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NLog;
using UserStorage.Entities;
using UserStorage.Replication;
using UserStorage.Replication.Events;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class SlaveStorage : MarshalByRefObject, ISlave
    {
        private readonly IRepository<User> repository;
        private readonly ReaderWriterLockSlim lockSlim;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static BooleanSwitch BooleanSwitch { get; set; } = new BooleanSwitch("switch", "Logger switcher");

        public SlaveStorage(IRepository<User> repository, IMaster master)
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

            master.AddEvent += OnAdd;
            master.DeleteEvent += OnDelete;

            lockSlim = new ReaderWriterLockSlim();
        }

        public int Add(User user)
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to add user in slave storage");
            }
            throw new NotSupportedException();
        }

        public User Search(int id)
        {
            if (id <= 0)
            {
                var exception = new ArgumentOutOfRangeException(nameof(id), "Search id in slave is out of range");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            try
            {
                lockSlim.EnterReadLock();
                return repository.Get(u => u.Id == id).FirstOrDefault();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public int[] SearchByName(string firstName, string lastName)
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

            try
            {
                lockSlim.EnterReadLock();
                return repository.Get(u => u.FirstName == firstName && u.LastName == lastName).Select(u => u.Id).ToArray();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public int[] SearchByPersonalId(string personalId)
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

            try
            {
                lockSlim.EnterReadLock();
                return repository.Get(u => u.PersonalId == personalId).Select(u => u.Id).ToArray();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public int[] SearchByVisaCountry(string country)
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

            try
            {
                lockSlim.EnterReadLock();
                return repository.Get(u => u.Visa.Country == country).Select(u => u.Id).ToArray();
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        public bool Delete(int userId)
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

        private void OnAdd(object sender, MessageEventArgs eventArgs)
        {
            repository.Add(eventArgs.UserData);

            if (BooleanSwitch.Enabled)
            {
                logger.Trace("Add user in slave storage");
            }
        }

        private void OnDelete(object sender, MessageEventArgs eventArgs)
        {
            repository.Delete(eventArgs.UserData.Id);

            if (BooleanSwitch.Enabled)
            {
                logger.Trace("Delete user from slave storage ");
            }
        }
    }
}
