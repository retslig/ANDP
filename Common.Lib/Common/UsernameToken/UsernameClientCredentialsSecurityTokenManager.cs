using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Security.Tokens;
using Common.Lib.Common.UsernameToken;

namespace Common.Lib.Common.UsernameToken
{
    public class UsernameClientCredentialsSecurityTokenManager : ClientCredentialsSecurityTokenManager
    {
        UsernameClientCredentials _userNameClientCredentials;

        public UsernameClientCredentialsSecurityTokenManager(UsernameClientCredentials userNameClientCredentials)
            : base(userNameClientCredentials)
        {
            this._userNameClientCredentials = userNameClientCredentials;
        }

        public override SecurityTokenProvider CreateSecurityTokenProvider(SecurityTokenRequirement tokenRequirement)
        {
            // handle this token for Custom
            if (tokenRequirement.TokenType == Constants.UsernameTokenType)
                return new UsernameTokenProvider(this._userNameClientCredentials.UsernameInfo);
            // return other tokens, e.g. server cert if needed
            else if (tokenRequirement is InitiatorServiceModelSecurityTokenRequirement)
            {
                if (tokenRequirement.TokenType == SecurityTokenTypes.X509Certificate)
                {
                    return new X509SecurityTokenProvider(_userNameClientCredentials.ServiceCertificate.DefaultCertificate);
                }
            }

            return base.CreateSecurityTokenProvider(tokenRequirement);
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {

            return new UsernameSecurityTokenSerializer(version);
        }

    }

    public class UsernameServiceCredentialsSecurityTokenManager : ServiceCredentialsSecurityTokenManager
    {
        UsernameServiceCredentials _userNameServiceCredentials;

        public UsernameServiceCredentialsSecurityTokenManager(UsernameServiceCredentials userNameServiceCredentials)
            : base(userNameServiceCredentials)
        {
            this._userNameServiceCredentials = userNameServiceCredentials;
        }

        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement.TokenType == Constants.UsernameTokenType)
            {
                outOfBandTokenResolver = null;
                return new UsernameTokenAuthenticator(_userNameServiceCredentials.Validator);
            }

            return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
        }

        public override SecurityTokenSerializer CreateSecurityTokenSerializer(SecurityTokenVersion version)
        {
            return new UsernameSecurityTokenSerializer(version);
        }
    }

}
