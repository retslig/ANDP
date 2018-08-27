/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * contributed by Pedro Felix
 * see license.txt
 */

using BrockAllen.MembershipReboot;
using Microsoft.Owin.Security;

namespace Common.Lib.MVC.Security.OwinBasicAuth
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public UserAccountService UserAccountService { get; private set; }
        public string Realm { get; private set; }

        public BasicAuthenticationOptions(string realm, UserAccountService userAccountService)
            : base("Basic")
        {
            Realm = realm;
            UserAccountService = userAccountService;
        }
    }
}