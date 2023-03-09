using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Segurplan.FrameworkExtensions.Identity {
    public abstract class ClaimsCollection : IEnumerable<Claim> {
        private readonly List<Claim> claims;

        public ClaimsCollection() {
            claims = new List<Claim>();
        }

        public void Add(string claim) {
            Add(new Claim(ClaimTypes.AuthorizationDecision, claim));
        }

        public void Add(Claim claim) {
            claims.Add(claim);
        }

        public IEnumerator<Claim> GetEnumerator() {
            return claims.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return claims.GetEnumerator();
        }
    }
}
