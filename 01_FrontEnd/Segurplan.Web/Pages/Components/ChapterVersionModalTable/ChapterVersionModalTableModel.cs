using System.Collections.Generic;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;
using static Segurplan.Core.Actions.Administration.ChapterActivityList.ChapterActivityListResponse;

namespace Segurplan.Web.Pages.Components.ChapterVersionModalTable {
    public class ChapterVersionModalTableModel {
        public ICollection<ChapterActivityListChapterVersion> ChapterVersions { get; set; }

        public bool HideDeleteIfOnlyOneVersion { get; set; }

        public ChapterActivitiesDeleteCheckModalModel DeleteCheck { get; set; }
    }
}
