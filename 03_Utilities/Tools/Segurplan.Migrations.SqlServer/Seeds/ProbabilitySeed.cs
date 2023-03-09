using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class ProbabilitySeed : IDataSeed {

        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            List<string> values = new List<string>{
                // "Alto",
                //"Medio",
                // "Bajo"

                "Alta",
                "Moderada",
                "Baja"
            };

            List<Probability> probabilities = new List<Probability>();

            foreach (var value in values) {
                probabilities.Add(new Probability { Value = value });
            }

            await context.Probability.AddRangeAsync(probabilities);
            context.SaveChanges();


        }
    }
}
