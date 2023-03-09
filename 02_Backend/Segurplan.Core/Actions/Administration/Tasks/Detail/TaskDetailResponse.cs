using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Tasks.Detail {
    public class TaskDetailResponse {
        public ApplicationTask Task { get; set; }

        public TaskDetailResponse(ApplicationTask task) {
            Task = task;
        }
    }
}
