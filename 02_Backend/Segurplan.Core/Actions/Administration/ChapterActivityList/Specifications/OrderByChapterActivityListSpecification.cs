using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications {
    public class OrderByChapterActivityListSpecification : Specification<ChapterActivityListResponse.ListItem> {
        public void ByNumber() => OrderBy(c => c.Number);
        public void ByNumberDesc() => ApplyOrderByDescending(c => c.Number);
        public void ByTituloAsc() => OrderBy(c => c.Title);
        public void ByTituloDesc() => ApplyOrderByDescending(c => c.Title);
        public void ByApprovementDateAsc() => OrderBy(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().ApprovementDate);
        public void ByApprovementDateDesc() => ApplyOrderByDescending(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().ApprovementDate);
        public void ByVersionNumberAsc() => OrderBy(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().VersionNumber);
        public void ByVersionNumberDesc() => ApplyOrderByDescending(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().VersionNumber);
        public void ByCreatedByAsc() => OrderBy(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().CreatedByNavigation == null ? "" : c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().CreatedByNavigation.CompleteName);
        public void ByCreatedByDesc() => ApplyOrderByDescending(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().CreatedByNavigation==null?"" : c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().CreatedByNavigation.CompleteName);
        public void ByApprovedByAsc() => OrderBy(c => c.ChapterVersion.OrderByDescending(x=>x.VersionNumber).FirstOrDefault().AprovedByNavigation == null ? "" : c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().AprovedByNavigation.CompleteName);
        public void ByApprovedByDesc() => ApplyOrderByDescending(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().AprovedByNavigation==null?"": c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().AprovedByNavigation.CompleteName);
        public void ByReviewerAsc() => OrderBy(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().IdReviewerNavigation == null ? "" : c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().IdReviewerNavigation.CompleteName);
        public void ByReviewerDesc() => ApplyOrderByDescending(c => c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().IdReviewerNavigation==null?"": c.ChapterVersion.OrderByDescending(x => x.VersionNumber).FirstOrDefault().IdReviewerNavigation.CompleteName);
    }
}
