using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Migrations.SqlServer.Seeds {
    public class DefaultSafetyStudyPlanFileSeed : IDataSeed {
        public async Task Seed(SegurplanContext context, CancellationToken cancellationToken = default) {

            var basePath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            var template = File.ReadAllBytes(Path.Combine(basePath, "Helpers\\DefaultSafetyStudyPlanFileData\\LogoElecnor.jpg"));

            DefaultSafetyStudyPlanFile defaultData = new DefaultSafetyStudyPlanFile {
                FileName = "LogoElecnor.jpg",
                FileData = template,
                IdPlanFileType = 1,
                FileSize = template.Length

            };

            await context.DefaultSafetyStudyPlanFile.AddAsync(defaultData);
            context.SaveChanges();
        }
    }
}
