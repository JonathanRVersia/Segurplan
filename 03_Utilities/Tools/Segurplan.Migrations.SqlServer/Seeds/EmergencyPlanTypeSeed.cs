using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class EmergencyPlanTypeSeed : IDataSeed {

        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            List<string> values = new List<string> {
                "Corto",
                "Mediano",
                "Grande"
            };

            List<EmergencyPlanType> EmergencyList = new List<EmergencyPlanType>();

            foreach (var value in values) {
                EmergencyList.Add(new EmergencyPlanType { Caption = value });
            }

            await context.EmergencyPlanType.AddRangeAsync(EmergencyList);
            context.SaveChanges();

        }
    }
}
