using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Articles {
    public class ArticlesListResponse {
        public List<ApplicationArticle> ArticlesList { get; set; } = new List<ApplicationArticle>();
        public int TotalRows { get; set; }

        public ArticlesListResponse(List<ApplicationArticle> articlesList, int totalRows) {
            ArticlesList = articlesList;
            TotalRows = totalRows;
        }
    }
}
