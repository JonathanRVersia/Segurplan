using System.Collections.Generic;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;

namespace Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList {
    public class RisksAndPreventiveMeasuresListModel {

        public bool IsSearch { get; set; }
        public RiskAndPreventiveMeasuresSearch Search { get; set; } = new RiskAndPreventiveMeasuresSearch();

        public List<RisksAndPreventiveMeasuresTableDataModel> TableValues { get; set; } = new List<RisksAndPreventiveMeasuresTableDataModel>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public string SearchValues { get; set; }
        public string ShortOrder { get; set; }
        public string IndexRoute { get; set; }
        public string DetailsRoute { get; set; }
        public string IndexPage { get; set; }
    }
}
