using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Tasks {
    public class TasksListResponse {
        public List<ApplicationTask> TasksList { get; set; } = new List<ApplicationTask>();
        public int TotalRows { get; set; }
        public TasksListResponse(List<ApplicationTask> tasksList, int totalRows) {
            TasksList = tasksList;
            TotalRows = totalRows;
        }
    }
}
