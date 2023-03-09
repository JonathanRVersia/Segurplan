using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.Users.Details {
    public class UserDetailsResponse {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string CompleteName { get; set; }
        public string UserRole { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Email { get; set; }
        public string UserADGuid { get; set; }
        public bool IsSuscribed { get; set; }
    }
}
