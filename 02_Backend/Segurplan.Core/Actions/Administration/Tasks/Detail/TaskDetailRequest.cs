using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Detail {
    public class TaskDetailRequest : IRequest<IRequestResponse<TaskDetailResponse>> {
        public int Id { get; set; }
    }
}
