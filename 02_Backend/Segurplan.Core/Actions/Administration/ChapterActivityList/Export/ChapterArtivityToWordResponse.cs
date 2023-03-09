using System.IO;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Export {
    public class ChapterArtivityToWordResponse {
        public MemoryStream MemoryFile;
        public ChapterArtivityToWordResponse(MemoryStream memoryFile) {
            this.MemoryFile = memoryFile;
        }
    }
}
