using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class UpdateRequestHandler : UpdatePlanBase, IRequestHandler<UpdatePlanRequest, IRequestResponse<UpdatePlanResponse>> {

        public UpdateRequestHandler(SegurplanContext context) : base(context) {
        }

        public async Task<IRequestResponse<UpdatePlanResponse>> Handle(UpdatePlanRequest request, CancellationToken cancellationToken) {
            try {
                await SavePlanInformation(request);

                return RequestResponse.Ok(new UpdatePlanResponse());

            } catch (Exception exc) {

                return RequestResponse.Ok(new UpdatePlanResponse { ErrorCode = 99, ErrorDescription = exc.ToString() });
            }
        }
    }
}
