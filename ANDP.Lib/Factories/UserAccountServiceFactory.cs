using System;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;

namespace ANDP.Lib.Factories
{
    public static class UserAccountServiceFactory
    {
        public static string ConnectionString { get; set; }

        public static UserAccountService Create()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentNullException("ConnectionString", "ConnectionString is empty.");

            var config = new MembershipRebootConfiguration
            {
                PasswordHashingIterationCount = 10000,
                RequireAccountVerification = false,
                //config.DefaultTenant = "",
                MultiTenant = true
            };
            return new UserAccountService(config,
                new DefaultUserAccountRepository(
                    new DefaultMembershipRebootDatabase(ConnectionString)));
        }
    }
}
