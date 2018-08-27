
using System;
using System.Globalization;using System.Web.Mvc;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.Interfaces;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.MVC.Attributes;
using Common.Lib.MVC.Extensions;
using Common.Lib.Mvc.Resources;

namespace ANDP.ProvisionCenter.Mvc.Controllers
{
    [Localization]
    public class HomeController : Controller
    {
        private readonly ILanguageResourceService _languageResourceService;
        private readonly IOrderService _orderService;

        public HomeController(ILanguageResourceService languageResourceService)
        {
            if (UserContext.IsAuthenticated)
            {
                var tenantId = UserContext.RetrieveTenantId();

                _languageResourceService = languageResourceService;
                _orderService = OrderServiceFactory.Create(tenantId);
            }
        }

        public ActionResult Index()
        {
            //ViewBag.Title = HttpContext.GetLocalResourceObject("Title");
            ViewBag.Title = Global.Title;

            ////_languageResourceService.ResourceType = "Home/Index";
            //_languageResourceService.ResourceType = string.Concat(this.GetType().Name.Substring(0, this.GetType().Name.Length - 10), "/", System.Reflection.MethodBase.GetCurrentMethod().Name);
            //CultureInfo currentCultureInfo = Thread.CurrentThread.CurrentUICulture;
            //ViewBag.Title = _languageResourceService.GetResourceByCultureAndKey(currentCultureInfo, "Greeting");

            var results = _orderService.RetrieveRecentlyProvisionedServices(5);

            return View(results);
        }

        [HttpGet]
        public ActionResult GetCurrentCulture()
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            CultureInfo uici = CultureInfo.CurrentUICulture;

            return Json(ci.DisplayName, JsonRequestBehavior.AllowGet);
        }
    }
}
