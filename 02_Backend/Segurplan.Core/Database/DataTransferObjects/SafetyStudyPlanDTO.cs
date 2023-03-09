using System;
using System.Collections.Generic;
using System.Text;
using Segurplan.Core.Database.DataTransferObjects.Enum;

namespace Segurplan.Core.Database.DataTransferObjects {
    public class SafetyStudyPlanDTO : SegurplanDTOBase {
        public CenterDTO Center { get; set; }
        public TypeDTO Type { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public TypeDTO ProjectType { get; set; }
        public string CustomerName { get; set; }
        public string Activity { get; set; }
        public string ProducedBy { get; set; }
        public UserDTO Aprover { get; set; }
        public TemplateDTO Template { get; set; }
        public PlanType? PlanType { get; set; }
    }
}
