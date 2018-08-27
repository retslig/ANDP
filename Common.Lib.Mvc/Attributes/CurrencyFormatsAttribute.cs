using System;
using System.Web.Mvc;

namespace Common.Lib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CurrencyFormatsAttribute : ActionFilterAttribute
    {
        public CurrencyFormatsAttribute()
        {
            
        }

    }
}
