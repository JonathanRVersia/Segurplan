using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Actions.Plans.PlanManagement.Read;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;


namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class UpdatePlanBase : ReadPlanGeneralDataRequestHandlerBase {

        public UpdatePlanBase(SegurplanContext context) : base(context) {
        }

        protected async Task SavePlanInformation(UpdatePlanRequestBase request) {

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () => {

                using (var trans = dbContext.Database.BeginTransaction()) {

                    try {

                        UpdateSafetyPlan(request.PlanInformation, request.UserId);

                        UpdateSafetyPlanDetails(request.PlanInformation, request.UserId);

                        var dbFiles = UpdateSafetyPlanAnagrams(request.PlanInformation.Id, request.PlanInformation.GeneralData.Anagrams, request.UserId, request.PlanInformation.GeneralData.DeleteExistingFileIds);

                        UpdateSafetyPlanActivities(request.PlanInformation.Id, request.PlanInformation.ActivityLists.PlanActivities);

                        UpdateSafetyPlanPlanes(request.PlanInformation.Id, request.PlanInformation.SelectedPlanes, request.UserId);

                        UpdateBudget(request.PlanInformation, request.UserId);

                        await RemoveBudgetDetails(request.PlanInformation.IdBudget);

                        AddBudgetDetails(request.UserId, request.PlanInformation.IdBudget, request.PlanInformation.Budget.SelectedArticles);

                        dbContext.SaveChanges();

                        trans.Commit();

                        if (dbFiles.Any())
                            //request.PlanInformation.GeneralData.Anagram.Id = dbFile.Id;
                            foreach (var dbFile in dbFiles) {
                                request.PlanInformation.GeneralData.Anagrams[dbFiles.IndexOf(dbFile)].Id = dbFile.Id;
                            }


                    } catch (Exception exc) {

                        trans.Rollback();
                        throw exc;
                    }
                }
            });
        }

        private void UpdateBudget(SafetyPlan plan, int userId) {

            var dbBudget = dbContext.Budget.Where(x => x.Id == plan.Budget.Id).FirstOrDefault();

            dbBudget.ModifiedBy = userId;
            dbBudget.UpdateDate = DateTime.Now;
            dbBudget.ApplicabePercentage = plan.Budget.ApplicabePercentage;
            dbBudget.StudyBudget = plan.Budget.StudyBudget;
            dbBudget.Difference = plan.Budget.Difference;

            dbContext.Budget.Update(dbBudget);
            dbContext.SaveChanges();
        }

        private void AddBudgetDetails(int userId, int budgetId, List<ApplicationArticle> selectedArticles) {

            if (selectedArticles != null) {
                if (selectedArticles.Any()) {
                    foreach (var article in selectedArticles) {
                        var budgetDetail = new BudgetDetail() {
                            IdArticle = article.Id,
                            IdBudget = budgetId,
                            QuantityUnits = article.Unit,
                            UnitPrice = article.PriceDurationWork,
                            CreatedBy = userId,
                            CreateDate = DateTime.Now,
                            ModifiedBy = userId,
                            UpdateDate = DateTime.Now,
                        };
                        dbContext.BudgetDetail.Add(budgetDetail);
                    }
                }
            }

        }

        private async Task RemoveBudgetDetails(int budgetId) {

            var dbBudgetDetails = dbContext.BudgetDetail.Where(x => x.IdBudget == budgetId);

            if (dbBudgetDetails != null && dbBudgetDetails.Count() > 0) {
                dbContext.BudgetDetail.RemoveRange(dbBudgetDetails);
            }
        }

        private void UpdateSafetyPlan(SafetyPlan planInformation, int userId) {

            var dbPlan = (from pl in dbContext.SafetyStudyPlan where pl.Id == planInformation.Id select pl).FirstOrDefault();

            if (dbPlan == null) {
                throw new Exception("Plan doesn't exist!");
            }

            var newPlan = PlanManagementCommon.ConvertToSafetyStudyPlan(planInformation);
            CheckInformationHasChanged(newPlan, dbPlan);

            dbPlan.UpdateDate = DateTime.Now;
            dbPlan.ModifiedBy = userId;

            var result = dbContext.SafetyStudyPlan.Update(dbPlan);

            if (result.State != EntityState.Modified) {

                throw new Exception("Error updating plan");
            }

            planInformation.CreatedBy = dbPlan.CreatedBy;
            planInformation.ModifiedBy = dbPlan.ModifiedBy;
            planInformation.IdBudget = dbPlan.IdBudget;
        }

        private void UpdateSafetyPlanDetails(SafetyPlan planInformation, int userId) {

            var dbDetails = dbContext.SafetyStudyPlanDetails.FirstOrDefault(p => p.IdPlan == planInformation.Id);

            if (dbDetails != null) {

                dbDetails.Localization = planInformation.AdditionalData.Localization;
                dbDetails.Municipality = planInformation.AdditionalData.Municipality;
                dbDetails.WorkersNumber = planInformation.AdditionalData.WorkersNumber;
                dbDetails.OrganizationalStructure = planInformation.AdditionalData.OrganizationalStructure;
                dbDetails.Promoter = planInformation.AdditionalData.Promoter;
                dbDetails.PSSBudget = planInformation.AdditionalData.PSSBudget;
                dbDetails.SecurityBudget = planInformation.AdditionalData.SecurityBudget;
                dbDetails.SituationDescription = planInformation.AdditionalData.SituationDescription;
                dbDetails.ActivityDescription = planInformation.AdditionalData.ActivityDescription;
                dbDetails.AffectedServices = planInformation.AdditionalData.AffectedServices;
                dbDetails.AffectedServicesDescription = planInformation.AdditionalData.AffectedServicesDescription;
                dbDetails.AssistanceCenters = planInformation.AdditionalData.AssistanceCenters;
                dbDetails.CompanyName = planInformation.AdditionalData.CompanyName;
                dbDetails.IdEmergencyPlanType = planInformation.AdditionalData.IdEmergencyPlanType;
                dbDetails.EmergencyPlanDescription = planInformation.AdditionalData.EmergencyPlanDescription;
                dbDetails.ExecutionBudget = planInformation.AdditionalData.ExecutionBudget;
                dbDetails.ExecutionTime = planInformation.AdditionalData.ExecutionTime;
                dbDetails.ExecutionTimeDays = planInformation.AdditionalData.ExecutionTimeDays;
                dbDetails.ExecutionTimeMonths = planInformation.AdditionalData.ExecutionTimeMonths;

                dbDetails.ModifiedBy = userId;
                dbDetails.UpdateDate = DateTime.Now;

                dbContext.SafetyStudyPlanDetails.Update(dbDetails);
            } else {
                SafetyStudyPlanDetails det = new SafetyStudyPlanDetails {

                    IdPlan = planInformation.Id,
                    Localization = planInformation.AdditionalData.Localization,
                    Municipality = planInformation.AdditionalData.Municipality,
                    WorkersNumber = planInformation.AdditionalData.WorkersNumber,
                    OrganizationalStructure = planInformation.AdditionalData.OrganizationalStructure,
                    Promoter = planInformation.AdditionalData.Promoter,
                    PSSBudget = planInformation.AdditionalData.PSSBudget,
                    SecurityBudget = planInformation.AdditionalData.SecurityBudget,
                    SituationDescription = planInformation.AdditionalData.SituationDescription,
                    ActivityDescription = planInformation.AdditionalData.ActivityDescription,
                    AffectedServices = planInformation.AdditionalData.AffectedServices,
                    AffectedServicesDescription = planInformation.AdditionalData.AffectedServicesDescription,
                    AssistanceCenters = planInformation.AdditionalData.AssistanceCenters,
                    CompanyName = planInformation.AdditionalData.CompanyName,
                    IdEmergencyPlanType = planInformation.AdditionalData.IdEmergencyPlanType,
                    EmergencyPlanDescription = planInformation.AdditionalData.EmergencyPlanDescription,
                    ExecutionBudget = planInformation.AdditionalData.ExecutionBudget,
                    ExecutionTime = planInformation.AdditionalData.ExecutionTime,
                    ExecutionTimeDays = planInformation.AdditionalData.ExecutionTimeDays,
                    ExecutionTimeMonths = planInformation.AdditionalData.ExecutionTimeMonths,
                    CreatedBy = userId,
                    CreateDate = DateTime.Now,
                    ModifiedBy = userId,
                    UpdateDate = DateTime.Now
                };

                try {

                    dbContext.SafetyStudyPlanDetails.Add(det);
                    dbContext.SaveChanges();
                } catch (Exception ex) {

                    throw;
                }

            }
        }

        private List<SafetyStudyPlanFile> UpdateSafetyPlanAnagrams(int planId, List<PlanFile> anagrams, int userId, List<int> deleteExistingFilesId) {
            deleteExistingFilesId = deleteExistingFilesId is null ? new List<int>() : deleteExistingFilesId;

            if (deleteExistingFilesId.Any()) {
                var dbPlanFile = (from plf in dbContext.SafetyStudyPlanFile
                                  where deleteExistingFilesId.Any(id => id == plf.Id) && plf.IdPlanFileType == (int)AppPlanFileType.Anagram
                                  select new SafetyStudyPlanFile {
                                      FileName = plf.FileName,
                                      Id = plf.Id
                                  });

                if (dbPlanFile.Any()) {
                    dbContext.SafetyStudyPlanFile.RemoveRange(dbPlanFile);
                }
            }

            if (anagrams.Any()) {
                var dbFiles = PlanManagementCommon.ConvertToSafetyStudyPlanFile(planId, anagrams, userId);
                dbFiles.ForEach(x => {
                    x.CreateDate = DateTime.Now;
                    x.UpdateDate = x.CreateDate;
                });
                //dbFile.IdPlanFileType = (int)AppPlanFileType.Anagram;

                foreach (var dbFile in dbFiles) {
                    var fileResult = dbContext.SafetyStudyPlanFile.Add(dbFile);

                    if (fileResult.State != EntityState.Added) {
                        throw new Exception("Error updating plan anagram");
                    }
                }

                return dbFiles;
            }
            return new List<SafetyStudyPlanFile>();
        }

        private PlanActivityVersion CreateCustomSafetyPlanActivities(int planId, SelectedPlanActivity customActivity) {

            var customSafetyPlanActivity = dbContext.CustomActivity.Where(cAct => cAct.Id == customActivity.CustomActivityId).Select(cAct => new PlanActivityVersion {
                CustomActivityId = cAct.Id,
                IdPlan = planId,
                Position = customActivity.ActivityPosition,
                WordDescription = customActivity.WordDescription,
                ChapterPosition = customActivity.ChapterPosition,
                ChapterDescription = customActivity.ChapterDescription,
                SubChapterPosition = customActivity.SubChapterPosition,
                SubChapterDescription = customActivity.SubChapterDescription,
                AvailableActivitiId = customActivity.AvailableActivitiId
            }).FirstOrDefault();

            return customSafetyPlanActivity;
        }

        private void UpdateSafetyPlanActivities(int planId, List<SelectedPlanActivity> activityList) {

            // Delete current activities
            var currentActs = dbContext.PlanActivityVersion.Where(pav => pav.IdPlan == planId);
            if (currentActs != null && currentActs.Count() > 0) {

                dbContext.PlanActivityVersion.RemoveRange(currentActs);
            }

            List<PlanActivityVersion> activityVersionList = new List<PlanActivityVersion>(activityList.Count);
            foreach (var planAct in activityList) {

                if (!planAct.IsCustomActivity) {
                    var actV = (from av in dbContext.ActivityVersion
                                join sv in dbContext.SubChapterVersion on av.IdSubChapterVersion equals sv.Id
                                join cv in dbContext.ChapterVersion on sv.IdChapterVersion equals cv.Id
                                join c in dbContext.Chapter on cv.IdChapter equals c.Id
                                //join cvi in dbContext.ChapterVersionInfo on c.IdVersionInfo equals cvi.Id
                                where av.IdActivity == planAct.Id
                                select new PlanActivityVersion {
                                    IdActivityVersion = av.Id,
                                    IdPlan = planId,
                                    Position = planAct.ActivityPosition,
                                    WordDescription = planAct.WordDescription,
                                    ChapterPosition = planAct.ChapterPosition,
                                    ChapterDescription = planAct.ChapterDescription,
                                    SubChaptId = sv.IdSubChapter,
                                    SubChapterPosition = planAct.SubChapterPosition,
                                    SubChapterDescription = planAct.SubChapterDescription,
                                    AvailableActivitiId = planAct.AvailableActivitiId,


                                }).FirstOrDefault();

                    if (actV != null) {

                        activityVersionList.Add(actV);
                    }

                } else {
                    activityVersionList.Add(CreateCustomSafetyPlanActivities(planId, planAct));
                }

            }

            // Getting the activity versions
            if (activityVersionList != null) {

                dbContext.PlanActivityVersion.AddRangeAsync(activityVersionList);
            }
        }


        private void UpdateSafetyPlanPlanes(int planId, List<SafetyPlanPlane> selectedPlanes, int userId) {

            var storedPlans = dbContext.SafetyStudyPlanPlane.Where(selPlane => selPlane.IdSafetyStudyPlan == planId).ToList();
            if (storedPlans.Any()) {

                dbContext.SafetyStudyPlanPlane.RemoveRange(storedPlans);
            }

            List<SafetyStudyPlanPlane> safetyStudyPlanPlanes = new List<SafetyStudyPlanPlane>();

            foreach (SafetyPlanPlane planPlane in selectedPlanes) {
                safetyStudyPlanPlanes.Add(new SafetyStudyPlanPlane {
                    Id = planPlane.Id,
                    IdSafetyStudyPlan = planPlane.IdPlan,
                    IdPlane = planPlane.IdPlane,
                    Position = planPlane.Position,
                    Description = planPlane.Description,
                    FamilyName = planPlane.FamilyName,
                    IsAvailable = planPlane.IsAvailable,
                    ContainsFile = planPlane.ContainsFile
                });
            }

            if (storedPlans.Any()) {

                List<SafetyStudyPlanPlane> toUpdatePlanes = safetyStudyPlanPlanes.Where(pl => storedPlans.Select(plPlane => plPlane.Id).Contains(pl.Id)).ToList();
                toUpdatePlanes.Select(pl => {
                    pl.ModifiedBy = userId;
                    pl.UpdateDate = DateTime.Now;
                    pl.CreateDate = storedPlans.Find(storedPl => storedPl.Id == pl.Id).CreateDate;
                    pl.CreatedBy = storedPlans.Find(storedPl => storedPl.Id == pl.Id).CreatedBy;
                    //                    pl.Id = 0;
                    return pl;
                }).ToList();

                safetyStudyPlanPlanes.RemoveAll(pl => storedPlans.Any(prop => prop.Id == pl.Id));


                dbContext.SafetyStudyPlanPlane.UpdateRange(toUpdatePlanes);
            }


            if (safetyStudyPlanPlanes.Any()) {

                safetyStudyPlanPlanes.Select(pl => { pl.Id = 0; pl.CreateDate = DateTime.Now; pl.CreatedBy = userId; pl.ModifiedBy = userId; pl.UpdateDate = DateTime.Now; return pl; }).ToList();

                dbContext.SafetyStudyPlanPlane.AddRange(safetyStudyPlanPlanes);
            }
        }

        private static bool CheckInformationHasChanged(SafetyStudyPlan updatedPlan, SafetyStudyPlan currentPlan) {
            bool updated = false;

            if (updatedPlan.IdCustomer > 0 && updatedPlan.IdCustomer != currentPlan.IdCustomer) {
                updated = true;
                currentPlan.IdCustomer = updatedPlan.IdCustomer;
            }

            if (updatedPlan.PlanCustomerDescription != currentPlan.PlanCustomerDescription) {
                currentPlan.PlanCustomerDescription = updatedPlan.PlanCustomerDescription;
                updated = true;
            }

            if (updatedPlan.IdDelegation > 0 && updatedPlan.IdDelegation != currentPlan.IdDelegation) {
                updated = true;
                currentPlan.IdDelegation = updatedPlan.IdDelegation;
            }

            if (updatedPlan.IdPlanType > 0 && updatedPlan.IdPlanType != currentPlan.IdPlanType) {
                updated = true;
                currentPlan.IdPlanType = updatedPlan.IdPlanType;
            }

            if (updatedPlan.IdTemplate > 0 && updatedPlan.IdTemplate != currentPlan.IdTemplate) {
                updated = true;
                currentPlan.IdTemplate = updatedPlan.IdTemplate;
            }

            if (updatedPlan.IdGeneralActivity != currentPlan.IdGeneralActivity) {
                updated = true;
                currentPlan.IdGeneralActivity = updatedPlan.IdGeneralActivity;
            }

            if (!string.IsNullOrEmpty(updatedPlan.ProjectName) && updatedPlan.ProjectName != currentPlan.ProjectName) {
                updated = true;
                currentPlan.ProjectName = updatedPlan.ProjectName;
            }

            if (updatedPlan.IdCreatorProfile != currentPlan.IdCreatorProfile) {
                updated = true;
                currentPlan.IdCreatorProfile = updatedPlan.IdCreatorProfile;
            }

            if (updatedPlan.IdReviewer != currentPlan.IdReviewer) {
                updated = true;
                currentPlan.IdReviewer = updatedPlan.IdReviewer;
            }

            if (updatedPlan.ApproverName != currentPlan.ApproverName) {
                updated = true;
                currentPlan.ApproverName = updatedPlan.ApproverName;
            }

            if (updatedPlan.CreatorName != currentPlan.CreatorName) {
                updated = true;
                currentPlan.CreatorName = updatedPlan.CreatorName;
            }

            if (updatedPlan.IdAffiliatedCompany != currentPlan.IdAffiliatedCompany) {
                updated = true;
                currentPlan.IdAffiliatedCompany = updatedPlan.IdAffiliatedCompany;
            }

            if (updatedPlan.IdBusinessAddress != currentPlan.IdBusinessAddress) {
                updated = true;
                currentPlan.IdBusinessAddress = updatedPlan.IdBusinessAddress;
            }

            return updated;
        }
    }
}
