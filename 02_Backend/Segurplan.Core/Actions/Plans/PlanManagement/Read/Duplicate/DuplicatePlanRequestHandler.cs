using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Plans.PlanManagement.DuplicatePlanCustomChapters;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.Duplicate {

    public class DuplicatePlanRequestHandler : ReadPlanGeneralDataRequestHandlerBase, IRequestHandler<DuplicatePlanRequest, IRequestResponse<EditPlanGeneralDataResponse>> {

        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public DuplicatePlanRequestHandler(SegurplanContext context, IMapper mapper, IMediator mediator) : base(context) {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<IRequestResponse<EditPlanGeneralDataResponse>> Handle(DuplicatePlanRequest request, CancellationToken cancellationToken) {

            IRequestResponse<EditPlanGeneralDataResponse> result = null;

            // Getting original plan from database
            var dbPlan = await dbContext.SafetyStudyPlan.Where(p => p.Id == request.OriginalPlanId).FirstOrDefaultAsync();
            if (dbPlan == null) {
                throw new Exception($"The plan {request.OriginalPlanId} does not exist!");
            }

            //Getting original Budget
            var originalBudget = await dbContext.Budget.Where(p => p.Id == dbPlan.IdBudget).FirstOrDefaultAsync();

            //Duplicating Budget
            var budgetId = await DuplicateBudget(originalBudget, request.UserId).ConfigureAwait(true);

            //Getting original BudgetDetails
            var originalBudgetDetails = await dbContext.BudgetDetail.Where(p => p.IdBudget == originalBudget.Id).ToListAsync();

            //Duplicate BudgetDetails
            await DuplicateBudgetDetails(request.UserId, budgetId, originalBudgetDetails);

            // Filling the new plan information based in the original one
            var duplicatedPlan = GetDuplicated(request.UserId, request.PlanTitle, dbPlan, budgetId);
            await dbContext.SafetyStudyPlan.AddAsync(duplicatedPlan);

            // Saving changes to get the duplicated plan identifier
            await dbContext.SaveChangesAsync();


            // Duplicating the plan details
            SafetyStudyPlanDetails duplicatedDetails = null;
            var dbDetails = await dbContext.SafetyStudyPlanDetails.FirstOrDefaultAsync(spd => spd.IdPlan == request.OriginalPlanId);
            if (dbDetails != null) {

                duplicatedDetails = new SafetyStudyPlanDetails {
                    IdPlan = duplicatedPlan.Id,
                    ActivityDescription = dbDetails.ActivityDescription,
                    AffectedServices = dbDetails.AffectedServices,
                    IdEmergencyPlanType = dbDetails.IdEmergencyPlanType,
                    ExecutionTimeMonths = dbDetails.ExecutionTimeMonths,
                    ExecutionTimeDays = dbDetails.ExecutionTimeDays,
                    ExecutionTime = dbDetails.ExecutionTime,
                    ExecutionBudget = dbDetails.ExecutionBudget,
                    EmergencyPlanDescription = dbDetails.EmergencyPlanDescription,
                    AffectedServicesDescription = dbDetails.AffectedServicesDescription,
                    CompanyName = dbDetails.CompanyName,
                    AssistanceCenters = dbDetails.AssistanceCenters,
                    Localization = dbDetails.Localization,
                    Municipality = dbDetails.Municipality,
                    WorkersNumber = dbDetails.WorkersNumber,
                    OrganizationalStructure = dbDetails.OrganizationalStructure,
                    Promoter = dbDetails.Promoter,
                    PSSBudget = dbDetails.PSSBudget,
                    SecurityBudget = dbDetails.SecurityBudget,
                    SituationDescription = dbDetails.SituationDescription,
                    CreatedBy = request.UserId,
                    ModifiedBy = request.UserId
                };

                await dbContext.SafetyStudyPlanDetails.AddAsync(duplicatedDetails);
            }

            // Getting original plan files or set default 
            var fileList = await dbContext.SafetyStudyPlanFile.Where(pf => pf.IdSafetyStudyPlan == request.OriginalPlanId).ToListAsync();
            if (!fileList.Any()) {
                fileList = await dbContext.DefaultSafetyStudyPlanFile.ProjectTo<SafetyStudyPlanFile>(mapper.ConfigurationProvider).ToListAsync();
            }
            if (fileList != null && fileList.Count() > 0) {

                foreach (var planFile in fileList) {

                    var file = new SafetyStudyPlanFile {
                        CreatedBy = request.UserId,
                        FileData = planFile.FileData,
                        FileName = planFile.FileName,
                        FileSize = planFile.FileSize,
                        IdPlanFileType = planFile.IdPlanFileType,
                        IdSafetyStudyPlan = duplicatedPlan.Id,
                        TimeStamp = planFile.TimeStamp,
                        ModifiedBy = request.UserId
                    };

                    await dbContext.SafetyStudyPlanFile.AddAsync(file);
                }
            }

            // Getting original plan activities
            var planActivities = await dbContext.PlanActivityVersion.Where(pa => pa.IdPlan == request.OriginalPlanId).ToListAsync();
            if (planActivities != null && planActivities.Count() > 0) {

                var newPlanActivityVersionsList = new List<PlanActivityVersion>();
                var customActivityVersionsToCopy = new List<PlanActivityVersion>();

                foreach (var activity in planActivities) {

                    var planActivityVersion = new PlanActivityVersion {

                        IdActivityVersion = activity.IdActivityVersion,
                        IdPlan = duplicatedPlan.Id,
                        ChapterPosition = activity.ChapterPosition,
                        SubChapterPosition = activity.SubChapterPosition,
                        Position = activity.Position,
                        WordDescription = activity.WordDescription,
                        ChapterDescription = activity.ChapterDescription,
                        SubChapterDescription = activity.SubChapterDescription,
                        AvailableActivitiId = activity.AvailableActivitiId,
                        CustomActivityId=activity.CustomActivityId
                    };

                    if (planActivityVersion.CustomActivityId == null) {
                        newPlanActivityVersionsList.Add(planActivityVersion);
                    } else {
                        //Me tengo que que crear copia de los custom que haya y luego devolver List de PlanActivityVersion y añadirlo a la lista
                        customActivityVersionsToCopy.Add(planActivityVersion);
                    }
                }

                if (customActivityVersionsToCopy.Any()) {
                    var response = await mediator.Send(new DuplicatePlanCustomChaptersRequest { PlanActivityVersions = customActivityVersionsToCopy });

                    if (response.Status == RequestStatus.Ok) {

                        newPlanActivityVersionsList.AddRange(response.Value.UpdatedPlanActivityVersions);
                    }
                }

                if (newPlanActivityVersionsList.Any()) {
                    await dbContext.PlanActivityVersion.AddRangeAsync(newPlanActivityVersionsList);
                }
            }

            // Getting original plan planes
            var planPlanes = await dbContext.SafetyStudyPlanPlane.Include(x => x.Files).Where(pp => pp.IdSafetyStudyPlan == request.OriginalPlanId).ToListAsync();
            if (planPlanes != null && planPlanes.Count() > 0) {

                var files = new List<SafetyStudyPlanPlaneFile>();

                foreach (var plane in planPlanes) {

                    foreach (var file in plane.Files) {
                        files.Add(new SafetyStudyPlanPlaneFile {
                            Data = file.Data,
                            FileName = file.FileName,
                            Name = file.Name
                        });
                    }

                    await dbContext.SafetyStudyPlanPlane.AddAsync(new SafetyStudyPlanPlane {

                        IdPlane = plane.IdPlane,
                        IdSafetyStudyPlan = duplicatedPlan.Id,
                        Position = plane.Position,
                        Description = plane.Description,
                        FamilyName = plane.FamilyName,
                        Files = files,
                        CreatedBy = request.UserId,
                        ModifiedBy = request.UserId
                    });
                }
            }

            // Saving information
            await dbContext.SaveChangesAsync();

            //trans.Commit();

            // Getting plan information from database
            duplicatedPlan = dbContext.SafetyStudyPlan
                .Include(p => p.IdDelegationNavigation)
                .Include(p => p.SafetyStudyPlanFile)
                .Include(p => p.IdBusinessAddressNavigation)
                .Include(p => p.IdAffiliatedCompanyNavigation)
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdGeneralActivityNavigation)
                .Include(p => p.IdTemplateNavigation)
                .Include(p => p.CreatedByNavigation)
                .Include(p => p.IdCreatorProfileNavigation)
                .Include(p => p.IdReviewerNavigation)
                .Include(p => p.ModifiedByNavigation)
                .FirstOrDefault(p => p.Id == duplicatedPlan.Id);

            result = RequestResponse.Ok(
                new EditPlanGeneralDataResponse {
                    PlanInformation = PlanManagementCommon.ConvertToSafetyPlan(duplicatedPlan, duplicatedDetails),
                    TemplateList = await GetTemplateList(),
                    DelegationList = await GetDelegationList(),
                    CustomerList = await GetCustomerList(),
                    BusAddList = await GetBusinessAddressList(),
                    GenActList = await GetGeneralActivityList(),
                    ProfileList = await GetProfileList(),
                    UserList = await GetUserList(),
                    AffiliatedCompanyList = await GetAffiliatedCompanyList()
                });

            return result;
        }

        private static SafetyStudyPlan GetDuplicated(int currentUserID, string newName, SafetyStudyPlan originalPlan, int budgetId) {
            return new SafetyStudyPlan {
                ProjectName = newName,
                CreatorName = originalPlan.CreatorName,
                IdBusinessAddress = originalPlan.IdBusinessAddress,
                IdAffiliatedCompany = originalPlan.IdAffiliatedCompany,
                PlanCustomerDescription = originalPlan.PlanCustomerDescription,
                IdCustomer = originalPlan.IdCustomer,
                IdDelegation = originalPlan.IdDelegation,
                IdPlanType = originalPlan.IdPlanType,
                IdTemplate = originalPlan.IdTemplate,
                IdBudget = budgetId,
                IdGeneralActivity = originalPlan.IdGeneralActivity,
                IdCreatorProfile = originalPlan.IdCreatorProfile,
                IdReviewer = originalPlan.IdReviewer,
                ApproverName = originalPlan.ApproverName,
                PlanReview = originalPlan.PlanReview,
                CreatedBy = currentUserID,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                ModifiedBy = currentUserID
            };
        }

        private async Task<int> DuplicateBudget(Budget originalBudget, int userId) {

            var newBudget = new Budget();
            if (originalBudget != null) {
                newBudget = new Budget {
                    ApplicabePercentage = originalBudget.ApplicabePercentage,
                    StudyBudget = originalBudget.StudyBudget,
                    Difference = originalBudget.Difference,
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

        private async Task DuplicateBudgetDetails(int userId, int budgetId, List<BudgetDetail> originalBudgetDetails) {

            if (originalBudgetDetails != null) {
                if (originalBudgetDetails.Any()) {
                    foreach (var article in originalBudgetDetails) {
                        var budgetDetail = new BudgetDetail() {
                            IdArticle = article.IdArticle,
                            IdBudget = budgetId,
                            QuantityUnits = article.QuantityUnits,
                            UnitPrice = article.UnitPrice,
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
    }
}
