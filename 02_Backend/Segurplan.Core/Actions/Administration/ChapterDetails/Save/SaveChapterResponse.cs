
namespace Segurplan.Core.Actions.Administration.ChapterDetails.Save {
    public class SaveChapterResponse {

        public SaveChapterResponse(int savedId,int createdChapterId) {
            SavedId = savedId;
            CreatedChapterId = createdChapterId;
        }

        public int SavedId { get; }
        public int CreatedChapterId { get; }
    }
}
