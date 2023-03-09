using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class Probability {

        public int Id { get; set; }

        public string Value { get; set; }

        public List<RiskLevelBySeriousnessAndProbability> RiskLevelBySeriousnessAndProbabilities { get; set; }
    }
}
