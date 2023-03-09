using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Detail {
    public class DetailRiskAndPreventiveMeasuresRequest : IRequest<IRequestResponse<DetailRiskAndPreventiveMeasuresResponse>> {
        public int Id { get; set; }
    }
}
