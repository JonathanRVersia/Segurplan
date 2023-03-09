using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.DataAccessLayer.Database.Identity {
 
    public class User : IdentityUser<int> {
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CompleteName { get; set; }
        public IEnumerable<BudgetDetail> BudgetDetailCreatedByNavigation { get; set; }
        public IEnumerable<BudgetDetail> BudgetDetailModifiedByNavigation { get; set; }
        public IEnumerable<ArticleTaskDetail> ArticleTaskDetailCreatedByNavigation { get; set; }
        public IEnumerable<ArticleTaskDetail> ArticleTaskDetailModifiedByNavigation { get; set; }
        public IEnumerable<Tasks> TaskCreatedByNavigation { get; set; }
        public IEnumerable<Tasks> TaskModifiedByNavigation { get; set; }
        public IEnumerable<ArticleFamily> ArticleFamilyCreatedByNavigation { get; set; }
        public IEnumerable<ArticleFamily> ArticleFamilyModifiedByNavigation { get; set; }
        public IEnumerable<Article> ArticleCreatedByNavigation { get; set; }
        public IEnumerable<Article> ArticleModifiedByNavigation { get; set; }
        public IEnumerable<Budget> BudgetCreatedByNavigation { get; set; }
        public IEnumerable<Budget> BudgetModifiedByNavigation { get; set; }

        public IEnumerable<Template> TemplateModifiedByNavigation { get; set; }
        public IEnumerable<Template> TemplateCreatedByNavigation { get; set; }

        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanModifiedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanIdApproverNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanIdReviewerNavigation { get; set; }
        public IEnumerable<SafetyStudyPlan> SafetyStudyPlanCreatedByNavigation { get; set; }

        public IEnumerable<PlanType> PlanTypeModifiedByNavigation { get; set; }
        public IEnumerable<PlanType> PlanTypeCreatedByNavigation { get; set; }

        public IEnumerable<Delegation> DelegationModifiedByNavigation { get; set; }
        public IEnumerable<Delegation> DelegationCreatedByNavigation { get; set; }

        public IEnumerable<BusinessAddress> BusinessAddressModifiedByNavigation { get; set; }
        public IEnumerable<BusinessAddress> BusinessAddressCreatedByNavigation { get; set; }

        public IEnumerable<SafetyStudyPlanDetails> SafetyStudyPlanDetailsModifiedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlanDetails> SafetyStudyPlanDetailsCreatedByNavigation { get; set; }

        public IEnumerable<PlanReview> PlanReviewCreatedByNavigation { get; set; }
        public IEnumerable<PlanReview> PlanReviewIdReviserNavigation { get; set; }
        public IEnumerable<PlanReview> PlanReviewModifiedByNavigation { get; set; }

        public IEnumerable<Activity> ActivityCreatedByNavigation { get; set; }
        public IEnumerable<Activity> ActivityModifiedByNavigation { get; set; }

        public IEnumerable<ActivityVersion> ActivityVersionIdApproverNavigation { get; set; }
        public IEnumerable<ActivityVersion> ActivityVersionCreatedByNavigation { get; set; }
        public IEnumerable<ActivityVersion> ActivityVersionModifiedByNavigation { get; set; }
        public IEnumerable<ActivityVersion> ActivityVersionIdReviewerNavigation { get; set; }

        public IEnumerable<Chapter> ChapterCreatedByNavigation { get; set; }
        public IEnumerable<Chapter> ChapterModifiedByNavigation { get; set; }

        public IEnumerable<ChapterVersion> ChapterVersionIdApproverNavigation { get; set; }
        public IEnumerable<ChapterVersion> ChapterVersionIdReviewerNavigation { get; set; }
        public IEnumerable<ChapterVersion> ChapterVersionCreatedByNavigation { get; set; }
        public IEnumerable<ChapterVersion> ChapterVersionModifiedByNavigation { get; set; }
        public IEnumerable<UserChapterVersion> ChapterVersionProducedByNavigation { get; set; }

        public IEnumerable<SubChapter> SubChapterCreatedByNavigation { get; set; }
        public IEnumerable<SubChapter> SubChapterModifiedByNavigation { get; set; }

        public IEnumerable<SubChapterVersion> SubChapterVersionIdApproverNavigation { get; set; }
        public IEnumerable<SubChapterVersion> SubChapterVersionCreatedByNavigation { get; set; }
        public IEnumerable<SubChapterVersion> SubChapterVersionModifiedByNavigation { get; set; }
        public IEnumerable<SubChapterVersion> SubChapterVersionIdReviewerNavigation { get; set; }

        public IEnumerable<AffiliatedCompany> AffiliatedCompanyCreatedByNavigation { get; set; }
        public IEnumerable<AffiliatedCompany> AffiliatedCompanyModifiedByNavigation { get; set; }

        public IEnumerable<Customer> CustomerCreatedByNavigation { get; set; }
        public IEnumerable<Customer> CustomerModifiedByNavigation { get; set; }

        public IEnumerable<SafetyStudyPlanFile> SafetyStudyPlanFileCreatedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlanFile> SafetyStudyPlanFileModifiedByNavigation { get; set; }

        public IEnumerable<GeneralActivity> GeneralActivityCreatedByNavigation { get; set; }
        public IEnumerable<GeneralActivity> GeneralActivityModifiedByNavigation { get; set; }

        public IEnumerable<PreventiveMeasure> PreventiveMeasureCreatedByNavigation { get; set; }
        public IEnumerable<PreventiveMeasure> PreventiveMeasureModifiedByNavigation { get; set; }

        public IEnumerable<Plane> PlaneCreatedByNavigation { get; set; }
        public IEnumerable<Plane> PlaneModifiedByNavigation { get; set; }

        public IEnumerable<PlaneFamily> PlanFamilyCreatedByNavigation { get; set; }
        public IEnumerable<PlaneFamily> PlanFamilyModifiedByNavigation { get; set; }

        public IEnumerable<SafetyStudyPlanPlane> SafetyStudyPlanPlaneCreatedByNavigation { get; set; }
        public IEnumerable<SafetyStudyPlanPlane> SafetyStudyPlanPlaneModifiedByNavigation { get; set; }
    }
}
