using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Common.Lib.Extensions
{
    public static class SecurityClaimsExtension
    {
        /// <summary>
        /// Determines if a claim of claimType AND claimValue exists in any of the identities.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="type">the type of the claim to match.</param>
        /// <param name="value">the value of the claim to match.</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <returns>
        /// true if a claim is matched, false otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type
        /// or
        /// value</exception>
        public static bool HasClaim(this IEnumerable<Claim> claims, string type, string value, bool caseSensitive)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return caseSensitive ?
                claims.Any(claim => claim.Value == value && claim.Type == type) :
                claims.Any(claim => claim.Value.ToLower() == value && claim.Type.ToLower() == type);
        }
    }
}
