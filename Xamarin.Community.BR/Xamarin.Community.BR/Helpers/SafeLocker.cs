using System;
using System.Threading;

namespace Xamarin.Community.BR.Helpers
{
    public static class SafeLocker
    {
        public static void For(object objectLock, Action safeAction)
        {
            if (safeAction is null || objectLock is null)
                return;

            if (!Monitor.TryEnter(objectLock))
                return;

            safeAction.Invoke();

            if (Monitor.IsEntered(objectLock))
                Monitor.Exit(objectLock);
        }
    }
}
