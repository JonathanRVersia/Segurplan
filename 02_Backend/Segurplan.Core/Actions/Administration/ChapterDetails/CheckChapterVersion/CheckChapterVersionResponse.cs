using System.Collections.Generic;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.CheckChapterVersion {
    public class CheckChapterVersionResponse {
        public CheckChapterVersionResponse(List<int> notValidChapterNumbers) {
            NotCurrentChapterNumbers = notValidChapterNumbers;
        }

        public List<int> NotCurrentChapterNumbers { get; set; }
    }
}
