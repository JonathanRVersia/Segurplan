using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Segurplan.FrameworkExtensions.Identity {
    public class AvailableClaims : IEnumerable<Claim> {
        private readonly IEnumerable<Claim> claims;

        public AvailableClaims(IEnumerable<ClaimsCollection> definedClaims) {
            claims = definedClaims.SelectMany(c => c);
        }

        public IEnumerator<Claim> GetEnumerator() {
            return claims.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return claims.GetEnumerator();
        }
    }
}
