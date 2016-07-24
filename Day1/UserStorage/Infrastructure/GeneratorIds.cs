using System;
using System.Collections.Generic;
using SimpleNumbersIterator;

namespace UserStorage.Infrastructure
{
    [Serializable]
    public class GeneratorIds : IGenerator
    {
        private readonly NumbersIterator enumerator;

        public GeneratorIds()
        {
            enumerator = new NumbersIterator();
        }

        public int Get()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            throw new InvalidOperationException();
        }

        public void Initialize(int? last = 0)
        {
            if (last > 0)
            {
                enumerator.SetCurrent(last.Value);
            }
        }
    }
}
