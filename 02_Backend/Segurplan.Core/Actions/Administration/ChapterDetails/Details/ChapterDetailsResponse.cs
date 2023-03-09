using Segurplan.Core.Actions.Administration.ChapterDetails.Models;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.Details {
    public class ChapterDetailsResponse {

        public ChapterDetailsChapterVersion ChapterVersion { get; set; }

        public ChapterDetailsResponse(ChapterDetailsChapterVersion chapterVersion) {
            ChapterVersion = chapterVersion;
        }

        //    public class ChapterDetailsChapterVersion  {
        //        public int Id { get; set; }

        //        public int IdChapter { get; set; }

        //        public string Title { get; set; }

        //        public DateTime? ApprovementDate { get; set; }

        //        public DateTime CreateDate { get; set; }

        //        public DateTime UpdateDate { get; set; }

        //        public ChapterDetailsUser AprovedByNavigation { get; set; }

        //        public ChapterDetailsUser CreatedByNavigation { get; set; }

        //        public ChapterDetailsUser IdReviewerNavigation { get; set; }

        //        public int? IdReviewer { get; set; }

        //        public int? IdApprover { get; set; }

        //        public List<UserChapterDetails> ProducedBy { get; set; }

        //        public ChapterDetailsVersionInfo IdVersionInfoNavigation { get; set; }

        //        public ICollection<ChapterDetailsSubChapterVersion> SubChapterVersion { get; set; }

        //        public string WorkDetails { get; set; }

        //        public string WorkOrganization { get; set; }

        //        public string MachineTool { get; set; }

        //        public string AssociatedDetails { get; set; }

        //        //Chapter on original
        //        public ChapterDetailsChapter IdChapterNavigation { get; set; }
        //    }

        //}

        //public class UserChapterDetails {

        //    //public int Id { get; set; }

        //    public int UserId { get; set; }

        //    public int ChapterVersionId { get; set; }

        //    public ChapterDetailsUser User { get; set; }
        //}

        //public class ChapterDetailsChapter {
        //    public ICollection<OtherChapterVersions> ChapterVersion { get; set; }
        //}

        //public class OtherChapterVersions {
        //    public int Id { get; set; }

        //    public string Title { get; set; }

        //    public DateTime? ApprovementDate { get; set; }

        //    public ChapterDetailsVersionInfo IdVersionInfoNavigation { get; set; }
        //}

        //public class ChapterDetailsSubChapterVersion {
        //    //Archivo de detalles asociados falta

        //    public int Id { get; set; }

        //    ////public int IdSubChapter { get; set; }

        //    ////public int IdChapterVersion { get; set; }

        //    ////public int Number { get; set; }

        //    public string Title { get; set; }

        //    //public string Description { get; set; }

        //    //public string WorkDetails { get; set; }

        //    //public string WorkOrganization { get; set; }

        //    //public string RiskEvaluation { get; set; }

        //    //public string MachineTool { get; set; }

        //    //public string AssociatedDetails { get; set; }

        //    //public string SupportFacilities { get; set; }

        //    //public int? IdReviewer { get; set; }

        //    //public DateTime? ReviewDate { get; set; }

        //    //public int? IdApprover { get; set; }

        //    //public DateTime? ApprovementDate { get; set; }

        //    //public int CreatedBy { get; set; }

        //    //public DateTime CreateDate { get; set; }

        //    //public int? ModifiedBy { get; set; }

        //    //public DateTime? UpdateDate { get; set; }

        //    //public SubChapter IdSubChapterNavigation { get; set; }

        //    //public ChapterVersion IdChapterVersionNavigation { get; set; }

        //    //public User AprovedByNavigation { get; set; }

        //    //public User IdReviewerNavigation { get; set; }

        //    //public User CreatedByNavigation { get; set; }

        //    //public User ModifiedByNavigation { get; set; }

        //    //public ICollection<ActivityVersion> ActivityVersion { get; set; }
        //}

        //public class ChapterDetailsVersionInfo {
        //    public int VersionNumber { get; set; }
        //}

        //public class ChapterDetailsUser {
        //    public int Id { get; set; }

        //    public string NormalizedUserName { get; set; }
        //}
    }
}
