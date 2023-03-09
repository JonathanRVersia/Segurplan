using System.Collections.Generic;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DetailDropdowns {

    public class ChapterDetailsDropdownResponse {

        public List<ChapterDetailsUserInfo> Users { get; set; }

        public ChapterDetailsDropdownResponse(List<ChapterDetailsUserInfo> users) {

            Users = users;
        }
    }
}
