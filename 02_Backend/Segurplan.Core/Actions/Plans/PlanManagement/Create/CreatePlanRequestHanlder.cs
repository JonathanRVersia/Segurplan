using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Segurplan.Core.Actions.Plans.PlanManagement.Read;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.BusinessObjects;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Create {
    public class CreatePlanRequestHanlder : ReadPlanGeneralDataRequestHandlerBase, IRequestHandler<CreatePlanRequest, IRequestResponse<EditPlanGeneralDataResponse>> {

        private readonly IMapper mapper;

        public CreatePlanRequestHanlder(SegurplanContext context, IMapper mapper) : base(context) {
            this.mapper = mapper;
        }

        public async Task<IRequestResponse<EditPlanGeneralDataResponse>> Handle(CreatePlanRequest request, CancellationToken cancellationToken) {

            request.UserId = 3;
            IRequestResponse<EditPlanGeneralDataResponse> res;
            if (request.PlanInformation == null) {

                var response = new EditPlanGeneralDataResponse {
                    DelegationList = await GetDelegationList(),
                    AffiliatedCompanyList = await GetAffiliatedCompanyList(),
                    CustomerList = await GetCustomerList(),
                    TemplateList = await GetTemplateList(),
                    BusAddList = await GetBusinessAddressList(),
                    GenActList = await GetGeneralActivityList(),
                    ProfileList = await GetProfileList(),
                    UserList = await GetUserList(),
                    PlanInformation = await GetDefaultPlanInformation()
                };

                if (!response.DelegationList.Any()
                    || !response.AffiliatedCompanyList.Any()
                    || !response.CustomerList.Any()
                    || !response.TemplateList.Any()
                    || !response.BusAddList.Any()
                    || !response.GenActList.Any()
                    || !response.ProfileList.Any()
                    || !response.UserList.Any()) {

                    return RequestResponse.Error<EditPlanGeneralDataResponse>();
                }

                return RequestResponse.Ok(response);

            } else {
                var strategy = dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () => {

                    using (var trans = dbContext.Database.BeginTransaction()) {

                        try {
                            //Adding budget and budget details
                            var budgetId = await CreateBudget(request.PlanInformation, request.UserId).ConfigureAwait(true);
                            await AddBudgetDetails(request.UserId, budgetId, request.PlanInformation.Budget.SelectedArticles);

                            // Adding the plan information
                            var dbPlan = PlanManagementCommon.ConvertToSafetyStudyPlan(request.PlanInformation);
                            //dbPlan.CreateDate = DateTime.Now;
                            //dbPlan.UpdateDate = DateTime.Now;
                            dbPlan.CreatedBy = request.UserId;
                            dbPlan.ModifiedBy = request.UserId;
                            dbPlan.IdBudget = budgetId;

                            List<SafetyStudyPlanFile> anagramFiles = new List<SafetyStudyPlanFile>();
                            if (request.PlanInformation.GeneralData.Anagrams.Any()) {
                                anagramFiles.AddRange(PlanManagementCommon.ConvertToSafetyStudyPlanFile(dbPlan.Id, request.PlanInformation.GeneralData.Anagrams, request.UserId));

                                dbPlan.SafetyStudyPlanFile = anagramFiles;
                            } else {
                                //Instrucción INSERT en conflicto con la restricción FOREIGN KEY 'FK_SafetyStudyPlanFile_Users_CreatedBy'.
                                //El conflicto ha aparecido en la base de datos 'Segurplan287', tabla 'dbo.Users', column 'Id'.
                                //Se terminó la instrucción.
                                anagramFiles = await dbContext.DefaultSafetyStudyPlanFile.ProjectTo<SafetyStudyPlanFile>(mapper.ConfigurationProvider).ToListAsync();
                                anagramFiles.Select(c => {
                                    c.Id = 0;
                                    c.CreatedBy = request.UserId;
                                    c.CreateDate = DateTime.Now;
                                    c.ModifiedBy = request.UserId;
                                    c.UpdateDate = DateTime.Now;
                                    return c;
                                }).ToList();

                                dbPlan.SafetyStudyPlanFile.AddRange(anagramFiles);
                            }

                            dbPlan.SafetyStudyPlanDetails.Add(mapper.Map<SafetyStudyPlanDetails>(request.PlanInformation.AdditionalData));
                            foreach (var planDetails in dbPlan.SafetyStudyPlanDetails) {
                                planDetails.CreatedBy = request.UserId;
                                planDetails.CreateDate = DateTime.Now;
                                planDetails.ModifiedBy = request.UserId;
                                planDetails.UpdateDate = DateTime.Now;
                            }


                            dbPlan.SafetyStudyPlanPlane = AddPlanPlanes(request);
                            dbPlan.IdPlanType = 1;
                            dbPlan.IdReviewer = request.UserId;
                            dbContext.SafetyStudyPlan.Add(dbPlan);

                            // Saving changes to retrieve the new plan identifier
                            dbContext.SaveChanges();

                            request.PlanInformation.Id = dbPlan.Id;

                            if (anagramFiles.Any(x => x.Id > 0)) {
                                request.PlanInformation.GeneralData.Anagrams = PlanManagementCommon.ConvertToPlanFile(anagramFiles);
                            }

                            if (request.PlanInformation.ActivityLists.PlanActivities != null && request.PlanInformation.ActivityLists.PlanActivities.Count > 0) {
                                List<PlanActivityVersion> activityVersionList = AddSafetyPlanActivities(request.PlanInformation.ActivityLists.PlanActivities, dbPlan);

                                if (activityVersionList.Any()) {

                                    await dbContext.PlanActivityVersion.AddRangeAsync(activityVersionList);
                                }
                            }

                            dbContext.SaveChanges();

                            trans.Commit();

                            return RequestResponse.Ok(new EditPlanGeneralDataResponse());

                        } catch (Exception exc) {

                            trans.Rollback();

                            throw exc;
                        }
                    }
                });

                return RequestResponse.Ok(new EditPlanGeneralDataResponse());
            }
        }

        private async Task<SafetyPlan> GetDefaultPlanInformation() {

            var defaultSafetyPlan = new SafetyPlan {
                GeneralData = new SafetyPlanGeneralData {
                    Anagrams = dbContext.DefaultSafetyStudyPlanFile.ProjectTo<PlanFile>(mapper.ConfigurationProvider).ToList()
                }
            };
            defaultSafetyPlan.GeneralData.Anagrams.ForEach(x => x.DefaultFile = true);


            return defaultSafetyPlan;

        }

        private async Task<int> CreateBudget(SafetyPlan plan, int userId) {

            var newBudget = new Budget();
            if (plan.Budget != null) {
                newBudget = new Budget {
                    ApplicabePercentage = plan.Budget.ApplicabePercentage,
                    StudyBudget = plan.Budget.StudyBudget,
                    Difference = plan.Budget.Difference,
                    CreatedBy = userId,
                    CreateDate = DateTime.Now,
                    ModifiedBy = userId,
                    UpdateDate = DateTime.Now,
                };
            } else {
                newBudget = new Budget {
                    ApplicabePercentage = 100,
                    StudyBudget = 0,
                    Difference = 0,
                    CreatedBy = userId,
                    CreateDate = DateTime.Now,
                    ModifiedBy = userId,
                    UpdateDate = DateTime.Now,
                };
            }

            dbContext.Budget.Add(newBudget);
            dbContext.SaveChanges();

            return (newBudget.Id);
        }

        private async Task AddBudgetDetails(int userId, int budgetId, List<ApplicationArticle> selectedArticles) {

            if (selectedArticles != null) {
                if (selectedArticles.Any()) {
                    foreach (var article in selectedArticles) {
                        var budgetDetail = new BudgetDetail() {
                            IdArticle = article.Id,
                            IdBudget = budgetId,
                            QuantityUnits = article.Unit,
                            UnitPrice = article.TotalPrice,
                            CreatedBy = userId,
                            CreateDate = DateTime.Now,
                            ModifiedBy = userId,
                            UpdateDate = DateTime.Now,
                        };
                        dbContext.BudgetDetail.Add(budgetDetail);
                    }
                }
            }

            dbContext.SaveChanges();
        }

        private ICollection<SafetyStudyPlanPlane> AddPlanPlanes(CreatePlanRequest request) {

            var result = mapper.Map<List<SafetyStudyPlanPlane>>(request.PlanInformation.SelectedPlanes);
            foreach (var plane in result) {
                plane.CreatedBy = request.UserId;
                plane.ModifiedBy = request.UserId;
            }
            return result;
        }

        private List<PlanActivityVersion> AddSafetyPlanActivities(List<SelectedPlanActivity> activityList, SafetyStudyPlan dbPlan) {

            List<PlanActivityVersion> activityVersionList = new List<PlanActivityVersion>();

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
                                    IdPlan = dbPlan.Id,
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
                    activityVersionList.Add(CreateCustomSafetyPlanActivities(dbPlan.Id, planAct));
                }
            }

            return activityVersionList;
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
    }
}
