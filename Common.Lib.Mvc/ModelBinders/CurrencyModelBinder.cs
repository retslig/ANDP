using System;
using System.Globalization;
using System.Web.Mvc;

namespace Common.Lib.MVC.ModelBinders
{
    public class CurrencyModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                decimal result;
                if (Decimal.TryParse(value.AttemptedValue, NumberStyles.Currency, CultureInfo.CurrentCulture, out result))
                    return result;

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("{0} is an invalid currency.", value.AttemptedValue));
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
