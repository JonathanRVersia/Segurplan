using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks {
    public class TasksListRequest : IRequest<IRequestResponse<TasksListResponse>> {
        public TasksListTableState TableState { get; set; }
        public TasksFilter TableFilter { get; set; }
        public TasksListRequest(TasksListTableState tableState, TasksFilter tableFilter) {
            TableState = tableState;
            TableFilter = tableFilter;
        }
    }
}
