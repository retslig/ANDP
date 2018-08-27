using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace Common.Lib.Extensions.UnityExtensions
{
    public class DisposingPerTenantLifeTimeManager : DisposingLifetimeManager, IDisposable
    {
        private readonly Func<HttpContextBase> _context;
        private readonly IDictionary<Guid, object> _store;
        private Guid RetrieveTenantId
        {
            get
            {
                //Get the current claims principal
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Get the claims values
                var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                   .Select(c => c.Value).SingleOrDefault();
                var sid = identity.Claims.Where(c => c.Type == ClaimTypes.Sid)
                                   .Select(c => c.Value).SingleOrDefault();
                
                return Guid.Parse("5A56264C-889B-411C-8A34-A986DB1CCE95");
            }
        }

        public DisposingPerTenantLifeTimeManager(Func<HttpContextBase> context)
        {
            _context = context;
            _store = new Dictionary<Guid, object>();
        }

        protected override object SynchronizedGetValue()
        {
            var tenantId = RetrieveTenantId;

            if (!_store.ContainsKey(tenantId))
                return null;

            return _store[tenantId];
        }

        protected override void SynchronizedSetValue(object newValue)
        {
            _store[RetrieveTenantId] = newValue;
        }

        private readonly List<WeakReference> _values = new List<WeakReference>();

        public override object GetValue()
        {
            return null;
        }

        public override void SetValue(object newValue)
        {
            RemoveDeadReferences();
            _values.Add(new WeakReference(newValue));
        }

        public override void RemoveValue()
        {
        }

        public override bool AppliesTo(object instance)
        {
            return _values.Any(wr => wr.Target == instance);
        }

        public override void RemoveValue(object instance)
        {
            // Silverlight does not have FindIndex(...) method, so we use an old fashioned loop.
            for (int i = _values.Count - 1; i >= 0; i--)
            {
                if (_values[i].Target == instance)
                {
                    _values.RemoveAt(i);
                    break;
                }
            }

            RemoveDeadReferences();
        }

        public void Dispose()
        {
            // Class must be IDisposable to be retained in the list of Lifetime Managers
            _values.Clear();
        }

        private void RemoveDeadReferences()
        {
            for (int i = _values.Count - 1; i >= 0; i--)
            {
                if (!_values[i].IsAlive)
                    _values.RemoveAt(i);
            }
        }
    }
}
