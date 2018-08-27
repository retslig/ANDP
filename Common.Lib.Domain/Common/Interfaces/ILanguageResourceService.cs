using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Common.Lib.Domain.Common.Interfaces
{
    public interface ILanguageResourceService : IDisposable
    {
        ListDictionary GetResourcesByTypeAndCulture(string resourceType, CultureInfo culture);
        string GetResourceByTypeAndCultureAndKey(string resourceType, CultureInfo culture, string resourceKey);
    }
}
