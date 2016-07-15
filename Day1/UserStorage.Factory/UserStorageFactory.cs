using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage.Entities;
using UserStorage.Factory.Infrastructure.Helpers;
using UserStorage.Infrastructure;
using UserStorage.Replication;
using UserStorage.Services;
using UserStorage.Storages;

namespace UserStorage.Factory
{
    public class UserStorageFactory
    {
        private static readonly IMaster<User> masterStorage 
            = new Storage(new XmlUserRepository(), new ValidatorUsers(), new GeneratorIds());

        private static readonly int slavesNumber = ReplicationSectionHelper.GetSlavesNumber();
        private static int count;

        static UserStorageFactory() { }

        private UserStorageFactory() { }

        public static IStorage<User> GetMaster => masterStorage as IStorage<User>;

        public static IStorage<User> GetSlave
        {
            get
            {
                if (count >= slavesNumber)
                {
                    throw new InvalidOperationException("Don't have more slave storages");
                }
                ISlave<User> slave = new SlaveStorage(new MemoryUserRepository(), masterStorage);

                count++;
                return slave as IStorage<User>;
            }
        }
    }
}
