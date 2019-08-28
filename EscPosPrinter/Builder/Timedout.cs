using System;

namespace EscPosPrinter.Builder
{
    public class Timedout
    {
        public static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            System.Threading.Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = System.Threading.Thread.CurrentThread;
                try
                {
                    action();
                }
                catch
                {
                    System.Threading.Thread.ResetAbort();// cancel hard aborting, lets to finish it nicely.
                }
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }
    }
}
