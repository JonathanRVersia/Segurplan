using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanGeneralDataRequestHandler : ReadPlanGeneralDataRequestHandlerBase, IRequestHandler<ViewPlanGeneralDataRequest, IRequestResponse<ReadPlanGeneralDataResponseBase>> {

        public ViewPlanGeneralDataRequestHandler(SegurplanContext context) : base(context) {
        }

        public async Task<IRequestResponse<ReadPlanGeneralDataResponseBase>> Handle(ViewPlanGeneralDataRequest request, CancellationToken cancellationToken) {

            var response = await GetPlanInformation(request.PlanId);

            return response == null ? RequestResponse.Error<ReadPlanGeneralDataResponseBase>() : RequestResponse.Ok(new ReadPlanGeneralDataResponseBase { PlanInformation = response });

        }
    }
}
