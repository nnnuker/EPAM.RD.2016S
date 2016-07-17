using System;
using System.CodeDom;
using System.Linq;
using System.Reflection;
using MyInterfaces;

namespace DoSomethingClient
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        // Before making this call make sure that MyInterface assembly is signed with mykey.snk file. See Signing tab in MyInterface project properties editor.
        // Usage:
        // result = loader.Load("MyLibrary, Version=1.2.3.4, Culture=neutral, PublicKeyToken=f46a87b3d9a80705", input)
        public Result Load(string assemblyString, Input data)
        {
            // LoadFile() doesn't bind through Fusion at all - the loader just goes ahead and loads exactly what the caller requested.
            // It doesn't use either the Load or the LoadFrom context.
            // LoadFile() has a catch. Since it doesn't use a binding context, its dependencies aren't automatically found in its directory. 

            var assembly = Assembly.Load(assemblyString);

            return GetDoSomethingObject(assembly)?.DoSomething(data);
        }

        // Usage:
        // var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MyDomain\MyLibrary.dll");
        // result = loader.Load(path, input);
        public Result LoadFile(string path, Input data)
        {
            // LoadFrom() goes through Fusion and can be redirected to another assembly at a different path
            // but with that same identity if one is already loaded in the LoadFrom context.

            var assembly = Assembly.LoadFile(path);
            var types = assembly.GetTypes();

            // TODO: Find first type that has DoSomething attribute and don't implement IDoSomething.

            var type = types.FirstOrDefault(t => t.GetCustomAttributes(typeof(DoSomethingAttribute), false).Length != 0
                && !t.IsAssignableFrom(typeof(IDoSomething)));

            if (type == null)
                throw new ArgumentNullException("The requested type is not found.");

            var obj = Activator.CreateInstance(type);

            // TODO: MethodInfo mi = type.GetMethod("DoSomething");

            var method = type.GetMethod("DoSomething");
            Result result = method.Invoke(obj, new object[]{ data }) as Result;

            // TODO: result = mi.Invoke();

            return result;
        }

        // More details: http://stackoverflow.com/questions/1477843/difference-between-loadfile-and-loadfrom-with-net-assemblies
        public Result LoadFrom(string fileName, Input data)
        {
            var assembly = Assembly.LoadFrom(fileName);

            return GetDoSomethingObject(assembly)?.DoSomething(data);
        }

        private IDoSomething GetDoSomethingObject(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var type = types.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(IDoSomething))
                && t.GetCustomAttributes(typeof(DoSomethingAttribute), false).Length != 0);

            if (type == null)
                throw new ArgumentNullException("The requested type is not found.");

            return Activator.CreateInstance(type) as IDoSomething;
        }
    }
}
