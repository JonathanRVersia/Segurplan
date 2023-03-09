using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.MatrixDataList {
    public class MatrixDataListRequest : IRequest<IRequestResponse<MatrixDataListResponse>> {
    }
}
