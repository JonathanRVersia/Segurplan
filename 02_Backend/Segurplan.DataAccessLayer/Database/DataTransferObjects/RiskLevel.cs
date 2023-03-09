using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class RiskLevel {

        public int Id { get; set; }

        public string Level { get; set; }

        public string TrafficLightsColour { get; set; }

        public int LevelValue { get; set; }

        public List<RiskLevelBySeriousnessAndProbability> RiskLevelBySeriousnessAndProbabilities { get; set; }
    }
}
