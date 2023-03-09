using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.EntityFramework.Query;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList {
    public class ChapterActivityListResponse : IPaginable {

        public ChapterActivityListResponse(QueryResult<ListItem> chapters = null) {
            Chapters = chapters?.Results.ToList() ?? new List<ListItem>();
            IsPaginated = chapters.IsPaginated;
            Page = chapters.Page;
            PageSize = chapters.PageSize;
            SkippedRows = chapters.SkippedRows;
            TotalCount = chapters.TotalCount;
        }

        public List<ListItem> Chapters { get; set; }

        public bool IsPaginated { get; }
        public int? Page { get; }
        public int? PageSize { get; }
        public int? SkippedRows { get; }
        public int? TotalCount { get; }

        public class ListItem {
            public int Id { get; set; }

            public int Number { get; set; }

            public string Title { get; set; }

            //public ChapterActivityListVersionInfo IdVersionInfoNavigation { get; set; }

            public ICollection<ChapterActivityListChapterVersion> ChapterVersion { get; set; }
        }

        public class ChapterActivityListUser {
            public string CompleteName { get; set; }
        }

        //public class ChapterActivityListVersionInfo {
        //    public int VersionNumber { get; set; }
        //}

        public class ChapterActivityListChapterVersion {
            public int Id { get; set; }

            public int IdChapter { get; set; }

            public string Title { get; set; }

            public int IdChapterNavigationNumber { get; set; }

            public DateTime? ApprovementDate { get; set; }

            public ChapterActivityListUser AprovedByNavigation { get; set; }

            public ChapterActivityListUser CreatedByNavigation { get; set; }

            public ChapterActivityListUser IdReviewerNavigation { get; set; }

            public List<UserChapterDetails> ProducedBy { get; set; }

            public int VersionNumber { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime? EndDate { get; set; }

            public DateTime? ReviewDate { get; set; }

            //public ChapterActivityListVersionInfo IdVersionInfoNavigation { get; set; }
        }
    }
}
