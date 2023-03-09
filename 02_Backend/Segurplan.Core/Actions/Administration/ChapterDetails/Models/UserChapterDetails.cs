using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    //Many to many relationship between chapterVersion and User
    public class UserChapterDetails {
        public int UserId { get; set; }

        public int ChapterVersionId { get; set; }

        public ChapterDetailsUserInfo User { get; set; }
    }
}
