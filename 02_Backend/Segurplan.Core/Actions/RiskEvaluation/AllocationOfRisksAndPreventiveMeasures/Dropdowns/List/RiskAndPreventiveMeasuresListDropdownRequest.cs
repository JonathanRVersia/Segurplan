using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.List {
    public class RiskAndPreventiveMeasuresListDropdownRequest : IRequest<IRequestResponse<RiskAndPreventiveMeasuresListDropdownResponse>> {
        public int ChapterId { get; set; }
        public int SubChapterId { get; set; }
        public int ActivityId { get; set; }
        public bool Borrador { get; set; }
    }
}
