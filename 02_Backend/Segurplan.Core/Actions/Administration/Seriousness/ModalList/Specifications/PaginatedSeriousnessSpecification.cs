
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.Seriousness.ModalList.Specifications {
    public class PaginatedSeriousnessSpecification : Specification<SeriousnessListResponse.ListItem> {
        public PaginatedSeriousnessSpecification(int page, int pageSize) {
            Paginated((page - 1) * pageSize, pageSize);
        }
    }
}
