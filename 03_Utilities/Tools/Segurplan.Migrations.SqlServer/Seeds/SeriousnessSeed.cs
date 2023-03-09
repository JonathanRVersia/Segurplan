using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class SeriousnessSeed : IDataSeed {
        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            List<string> values = new List<string> {
                //"Mayor",
                //"Moderado",
                //"Menor"

                "Muy grave",
                "Grave",
                "Leve"
            };

            List<Seriousness> seriousnesses = new List<Seriousness>();

            foreach (var value in values) {
                seriousnesses.Add(new Seriousness { Value = value });
            }

            await context.Seriousness.AddRangeAsync(seriousnesses);
            context.SaveChanges();
        }
    }
}
