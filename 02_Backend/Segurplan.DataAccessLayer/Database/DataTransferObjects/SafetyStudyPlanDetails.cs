

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.Database.DataTransferObjects {
    public class SafetyStudyPlanDetails : AuditableTableBase {

        public int Id { get; set; }

        public int IdPlan { get; set; }

        public string CompanyName { get; set; }

        public string Promoter { get; set; }

        public string Localization { get; set; }
        
        public string Municipality { get; set; }

        public double ExecutionBudget { get; set; }

        public int ExecutionTimeDays { get; set; }

        public int ExecutionTimeMonths { get; set; }

        public double PSSBudget { get; set; }

        public int WorkersNumber { get; set; }


        public string OrganizationalStructure { get; set; }

        public string SituationDescription { get; set; }

        public string ActivityDescription { get; set; }

        public bool AffectedServices { get; set; }

        public string AffectedServicesDescription { get; set; }

        public string AssistanceCenters { get; set; }

        public int IdEmergencyPlanType { get; set; }

        public string EmergencyPlanDescription { get; set; }

        public string SecurityBudget { get; set; }

        public string ExecutionTime { get; set; }

        [ForeignKey("IdPlan")]
        [InverseProperty("SafetyStudyPlanDetails")]
        public SafetyStudyPlan IdPlanNavigation { get; set; }

        [ForeignKey("IdEmergencyPlanType")]
        [InverseProperty("SafetyStudyPlanDetails")]
        public EmergencyPlanType IdEmergencyPlanTypeNavigation { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SafetyStudyPlanDetailsCreatedByNavigation")]
        public User CreatedByNavigation { get; set; }

        [ForeignKey("ModifiedBy")]
        [InverseProperty("SafetyStudyPlanDetailsModifiedByNavigation")]
        public User ModifiedByNavigation { get; set; }
        
    }
}
