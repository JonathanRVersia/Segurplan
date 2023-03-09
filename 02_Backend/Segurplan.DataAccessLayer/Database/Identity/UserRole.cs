using Microsoft.AspNetCore.Identity;

namespace Segurplan.DataAccessLayer.Database.Identity {
    public partial class UserRole : IdentityUserRole<int> {
        public string NormalizedName { get; set; }
        public string UserName { get; set; }
        public string UserRoleName { get; set; }
        public string Email { get; set; }
        public string ModifiedDate { get; set; }
        public string CreateDate { get; set; }
        public string CompleteName { get; set; }
    }
}
