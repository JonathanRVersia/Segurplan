using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OldDBDataMigrator.Utils;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.DataAccessLayer.Enums;

namespace OldDBDataMigrator.DataMigration.Templates {
    public class UpdateAnagram {

        private readonly SegurplanContext segurplanContext;
        private readonly SeedUtils utils;

        public UpdateAnagram(SegurplanContext segurplanContext, SeedUtils utils) {
            this.segurplanContext = segurplanContext;
            this.utils = utils;
        }

        public async Task Initialize() {

            var dbFile = await segurplanContext.DefaultSafetyStudyPlanFile.FirstOrDefaultAsync();

            var basePath = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            basePath = basePath.Replace("OldDBDataMigrator", "03_Utilities");

            var localFile = File.ReadAllBytes(Path.Combine(basePath, "Tools\\Segurplan.Migrations.SqlServer\\Helpers\\DefaultSafetyStudyPlanFileData\\LogoElecnor.jpg"));

            DefaultSafetyStudyPlanFile localFileData = new DefaultSafetyStudyPlanFile {
                FileName = "LogoElecnor.jpg",
                FileData = localFile,
                IdPlanFileType = 1,
                FileSize = localFile.Length
            };

            dbFile.FileData = localFileData.FileData;
            dbFile.FileSize = localFileData.FileSize;
            segurplanContext.DefaultSafetyStudyPlanFile.Update(dbFile);

            var changes = await segurplanContext.SaveChangesAsync();

            if (changes > 0)
                utils.PrintSuccessMessage($"Anragama actualizado con éxito, {dbFile} actualizado");
        }
    }
}
