using Segurplan.Core.Actions.Administration.ChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Create {
    public class CreateChapterResponse {
        public ChapterDetailsChapterVersion ChapterVersion { get; set; }

        public CreateChapterResponse(ChapterDetailsChapterVersion chapterVersion) {
            ChapterVersion = chapterVersion;
        }
    }
}
