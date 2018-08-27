using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common.Lib.MVC.Attributes;


namespace Common.Lib.MVC.Providers.ModelMetaData
{
    public class ExtendedModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
                                                        Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metaData = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            var dateTimeFormatsAttributes = attributes.OfType<DateTimeFormatsAttribute>().FirstOrDefault();
            if (dateTimeFormatsAttributes != null)
                metaData.AdditionalValues.Add("DateTimeFormatsAttribute", dateTimeFormatsAttributes.AcceptedFormats);

            return metaData;
        }
    }
}
