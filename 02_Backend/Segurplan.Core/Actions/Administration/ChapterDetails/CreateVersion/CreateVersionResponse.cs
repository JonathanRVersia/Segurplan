
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CreateVersion {
    public class CreateVersionResponse {

        public CreateVersionResponse(ChapterDetailsChapterVersion chapterVersion) {
            ChapterVersion = chapterVersion;
        }

        public ChapterDetailsChapterVersion ChapterVersion { get; set; }
    }
}
