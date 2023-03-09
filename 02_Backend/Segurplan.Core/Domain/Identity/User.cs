using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Segurplan.Core.Database.Models;

namespace Segurplan.Core.Domain.Identity {
    public class User : IdentityUser<int> {
        public IEnumerable<Template> TemplateModifiedByNavigation { get; set; }
        public IEnumerable<Template> TemplateCreatedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanModifiedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanIdAproverNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanCreatedByNavigation { get; set; }
        public IEnumerable<ProjectType> ProjectTypeModifiedByNavigation { get; set; }
        public IEnumerable<ProjectType> ProjectTypeCreatedByNavigation { get; set; }
        public IEnumerable<PlanType> PlanTypeModifiedByNavigation { get; set; }
        public IEnumerable<PlanType> PlanTypeCreatedByNavigation { get; set; }
        public IEnumerable<Delegation> DelegationModifiedByNavigation { get; set; }
        public IEnumerable<Delegation> DelegationCreatedByNavigation { get; set; }
        public IEnumerable<Center> CenterModifiedByNavigation { get; set; }
        public IEnumerable<Center> CenterCreatedByNavigation { get; set; }
        public IEnumerable<BusinessAddress> BusinessAddressModifiedByNavigation { get; set; }
        public IEnumerable<BusinessAddress> BusinessAddressCreatedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlanDetails> SafetyStudyPlanDetailsModifiedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlanDetails> SafetyStudyPlanDetailsCreatedByNavigation { get; set; }
    }
}
