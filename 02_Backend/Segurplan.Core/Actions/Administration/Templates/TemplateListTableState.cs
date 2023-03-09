using System.Collections.Generic;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateListTableState {

        public const string IdFilter = "Id";
        public const string NameFilter = "Name";
        public const string NotesFilter = "Notes";
        public const string CreatedByFilter = "CreatedBy";
        public const string ModifiedDateFilter = "ModifiedDate";

        public TemplateListTableState() : this(0, 15, ListOrderMode.Asc, "") {}

        public TemplateListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
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
