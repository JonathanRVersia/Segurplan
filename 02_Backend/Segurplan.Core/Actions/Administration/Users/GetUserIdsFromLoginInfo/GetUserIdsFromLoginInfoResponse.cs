using System.Collections.Generic;

namespace Segurplan.Core.Actions.Administration.Users.GetUserIdsFromLoginInfo {
    public class GetUserIdsFromLoginInfoResponse {
        public List<int> UserIds { get; set; } = new List<int>();
    }
}
