using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.GetPreventiveMeasureNewCode {
    public class GetPreventiveMeasureNewCodeRequestHandler : IRequestHandler<GetPreventiveMeasureNewCodeRequest, IRequestResponse<GetPreventiveMeasureNewCodeResponse>> {

        private readonly SegurplanContext context;

        public GetPreventiveMeasureNewCodeRequestHandler(SegurplanContext context) {
            this.context = context;
        }

        public async Task<IRequestResponse<GetPreventiveMeasureNewCodeResponse>> Handle(GetPreventiveMeasureNewCodeRequest request, CancellationToken cancellationToken) {

            int code = 1;

            if (await context.PreventiveMeasure.AnyAsync())
                code = await context.PreventiveMeasure.MaxAsync(x => x.Code) + 1;

            return RequestResponse.Ok(new GetPreventiveMeasureNewCodeResponse(code));
        }
    }
}
