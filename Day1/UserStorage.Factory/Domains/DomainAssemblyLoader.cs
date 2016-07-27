using System;
using System.Linq;
using System.Net;
using System.Reflection;
using UserStorage.Replication;

namespace UserStorage.Factory.Domains
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        public IMaster MasterLoadFrom(string fileName, string repositoryType, string repositoryPath, 
            string validatorType, string generatorType, string messageSenderType)
        {
            var assembly = Assembly.LoadFrom(fileName);

            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IMaster)));

            var repository = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == repositoryType), repositoryPath);
            var validator = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == validatorType));
            var generator = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == generatorType));
            var messageSender = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == messageSenderType));

            var instance = Activator.CreateInstance(instanceType, repository, validator, generator, messageSender);
            return instance as IMaster;
        }

        public ISlave SlaveLoadFrom(string fileName, string repositoryType)
        {
            var assembly = Assembly.LoadFrom(fileName);

            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(ISlave)));

            var repository = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == repositoryType));

            var instance = Activator.CreateInstance(instanceType, repository);
            return instance as ISlave;

        }

        public ISlave SlaveLoadFrom(string fileName, string repositoryType, IPEndPoint clientEndPoint)
        {
            var assembly = Assembly.LoadFrom(fileName);

            var types = assembly.GetTypes();
            var instanceType = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(ISlave)));

            var repository = Activator.CreateInstance(types.FirstOrDefault(t => t.Name == repositoryType));

            var instance = Activator.CreateInstance(instanceType, repository, clientEndPoint);
            return instance as ISlave;

        }
    }
}