using System;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using Common.Lib.MVC.Exceptions;

namespace Common.Lib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PreventDuplicateFormSubmitAttribute : ActionFilterAttribute
    {
        private readonly string _redirectUrl;
        private readonly TimeSpan _allowedSubmissionTime;

        public PreventDuplicateFormSubmitAttribute(string redirectUrl, TimeSpan allowedSubmissionTime)
        {
            _redirectUrl = redirectUrl;
            _allowedSubmissionTime = allowedSubmissionTime;
        }

        public PreventDuplicateFormSubmitAttribute(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
            _allowedSubmissionTime = new TimeSpan(0, 0, 0, 60);
        }

        public PreventDuplicateFormSubmitAttribute()
        {
            _redirectUrl = "Home";
            _allowedSubmissionTime = new TimeSpan(0, 0, 0, 60);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            
            // check if session is supported
            if (ctx.Session != null)
            {
                //This will not work on thin clietns will have to come up with idea to have this be unique
                string empId = ctx.Request.Form["CurrentTimeEntry.EmployeeId"];
                if (!string.IsNullOrEmpty(empId))
                {
                    if (ctx.Session["PreventDuplicateFormSubmitTime" + empId] != null)
                    {
                        DateTime current = DateTime.Now;
                        DateTime previouslySubmitted = DateTime.ParseExact(ctx.Session["PreventDuplicateFormSubmitTime" + empId].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        previouslySubmitted = previouslySubmitted.Add(_allowedSubmissionTime);

                        //The time must be greater then previous time plus allowed time.
                        if(current > previouslySubmitted)
                        {
                            if (string.IsNullOrEmpty(_redirectUrl))
                                throw new SessionExpiredException("You have submitted this form already.");
                        
                            filterContext.Result = new RedirectResult(_redirectUrl);
                            return;
                        }
                    }
                    else
                    {
                        ctx.Session["PreventDuplicateFormSubmitTime" + empId] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        private readonly string _redirectUrl;

        public SessionExpireAttribute(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public SessionExpireAttribute()
        {
            _redirectUrl = "Home";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ctx = filterContext.HttpContext;

            // check if session is supported
            if (ctx.Session != null){
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        if (string.IsNullOrEmpty(_redirectUrl))
                            throw new SessionExpiredException("Please refresh you web browser and try again.");
                        
                        //ctx.Response.Redirect(_redirectUrl);
                        filterContext.Result = new RedirectResult(_redirectUrl);
                        return;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class HttpParamActionAttribute : ActionNameSelectorAttribute
    {
        private static string _defaultAction = "Action";

        public static string DefaultAction
        {
            get { return _defaultAction; }
            set { _defaultAction = value; }
        }

        public HttpParamActionAttribute()
        {
        }

        public HttpParamActionAttribute(string defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!actionName.Equals(_defaultAction, StringComparison.InvariantCultureIgnoreCase))
                return false;

            var request = controllerContext.RequestContext.HttpContext.Request;
            return request[methodInfo.Name] != null;
        }
    }
}
