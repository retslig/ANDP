using System;

namespace Common.Lib.Infastructure
{
    public class DisposableBase : IDisposable
    {
        private bool _disposed;
        protected bool Disposed
        {
            get
            {
                lock (this)
                {
                    return _disposed;
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            lock (this)
            {
                if (_disposed == false)
                {
                    Cleanup();
                    _disposed = true;

                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion

        protected virtual void Cleanup()
        {
            // override to provide cleanup
        }

        ~DisposableBase()
        {
            Cleanup();
        }

    }
}