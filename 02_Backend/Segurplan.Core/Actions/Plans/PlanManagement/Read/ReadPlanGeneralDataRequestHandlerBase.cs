using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Enums;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read {
    public class ReadPlanGeneralDataRequestHandlerBase {

        protected SegurplanContext dbContext;

        public ReadPlanGeneralDataRequestHandlerBase(SegurplanContext context) {
            dbContext = context;
        }

        protected async Task<SafetyPlan> GetPlanInformation(int planId) {

            SafetyPlan planData = null;
            //TODO: Remove includes for better performance, use automapper or select 
            var dbPlan = dbContext.SafetyStudyPlan
                    .Include(p => p.IdDelegationNavigation)
                    .Include(p => p.IdBusinessAddressNavigation)
                    .Include(p => p.IdAffiliatedCompanyNavigation)
                    .Include(p => p.IdCustomerNavigation)
                    .Include(p => p.IdGeneralActivityNavigation)
                    .Include(p => p.IdTemplateNavigation)
                    .Include(p => p.CreatedByNavigation)
                    .Include(p => p.IdCreatorProfileNavigation)
                    .Include(p => p.IdReviewerNavigation)
                    .Include(p => p.ModifiedByNavigation)
                    .FirstOrDefault(p => p.Id == planId);

            if (dbPlan != null) {

                var details = dbContext.SafetyStudyPlanDetails.FirstOrDefault(det => det.IdPlan == dbPlan.Id);

                planData = PlanManagementCommon.ConvertToSafetyPlan(dbPlan, details);

                var dbPlanAnagrams = dbContext.SafetyStudyPlanFile.Where(pf => pf.IdSafetyStudyPlan == planId).ToList();

                if (dbPlanAnagrams.Any()) {
                    foreach (var dbPlanAnagram in dbPlanAnagrams) {
                        planData.GeneralData.Anagrams.Add(new PlanFile {
                            Id = dbPlanAnagram.Id,
                            Name = dbPlanAnagram.FileName,
                            DataLength = dbPlanAnagram.FileSize
                        });
                    }

                } else {
                    var defaultAnagram = dbContext.DefaultSafetyStudyPlanFile.First();
                    planData.GeneralData.Anagrams.Add(new PlanFile {
                        Id = defaultAnagram.Id,
                        Name = defaultAnagram.FileName,
                        DataLength = defaultAnagram.FileSize,
                        DefaultFile = true
                    });
                }
            }

            return planData;
        }

        protected async Task<List<PlanDelegation>> GetDelegationList() =>
            dbContext.Delegation.Select(delegation => new PlanDelegation {
                Id = delegation.Id,
                Name = $"{delegation.Code} {delegation.Name}"
            }).ToList();

        protected async Task<List<PlanAffiliatedCompany>> GetAffiliatedCompanyList() =>
            dbContext.AffiliatedCompany.Select(affC => new PlanAffiliatedCompany {
                Id = affC.Id,
                Name = affC.Name
            }).ToList();

        protected async Task<List<PlanCustomer>> GetCustomerList() =>
            dbContext.Customer.Select(cust => new PlanCustomer {
                Id = cust.Id,
                Name = cust.Name
            }).ToList();

        protected async Task<List<PlanTemplate>> GetTemplateList() =>
            dbContext.Template
                .Where(x => x.TemplateType == TemplateType.PlanManagement)
                .Select(dbTpl => new PlanTemplate {
                    Id = dbTpl.Id,
                    Name = dbTpl.Name
                }).ToList();

        protected async Task<List<PlanBusinessAddress>> GetBusinessAddressList() =>
            dbContext.BusinessAddress.Select(dbAddress => new PlanBusinessAddress {
                Id = dbAddress.Id,
                Name = dbAddress.Name
            }).ToList();

        protected async Task<List<PlanGeneralActivity>> GetGeneralActivityList() =>
            dbContext.GeneralActivity.Select(genAct => new PlanGeneralActivity {
                Id = genAct.Id,
                Name = genAct.Name
            }).ToList();

        protected async Task<List<UserProfile>> GetProfileList() =>
            dbContext.Profile.Select(dbProfile => new UserProfile {
                Id = dbProfile.Id,
                Name = dbProfile.Name
            }).ToList();

        protected async Task<List<ApplicationUser>> GetUserList() =>
            dbContext.Users.Join(dbContext.UserRoles.Where(x=>x.RoleId==1),z=>z.Id,x=>x.UserId, (z, x) => new ApplicationUser {
                Id = z.Id,
                CompleteName = z.CompleteName
            }).ToList();
    }
}
