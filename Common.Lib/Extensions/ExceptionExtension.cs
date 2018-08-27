using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Common.Lib.Extensions
{
    public static class ExceptionExtension
    {
        public static Exception GetInnerMostExceptionWithEntityValidationInfo(this Exception ex)
        {
            var tempException = ex;
            while (tempException.InnerException != null)
            {
                tempException = tempException.InnerException;
            }

            string entityValidationErrors = "";
            if (ex is DbEntityValidationException)
            {
                foreach (var errors in ((DbEntityValidationException)ex).EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        entityValidationErrors += "In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage;
                    }
                }
            }

            string message = tempException.Message + RetrieveEntityExceptionDataAsString(ex);
            var customException = new Exception(message, tempException);
            return customException;
        }

        public static Exception GetInnerMostException(this Exception ex)
        {
            var tempException = ex;
            while (tempException.InnerException != null)
            {
                tempException = tempException.InnerException;
            }

            return tempException;
        }

        public static Exception AddEntityValidationInfo(this Exception ex)
        {
            string message = ex.Message + RetrieveEntityExceptionDataAsString(ex);
            var customException = new Exception(message, ex);
            return customException;
        }

        public static Exception WrapExceptionWithEntityValidationInfo(this Exception ex, string message)
        {
            message += RetrieveEntityExceptionDataAsString(ex);
            var customException = new Exception(message, ex);
            return customException;
        }

        public static Exception WrapException(this Exception ex, string message)
        {
            var customException = new Exception(message, ex);
            return customException;
        }

        public static List<object> RetrieveEntityExceptionDataAsObjectList(this Exception ex)
        {
            List<object> exceptionData = (from object key in ex.Data.Keys select ex.Data[key]).ToList();

            if (ex is DbEntityValidationException)
            {
                foreach (var errors in ((DbEntityValidationException)ex).EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        exceptionData.Add("In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage);
                    }
                }
            }

            return exceptionData;
        }

        public static string RetrieveEntityExceptionDataAsString(this Exception ex)
        {
            string message = string.Empty;

            if (ex is DbEntityValidationException)
            {
                message = ((DbEntityValidationException)ex).EntityValidationErrors.Aggregate(message, (current1, errors) =>
                    errors.ValidationErrors.Aggregate(current1, (current, error) => current + ("In Entity " + errors.Entry.Entity.GetType().Name + " - " + error.ErrorMessage + Environment.NewLine)));
            }

            return message;
        }
    }
}
