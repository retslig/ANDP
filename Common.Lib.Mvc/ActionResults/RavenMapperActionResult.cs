using System;
using System.Collections;
using System.Web.Mvc;
using Common.Lib.Infastructure;
using Common.Lib.MVC.Extensions;
using Common.Lib.Mapping;

namespace Common.Lib.MVC.ActionResults
{
    public class CommonMapperActionResult : ActionResult
    {
        public ViewResult View { get; private set; }
        private readonly ICommonMapper _iCommonMapper;
        private readonly Type _sourceType;
        private readonly Type _destType;

        public CommonMapperActionResult(ICommonMapper iCommonMapper, Type sourceType, Type destType, ViewResult view)
        {
            _iCommonMapper = iCommonMapper;
            _sourceType = sourceType;
            _destType = destType;
            View = view;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            dynamic model = ObjectFactory.CreateInstanceAndMap(_iCommonMapper, _sourceType, _destType, View.ViewData.Model);
            View.ViewData.Model = model;
            View.ViewData.ModelState.Merge(model.ValidationErrors as IDictionary, _iCommonMapper.FindTypeMapFor(_sourceType, _destType));

            View.ExecuteResult(context);
        }
    }
}
