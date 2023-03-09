using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.View {
    public class ViewPlanBudgetRequest : IRequest<IRequestResponse<ViewPlanBudgetResponse>> {

        public bool OnlySelected;

        public int PlanId { get; set; }
        public int BudgetId { get; set; }
        public int NumberWorkers { get; set; }
        public int DurationWork { get; set; }
        public int ApplicablePercentage { get; set; }
    }
}
