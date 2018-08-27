using System.Collections.Generic;
using System.Linq;

namespace Common.Lib.Data.Repositories.LanguageResource
{
    public class LanguageResourceRepository : ILanguageResourceRepository
    {
        private readonly ICommon_LanguageResource_Entities _iCommonLanguageResourceEntities;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceRepository" /> class.
        /// Protect this so that forces caller to inject repos.
        /// </summary>
        protected LanguageResourceRepository()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageResourceRepository" /> class.
        /// Force caller to inject repos.
        /// </summary>
        /// <param name="iCommonLanguageResourceEntities">The i Common entities.</param>
        public LanguageResourceRepository(ICommon_LanguageResource_Entities iCommonLanguageResourceEntities)
        {
            _iCommonLanguageResourceEntities = iCommonLanguageResourceEntities;
        }

        #endregion

        /// <summary>
        /// Retrieves the resources by culture and key.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IEnumerable<LanguageResource> RetrieveResourcesByTypeAndCultureAndKey(string type, string cultureCode, string key)
        {
            return _iCommonLanguageResourceEntities.LanguageResources.Where(
                        p => p.ResourceKey == key && p.CultureCode == cultureCode && p.ResourceType == type);
        }

        /// <summary>
        /// Retrieves the resources by culture.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="cultureCode">The culture code.</param>
        /// <returns></returns>
        public IEnumerable<LanguageResource> RetrieveResourcesByTypeAndCulture(string type, string cultureCode)
        {
            return _iCommonLanguageResourceEntities.LanguageResources.Where(
                        p => p.CultureCode == cultureCode && p.ResourceType == type);
        }
    }
}