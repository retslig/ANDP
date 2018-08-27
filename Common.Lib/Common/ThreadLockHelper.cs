using System;
using System.Threading;

namespace Common.Lib.Common
{
    public struct ThreadLockHelper : IDisposable
    {
        private object _lockedObject;
        private bool _lockTaken;

        public void Lock(object onObject, int timeoutSeconds)
        {
            var timeoutMillisecond = timeoutSeconds*1000;
#if DEBUG
            if (onObject == null) throw new ArgumentNullException("onObject in Lock must not be null.");
            if (_lockedObject != null) throw new InvalidOperationException("Illegal use of Lock: Lock method must only called once per Lock instance.");
            if (onObject.GetType().IsValueType) throw new InvalidOperationException("Illegal use of Lock. Must not lock on a value type object.");
#endif
            _lockedObject = onObject;
            Monitor.TryEnter(onObject, timeoutMillisecond, ref _lockTaken);
            if (_lockTaken == false)
            {
                throw new System.TimeoutException("Did not acquire lock in specified time.");
            }
        }

        public void Dispose()
        {
#if DEBUG
            if (_lockedObject == null) throw new InvalidOperationException("Illegal use of Lock. Lock must have been called before releasing/disposing.");
#endif
            if (_lockTaken)
            {
                _lockTaken = false;
                Monitor.Exit(_lockedObject);
            }
        }
    }
}
