
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.ModalList.Specifications {
    public class PaginatedPreventiveMeasuresSpecification : Specification<PreventiveMeasureListResponse.ListItem> {

        public PaginatedPreventiveMeasuresSpecification(int page, int pageSize) {

            Paginated((page - 1) * pageSize, pageSize);
        }

    }
}
