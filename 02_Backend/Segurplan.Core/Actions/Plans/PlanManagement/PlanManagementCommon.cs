using System;
using System.Collections.Generic;
using System.Linq;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;

namespace Segurplan.Core.Actions.Plans.PlanManagement {
    public class PlanManagementCommon {

        public static SafetyPlan ConvertToSafetyPlan(SafetyStudyPlan dbPlan, SafetyStudyPlanDetails details) {

            var plan = new SafetyPlan {
                Id = dbPlan.Id,
                IdBudget = dbPlan.IdBudget,
                GeneralData = new SafetyPlanGeneralData {
                    IdBusinessAddress = dbPlan.IdBusinessAddress,
                    BusinessAddress = dbPlan.IdBusinessAddressNavigation.Name,
                    IsEvaluation = dbPlan.IdPlanType == (int)BusinessPlanType.Evaluacion,
                    AffiliatedId = dbPlan.IdAffiliatedCompany,
                    AffiliatedName = dbPlan.IdAffiliatedCompanyNavigation != null
                        ? dbPlan.IdAffiliatedCompanyNavigation.Name
                        : string.Empty,
                    IdTemplate = dbPlan.IdTemplate,
                    TemplateName = dbPlan.IdTemplateNavigation != null
                        ? dbPlan.IdTemplateNavigation.Name
                        : string.Empty,
                    CenterName = "The center name",
                    IdDelegation = dbPlan.IdDelegation,
                    DelegationName = dbPlan.IdDelegationNavigation.Name,
                    IdGeneralActivity = dbPlan.IdGeneralActivity,
                    GeneralActivityName = dbPlan.IdGeneralActivityNavigation.Name,
                    IdCustomer = dbPlan.IdCustomer,
                    CustomerName = dbPlan.IdCustomerNavigation.Name,
                    CustomerDescription = dbPlan.PlanCustomerDescription,
                    PlanTitle = dbPlan.ProjectName,
                    CreatorName = !string.IsNullOrEmpty(dbPlan.CreatorName)
                        ? dbPlan.CreatorName
                        : dbPlan.CreatedByNavigation.CompleteName,
                    CreatorCategoryId = dbPlan.IdCreatorProfile,
                    CreatorCategoryName = dbPlan.IdCreatorProfileNavigation.Name,
                    ModifiedByName = dbPlan.ModifiedByNavigation != null
                        ? dbPlan.ModifiedByNavigation.CompleteName
                        : string.Empty,
                    ApproverName = dbPlan.ApproverName,
                    IdReviewer = dbPlan.IdReviewer,
                    ReviewerName = dbPlan.IdReviewerNavigation.CompleteName,
                },
                CreatedBy = dbPlan.CreatedBy,
                CreationDate = dbPlan.CreateDate,
                ModifiedBy = dbPlan.ModifiedBy > 0
                    ? dbPlan.ModifiedBy
                    : dbPlan.CreatedBy,
                ModifiedDate = dbPlan.UpdateDate != null
                    ? dbPlan.UpdateDate
                    : dbPlan.CreateDate,
                AdditionalData = details != null
                    ? GetPlanAdditionalData(details)
                    : new SafetyPlanAdditionalData()
            };

            //var dbPlanFile = dbPlan.SafetyStudyPlanFile.Where(pf => pf.IdPlanFileType == (int)AppPlanFileType.Anagram).FirstOrDefault();
            var dbPlanFiles = dbPlan.SafetyStudyPlanFile.Where(pf => pf.IdPlanFileType == (int)AppPlanFileType.Anagram).ToList();

            foreach (var dbPlanFile in dbPlanFiles) {
                plan.GeneralData.Anagrams.Add(new PlanFile {
                    Id = dbPlanFile.Id,
                    Data = dbPlanFile.FileData,
                    Name = dbPlanFile.FileName,
                    DataLength = dbPlanFile.FileSize
                });
            }

            //plan.GeneralData.Anagram = dbPlanFile != null
            //    ? new PlanFile {
            //        Id = dbPlanFile.Id,
            //        Data = dbPlanFile.FileData,
            //        Name = dbPlanFile.FileName,
            //        DataLength = dbPlanFile.FileSize
            //    }
            //    : null;

            return plan;
        }

