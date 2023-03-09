using Segurplan.Core.Actions.Plans.PlansData.Activities;

namespace Segurplan.Web.Pages.Components.PlanActivityCustomRow {
    public class PlanActivityCustomRowModel {

        public PlanChapter CustomChapter { get; set; }

        public int RowIndex { get; set; }

        public bool IsEditEnabled { get; set; }
    }
}
