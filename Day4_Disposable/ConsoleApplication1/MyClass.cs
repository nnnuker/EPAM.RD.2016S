using System;
using System.Runtime.InteropServices;

namespace ConsoleApplication1
{
    // TODO: The code below contains a lot of issues. Please, fix all of them.
    // Use as a guidelines:
    // https://msdn.microsoft.com/en-us/library/b1yfkh5e(v=vs.110).aspx
    // https://msdn.microsoft.com/en-us/library/b1yfkh5e%28v=vs.100%29.aspx?f=255&MSPPError=-2147217396
    // https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
    public class MyClass : IDisposable
    {
        private IntPtr buffer;       // unmanaged resource
        private SafeHandle resource; // managed resource
        private bool disposed = false;

        public MyClass()
        {
            this.buffer = Helper.AllocateBuffer();
            this.resource = Helper.GetResource();
        }

        ~MyClass()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (resource != null)
                        resource.Dispose();
                }

                CloseHandle(buffer);
                buffer = IntPtr.Zero;
            }
            disposed = true;
        }

        [DllImport("Kernel32")]
        private extern static bool CloseHandle(IntPtr handle);

        public void DoSomething()
        {
            // NOTE: Manupulation with _buffer and _resource in this line.
            Console.WriteLine("Call DoSomething: resource.IsClosed: " + resource.IsClosed + " buffer: " + buffer.ToInt32().ToString());
        }
    }
}
