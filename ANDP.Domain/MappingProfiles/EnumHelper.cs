using ANDP.Lib.Domain.Models;
using ANDP.Lib.Data.Repositories.Order;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public static class StatusTypeHelper
    {
        public static StatusType ToStatusType(this StatusTypeEnum statusTypeEnum)
        {
            return (StatusType)((int)statusTypeEnum);
        }

        public static StatusTypeEnum ToStatusTypeEnum(this StatusType statusType)
        {
            return (StatusTypeEnum)((int)statusType);
        }
    }

    public static class ActionTypeHelper
    {
        public static ActionType ToActionType(this ActionTypeEnum actionTypeEnum)
        {
            return (ActionType)((int)actionTypeEnum);
        }

        public static ActionTypeEnum ToActionTypeEnum(this ActionType actionType)
        {
            return (ActionTypeEnum)((int)actionType);
        }
    }

    public static class MethodTypeHelper
    {
        public static ProvisionByMethodType ToProvisionByMethodTypeEnum(this ProvisionByMethodTypeEnum provisionByMethodTypeEnum)
        {
            return (ProvisionByMethodType)((int)provisionByMethodTypeEnum);
        }

        public static ProvisionByMethodTypeEnum ToProvisionByMethodTypeEnum(this ProvisionByMethodType provisionByMethodType)
        {
            return (ProvisionByMethodTypeEnum)((int)provisionByMethodType);
        }
    }
}
