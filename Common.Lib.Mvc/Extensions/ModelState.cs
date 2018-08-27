using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Common.Lib.Mapping;

namespace Common.Lib.MVC.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Merges the specified model state.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="autoMapperTypeMap">The auto mapper type map.</param>
        public static void Merge(this ModelStateDictionary modelState, IDictionary dictionary, CommonTypeMap autoMapperTypeMap)
        {
            var maps = autoMapperTypeMap.GetPropertyMaps().Where(p=>p.SourceMember != null);

            foreach (var key in dictionary.Keys)
            {
                var propertyName = maps.FirstOrDefault(p => p.SourceMember.Name == key.ToString());
                string name = propertyName != null ? propertyName.DestinationProperty.Name : key.ToString();

                modelState.AddModelError(name, dictionary[key].ToString());
            }
        }

        /// <summary>
        /// Merges the specified model state.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="modelType">Type of the model if is a child class.</param>
        /// <param name="autoMapperTypeMap">The auto mapper type map.</param>
        public static void Merge(this ModelStateDictionary modelState, IDictionary dictionary, PropertyInfo modelType, CommonTypeMap autoMapperTypeMap)
        {
            string prefix = modelType != null ? modelType.Name : "";
            var maps = autoMapperTypeMap.GetPropertyMaps().Where(p => p.SourceMember != null);

            foreach (var key in dictionary.Keys)
            {
                var propertyName = maps.FirstOrDefault(p => p.SourceMember.Name == key.ToString());
                string name = propertyName != null ? propertyName.DestinationProperty.Name : key.ToString();

                modelState.AddModelError((string.IsNullOrEmpty(prefix) ? "" : (prefix + ".")) + name, dictionary[key].ToString());
            }
        }

        /// <summary>
        /// Merges the specified model state.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="dictionary">The dictionary.</param>
        public static void Merge(this ModelStateDictionary modelState, IDictionary dictionary)
        {
            foreach (var key in dictionary.Keys)
            {
                modelState.AddModelError(key.ToString(), dictionary[key].ToString());
            }
        }

        /// <summary>
        /// Merges the specified model state.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="modelType">Type of the model.</param>
        public static void Merge(this ModelStateDictionary modelState, IDictionary dictionary, PropertyInfo modelType)
        {
            string prefix = modelType != null ? modelType.Name : "";
            foreach (var key in dictionary.Keys)
            {
                modelState.AddModelError((string.IsNullOrEmpty(prefix) ? "" : (prefix + ".")) + key, dictionary[key].ToString());
            }
        }

        /// <summary>
        /// Retrieves the state of the dictionary of errors from model.
        /// This method helps to get the error information from the MVC "ModelState".
        /// We can not directly send the ModelState to the client in Json. The "ModelState"
        /// object has some circular reference that prevents it to be serialized to Json.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <returns></returns>
        public static Dictionary<string, object> RetrieveDictionaryOfErrorsFromModelState(this ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in modelState.Keys)
            {
                // Only send the errors to the client.
                if (modelState[key].Errors.Count > 0)
                {
                    errors[key] = modelState[key].Errors;
                }
            }

            return errors;
        }
    }
}
