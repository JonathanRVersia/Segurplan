using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Create {
    public class CreateSubChapterResponse {
        public SubChapterDetailsSubChapterVersion SubChapterVersion { get; set; }

        public CreateSubChapterResponse(SubChapterDetailsSubChapterVersion subChapterVersion) {
            SubChapterVersion = subChapterVersion;
        }
    }
}
