using System.Collections.Generic;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Plans.PlansData {


    public class TableState {

        public const string OrganizationFilter = "Organization";
        public const string TitleFilter = "PlanTitle";
        public const string ModifiedFilter = "ModifiedDate";
        public const string CustomerFilter = "Customer";
        public const string ActivityFilter = "Activity";
        public const string ProducedByFilter = "ProducedBy";


        public TableState() : this(0, 15, ListOrderMode.Asc, "", true, false) {

        }



        public TableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy, bool firstLoad, bool allPlans) {
            IndexPage = indexPage;
            PageRows = pageRows;
            OrderMode = oderMode;
            OrderBy = orderBy;
            FirstLoad = firstLoad;
            AllPlans = allPlans;

            PageRowList = new List<int>(3) { 15, 25, 50, 100 };
        }

        public int IndexPage { get; set; }
        public int PageRows { get; set; }
        public List<int> PageRowList { get; private set; }
        public ListOrderMode OrderMode { get; set; }
        public string OrderBy { get; set; }
        public bool FirstLoad { get; set; } = true;
        public bool AllPlans { get; set; }
    }
}
