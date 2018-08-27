using System.Collections.Generic;
using System.Linq;
using Common.Lib.Utility;
using Newtonsoft.Json.Linq;

namespace Common.Lib.Extensions
{
    public static class JObjectExtensions
    {
        public static SerializableDictionary<string, object> ToDictionary(this JObject obj)
        {
            var result = obj.ToObject<SerializableDictionary<string, object>>();

            var jObjectKeys = (from r in result
                               let key = r.Key
                               let value = r.Value
                               where value != null && value.GetType() == typeof(JObject)
                               select key).ToList();

            var jArrayKeys = (from r in result
                              let key = r.Key
                              let value = r.Value
                              where value != null && value.GetType() == typeof(JArray)
                              select key).ToList();

            jArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
            jObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

            return result;
        }
    }
}
