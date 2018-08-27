using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;

namespace Common.Lib.Common.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly CachePriorityType _cacheItemPriority;
        private readonly Action<CacheEntryRemovedArguments> _cacheEntryRemovedCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider"/> class.
        /// </summary>
        public MemoryCacheProvider()
        {
            _cacheEntryRemovedCallback = DoNothing;
            _cacheItemPriority = CachePriorityType.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public MemoryCacheProvider(Action<CacheEntryRemovedArguments> callback)
        {
            _cacheEntryRemovedCallback = callback;
            _cacheItemPriority = CachePriorityType.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheProvider" /> class.
        /// </summary>
        /// <param name="cacheEntryRemovedCallback">The cache entry removed callback.</param>
        /// <param name="cacheItemPriority">The cache item priority.</param>
        public MemoryCacheProvider(Action<CacheEntryRemovedArguments> cacheEntryRemovedCallback, CachePriorityType cacheItemPriority)
        {
            _cacheEntryRemovedCallback = cacheEntryRemovedCallback;
            _cacheItemPriority = cacheItemPriority;
        }

        public long Count<T>()
        {


            return _cache.Count(p => p.Value is T);
        }

        /// <summary>
        /// Fetches the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Fetch<T>(string key)
        {
            return (T)_cache[key];
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }

        public IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, List<string> filePaths)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FetchAll<T>()
        {
            return _cache.Where(p=>p.Value is T).Select(p => (T)p.Value);
        }

        /// <summary>
        /// Fetches the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="retrieveData">The retrieve data.</param>
        /// <returns></returns>
        public T Fetch<T>(string key, Func<T> retrieveData)
        {
            T value;
            if (!TryGetValue(key, out value))
                value = retrieveData();

            return value;
        }

        public T FetchAndCache<T>(string key, Func<T> retrieveData, DateTime absoluteExpiry)
        {
             var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                        ? CacheItemPriority.Default
                        : CacheItemPriority.NotRemovable,
            };

            T value;
            if (!TryGetValue(key, out value))
            {
                value = retrieveData();
                 policy.AbsoluteExpiration = absoluteExpiry;
                _cache.Set(key, value, policy);
            }
            return value;
        }

        public T FetchAndCache<T>(string key, Func<T> retrieveData, List<string> filePaths)
        {
            var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                       ? CacheItemPriority.Default
                       : CacheItemPriority.NotRemovable,
            };

            if (filePaths != null && filePaths.Count > 0)
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

            T value;
            if (!TryGetValue(key, out value))
            {
                value = retrieveData();
                _cache.Set(key, value, policy);
            }
            return value;
        }

        public IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, DateTime absoluteExpiry)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, TimeSpan slidingExpiry)
        {
            throw new NotImplementedException();
        }

        public T FetchAndCache<T>(string key, Func<T> retrieveData, TimeSpan slidingExpiry)
        {
            if (_cacheEntryRemovedCallback == null)
                throw new Exception("If you are using sliding expiry then you must also provide cacheEntryRemovedCallback method in constructor.  This is null");

            var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                       ? CacheItemPriority.Default
                       : CacheItemPriority.NotRemovable,
            };

            T value;
            if (!TryGetValue(key, out value))
            {
                value = retrieveData();
                policy.SlidingExpiration = (TimeSpan)slidingExpiry;
                policy.RemovedCallback += args => _cacheEntryRemovedCallback(args);
                _cache.Set(key, value, policy);
            }
            return value;
        }

        public void SetCache<T>(string key, T data, DateTime absoluteExpiry)
        {
            var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                    ? CacheItemPriority.Default
                    : CacheItemPriority.NotRemovable,
                AbsoluteExpiration = (DateTime) absoluteExpiry,
            };

            _cache.Set(key, data, policy);
        }

        public void SetCache<T>(string key, T data, TimeSpan slidingExpiry)
        {
            var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                    ? CacheItemPriority.Default
                    : CacheItemPriority.NotRemovable,
                SlidingExpiration = (TimeSpan) slidingExpiry,
            };

            _cache.Set(key, data, policy);
        }

        public void SetCache<T>(string key, T data, List<string> filePaths)
        {
            var policy = new CacheItemPolicy
            {
                Priority = (_cacheItemPriority == CachePriorityType.Default)
                        ? CacheItemPriority.Default
                        : CacheItemPriority.NotRemovable,
            };
            policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

            _cache.Set(key, data, policy);
        }

        private bool TryGetValue<T>(string key, out T value)
        {
            object cachedValue = _cache[key];
            if (cachedValue == null)
            {
                value = default(T);
                return false;
            }
            try
            {
                value = (T)cachedValue;
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        private void DoNothing(CacheEntryRemovedArguments args)
        {
              
        }
    }
}
