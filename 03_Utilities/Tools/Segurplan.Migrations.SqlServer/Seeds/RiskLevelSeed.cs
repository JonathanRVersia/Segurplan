using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class RiskLevelSeed : IDataSeed {
        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            List<(string, int, string)> values = new List<(string, int, string)>{
               ("INTOLERABLE",1, "danger" ),
               ("IMPORTANTE", 2, "danger"),
               ("MODERADO",  3,"warning" ),
               ("TOLERABLE",  4,"success" ),
               ( "TRIVIAL" ,  5,"success" )
            };

            List<RiskLevel> riskLevels = new List<RiskLevel>();

            foreach (var value in values) {
                riskLevels.Add(new RiskLevel {
                    Level = value.Item1,
                    LevelValue = value.Item2,
                    TrafficLightsColour = value.Item3
                });
            }

            await context.RiskLevel.AddRangeAsync(riskLevels);
            context.SaveChanges();

        }
    }
}
