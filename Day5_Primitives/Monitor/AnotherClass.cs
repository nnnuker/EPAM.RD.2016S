using System.Threading;

namespace Monitor
{
    // TODO: Use SpinLock to protect this structure.
    public class AnotherClass
    {
        private int _value;
        private SpinLock spinLock = new SpinLock();

        public int Counter
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public void Increase()
        {
            bool gotLock = false;
            try
            {
                spinLock.Enter(ref gotLock);
                _value++;
            }
            finally
            {
                if (gotLock) spinLock.Exit();
            }
        }

        public void Decrease()
        {
            bool gotLock = false;
            try
            {
                spinLock.Enter(ref gotLock);
                _value--;
            }
            finally
            {
                if (gotLock) spinLock.Exit();
            }
        }
    }
}
