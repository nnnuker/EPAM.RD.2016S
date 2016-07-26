using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace UserStorage.Infrastructure
{
    class CustomSerializationFormatter : IFormatter
    {
        private SerializationBinder binder;
        private StreamingContext context;
        private ISurrogateSelector surrogateSelector;

        public ISurrogateSelector SurrogateSelector
        {
            get { return surrogateSelector; }
            set { surrogateSelector = value; }
        }

        public SerializationBinder Binder
        {
            get { return binder; }
            set { binder = value; }
        }

        public StreamingContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public CustomSerializationFormatter()
        {
            context = new StreamingContext(StreamingContextStates.All);
        }

        public object Deserialize(Stream serializationStream)
        {
            StreamReader sr = new StreamReader(serializationStream);

            // Get Type from serialized data.
            string line = sr.ReadLine();
            char[] delim = { '=' };
            string[] sarr = line.Split(delim);
            string className = sarr[1];
            Type type = Type.GetType(className);

            // Create object of just found type name.
            object obj = FormatterServices.GetUninitializedObject(type);

            // Get type members.
            MemberInfo[] members = FormatterServices.GetSerializableMembers(obj.GetType(), Context);

            // Create data array for each member.
            object[] data = new object[members.Length];

            // Store serialized variable name -> value pairs.
            Dictionary<string, object> sdict = new Dictionary<string, object>();
            while (sr.Peek() >= 0)
            {
                line = sr.ReadLine();
                sarr = line.Split(delim);

                // key = variable name, value = variable value.
                sdict[sarr[0].Trim()] = sarr[1].Trim();
            }
            sr.Close();

            // Store for each member its value, converted from string to its type.
            for (int i = 0; i < members.Length; ++i)
            {
                FieldInfo fi = ((FieldInfo)members[i]);
                if (!sdict.ContainsKey(fi.Name))
                    throw new SerializationException("Missing field value : " + fi.Name);
                data[i] = Convert.ChangeType(sdict[fi.Name], fi.FieldType);
            }

            // Populate object members with theri values and return object.
            return FormatterServices.PopulateObjectMembers(obj, members, data);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            // Get fields that are to be serialized.
            MemberInfo[] members = FormatterServices.GetSerializableMembers(graph.GetType(), Context);

            // Get fields data.
            object[] objs = FormatterServices.GetObjectData(graph, members);

            // Write class name and all fields & values to file
            StreamWriter sw = new StreamWriter(serializationStream);
            sw.WriteLine("@ClassName={0}", graph.GetType().FullName);
            for (int i = 0; i < objs.Length; ++i)
            {
                sw.WriteLine("{0}={1}", members[i].Name, objs[i].ToString());
            }
            sw.Close();
        }
    }
}