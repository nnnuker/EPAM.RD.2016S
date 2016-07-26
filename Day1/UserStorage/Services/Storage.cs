using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using UserStorage.Entities;
using UserStorage.Infrastructure;
using UserStorage.Replication;
using UserStorage.Replication.Events;
using UserStorage.Storages;

namespace UserStorage.Services
{
    public class Storage : MarshalByRefObject, IMaster
    {
        #region Fields

        private readonly IRepository<User> repository;
        private readonly IValidator<User> validator;
        private readonly IGenerator generator;
        private readonly IMessageSender messageSender;
        private readonly ReaderWriterLockSlim lockSlim;

        private readonly List<ISlave> usersNotifyOnAdd;
        private readonly List<ISlave> usersNotifyOnDelete;

        #endregion

        #region Logger

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static BooleanSwitch BooleanSwitch { get; set; } = new BooleanSwitch("switch", "Logger switcher");

        #endregion

        public Storage(IRepository<User> repository, IValidator<User> validator, IGenerator generator, IMessageSender messageSender)
        {
            if (repository == null || validator == null || generator == null || messageSender == null)
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
            this.messageSender = messageSender;

            this.generator.Initialize(repository.GetAll().Max(u=>u.Id));

            this.lockSlim = new ReaderWriterLockSlim();

            usersNotifyOnAdd = new List<ISlave>();
            usersNotifyOnDelete = new List<ISlave>();
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

                try
                {
                    lockSlim.EnterWriteLock();
                    repository.Add(user);
                    OnAddUser(new Message(new[] {user}, MessageEnum.Add));
                }
                finally
                {
                    lockSlim.ExitWriteLock();
                }
                
                if (BooleanSwitch.Enabled)
                    logger.Trace("Added new user with id " + user.Id.ToString());

                return user.Id;
            }

            return findResult.First().Id;
        }

        public User Search(int id)
        {
            if (id <= 0)
            {
                var exception = new ArgumentOutOfRangeException(nameof(id), "Search id in master is out of range");
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
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(personalId));
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
                var exception = new ArgumentException("Value cannot be null or empty.", nameof(country));
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
            if (userId <= 0)
            {
                var exception = new ArgumentOutOfRangeException(nameof(userId) + " is less zero.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            try
            {
                lockSlim.EnterWriteLock();
                var findResult = repository.Get(u => u.Id == userId).FirstOrDefault();

                if (findResult == null) return false;

                repository.Delete(userId);
                OnDeleteUser(new Message(new[] {findResult}, MessageEnum.Delete));
                if (BooleanSwitch.Enabled)
                    logger.Trace("Attempt to delete user " + userId);
                return true;
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public void Save()
        {
            repository.Save();
        }

        public void SubscribeToAddUser(ISlave slave)
        {
            if (slave == null)
            {
                var exception = new ArgumentNullException(nameof(slave) + " is null ref object.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            messageSender.SendMessage(new []{slave}, new Message(repository.GetAll(), MessageEnum.Add));
            usersNotifyOnAdd.Add(slave);
        }

        public void SubscribeToDeleteUser(ISlave slave)
        {
            if (slave == null)
            {
                var exception = new ArgumentNullException(nameof(slave) + " is null ref object.");
                if (BooleanSwitch.Enabled)
                {
                    logger.Error(exception.Message);
                }
                throw exception;
            }

            usersNotifyOnDelete.Add(slave);
        }

        private void OnAddUser(Message e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            messageSender.SendMessage(usersNotifyOnAdd, e);

            //IPEndPoint ip = new IPEndPoint(new IPAddress(545456), 5555);
        }

        private void OnDeleteUser(Message e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            messageSender.SendMessage(usersNotifyOnDelete, e);
        }
    }
}