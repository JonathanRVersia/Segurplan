using System.Collections.Generic;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Models {
    public class ChapterDetailsChapter {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Number { get; set; }

        public ICollection<OtherChapterVersions> ChapterVersion { get; set; }
    }
}
