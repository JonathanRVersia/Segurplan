using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.RiskLevelByProbabilityAndSeriousness {
    public class RiskLevelByProbabilityAndSeriousnessRequest : IRequest<IRequestResponse<RiskLevelByProbabilityAndSeriousnessResponse>> {
    }
}
