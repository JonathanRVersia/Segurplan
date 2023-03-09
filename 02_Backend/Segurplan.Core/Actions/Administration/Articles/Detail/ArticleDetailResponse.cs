using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Articles.Detail {
    public class ArticleDetailResponse {
        public ApplicationArticle Article { get; set; }
        public List<ApplicationArticleFamily> Family { get; set; }
        public ArticleDetailResponse(ApplicationArticle article, List<ApplicationArticleFamily> family) {
            Article = article;
            Family = family;
        }
    }
}
