using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.FrameworkExtensions.Query;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.List {
    public class ListRisksAndPreventiveMeasuresRequest : IRequest<IRequestResponse<ListRisksAndPreventiveMeasuresResponse>> {
        public IEnumerable<ISpecification<ListRisksAndPreventiveMeasuresResponse.ListItem>> Specifications { get; set; }
    }
}
