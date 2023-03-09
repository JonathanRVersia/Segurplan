using System.Collections.Generic;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Files.Download {
    public class DownloadAnagramResponse {
        public List<SafetyStudyPlanFile> Files { get; set; }

    }
}
