using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ANDP.Lib.Factories;
using BrockAllen.MembershipReboot;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Common.Lib.MVC.Security.Claims
{
    public class MembershipRebootOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //private readonly UserAccountService _userAccountService;

        //public MembershipRebootOAuthAuthorizationServerProvider(UserAccountService userAccountService)
        //{
        //    _userAccountService = userAccountService;
        //}

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var _userAccountService = UserAccountServiceFactory.Create();
            string cid, csecret;
            if (context.TryGetBasicCredentials(out cid, out csecret))
            {
                if (_userAccountService.Authenticate("clients", cid, csecret))
                {
                    context.OwinContext.Set<string>("as:client_id", cid);
                    context.Validated();
                }
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", "*");

            return Task.FromResult<object>(null);
        }

        public override Task ValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            var _userAccountService = UserAccountServiceFactory.Create();
            if (context.TokenRequest.IsResourceOwnerPasswordCredentialsGrantType)
            {
                if (_userAccountService.Authenticate(context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Scope[0],
                    context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.UserName,
                    context.TokenRequest.ResourceOwnerPasswordCredentialsGrant.Password))
                {
                    context.Validated();
                }
            }

            if (context.TokenRequest.IsRefreshTokenGrantType)
            {
                var token = context.TokenRequest.Parameters.Get("refresh_token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }
        
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var _userAccountService = UserAccountServiceFactory.Create();
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            UserAccount user;
            if (_userAccountService.Authenticate(context.Scope.FirstOrDefault(), context.UserName, context.Password, out user))
            {
                var claims = user.GetAllClaims();
                var id = new ClaimsIdentity(claims, "MembershipReboot");
 
                // create metadata to pass on to refresh token provider
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        { "as:client_id", context.ClientId }
                    });
 
                var ticket = new AuthenticationTicket(id, props);
                context.Validated(ticket);
            }

            return base.GrantResourceOwnerCredentials(context);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.OwinContext.Get<string>("as:client_id");

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return;
            }

            context.Validated();
        }
    }
}