using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.DetailDropdowns {
    public class SubChapterDetailsDropdownsRequest : IRequest<IRequestResponse<SubChapterDetailsDropdownsResponse>> {
    }
}
