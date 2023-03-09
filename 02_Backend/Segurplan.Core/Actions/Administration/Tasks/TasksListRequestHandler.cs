using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Tasks {
    public class TasksListRequestHandler : IRequestHandler<TasksListRequest, IRequestResponse<TasksListResponse>> {
        protected readonly TasksDam tasksDam;
        public TasksListRequestHandler(TasksDam tasksDam) {
            this.tasksDam = tasksDam;
        }
        public async Task<IRequestResponse<TasksListResponse>> Handle(TasksListRequest request, CancellationToken cancellationToken) {
            return await GetTasksList(request);
        }
        private async Task<IRequestResponse<TasksListResponse>> GetTasksList(TasksListRequest request) {
            var manager = new TasksListManager(tasksDam);
            var tasksList = await manager.GetTasksList(request.TableState, request.TableFilter);

            return RequestResponse.Ok(new TasksListResponse(tasksList, manager.FilteredTasks));
        }
    }
}
