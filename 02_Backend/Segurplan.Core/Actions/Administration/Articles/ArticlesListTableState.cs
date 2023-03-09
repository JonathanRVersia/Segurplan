using System.Collections.Generic;
using Segurplan.Core.Helpers;

namespace Segurplan.Core.Actions.Administration.Articles {
    public class ArticlesListTableState {
        public const string IdSort = "Id";
        public const string NameSort = "Name";
        public const string FamilySort = "Family";
        public const string PercentageSort = "Percentage";
        public const string TimeOfWorkSort = "TimeOfWork";
        public const string AmortizationTimeSort = "AmortizationTime";
        public const string MinimumUnitSort = "MinimumUnit";
        public const string PriceSort = "Price";

        public ArticlesListTableState() : this(0, 15, ListOrderMode.Asc, "") {

        }
        public ArticlesListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
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
