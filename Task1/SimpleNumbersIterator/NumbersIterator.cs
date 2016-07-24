using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNumbersIterator
{
    [Serializable]
    public class NumbersIterator : IEnumerator<int>
    {
        private int current;

        public int Current => current;
        object IEnumerator.Current => Current;

        public NumbersIterator()
        {
        }

        public void SetCurrent(int current)
        {
            this.current = current;
        }

        public bool MoveNext()
        {
            try
            {
                checked
                {
                    while (current < int.MaxValue)
                    {
                        current++;
                        if (IsSimple(current))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (OverflowException)
            {
                Reset();
                return false;
            }

            return true;
        }

        public void Reset()
        {
            current = 0;
        }

        public void Dispose()
        {
        }

        private bool IsSimple(int num)
        {
            for (int i = 2; i <= num / 2; i++)
            {
                if (num % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
