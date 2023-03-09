using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Segurplan.DataAccessLayer.Database.Identity {
    public partial class Role : IdentityRole {

        public IEnumerable<UserRole> UserRoleNavigation { get; set; }

    }
}
