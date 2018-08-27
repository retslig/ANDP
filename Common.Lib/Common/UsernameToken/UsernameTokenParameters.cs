using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.ServiceModel.Security.Tokens;

namespace Common.Lib.Common.UsernameToken
{
    /// <summary>
    /// UsernameTokenParameters for use with the Username Token
    /// </summary>
    public class UsernameTokenParameters : SecurityTokenParameters
    {
        protected override SecurityTokenParameters CloneCore()
        {
            return new UsernameTokenParameters();
        }

        protected override void InitializeSecurityTokenRequirement(SecurityTokenRequirement requirement)
        {
            requirement.TokenType = Constants.UsernameTokenType;
            return; 
        }

        // A username token has no crypto, no windows identity and supports only client authentication
        protected override bool HasAsymmetricKey { get { return false; } }
        protected override bool SupportsClientAuthentication { get { return true; } }
        protected override bool SupportsClientWindowsIdentity { get { return false; } }
        protected override bool SupportsServerAuthentication { get { return false; } }

        protected override SecurityKeyIdentifierClause CreateKeyIdentifierClause(SecurityToken token, SecurityTokenReferenceStyle referenceStyle)
        {
            if (referenceStyle == SecurityTokenReferenceStyle.Internal)
            {
                return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
            }
            else
            {
                throw new NotSupportedException("External references are not supported for username tokens");
            }
        }
    }
}
