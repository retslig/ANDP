using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Common.Lib.Common.UsernameToken
{
    public class UserNameTokenAuthorizationPolicy : IAuthorizationPolicy
    {
        string _id;
        ClaimSet _issuer;
        IEnumerable<ClaimSet> _issuedClaimSets;

        public UserNameTokenAuthorizationPolicy(ClaimSet issuedClaims)
        {
            if (issuedClaims == null)
                throw new ArgumentNullException("issuedClaims");
            _issuer = issuedClaims.Issuer;
            _issuedClaimSets = new ClaimSet[] { issuedClaims };
            _id = Guid.NewGuid().ToString();
        }

        public ClaimSet Issuer { get { return _issuer; } }

        public string Id { get { return _id; } }

        public bool Evaluate(EvaluationContext context, ref object state)
        {
            foreach (ClaimSet issuance in _issuedClaimSets)
            {
                context.AddClaimSet(this, issuance);
            }
            return true;
        }
    }
}
