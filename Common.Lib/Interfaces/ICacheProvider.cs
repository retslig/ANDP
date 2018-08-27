using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Lib.Interfaces
{
    public interface ICacheProvider
    {
        void ClearCache(string key);
        void SetCache<T>(string key, T data, TimeSpan slidingExpiry);
        void SetCache<T>(string key, T data, DateTime absoluteExpiry);
        void SetCache<T>(string key, T data, List<string> filePaths);
        long Count<T>();
        T Fetch<T>(string key);
        T Fetch<T>(string key, Func<T> retrieveData);
        T FetchAndCache<T>(string key, Func<T> retrieveData, DateTime absoluteExpiry);
        T FetchAndCache<T>(string key, Func<T> retrieveData, TimeSpan slidingExpiry);
        T FetchAndCache<T>(string key, Func<T> retrieveData, List<string> filePaths);
        IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, DateTime absoluteExpiry);
        IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, TimeSpan slidingExpiry);
        IEnumerable<T> FetchAndCache<T>(string key, Func<IEnumerable<T>> retrieveData, List<string> filePaths);
        IEnumerable<T> FetchAll<T>();
    }
}
