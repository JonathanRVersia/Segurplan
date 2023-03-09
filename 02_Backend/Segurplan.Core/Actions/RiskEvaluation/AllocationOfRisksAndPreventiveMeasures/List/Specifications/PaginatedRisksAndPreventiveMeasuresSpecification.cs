using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List.Specifications {
    public class PaginatedRisksAndPreventiveMeasuresSpecification : Specification<ListRisksAndPreventiveMeasuresResponse.ListItem> {
        public PaginatedRisksAndPreventiveMeasuresSpecification(int page, int pageSize) {

            Paginated((page - 1) * pageSize, pageSize);
        }
    }
}
