using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;

namespace ANDP.ProvisionCenter.Mvc.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        [ValidateInput(false)]
        public ActionResult General(Exception exception, string controllerName, string controllerAction)
        {
            List<object> exceptionData = (from object key in exception.Data.Keys select exception.Data[key]).ToList();

            if (exception is DbEntityValidationException)
            {
                foreach (var errors in ((DbEntityValidationException)exception).EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        exceptionData.Add("In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage);
                    }
                }
            }

            _logger.WriteLogEntry(exceptionData, string.Format("Error caught in controller: {0} and Action: {1}", controllerName, controllerAction),
                LogLevelType.Error, exception, "", System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName());

            ViewData["exceptionStackTrace"] = exception.StackTrace ?? "No Stack Trace";
            return View("Error", new HandleErrorInfo(exception, controllerName ?? "Error", controllerAction ?? "General"));
        }

        [ValidateInput(false)]
        public ActionResult Http404(Exception exception)
        {
            List<object> exceptionData = (from object key in exception.Data.Keys select exception.Data[key]).ToList();
            _logger.WriteLogEntry(exceptionData, "HTTP 404", LogLevelType.Error, exception);

            ViewData["exceptionStackTrace"] = "";
            return View("Error", new HandleErrorInfo(exception, "Unknown", "Unknown"));
        }

        [ValidateInput(false)]
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}