using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IPEndPoint clientEndPoint;
        private readonly ReaderWriterLockSlim lockSlim;

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public static BooleanSwitch BooleanSwitch { get; set; } = new BooleanSwitch("switch", "Logger switcher");

        public IPEndPoint ClientEndPoint => clientEndPoint;

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

            master.SubscribeToAddUser(this);
            master.SubscribeToDeleteUser(this);

            lockSlim = new ReaderWriterLockSlim();
        }

        public SlaveStorage(IRepository<User> repository, IPEndPoint clientEndPoint)
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
            this.clientEndPoint = clientEndPoint;

            lockSlim = new ReaderWriterLockSlim();

            ListenForNotify();

            master.SubscribeToAddUser(this);
            master.SubscribeToDeleteUser(this);
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

        public void OnNotifyUser(Message message)
        {
            switch (message.MessageType)
            {
                case MessageEnum.Add:
                    {
                        foreach (var user in message.UserData)
                        {
                            repository.Add(user);
                        }
                        break;
                    }
                case MessageEnum.Delete:
                    {
                        foreach (var user in message.UserData)
                        {
                            repository.Delete(user.Id);
                        }
                        break;
                    }
                default:
                    throw new InvalidEnumArgumentException(nameof(message.MessageType));
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

        private void ListenForNotify()
        {
            var thread = new Thread(() =>
            {
                TcpListener listener = null;
                try
                {
                    listener = new TcpListener(clientEndPoint);
                    listener.Start();

                    var formatter = new BinaryFormatter();

                    while (true)
                    {
                        var client = listener.AcceptTcpClient();
                        using (var stream = client.GetStream())
                        {
                            var message = formatter.Deserialize(stream) as Message;
                            OnNotifyUser(message);
                        }
                    }
                }
                finally
                {
                    listener?.Stop();
                }
            });

            thread.IsBackground = true;

            thread.Start();

        }
    }
}
