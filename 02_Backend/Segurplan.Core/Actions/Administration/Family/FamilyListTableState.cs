using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Administration.Family {
    public class FamilyListTableState {
        public const string IdFilter = "Id";
        public const string FamilyFilter = "Family";

        public FamilyListTableState() : this(0, 15, ListOrderMode.Asc, "") {
        }
        public FamilyListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
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
