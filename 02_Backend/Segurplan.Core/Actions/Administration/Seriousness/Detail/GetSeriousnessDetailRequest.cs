using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Seriousness.Detail {
    public class GetSeriousnessDetailRequest : IRequest<IRequestResponse<GetSeriousnessDetailResponse>> {
        public int Id { get; set; }
    }
}
