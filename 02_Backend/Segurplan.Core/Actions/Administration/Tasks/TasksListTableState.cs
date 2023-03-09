using System.Collections.Generic;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Administration.Tasks {
    public class TasksListTableState {

        #region CONSTANTS

        public const string IdSort = "Id";
        public const string NameSort = "Name";
        
        #endregion

        #region PROPERTIES
        public int IndexPage { get; set; }
        public int PageRows { get; set; }
        public List<int> PageRowList { get; private set; }
        public ListOrderMode OrderModeDesc { get; set; }
        public string OrderBy { get; set; } 
        #endregion

        public TasksListTableState() : this(0, 15, ListOrderMode.Asc, "") {

        }
        public TasksListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
            IndexPage = indexPage;
            PageRows = pageRows;
            OrderModeDesc = oderMode;
            OrderBy = orderBy;
            PageRowList = new List<int>(3) { 15, 25, 50, 100 };
        }
    }
}
