using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class RiskLevelBySeriousnessAndProbability {

        public int Id { get; set; }

        public int SeriousnessId { get; set; }
        public Seriousness Seriousness { get; set; }
        public int ProbabilityId { get; set; }
        public Probability Probability { get; set; }
        public int RiskLevelId { get; set; }
        public RiskLevel RiskLevel { get; set; }

    }
}
