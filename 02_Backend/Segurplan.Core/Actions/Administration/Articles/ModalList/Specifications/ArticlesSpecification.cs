using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Articles.ModalList.Specifications {
    public class ArticlesSpecification : Specification<ArticlesListResponse.ListItem> {
        public void ByContainsWord(string word) => Criteria(pm => pm.Name.Contains(word));
    }
}
