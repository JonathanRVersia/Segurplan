using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Segurplan.Core.BusinessObjects {

    public class SafetyPlanAdditionalData {

        public enum EmergencyPlanType {
            Short = 1,
            Medium = 2,
            Long = 3
        }

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

        public string ActivityDescription { get; set; } = "";

        public bool AffectedServices { get; set; }

        public string AffectedServicesDescription { get; set; }

        public string AssistanceCenters { get; set; }

        public int IdEmergencyPlanType { get; set; } = 1;

        public string EmergencyPlanDescription { get; set; } = "";

        public string SecurityBudget { get; set; }

        public string ExecutionTime { get; set; }
    }
}
