using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.Detail {
    public class RiskAndPreventiveMeasuresDetailDropdownRequest : IRequest<IRequestResponse<RiskAndPreventiveMeasuresDetailDropdownResponse>> {
        public bool IsEdit { get; set; }
        public bool Vigente { get; set; }
    }
}
