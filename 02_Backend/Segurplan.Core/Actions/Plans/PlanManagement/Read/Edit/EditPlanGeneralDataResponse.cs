using System.Collections.Generic;

using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit {
    public class EditPlanGeneralDataResponse : ReadPlanGeneralDataResponseBase {

        public List<PlanTemplate> TemplateList = new List<PlanTemplate>();
        public List<PlanAffiliatedCompany> AffiliatedCompanyList = new List<PlanAffiliatedCompany>();
        public List<PlanDelegation> DelegationList = new List<PlanDelegation>();
        public List<PlanCustomer> CustomerList = new List<PlanCustomer>();
        public List<PlanBusinessAddress> BusAddList = new List<PlanBusinessAddress>();
        public List<PlanGeneralActivity> GenActList = new List<PlanGeneralActivity>();
        public List<UserProfile> ProfileList = new List<UserProfile>();
        public List<ApplicationUser> UserList = new List<ApplicationUser>();
    }
}
