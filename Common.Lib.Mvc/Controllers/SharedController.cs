using System;
using System.IO;
using System.Web.Mvc;
using Common.Lib.Utility;
using Common.Lib.MVC.Helpers;

namespace Common.Lib.MVC.Controllers
{
    /// <summary>
    /// Shared MVC Controller class that is used to retrieve views and or data
    /// for multiple applications
    /// </summary>
    public class SharedController: Controller
    {
        /// <summary>
        /// Gets the calendar event.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="busy">if set to <c>true</c> [busy].</param>
        /// <param name="beginDate">The begin date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="reminderTime">The reminder time.</param>
        /// <returns>The the ics file, you can save or open right there from the prompt.</returns>
        [HttpGet]
        public ActionResult GetCalendarEvent(string location, string subject, string body, int priority, bool busy, DateTime beginDate, DateTime endDate, int reminderTime)
        {
            var ms = CalendarHelper.GenerateCalendarIcsMemoryStream(location, subject, body, priority, busy, beginDate, endDate, reminderTime) as MemoryStream;
            return File(ms, "text/calendar", Guid.NewGuid() + ".ics");
        }

        /// <summary>
        /// Example uses in html:
        /// <link href=@Url.Action("GetEmbeddedResource", "Shared", new { resourceName = "Common.Lib.MVC.Content.Site.css", pluginAssemblyName = @Url.Content("~/bin/Common.Lib.MVC.dll") }) rel="stylesheet" type="text/css" >
        /// <script src=@Url.Action("GetEmbeddedResource", "Shared", new { resourceName = "Common.Lib.MVC.Scripts.BrowserDetection.js", pluginAssemblyName = @Url.Content("~/bin/Common.Lib.MVC.dll") }) type="text/javascript" ></script>
        /// <img src=@Url.Action("GetEmbeddedResource", "Shared", new { resourceName = "Common.Lib.MVC.Content.ajax-loader.gif", pluginAssemblyName = @Url.Content("~/bin/Common.Lib.MVC.dll") }) />
        /// Gets the embedded resource.
        /// </summary>
        /// <param name="pluginAssemblyName">Name of the plugin assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        [HttpGet]
        public FileStreamResult GetEmbeddedResource(string pluginAssemblyName, string resourceName)
        {
            try
            {
                string physicalPath = Server.MapPath(pluginAssemblyName);
                Stream stream = ResourceHelper.GetEmbeddedResource(physicalPath, resourceName);
                return new FileStreamResult(stream, GetMediaType(resourceName));
                //return new FileStreamResult(stream, GetMediaType(tempResourceName));
            }
            catch (Exception)
            {
                return new FileStreamResult(new MemoryStream(), GetMediaType(resourceName));
            }
        }

        private string GetMediaType(string fileId)
        {
            if (fileId.EndsWith(".js"))
            {
                return "text/javascript";
            }
            else if (fileId.EndsWith(".css"))
            {
                return "text/css";
            }
            else if (fileId.EndsWith(".jpg"))
            {
                return "image/jpeg";
            }
            else if (fileId.EndsWith(".gif"))
            {
                return "image/gif";
            }
            else if (fileId.EndsWith(".png"))
            {
                return "image/png";
            }
            return "text";
        }
    }
}
