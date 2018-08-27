using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Lib.Extensions
{
    static class CloneExtensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
