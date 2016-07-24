using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UserStorage.Factory.Domains;
using UserStorage.Factory.Infrastructure.CustomConfigSections;
using UserStorage.Factory.Infrastructure.Helpers;
using UserStorage.Replication;
using UserStorage.Services;

namespace UserStorage.Factory
{
    public class UserStorageFactory
    {
        private static readonly IMaster masterStorage = GetMasterStorage();

        private static int count;
        private static readonly IEnumerable<StorageElement> slaveElements = ReplicationSectionHelper.GetSlaveSections();

        static UserStorageFactory() { }

        private UserStorageFactory() { }

        public static IUserStorage GetMaster => masterStorage;

        public static IUserStorage GetSlave
        {
            get
            {
                if (count >= slaveElements.Count())
                {
                    throw new InvalidOperationException("Don't have more slave storages");
                }
                ISlave slave = GetSlaveStorage(count);

                count++;
                return slave;
            }
        }

        private static IMaster GetMasterStorage()
        {
            var section = ReplicationSectionHelper.GetMasterSection();

            AppDomain domain = AppDomain.CreateDomain(section.DomainName);

            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                typeof(DomainAssemblyLoader).FullName);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");
            return loader.MasterLoadFrom(path, section.Repository.RepositoryType, section.Repository.Path, section.Validator.ValidatorType, 
                section.Generator.GeneratorType);
        }

        private static ISlave GetSlaveStorage(int number)
        {
            var configElement = slaveElements.ElementAt(number);
            AppDomain domain = AppDomain.CreateDomain(configElement.DomainName);

            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                typeof(DomainAssemblyLoader).FullName);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");
            return loader.SlaveLoadFrom(path, masterStorage);
        }
    }
}
