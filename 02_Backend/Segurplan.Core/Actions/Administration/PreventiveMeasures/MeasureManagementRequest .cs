using MediatR;
using Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.PreventiveMeasures {
    public class MeasureManagementRequest : IRequest<IRequestResponse<MeasureManagementResponse>> {

        public ApplicationPreventiveMeasure Measure;
        public AdministrationActionType CurrentOperation;
        public int CurrentUserId { get; set; }


        public MeasureManagementRequest(ApplicationPreventiveMeasure measure, AdministrationActionType currentOperation, int currentUserId = -1) {
            Measure = measure;
            CurrentOperation = currentOperation;
            CurrentUserId = currentUserId;
        }


    }
}
