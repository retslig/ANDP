/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using Owin;

namespace Thinktecture.IdentityManager.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityManagerConfiguration()
            {
                UserManagerFactory = Thinktecture.IdentityManager.MembershipReboot.UserManagerFactory.Create
                //UserManagerFactory = Thinktecture.IdentityManager.AspNetIdentity.UserManagerFactory.Create
            };

            //Comment out when local for debugging
            app.Map("/IdentityManager", _ => { _.UseIdentityManager(options); });
            //Comment out when local for production
            //app.Map("", _ => { _.UseIdentityManager(options); });
        }
    }

    
}