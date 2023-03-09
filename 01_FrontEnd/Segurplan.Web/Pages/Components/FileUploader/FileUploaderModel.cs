using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Web.Pages.Components.FileUploader {
    public class FileUploaderModel {

        public bool EditEnabled { get; set; }

        public List<PlanFile> Files { get; set; } = new List<PlanFile>();

        public string InputId { get; set; }

        public int PlanId { get; set; }

        public string Destination { get; set; }
    }
}
