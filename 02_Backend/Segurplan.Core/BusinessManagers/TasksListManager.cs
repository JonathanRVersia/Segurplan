using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Administration.Tasks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.Core.BusinessManagers {
    class TasksListManager {
        public const string FilterByName = "Name";
        public bool OrderModeDesc = false;

        public TasksListTableState TableState { get; set; }
        public TasksFilter TableFilter { get; set; }
        public int FilteredTasks { get; internal set; }
        public int IndexPage { get; internal set; }
        public int RemainingRows { get; internal set; }
        public int Takep { get; internal set; }

        private readonly TasksDam tasksDam;

        public TasksListManager(TasksDam tasksDam) {
            this.tasksDam = tasksDam;
        }

        public async Task<List<ApplicationTask>> GetTasksList(TasksListTableState tableState, TasksFilter tableFilter) {
            try {
                TableState = tableState;
                TableFilter = tableFilter;

                var dbResponse = await tasksDam.SelectAll();
                dbResponse = !string.IsNullOrEmpty(TableFilter.Name) ? await tasksDam.ApplyFilters(dbResponse, FilterByName, TableFilter.Name.ToString()) : dbResponse;

                FilteredTasks = dbResponse.Count();
                IndexPage = tableState.IndexPage;
                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
                dbResponse = await tasksDam.OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);

                RemainingRows = FilteredTasks - (tableState.PageRows * IndexPage);
                Takep = tableState.PageRows;
                if (RemainingRows - tableState.PageRows < 0) {
                    Takep = RemainingRows;
                }
                if (RemainingRows == 0) {
                    IndexPage = IndexPage - 1;
                }
                if (dbResponse.Count() > 0) {
                    dbResponse = await tasksDam.GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
                }

                var response = new List<ApplicationTask>();
                foreach (var dbTask in dbResponse) {
                    response.Add(new ApplicationTask {
                        Id = dbTask.Id,
                        Name = dbTask.Name
                    });
                }
                return response;

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
