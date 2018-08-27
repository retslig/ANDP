using System;
using System.Web.Mvc;

namespace Common.Lib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeFormatsAttribute : ActionFilterAttribute
    {
        public string[] AcceptedFormats;

        public DateTimeFormatsAttribute()
        {
            
        }

        public DateTimeFormatsAttribute(string[] acceptedFormats)
        {
            AcceptedFormats = acceptedFormats;
        }
    }
}
