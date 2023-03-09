using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Articles.ModalList.Specifications {
    public class PaginationArticleSpecification : Specification<ArticlesListResponse.ListItem> {
        public PaginationArticleSpecification(int page, int pageSize) {
            Paginated((page - 1) * pageSize, pageSize);
        }
    }
}
