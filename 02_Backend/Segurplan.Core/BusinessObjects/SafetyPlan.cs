using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Core.BusinessObjects {
    public class SafetyPlan {

        public int Id { get; set; }

        public int IdBudget { get; set; }

        public string DuplicatedPlanTitle { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }        

        public SafetyPlanGeneralData GeneralData { get; set; } = new SafetyPlanGeneralData();

        public SafetyPlanAdditionalData AdditionalData { get; set; } = new SafetyPlanAdditionalData();

        public SafetyPlanActivities ActivityLists { get; set; } = new SafetyPlanActivities();

        public List<SafetyPlanPlane> SelectedPlanes { get; set; } = new List<SafetyPlanPlane>();

        public ApplicationBudget Budget { get; set; } = new ApplicationBudget();
    }
}
