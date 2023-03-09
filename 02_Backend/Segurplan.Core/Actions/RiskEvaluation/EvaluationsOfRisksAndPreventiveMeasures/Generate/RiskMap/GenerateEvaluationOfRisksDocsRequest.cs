using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.EvaluationsOfRisksAndPreventiveMeasures.Generate.RiskMap {
    public class GenerateEvaluationOfRisksDocsRequest : IRequest<IRequestResponse<GenerateEvaluationOfRisksDocsRequestResponse>> {
        public string TargetTemplate { get; set; }
        public List<ChaptSubChaptActFilterData> FilterData { get; set; }
        public string Title { get; set; }
    }
}
