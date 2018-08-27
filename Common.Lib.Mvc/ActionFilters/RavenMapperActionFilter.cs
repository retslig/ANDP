using System;
using System.Web.Mvc;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;

namespace Common.Lib.MVC.ActionFilters
{
    public class CommonMapperActionFilter : BaseActionFilter
    {
        private readonly ICommonMapper _iCommonMapper;
        private readonly Type _sourceType;
        private readonly Type _destType;

        public CommonMapperActionFilter(ICommonMapper iCommonMapper, Type sourceType, Type destType)
        {
            _iCommonMapper = iCommonMapper;
            _sourceType = sourceType;
            _destType = destType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = filterContext.Controller.ViewData.Model;
           
            object viewModel = ObjectFactory.CreateInstanceAndMapAndValidate(_iCommonMapper, _sourceType, _destType, model);
            //ModelState.Merge(viewModel.ValidationErrors, _iCommonMapper.FindTypeMapFor(_sourceType, _destType));

            filterContext.Controller.ViewData.Model = viewModel;
        }
    }

    public abstract class BaseActionFilter : IActionFilter, IResultFilter
    {
        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public virtual void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
