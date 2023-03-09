using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class EmergencyPlanType {     

        public EmergencyPlanType() {
            SafetyStudyPlanDetails = new HashSet<SafetyStudyPlanDetails>();
        }

        public int Id { get; set; }

        public string Caption { get; set; }

        [InverseProperty("IdEmergencyPlanTypeNavigation")]
        public ICollection<SafetyStudyPlanDetails> SafetyStudyPlanDetails { get; set; }
    }
}
