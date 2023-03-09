using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class Z_RiskLevelBySeriousnessAndProbabilitiesSeed : IDataSeed {

        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            List<RiskLevelBySeriousnessAndProbability> riskLevelBySeriousnessAndProbabilities = new List<RiskLevelBySeriousnessAndProbability> {
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 5, SeriousnessId = 3, ProbabilityId = 3 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 4, SeriousnessId = 3, ProbabilityId = 2 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 4, SeriousnessId = 2, ProbabilityId = 3 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 3, SeriousnessId = 3, ProbabilityId = 1 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 3, SeriousnessId = 2, ProbabilityId = 2 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 3, SeriousnessId = 1, ProbabilityId = 3 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 2, SeriousnessId = 2, ProbabilityId = 1 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 2, SeriousnessId = 1, ProbabilityId = 2 },
                new RiskLevelBySeriousnessAndProbability { RiskLevelId = 1, SeriousnessId = 1, ProbabilityId = 1 }
            };


            context.RiskLevelBySeriousnessAndProbabilities.AddRange(riskLevelBySeriousnessAndProbabilities);
            await context.SaveChangesAsync();
        }
    }
}
