
namespace Common.Lib.Security
{
    public static class ClaimsConstants
    {
        public static string TenantIdClaimType { get { return "http://www.qssolutions.net/claims/tenant"; } }
        public static string TenantNameClaimType { get { return "http://www.qssolutions.net/claims/tenantName"; } }
        public static string UserNameWithoutTenant { get { return "http://www.qssolutions.net/claims/usernamewithouttenant"; } }
        public static string MvcClaimType { get { return "http://www.qssolutions.net/claims/mvc/"; } }
        public static string ApiClaimType { get { return "http://www.qssolutions.net/claims/api/"; } }
    }
}
