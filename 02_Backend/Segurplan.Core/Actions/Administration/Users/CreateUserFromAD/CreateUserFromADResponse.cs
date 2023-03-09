using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.Users.CreateUserFromAD {
    public class CreateUserFromADResponse {

        public bool ExistsInDB { get; set; }
        public bool NotExistsInAd { get; set; }
        public string UserName { get; set; }
        public string CompleteName { get; set; }
        public string Email { get; set; }
        public string UserADGuid { get; set; }
    }
}
