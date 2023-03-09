
namespace Segurplan.Core.Actions.Administration.SubChapterDetails.Save {
    public class SaveSubChapterResponse {
        public SaveSubChapterResponse(int savedId,int createdSubChapterId) {
            SavedId = savedId;
            CreatedSubChapterId = createdSubChapterId;
        }

        public int SavedId { get;  }
        public int CreatedSubChapterId { get; }
    }
}
