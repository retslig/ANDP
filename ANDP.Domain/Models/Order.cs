using System;
using System.Collections.Generic;
using Common.Lib.Infastructure;
using Common.Lib.Utility;
using CustomValidationService = ANDP.Lib.Domain.Services.CustomValidationService;

namespace ANDP.Lib.Domain.Models
{
    public class Order
    {
        #region ************************** Core *******************************

        public int Id { get; set; } // Id (Primary key)
        public string ExternalOrderId { get; set; } // ExternalOrderId
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public int Priority { get; set; } // Priority
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public string OrginatingIp { get; set; } // OrginatingIp
        public StatusType StatusType { get; set; }
        public ActionType ActionType { get; set; }
        public string ResultMessage { get; set; } // ResultMessage
        public string Log { get; set; }
        public bool ResponseSent { get; set; }
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string ServiceProvider { get; set; } // ILEC or CLEC
        public string Configuration { get; set; }
        public string Product { get; set; }
        public string ClassOfService { get; set; }
        public string CSR { get; set; } // CreatedByUser
        public string Notes { get; set; }

        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public Account Account { get; set; }
        public List<Service> Services { get; set; } // Service.FK_Service_Order //Note:Has to be a list instead of a icollection so that it can be serialized.
        
        #endregion

        #region ************************** Custom for NewNet *******************************

        public string NewNetCaseId { get; set; } // NewNet CaseId
        public string NewNetRouteIndex { get; set; } // NewNet RouteIndex
        public string NewNetTriggerId { get; set; } // NewNet TriggerId
        public string NewNetProcessId { get; set; } // NewNet ProcessId
        public string NewNetTaskId { get; set; } // NewNet TaskId 
        public string NewNetUserId { get; set; } // NewNet UserId

         #endregion

        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(CustomValidationService customValidationService)
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            if (string.IsNullOrWhiteSpace(ExternalOrderId))
            {
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.ExternalOrderId), "Order.ExternalOrderId is a mandatory field.");
            }

            if (string.IsNullOrWhiteSpace(ExternalAccountId))
            {
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.ExternalAccountId), "Order.ExternalAccountId is a mandatory field.");
            }
            
            if (String.IsNullOrWhiteSpace(ExternalCompanyId))
            {
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.ExternalCompanyId), "Order.ExternalCompanyId is a mandatory field.");
            }
            else
            {
                //ToDo: probably want to call a repo method to make sure that the company exists in the Companies table. Then again the FK in the database will enforce this as well.
                //Would this be a wastefully database hit?
            }

            if (Priority == null || Priority < 1)
            {
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Priority), "Order.Priority is a mandatory field.");
            }

            if (ProvisionDate == null || ProvisionDate == DateTime.MinValue)
            {
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.ProvisionDate), "Order.ProvisionDate is a mandatory field.");
            }

            if (Account == null)
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Account), "Order.Account is a mandatory field.");
            else
            {
                if (Account.Validate())
                {
                    foreach (var validationError in Account.ValidationErrors)
                    {
                        if (!ValidationErrors.ContainsKey(validationError.Key))
                        {
                            ValidationErrors.Add(validationError.Key, validationError.Value);
                        }
                    }
                }
            }

            if (Services == null)
                ValidationErrors.Add(LambdaHelper<Order>.GetPropertyName(x => x.Services), "Order.Services is a mandatory field.");
            else
            {
                foreach (var service in Services)
                {
                    if (service.Validate(customValidationService))
                    {
                        foreach (var validationError in service.ValidationErrors)
                        {
                            if (!ValidationErrors.ContainsKey(validationError.Key))
                            {
                                ValidationErrors.Add(validationError.Key, validationError.Value);
                            }
                        }
                    }
                }
            }

            return ValidationErrors.Count > 0;
        }

    }
}
