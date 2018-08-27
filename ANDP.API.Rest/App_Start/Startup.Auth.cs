
using ANDP.Lib.Factories;
using BrockAllen.MembershipReboot;
using Common.Lib.MVC.Security.Claims;
using Common.Lib.MVC.Security.OwinBasicAuth;
using Common.Lib.Security;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ANDP.API.Rest
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //// Enable the application to use a cookie to store information for the signed in user
            //// and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Configure the application for OAuth based flow
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new ApplicationOAuthProvider(PublicClientId),
            //    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    AllowInsecureHttp = true
            //};

            //// Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});

            // This must come first to intercept the /Token requests
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            var oauthServerConfig = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = AuthenticationConstants.AllowInsecureHttp,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = SecurityTokenConstants.TokenLifeTime,
                Provider = new MembershipRebootOAuthAuthorizationServerProvider(),
                RefreshTokenProvider = new MembershipRebootOAuthAuthorizationServerRefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(oauthServerConfig);

            var oauthConfig = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = AuthenticationConstants.BearerAuthType
            };
            app.UseOAuthBearerAuthentication(oauthConfig);

            app.UseBasicAuthentication(new BasicAuthenticationOptions("qssolutions.net", UserAccountServiceFactory.Create())
            {
                AuthenticationType = AuthenticationConstants.BasicAuthType,
                AuthenticationMode = AuthenticationMode.Active
            });
        }
    }
}
