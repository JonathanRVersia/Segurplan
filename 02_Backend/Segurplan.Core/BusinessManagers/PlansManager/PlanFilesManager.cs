using System.Collections.Generic;
using System.Threading.Tasks;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;

namespace Segurplan.Core.BusinessManagers.PlansManager {
    internal class PlanFilesManager {
        private readonly SafetyStudyPlanFileDam dam;

        internal PlanFilesManager(SafetyStudyPlanFileDam planFileDam) {
            dam = planFileDam;
        }

        public async Task<List<PlanFile>> LoadPlanFiles(int planId) {
            List<PlanFile> fileList;

            var dbFiles = await dam.SelectByPlanId(planId);

            if (dbFiles == null) {
                fileList = new List<PlanFile>(0);
            } else {
                fileList = new List<PlanFile>(dbFiles.Count);
                foreach (var file in dbFiles) {
                    fileList.Add(new PlanFile {
                        Id = file.Id,
                        Name = file.FileName,
                        DataLength = file.FileData.Length
                    });
                }
            }

            return fileList;
        }
    }
}
