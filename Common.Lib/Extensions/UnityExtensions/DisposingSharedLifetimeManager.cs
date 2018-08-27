using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.Extensions.UnityExtensions
{
    public class DisposingSharedLifetimeManager : DisposingLifetimeManager, IDisposable
    {
        private object _object;

        public void Dispose()
        {
        }

        public override object GetValue()
        {
            return _object;
        }

        protected override object SynchronizedGetValue()
        {
            throw new NotImplementedException();
        }

        protected override void SynchronizedSetValue(object newValue)
        {
            throw new NotImplementedException();
        }

        public override void RemoveValue()
        {
            _object = null;
        }

        public override void SetValue(object newValue)
        {
            _object = newValue;
        }

        public override bool AppliesTo(object instance)
        {
            return instance == _object;
        }

        public override void RemoveValue(object instance)
        {
            RemoveValue();
        }
    }
}
