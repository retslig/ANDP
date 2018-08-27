using System;
using System.Web.Mvc;
using Common.Lib.MVC.ActionFilters;
using Common.Lib.Mapping;

namespace Common.Lib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommonMapperAttribute : ActionFilterAttribute
    {
        private readonly Type _sourceType;
        private readonly Type _destType;

        public CommonMapperAttribute(Type sourceType, Type destType)
        {
            _sourceType = sourceType;
            _destType = destType;
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type DestType
        {
            get { return _destType; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ICommonMapper mappingEngine = DependencyResolver.Current.GetService<ICommonMapper>() ?? GetDefaultMapperFor(SourceType, DestType);
            var filter = new CommonMapperActionFilter(mappingEngine, _sourceType, _destType);
            filter.OnActionExecuted(filterContext);
        }

        private static ICommonMapper GetDefaultMapperFor(Type sourceType, Type destType)
        {
            //try
            //{
            //    Mapper.CreateMap(sourceType, destType);
            //    return Mapper.Engine;
            //}
            //finally 
            //{
            //    Mapper.Reset();
            //}
            return null;
        }
    }
}
