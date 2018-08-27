/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * contributed by Pedro Felix
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ANDP.Lib.Factories;
using BrockAllen.MembershipReboot;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace Common.Lib.MVC.Security.OwinBasicAuth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly string _challenge;

        public BasicAuthenticationHandler(BasicAuthenticationOptions options)
        {
            _challenge = "Basic realm=" + options.Realm;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var authzValue = Request.Headers.Get("Authorization");
            if (string.IsNullOrEmpty(authzValue) || !authzValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            var token = authzValue.Substring("Basic ".Length).Trim();
            var claims = await TryGetPrincipalFromBasicCredentials(token);

            if (claims == null)
            {
                return null;
            }
            else
            {
                var id = new ClaimsIdentity(claims, Options.AuthenticationType);
                var ticket = new AuthenticationTicket(id, new AuthenticationProperties());
                return ticket;
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);
                if (challenge != null)
                {
                    Response.Headers.AppendValues("WWW-Authenticate", _challenge);
                }
            }

            return Task.FromResult<object>(null);
        }

        async Task<IEnumerable<Claim>> TryGetPrincipalFromBasicCredentials(string credentials)
        {
            string pair;
            try
            {
                pair = Encoding.UTF8.GetString(
                    Convert.FromBase64String(credentials));
            }
            catch (FormatException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            var ix = pair.IndexOf(':');
            if (ix == -1)
            {
                return null;
            }

            var username = pair.Substring(0, ix);
            var pw = pair.Substring(ix + 1);

            return await ValidationFunction(username, pw);
        }

        private Task<IEnumerable<Claim>> ValidationFunction(string userName, string password)
        {
            IEnumerable<Claim> claims = null;
            UserAccount user;
            string tenant;

            var parts = userName.Split('\\');
            if (parts.Length > 1)
            {
                tenant = parts[0];
                userName = parts[1];
            }
            else
            {
                throw new Exception("Cannot determine tenant and username.");
            }

            var userAccountService = UserAccountServiceFactory.Create();
            if (userAccountService.Authenticate(tenant, userName, password, out user))
            {
                claims = user.GetAllClaims();
            }

            return Task.FromResult(claims);
        }
    }
}