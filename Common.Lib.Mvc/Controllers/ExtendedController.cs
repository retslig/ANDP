using System.IO;
using System.Web.Mvc;
using Common.Lib.MVC.ActionResults;
using Common.Lib.Mapping;

namespace Common.Lib.MVC.Controllers
{
    public class ExtendedController : Controller
    {
        public string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult =
                    ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext,
                    viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        protected CommonMapperActionResult AutoMapView<TDestination>(ViewResult viewResult)
        {
            return new CommonMapperActionResult(_iCommonMapper, viewResult.ViewData.Model.GetType(), typeof(TDestination), viewResult);
        }

        protected virtual ICommonMapper _iCommonMapper { get; set; }
    }
}
