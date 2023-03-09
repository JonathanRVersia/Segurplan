using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Delete {
    public class DeleteRiskAndPreventiveMeasureRequest : IRequest<IRequestResponse<DeleteRiskAndPreventiveMeasureResponse>> {
        public int RiskAndPreventiveMeasureId;
    }
}
