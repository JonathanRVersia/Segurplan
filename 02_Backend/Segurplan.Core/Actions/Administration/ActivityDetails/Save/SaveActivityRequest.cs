using MediatR;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.ActivityDetails.Save {
    public class SaveActivityRequest : IRequest<IRequestResponse<SaveActivityResponse>> {
        public int Id { get; set; }

        public int IdActivity { get; set; }

        public int IdSubChapterVersion { get; set; }

        public int SubChapterId { get; set; }

        public int Number { get; set; }

        public string Description { get; set; }

        public string WordDescription { get; set; }

        public string WorkDetails { get; set; }

        public string WorkOrganization { get; set; }

        //public string RiskEvaluation { get; set; }

        public string MachineTool { get; set; }

        public int? RelationsId { get; set; }

        //public string AssociatedDetails { get; set; }

        //public string SupportFacilities { get; set; }
    }
}
