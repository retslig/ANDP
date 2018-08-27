using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Compilation;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.Infastructure;

namespace Common.Lib.MVC.Providers.Resource
{
    public class DatabaseResourceProvider : DisposableBase, IResourceProvider
    {
        public string ResourceType { get; private set; }
        private readonly ILanguageResourceService _languageResourceService;

        //resource cache
        private readonly Dictionary<string, Dictionary<string, string>> _resourceCache = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseResourceProvider" /> class.
        /// </summary>
        /// <param name="languageResourceService">The language resource service.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public DatabaseResourceProvider(ILanguageResourceService languageResourceService, string resourceType)
        {
            ResourceType = resourceType;
            _languageResourceService = languageResourceService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseResourceProvider" /> class.
        /// </summary>
        protected DatabaseResourceProvider()
        {

        }

        #region IResourceProvider Members

        /// <summary>
        /// Retrieves a resource entry based on the specified culture and 
        /// resource key. The resource type is based on this instance of the
        /// DBResourceProvider as passed to the constructor.
        /// To optimize performance, this function caches values in a dictionary
        /// per culture.
        /// </summary>
        /// <param name="resourceKey">The resource key to find.</param>
        /// <param name="culture">The culture to search with.</param>
        /// <returns>If found, the resource string is returned. 
        /// Otherwise an empty string is returned.</returns>
        public object GetObject(string resourceKey, CultureInfo culture)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException("DatabaseResourceProvider object is already disposed.");
            }

            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }

            if (culture == null)
            {
                culture = CultureInfo.CurrentUICulture;
            }

            string resourceValue = null;
            Dictionary<string, string> resCacheByCulture = null;
            // check the cache first
            // find the dictionary for this culture
            // check for the inner dictionary entry for this key
            if (_resourceCache.ContainsKey(culture.Name))
            {
                resCacheByCulture = _resourceCache[culture.Name];
                if (resCacheByCulture.ContainsKey(resourceKey))
                {
                    resourceValue = resCacheByCulture[resourceKey];
                }
            }

            // if not in the cache, go to the database
            if (resourceValue == null)
            {
                resourceValue = _languageResourceService.GetResourceByTypeAndCultureAndKey(ResourceType, culture, resourceKey);

                // add this result to the cache
                // find the dictionary for this culture
                // add this key/value pair to the inner dictionary
                lock (this)
                {
                    if (resCacheByCulture == null)
                    {
                        resCacheByCulture = new Dictionary<string, string>();
                        _resourceCache.Add(culture.Name, resCacheByCulture);
                    }
                    resCacheByCulture.Add(resourceKey, resourceValue);
                }
            }
            return resourceValue;
        }

        /// <summary>
        /// Returns a resource reader.
        /// </summary>
        public System.Resources.IResourceReader ResourceReader
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException("DatabaseResourceProvider object is already disposed.");
                }

                // this is required for implicit resources 
                // this is also used for the expression editor sheet 

                ListDictionary resourceDictionary = _languageResourceService.GetResourcesByTypeAndCulture(ResourceType, CultureInfo.InvariantCulture);

                return new DatabaseResourceReader(resourceDictionary);
            }

        }

        #endregion

        protected override void Cleanup()
        {
            try
            {
                if (_languageResourceService != null)
                    GC.SuppressFinalize(_languageResourceService);
                _resourceCache.Clear();
            }
            finally
            {
                base.Cleanup();
            }
        }
    }
}
