using System;
using ANDP.Lib.Data.Repositories.Order;
using Common.Lib.Infastructure;
using Common.Lib.Utility;

namespace ANDP.Lib.Domain.Services
{
    public class CustomValidationService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly int _companyId;

        public CustomValidationService(IOrderRepository orderRepository, int companyId)
        {
            _orderRepository = orderRepository;
            _companyId = companyId;
        }

        public bool Validate(object o, Type type)
        {
            return Validate(o, type, _orderRepository.RetrieveCustomValidation(_companyId, type.Name));
        }

        public bool Validate(object o, Type type, string customSourceValidationCode)
        {
            string baseClass =
                "    public class CustomValidation" +
                "    {" +
                "        public bool Validate(object o)" +
                "        {" +
                "            if (o == null)" +
                "                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Services), \"Order.Services is a mandatory field.\");" +
                "            if (!(o is Order))" +
                "                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Services), \"Order.Services is a mandatory field.\");" +
                customSourceValidationCode + 
                "            return ValidationErrors.Count > 0;" +
                "        }" +
                "        public SerializableDictionary<string, string> ValidationErrors { get; set; }" +
                "    }";

            string className = "CustomValidation";
            string methodName = "Validate";
            string propertyName = "ValidationErrors";
            var referencedAssemblies = new[] { "Common.Lib.Utility", "ANDP.Lib.Domain.Models" };

            DynamicCodeService.CompileSourceCodeDom(customSourceValidationCode, referencedAssemblies);

            var result = (bool)DynamicCodeService.ExecuteMethodFromAssembly(className, methodName, null);
            ValidationErrors = (SerializableDictionary<string, string>)DynamicCodeService.RetrievePropertyValueFromAssembly(className, propertyName);
            return result;
        }

        public SerializableDictionary<string, string> ValidationErrors { get; set; }
    }
}

//namespace tests
//{
//    using ANDP.Lib.Domain.Models;
//    using Common.Lib.Utility;

//    public class CustomValidation
//    {
//        public bool Validate(object o)
//        {
//            if (o == null)
//                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Services), "Order.Services is a mandatory field.");

//            if (!(o is Order))
//                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Services), "Order.Services is a mandatory field.");

//            //Do Stuff Here


//            return ValidationErrors.Count > 0;
//        }

//        public SerializableDictionary<string, string> ValidationErrors { get; set; }
//    }
//}