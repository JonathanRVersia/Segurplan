using System;
using System.Collections.Generic;
using System.Text;
using static Segurplan.Core.Actions.Administration.ChapterActivityList.ChapterActivityListResponse;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.GetChapterVersions {
    public class GetChapterVersionsResponse {
        public GetChapterVersionsResponse(ICollection<ChapterActivityListChapterVersion> chapterVersions) {
            ChapterVersions = chapterVersions;
        }

        public ICollection<ChapterActivityListChapterVersion> ChapterVersions { get; set; }
    }
}
