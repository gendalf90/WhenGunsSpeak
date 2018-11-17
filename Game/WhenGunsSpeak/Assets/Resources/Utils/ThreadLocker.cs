using System;
using System.Threading;

namespace Utils
{
    public class ThreadLocker
    {
        private SpinLock spinlock = new SpinLock();

        public void Do(Action action)
        {
            bool lockTaken = false;

            try
            {
                spinlock.Enter(ref lockTaken);
                action();
            }
            finally
            {
                if (lockTaken)
                {
                    spinlock.Exit(false);
                }
            }
        }
    }
}
