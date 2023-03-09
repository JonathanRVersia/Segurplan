using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class OtherChapterVersions {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? ApprovementDate { get; set; }

        public int VersionNumber { get; set; }

        public DateTime? EndDate { get; set; }

        //public ChapterDetailsVersionInfo IdVersionInfoNavigation { get; set; }
    }
}
