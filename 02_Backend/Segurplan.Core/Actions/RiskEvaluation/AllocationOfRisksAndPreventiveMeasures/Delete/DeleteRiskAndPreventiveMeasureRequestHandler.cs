using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Delete {
    public class DeleteRiskAndPreventiveMeasureRequestHandler : IRequestHandler<DeleteRiskAndPreventiveMeasureRequest, IRequestResponse<DeleteRiskAndPreventiveMeasureResponse>> {

        private readonly SegurplanContext context;

        public DeleteRiskAndPreventiveMeasureRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<DeleteRiskAndPreventiveMeasureResponse>> Handle(DeleteRiskAndPreventiveMeasureRequest request, CancellationToken cancellationToken) {

            ClearPreventiveMeasures(request);

            context.RisksAndPreventiveMeasures.Remove(new RisksAndPreventiveMeasures { Id = request.RiskAndPreventiveMeasureId });
            int deleted = await context.SaveChangesAsync();

            if (deleted < 1)
                return RequestResponse.NotFound<DeleteRiskAndPreventiveMeasureResponse>();

            return RequestResponse.Ok(new DeleteRiskAndPreventiveMeasureResponse());
        }

        private void ClearPreventiveMeasures(DeleteRiskAndPreventiveMeasureRequest request) {

            var oldMeasures = context.RiskAndPreventiveMeasuresMeasures.Where(x => x.RisksAndPreventiveMeasuresId == request.RiskAndPreventiveMeasureId).ToList();
            context.RemoveRange(oldMeasures);

            context.SaveChanges();

            foreach (var oldMeasure in oldMeasures) {
                context.Entry(oldMeasure).State = EntityState.Detached;
            }

        }
    }
}
