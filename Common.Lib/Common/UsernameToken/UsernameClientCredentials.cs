//  Copyright (c) Microsoft Corporation.  All Rights Reserved.

using System;
using System.IdentityModel.Selectors;
using System.ServiceModel.Description;

namespace Common.Lib.Common.UsernameToken
{
    /// <summary>
    /// UsernameClientCredentials for use with Username Token
    /// </summary>
    public class UsernameClientCredentials : ClientCredentials
    {
        UsernameInfo _usernameInfo;

        public UsernameClientCredentials(UsernameInfo usernameInfo)
            : base()
        {
            if (usernameInfo == null)
                throw new ArgumentNullException("usernameInfo");

            _usernameInfo = usernameInfo;
        }

        public UsernameInfo UsernameInfo
        {
            get { return _usernameInfo; }
        }

        protected override ClientCredentials CloneCore()
        {
            return new UsernameClientCredentials(_usernameInfo);
        }

        public override SecurityTokenManager CreateSecurityTokenManager()
        {
            return new UsernameClientCredentialsSecurityTokenManager(this);
        }
    }
}
