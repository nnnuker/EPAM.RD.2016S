using System;
using System.Collections.Generic;
using SimpleNumbersIterator;

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

        public void Initialize(int? last = 1)
        {
            if (last > 1)
            {
                enumerator = NumbersIterator.GetEnumerator(last.Value);
            }
        }
    }
}
