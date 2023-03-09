using System.Collections.Generic;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class RisksAndPreventiveMeasures : AuditableTableBase {
        public int Id { get; set; }

        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        public int SubChapterId { get; set; }
        public SubChapter SubChapter { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }

        public int RiskId { get; set; }
        public Risk Risk { get; set; }

        public List<RiskAndPreventiveMeasuresMeasures> PreventiveMeasures { get; set; }

        public int ProbabilityId { get; set; }
        public Probability Probability { get; set; }

        public int SeriousnessId { get; set; }
        public Seriousness Seriousness { get; set; }

        public int RiskLevelId { get; set; }
        public RiskLevel RiskLevel { get; set; }

        public int RiskOrder { get; set; }

    }
}
