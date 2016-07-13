using SimpleNumbersIterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Infrastructure
{
    public class GeneratorIds : IGenerator
    {
        private IEnumerator<int> enumerator;

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
    }
}