        public static SafetyStudyPlan ConvertToSafetyStudyPlan(SafetyPlan boPlan) {
            return new SafetyStudyPlan {
                Id = boPlan.Id,
                IdBudget = boPlan.IdBudget,
                IdPlanType = boPlan.GeneralData.IsEvaluation ? (int)BusinessPlanType.Evaluacion : (int)BusinessPlanType.MapaRiesgos,
                IdDelegation = (int)boPlan.GeneralData.IdDelegation,
                IdBusinessAddress = (int)boPlan.GeneralData.IdBusinessAddress,
                IdAffiliatedCompany = (int)boPlan.GeneralData.AffiliatedId,
                IdCustomer = (int)boPlan.GeneralData.IdCustomer,
                IdTemplate = (int)boPlan.GeneralData.IdTemplate,
                IdGeneralActivity = (int)boPlan.GeneralData.IdGeneralActivity,
                PlanCustomerDescription = boPlan.GeneralData.CustomerDescription,
                ProjectName = boPlan.GeneralData.PlanTitle,
                CreatorName = boPlan.GeneralData.CreatorName,
                IdCreatorProfile = (int)boPlan.GeneralData.CreatorCategoryId,
                ApproverName = boPlan.GeneralData.ApproverName,
                IdReviewer = (int)boPlan.GeneralData.IdReviewer,
                CreatedBy = boPlan.CreatedBy,
            };
        }

        public static PlanFile ConvertToPlanFile(SafetyStudyPlanFile dbFile, bool getData = false) {

            return new PlanFile {
                Id = dbFile.Id,
                Name = dbFile.FileName,
                Data = getData ? dbFile.FileData : null
            };
        }
        public static List<PlanFile> ConvertToPlanFile(List<SafetyStudyPlanFile> dbFiles, bool getData = false) {
            List<PlanFile> files = new List<PlanFile>();
            foreach (var file in dbFiles) {
                files.Add(ConvertToPlanFile(file, getData));
            }

            return files;
        }

        public static SafetyStudyPlanFile ConvertToSafetyStudyPlanFile(int planId, PlanFile planFile, int userId) => new SafetyStudyPlanFile {
            IdSafetyStudyPlan = planId,
            FileName = planFile.Name,
            FileData = planFile.Data,
            FileSize = planFile.Data.Length,
            TimeStamp = DateTime.Now,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            CreatedBy = userId,
            ModifiedBy = userId,
            IdPlanFileType = (int)AppPlanFileType.Anagram
        };

        public static List<SafetyStudyPlanFile> ConvertToSafetyStudyPlanFile(int planId, List<PlanFile> planFiles, int userId) {
            List<SafetyStudyPlanFile> response = new List<SafetyStudyPlanFile>();
            foreach (var planFile in planFiles) {
                response.Add(ConvertToSafetyStudyPlanFile(planId, planFile, userId));
            }
            return response;
        }

        private static SafetyPlanAdditionalData GetPlanAdditionalData(SafetyStudyPlanDetails additionalData) =>

            new SafetyPlanAdditionalData {

                Id = additionalData.Id,
                IdPlan = additionalData.IdPlan,
                ActivityDescription = additionalData.ActivityDescription,
                AffectedServices = additionalData.AffectedServices,
                AffectedServicesDescription = additionalData.AffectedServicesDescription,
                AssistanceCenters = additionalData.AssistanceCenters,
                CompanyName = additionalData.CompanyName,
                IdEmergencyPlanType = additionalData.IdEmergencyPlanType,
                EmergencyPlanDescription = additionalData.EmergencyPlanDescription,
                ExecutionBudget = additionalData.ExecutionBudget,
                ExecutionTime = additionalData.ExecutionTime,
                ExecutionTimeDays = additionalData.ExecutionTimeDays,
                ExecutionTimeMonths = additionalData.ExecutionTimeMonths,
                Localization = additionalData.Localization,
                Municipality = additionalData.Municipality,
                WorkersNumber = additionalData.WorkersNumber,
                OrganizationalStructure = additionalData.OrganizationalStructure,
                Promoter = additionalData.Promoter,
                PSSBudget = additionalData.PSSBudget,
                SecurityBudget = additionalData.SecurityBudget,
                SituationDescription = additionalData.SituationDescription,
            };

    }
}
