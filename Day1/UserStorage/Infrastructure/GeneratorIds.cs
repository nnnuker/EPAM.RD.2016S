using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SimpleNumbersIterator;
using UserStorage.Entities;
using UserStorage.Infrastructure.Helpers;

namespace UserStorage.Infrastructure
{
    public class GeneratorIds : IGenerator
    {
        private readonly IEnumerator<int> enumerator;

        public int LastId { get; set; }

        public GeneratorIds()
        {
            enumerator = NumbersIterator.GetEnumerator();
        }

        public int Get()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            throw new InvalidOperationException();
        }

        private int GetLastId()
        {
            throw new NotImplementedException();
        }
    }
}
