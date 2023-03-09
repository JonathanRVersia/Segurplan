using System.Collections.Generic;

namespace Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal {
    public class ChapterActivitiesDeleteCheckModalModel {
        public int? ChapterToRemoveId { get; set; }

        public List<int> SubChapterToRemoveIds { get; set; }

        public bool ActivityHasPlan { get; set; }

        public string Message { get; set; }
    }
}
