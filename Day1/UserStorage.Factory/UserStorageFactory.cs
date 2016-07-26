using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using UserStorage.Factory.Domains;
using UserStorage.Factory.Infrastructure.CustomConfigSections;
using UserStorage.Factory.Infrastructure.Helpers;
using UserStorage.Replication;
using UserStorage.Services;

namespace UserStorage.Factory
{
    public static class UserStorageFactory
    {
        private static readonly IMaster masterStorage = GetMasterStorage();

        private static int count;

        private static readonly IEnumerable<StorageElement> slaveElements = ReplicationSectionHelper.GetSlaveSections();

        static UserStorageFactory() { }

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
            var masterElement = ReplicationSectionHelper.GetMasterSection();
            AppDomain domain = AppDomain.CreateDomain(masterElement.DomainName);

            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                typeof(DomainAssemblyLoader).FullName);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");

            return loader.MasterLoadFrom(path, masterElement.Repository.RepositoryType, masterElement.Repository.Path, 
                masterElement.Validator.ValidatorType, masterElement.Generator.GeneratorType, masterElement.MessageSender.MessageSenderType);
        }

        private static ISlave GetSlaveStorage(int number)
        {
            var configElement = slaveElements.ElementAt(number);
            AppDomain domain = AppDomain.CreateDomain(configElement.DomainName);

            var loader = (DomainAssemblyLoader)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                typeof(DomainAssemblyLoader).FullName);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage.dll");

            if (configElement.TcpInfo.Address != string.Empty)
            {
                return GetSlaveTcp(loader, path, configElement);
            }

            return loader.SlaveLoadFrom(path, masterStorage, configElement.Repository.RepositoryType);
        }

        private static ISlave GetSlaveTcp(DomainAssemblyLoader loader, string path, StorageElement section)
        {
            IPEndPoint slaveEndPoint = new IPEndPoint(IPAddress.Parse(section.TcpInfo.Address), section.TcpInfo.Port);
            

            return loader.SlaveLoadFrom(path, masterStorage, section.Repository.RepositoryType, slaveEndPoint);
        }
    }
}
