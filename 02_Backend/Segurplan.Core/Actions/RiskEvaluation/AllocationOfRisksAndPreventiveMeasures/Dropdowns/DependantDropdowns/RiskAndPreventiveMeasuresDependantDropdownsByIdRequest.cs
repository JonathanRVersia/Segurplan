using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.RiskEvaluation.AllocationOfRisksAndPreventiveMeasures.Dropdowns.DependantDropdowns {
    public class RiskAndPreventiveMeasuresDependantDropdownsByIdRequest : IRequest<IRequestResponse<RiskAndPreventiveMeasuresDependantDropdownsByIdResponse>> {
        public int DependantRelationId { get; set; }
        public string Target { get; set; }
        public int ChapterId { get; set; }
        public int SubChapterId { get; set; }
        public int ChapterVersionId { get; set; }
        public int? ActivityId { get; set; }
    }
}
