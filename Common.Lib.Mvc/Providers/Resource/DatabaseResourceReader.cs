using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Resources;
using Common.Lib.Infastructure;

namespace Common.Lib.MVC.Providers.Resource
{
    public class DatabaseResourceReader : DisposableBase, IResourceReader, IEnumerable<KeyValuePair<string, object>>
    {
        private ListDictionary _resourceDictionary;

        public DatabaseResourceReader(ListDictionary resourceDictionary)
        {
            Debug.WriteLine("DatabaseResourceProvider()");

            _resourceDictionary = resourceDictionary;
        }

        protected override void Cleanup()
        {
            try
            {
                _resourceDictionary = null;
            }
            finally
            {
                base.Cleanup();
            }
        }

        #region IResourceReader Members

        public void Close()
        {
            Dispose();
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            Debug.WriteLine("DatabaseResourceReader.GetEnumerator()");

            // NOTE: this is the only enumerator called by the runtime for 
            // implicit expressions

            if (Disposed)
            {
                throw new ObjectDisposedException("DatabaseResourceReader object is already disposed.");
            }

            return _resourceDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("DatabaseResourceReader object is already disposed.");
            }

            return _resourceDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>> Members

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("DatabaseResourceReader object is already disposed.");
            }

            return _resourceDictionary.GetEnumerator() as IEnumerator<KeyValuePair<string, object>>;
        }

        #endregion
    }
}

