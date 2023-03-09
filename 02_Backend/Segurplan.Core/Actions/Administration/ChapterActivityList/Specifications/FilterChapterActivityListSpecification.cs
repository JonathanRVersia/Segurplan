using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications {
    public class FilterChapterActivityListSpecification : Specification<ChapterActivityListResponse.ListItem> {
        public void ByChapterNumber(int? chapterNumber) => Criteria(ch => ch.Number == chapterNumber);

        public void ByTitle(string title) => Criteria(ch => ch.Title.Contains(title));

        public void ByCreatedBy(string userName) => Criteria(ch => ch.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().CreatedByNavigation.CompleteName.ToUpper().Contains(userName.ToUpper()));

        public void ByReviewedBy(string userName) => Criteria(ch => ch.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().IdReviewerNavigation.CompleteName.ToUpper().Contains(userName.ToUpper()));

        public void ByApprovedBy(string userName) => Criteria(ch => ch.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().AprovedByNavigation.CompleteName.ToUpper().Contains(userName.ToUpper()));
    }
}
