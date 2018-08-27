
namespace Common.Lib.Security
{
    public static class AuthenticationConstants
    {
        public static string BasicAuthType { get { return "Basic"; } }
        public static string BearerAuthType { get { return "Bearer"; } }
        public static string AngularAuthToken { get { return "AngularAuthToken"; } }
        //Must set this to false when local debugging.
        //Must be true for prod
        public static bool AllowInsecureHttp { get { return true; } }
    }
}
