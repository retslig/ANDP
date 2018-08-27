using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using Common.Lib.Data.Repositories.LanguageResource;
using System.Runtime.CompilerServices;
using System.Threading;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using LanguageResource = Common.Lib.Domain.Common.Models.LanguageResource;
using DaoLanguageResource = Common.Lib.Data.Repositories.LanguageResource.LanguageResource;

namespace Common.Lib.Domain.Common.Services
{
    public class LanguageResourceService : ILanguageResourceService
    {
        #region private variables

        private readonly ILanguageResourceRepository _iLanguageResourceRepository;
        private readonly ICommonMapper _iCommonMapper;

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="iCommonMapper">The i common mapper.</param>
        /// <param name="defaultResourceCulture">The default resource culture.</param>
        public LanguageResourceService(ILanguageResourceRepository repository, ICommonMapper iCommonMapper, string defaultResourceCulture)
        {
            _iLanguageResourceRepository = repository;
            DefaultResourceCulture = defaultResourceCulture;
            _iCommonMapper = iCommonMapper;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="iCommonMapper">The i common mapper.</param>
        public LanguageResourceService(ILanguageResourceRepository repository, ICommonMapper iCommonMapper)
        {
            _iLanguageResourceRepository = repository;
            _iCommonMapper = iCommonMapper;
            DefaultResourceCulture = "en-US";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceService" /> class.
        /// </summary>
        /// <param name="defaultResourceCulture">The default resource culture.</param>
        protected LanguageResourceService(string defaultResourceCulture, ICommonMapper iCommonMapper)
        {
            DefaultResourceCulture = defaultResourceCulture;
            _iCommonMapper = iCommonMapper;
            //_repository = new LanguageResourceRepository();
            //DefaultResourceCulture = "en-US";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceService" /> class.
        /// </summary>
        protected LanguageResourceService(ICommonMapper iCommonMapper)
        {
            _iCommonMapper = iCommonMapper;
            //_repository = new LanguageResourceRepository();
            //DefaultResourceCulture = "en-US";
        }

        #endregion

        #region implement functions

        public string DefaultResourceCulture { get; private set; }

        /// <summary>
        /// Gets the resource by type and culture and key internal.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Data.DataException"></exception>
        private string GetResourceByTypeAndCultureAndKeyInternal(string resourceType, CultureInfo culture, string resourceKey)
        {
            // we should only get one back, but just in case, we'll iterate reader results
            var resources = new List<string>();
            string resourceValue;

            var languageResource = _iLanguageResourceRepository.RetrieveResourcesByTypeAndCultureAndKey(resourceType, culture.Name, resourceKey).ToList();
            var languageResources = ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoLanguageResource>, IEnumerable<LanguageResource>>(_iCommonMapper, languageResource).ToList();

            languageResources.ForEach(x => resources.Add(x.Value));

            // we should only get 1 back, this is just to verify the tables aren't incorrect
            if (resources.Count == 0)
            {
                // is this already fallback location?
                if (culture.Name == DefaultResourceCulture)
                {
                    throw new InvalidOperationException(string.Format(Thread.CurrentThread.CurrentUICulture,
                                                                      "Unable to find a default resource for {0}.",
                                                                      resourceKey));
                }

                // try to get parent culture
                culture = culture.Parent;
                if (culture.Name.Length == 0)
                {
                    // there isn't a parent culture, change to neutral
                    culture = new CultureInfo(DefaultResourceCulture);
                }
                resourceValue = GetResourceByTypeAndCultureAndKeyInternal(resourceType, culture, resourceKey);
            }
            else if (resources.Count == 1)
            {
                resourceValue = resources[0];
            }
            else
            {
                // if > 1 row returned, log an error, we shouldn't have > 1 value for a resourceKey!
                throw new DataException(string.Format(Thread.CurrentThread.CurrentUICulture,
                                                      "An internal data error has occurred. A duplicate resource entry was found for {0}.",
                                                      resourceKey));
            }

            return resourceValue;
        }

        /// <summary>
        /// Gets the resources by type and culture.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ListDictionary GetResourcesByTypeAndCulture(string resourceType, CultureInfo culture)
        {
            // make sure we have a default culture at least
            if (culture == null || culture.Name.Length == 0)
            {
                culture = new CultureInfo(DefaultResourceCulture);
            }
            var languageResource = _iLanguageResourceRepository.RetrieveResourcesByTypeAndCulture(resourceType, culture.Name).ToList();
            var languageResources = ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoLanguageResource>, IEnumerable<LanguageResource>>(_iCommonMapper, languageResource).ToList();

            // create the dictionary
            var resourceDictionary = new ListDictionary();

            languageResources.ForEach(x => resourceDictionary.Add(x.Key, x.Value));

            // TODO: check dispose results
            return resourceDictionary;
        }

        /// <summary>
        /// Gets the resource by type and culture and key.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetResourceByTypeAndCultureAndKey(string resourceType, CultureInfo culture, string resourceKey)
        {
            var resourceValue = string.Empty;

            // make sure we have a default culture at least
            if (culture == null || culture.Name.Length == 0)
            {
                culture = new CultureInfo(DefaultResourceCulture);
            }

            // recurse to find resource, includes fallback behavior
            resourceValue = GetResourceByTypeAndCultureAndKeyInternal(resourceType, culture, resourceKey);

            return resourceValue;
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
