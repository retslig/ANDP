using System;
using System.Globalization;
using System.Web.Mvc;

namespace Common.Lib.MVC.ModelBinders
{
    public class TimeSpanModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (value.Length)
                {
                    case 5:
                        //do not need to do anything.
                        break;
                    case 4:
                        value = value.Substring(0, 2) + ":" + value.Substring(2, 2);
                        break;
                }

                //http://msdn.microsoft.com/en-us/library/ee372286.aspx
                string[] formats = { "c", "g", "G" };
                TimeSpan result;
                if (TimeSpan.TryParseExact(value, formats, CultureInfo.CurrentCulture, TimeSpanStyles.None, out result))
                    return result;

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("{0} is an invalid time span.", value));
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
