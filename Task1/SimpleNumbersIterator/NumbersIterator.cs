using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNumbersIterator
{
    public static class NumbersIterator
    {
        public static IEnumerator<int> GetEnumerator()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (IsSimple(i))
                {
                    yield return i;
                }
            }
        }

        private static bool IsSimple(int num)
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
