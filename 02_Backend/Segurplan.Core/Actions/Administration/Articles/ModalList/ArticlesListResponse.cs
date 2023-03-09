using System.Collections.Generic;
using System.Linq;
using Segurplan.FrameworkExtensions.EntityFramework.Query;

namespace Segurplan.Core.Actions.Administration.Articles.ModalList {
    public class ArticlesListResponse {
        public ArticlesListResponse(QueryResult<ListItem> articles) {
            this.Articles = articles?.Results.ToList() ?? new List<ListItem>();

            IsPaginated = articles.IsPaginated;
            Page = articles.Page;
            PageSize = articles.PageSize;
            SkippedRows = articles.SkippedRows;
            TotalCount = articles.TotalCount;
        }
        public List<ListItem> Articles;
        public bool IsPaginated { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? SkippedRows { get; set; }
        public int? TotalCount { get; set; }
        public class ListItem {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
