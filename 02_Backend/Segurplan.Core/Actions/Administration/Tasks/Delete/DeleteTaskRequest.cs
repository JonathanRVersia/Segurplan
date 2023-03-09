using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Delete {
    public class DeleteTaskRequest: IRequest<IRequestResponse<DeleteTaskResponse>> {
        public int Id { get; set; }
    }
}
