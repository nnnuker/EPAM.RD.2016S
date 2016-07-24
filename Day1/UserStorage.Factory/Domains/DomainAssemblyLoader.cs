using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UserStorage.Entities;
using UserStorage.Infrastructure;
using UserStorage.Replication;
using UserStorage.Storages;

namespace UserStorage.Factory.Domains
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public IMaster MasterLoadFrom(string fileName, string repositoryType, string repositoryPath, string validatorType, string generatorType)
        {
            var assembly = Assembly.LoadFrom(fileName);
            
            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IMaster)));

            var repository = Activator.CreateInstance(types.FirstOrDefault(t=>t.Name == repositoryType), repositoryPath);
            var validator = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == validatorType));
            var generator = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == generatorType));

            var instance = Activator.CreateInstance(instanceType, repository, validator, generator);
            return instance as IMaster;
        }

        public ISlave SlaveLoadFrom(string fileName, IMaster master)
        {
            var assembly = Assembly.LoadFrom(fileName);
            
            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(ISlave)));

            var repository = new MemoryUserRepository();

            var instance = Activator.CreateInstance(instanceType, repository, master);
            return instance as ISlave;

        }
    }
}