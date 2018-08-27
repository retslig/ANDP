using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Common.Lib.MVC.ModelBinders
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var acceptedFormats = new List<string>();
            //[DateTimeFormats(AcceptedFormats = new[] { "{0:MM-dd-yyyy}", "{0:MMddyyyy}", "{0:MMddyy}" })]
            if (bindingContext.ModelMetadata.AdditionalValues.ContainsKey("DateTimeFormatsAttribute"))
            {
                acceptedFormats = new List<string>((string[])bindingContext.ModelMetadata.AdditionalValues["DateTimeFormatsAttribute"])
                    {
                        //Always accept Default Date 
                        //May need to add a new one at some time for mm/dd/yy hh:mm:ss am/pm for the database value that comes in.
                        "{0:MM/dd/yyyy}",
                        "{0:MM/d/yyyy}",
                        "{0:M/dd/yyyy}",
                        "{0:M/d/yyyy}",
                        "{0:MM/dd/yyyy hh:mm:ss tt}",
                        "{0:MM/d/yyyy hh:mm:ss tt}",
                        "{0:M/dd/yyyy hh:mm:ss tt}",
                        "{0:M/d/yyyy hh:mm:ss tt}",
                        
                    };

                ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (acceptedFormats.Any() && !string.IsNullOrWhiteSpace(value.AttemptedValue))
                {
                    foreach (var acceptedFormat in acceptedFormats)
                    {
                        DateTime date;
                        string displayFormat = acceptedFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                        // use the format specified in the DisplayFormat attribute to parse the date
                        if (DateTime.TryParseExact(value.AttemptedValue, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                            return date;
                    }

                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("{0} is an invalid date format.", value.AttemptedValue));
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
