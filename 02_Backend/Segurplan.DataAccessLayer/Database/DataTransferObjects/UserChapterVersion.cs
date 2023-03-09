using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class UserChapterVersion {

        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ChapterVersionId { get; set; }

        public ChapterVersion ChapterVersion { get; set; }
    }
}
