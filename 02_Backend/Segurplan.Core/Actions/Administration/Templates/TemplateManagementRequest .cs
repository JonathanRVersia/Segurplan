using MediatR;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateManagementRequest : IRequest<IRequestResponse<TemplateManagementResponse>> {
        public ApplicationTemplate Template;
        public AdministrationActionType CurrentOperation;
        public int CurrentUserId { get; set; }

        public TemplateManagementRequest(ApplicationTemplate template, AdministrationActionType currentOperation, int currentUserId = -1) {
            Template = template;
            CurrentOperation = currentOperation;
            CurrentUserId = currentUserId;
        }
    }
}
