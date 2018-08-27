namespace Common.Lib.Security
{
    public class Oauth2AuthenticationSettings
    {
        public string Url { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TenantName { get; set; }
    }
}