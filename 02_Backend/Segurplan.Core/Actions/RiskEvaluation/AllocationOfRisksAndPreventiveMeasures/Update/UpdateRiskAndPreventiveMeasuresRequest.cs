using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Update {
    public class UpdateRiskAndPreventiveMeasuresRequest : IRequest<IRequestResponse<UpdateRiskAndPreventiveMeasuresResponse>> {

        public UpdateRiskAndPreventiveMeasuresModel RiskAndPreventiveMeasures { get; set; }
        public int UserId { get; set; }

    }



}
