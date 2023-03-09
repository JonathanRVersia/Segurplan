using System.Collections.Generic;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Administration.Seriousness {
    public class SeriousnessListTableState {

        public const string IdFilter = "Id";
        public const string ValueFilter = "Value";

        public SeriousnessListTableState() : this(0, 15, ListOrderMode.Asc, "") {

        }
        public SeriousnessListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
            IndexPage = indexPage;
            PageRows = pageRows;
            OrderModeDesc = oderMode;
            OrderBy = orderBy;
            PageRowList = new List<int>(3) { 15, 25, 50, 100 };
        }
        public int IndexPage { get; set; } 
        public int PageRows { get; set; }
        public List<int> PageRowList { get; private set; }
        public ListOrderMode OrderModeDesc { get; set; }
        public string OrderBy { get; set; }
    }
}
