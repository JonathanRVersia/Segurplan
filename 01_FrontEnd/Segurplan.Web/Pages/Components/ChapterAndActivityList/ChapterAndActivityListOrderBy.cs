using System;
using Segurplan.Core.Actions.Administration.ChapterActivityList.Specifications;

namespace Segurplan.Web.Pages.Components.ChapterAndActivityList {
    public class ChapterAndActivityListOrderBy {
        public string SortOrder { get; set; } = nameof(SortOrderEnum.ByChapterNumberAsc);

        public OrderByChapterActivityListSpecification DefaultSpecification() {
            var orderBySpecifications = new OrderByChapterActivityListSpecification();
            orderBySpecifications.ByNumber();
            return orderBySpecifications;
        }

        public OrderByChapterActivityListSpecification ToSpecification() {
            var orderBySpecifications = new OrderByChapterActivityListSpecification();

            switch (Enum.Parse(enumType: typeof(SortOrderEnum), SortOrder)) {
                case SortOrderEnum.ByChapterNumberAsc:
                    orderBySpecifications.ByNumber();
                    break;
                case SortOrderEnum.ByChapterNumberDesc:
                    orderBySpecifications.ByNumberDesc();
                    break;
                case SortOrderEnum.ByTituloAsc:
                    orderBySpecifications.ByTituloAsc();
                    break;
                case SortOrderEnum.ByTituloDesc:
                    orderBySpecifications.ByTituloDesc();
                    break;
                case SortOrderEnum.ByApprovementDateAsc:
                    orderBySpecifications.ByApprovementDateAsc();
                    break;
                case SortOrderEnum.ByApprovementDateDesc:
                    orderBySpecifications.ByApprovementDateDesc();
                    break;
                case SortOrderEnum.ByVersionNumberAsc:
                    orderBySpecifications.ByVersionNumberAsc();
                    break;
                case SortOrderEnum.ByVersionNumberDesc:
                    orderBySpecifications.ByVersionNumberDesc();
                    break;
                case SortOrderEnum.ByCreatedByAsc:
                    orderBySpecifications.ByCreatedByAsc();
                    break;
                case SortOrderEnum.ByCreatedByDesc:
                    orderBySpecifications.ByCreatedByDesc();
                    break;
                case SortOrderEnum.ByApprovedByAsc:
                    orderBySpecifications.ByApprovedByAsc();
                    break;
                case SortOrderEnum.ByApprovedByDesc:
                    orderBySpecifications.ByApprovedByDesc();
                    break;
                case SortOrderEnum.ByReviewerAsc:
                    orderBySpecifications.ByReviewerAsc();
                    break;
                case SortOrderEnum.ByReviewerDesc:
                    orderBySpecifications.ByReviewerDesc();
                    break;
            }
            return orderBySpecifications;
        }
    }

    public enum SortOrderEnum {
        ByChapterNumberAsc,
        ByChapterNumberDesc,
        ByTituloAsc,
        ByTituloDesc,
        ByApprovementDateAsc,
        ByApprovementDateDesc,
        ByVersionNumberAsc,
        ByVersionNumberDesc,
        ByCreatedByAsc,
        ByCreatedByDesc,
        ByApprovedByAsc,
        ByApprovedByDesc,
        ByReviewerAsc,
        ByReviewerDesc
    }
}
