
using System;
using System.Linq;
using System.Web.Mvc;

namespace Common.Lib.MVC.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// To the select list.
        /// Since Enum Type implements IConvertible interface will use this and then check type afterward.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="selected">The selected.</param>
        /// <returns></returns>
        public static SelectList ToSelectList<T>(this T enumeration, string selected) //where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            var source = Enum.GetValues(typeof(T));
            var items = source.Cast<object>().ToDictionary(key => (int)key, value => Enum.GetName(typeof(T), value));
            return new SelectList(items, "Key", "Value", selected);
        }
    }
}
