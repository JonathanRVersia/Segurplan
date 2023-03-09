using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Articles.FamilyDataList {
    public class FamilyDataListRequest : IRequest<IRequestResponse<FamilyDataListResponse>> {
    }
}
