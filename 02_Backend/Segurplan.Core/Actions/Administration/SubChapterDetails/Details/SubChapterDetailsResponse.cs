using Segurplan.Core.Actions.Administration.SubChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Details {
    public class SubChapterDetailsResponse {
        public SubChapterDetailsSubChapterVersion SubChapterVersion { get; set; }

        public SubChapterDetailsResponse(SubChapterDetailsSubChapterVersion subChapterVersion) {
            SubChapterVersion = subChapterVersion;
        }
    }
}
