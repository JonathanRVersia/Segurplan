using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks.Save {
    public class SaveTaskRequest : IRequest<IRequestResponse<SaveTaskResponse>> {
        public int UserId { get; set; }
        public ApplicationTask Task { get; set; }
    }
}
