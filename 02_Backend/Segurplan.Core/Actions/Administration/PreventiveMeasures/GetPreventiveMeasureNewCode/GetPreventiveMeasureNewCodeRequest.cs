using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures.GetPreventiveMeasureNewCode {
    public class GetPreventiveMeasureNewCodeRequest : IRequest<IRequestResponse<GetPreventiveMeasureNewCodeResponse>> {
    }
}
