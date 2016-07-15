﻿using System;
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

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

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
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to delete user from slave storage");
            }
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            if (BooleanSwitch.Enabled)
            {
                logger.Error("Attempt to delete user from slave storage");
            }
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
            if (BooleanSwitch.Enabled)
            {
                logger.Trace("Slave storage got update.");
            }
            repository.UpdateRepository(entities);
        }
    }
}