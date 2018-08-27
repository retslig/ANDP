using System.Collections.Generic;

namespace Common.Lib.Data.Repositories.LanguageResource
{
    public interface ILanguageResourceRepository
    {
        IEnumerable<LanguageResource> RetrieveResourcesByTypeAndCultureAndKey(string type, string cultureCode, string key);
        IEnumerable<LanguageResource> RetrieveResourcesByTypeAndCulture(string type, string cultureCode);
    }
}
