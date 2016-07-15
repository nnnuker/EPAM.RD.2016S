using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserStorage.Entities;
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

        static UserStorageFactory() { }

        private UserStorageFactory() { }

        public static IStorage<User> GetMaster => masterStorage as IStorage<User>;

        public static IStorage<User> GetSlave
        {
            get
            {
                ISlave<User> slave = new SlaveStorage(new MemoryUserRepository());
                
                masterStorage.Subscribe(slave);

                return slave as IStorage<User>;
            }
        }
    }
}
