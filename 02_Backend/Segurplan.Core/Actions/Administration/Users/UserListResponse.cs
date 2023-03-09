using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Actions.Administration.Users {
    public class UserListResponse {


        public List<ApplicationUser> UserList { get; set; }

        public int TotalRows { get; set; }


        public UserListResponse(List<ApplicationUser> userList, int totalRows) {

            UserList = userList;
            TotalRows = totalRows;
        }


    }
}
