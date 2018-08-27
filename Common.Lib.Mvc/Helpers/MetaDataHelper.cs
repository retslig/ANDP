using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Common.Lib.MVC.Helpers
{
    public class ExtendedDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class HideColumnAttribute : Attribute
        {
            public HideColumnAttribute()
                : this(true, true)
            {
            }

            public HideColumnAttribute(bool hideColumnInPopUp, bool hideColumnInGrid)
            {
                HideColumnInPopUp = hideColumnInPopUp;
                HideColumnInGrid = hideColumnInGrid;
            }

            public bool HideColumnInPopUp { get; set; }

            public bool HideColumnInGrid { get; set; }
        }

        public class ExtendedMetadata : DataAnnotationsModelMetadata
        {
            public ExtendedMetadata(DataAnnotationsModelMetadataProvider provider, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName,
                                            DisplayColumnAttribute displayColumnAttribute, IEnumerable<Attribute> attributes)
                : base(provider, containerType, modelAccessor, modelType, propertyName, displayColumnAttribute)
            {
                Attributes = new List<Attribute>(attributes);
            }

            public IList<Attribute> Attributes { get; private set; }
        }


        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            string errorMessage = attributes.OfType<RemoteAttribute>()
                                        .Select(field => field.ErrorMessage)
                                        .LastOrDefault();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                metadata.AdditionalValues.Add("ValidationErrorMessage", errorMessage);
            }

            var hiddenAttribute = attributes.OfType<HideColumnAttribute>().LastOrDefault();
            if (hiddenAttribute != null)
            {
                metadata.AdditionalValues.Add("HideColumnAttribute", hiddenAttribute);
            }

            return metadata;

        }
    }
}
