using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.Migrations.SqlServer.Helpers;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class RisksSeed : IDataSeed {
        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            var data = RisksData.Risks;

            List<Risk> risks = new List<Risk>();

            foreach (var risk in data) {
                risks.Add(new Risk {
                    Code = risk.Key,
                    Name = risk.Value
                });
            }

            await context.Risk.AddRangeAsync(risks);
            context.SaveChanges();

        }
    }
}
