/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * contributed by Pedro Felix
 * see license.txt
 */

using Owin;
//using Thinktecture.IdentityModel.Owin;

namespace Common.Lib.MVC.Security.OwinBasicAuth
{
    public static class BasicAuthMiddlewareExtensions
    {
        //public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, string realm, BasicAuthenticationMiddleware.CredentialValidationFunction  validationFunction)
        //{
        //    var options = new BasicAuthenticationOptions(realm, validationFunction);
        //    return app.UseBasicAuthentication(options);
        //}

        public static IAppBuilder UseBasicAuthentication(this IAppBuilder app, BasicAuthenticationOptions options)
        {
            return app.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}