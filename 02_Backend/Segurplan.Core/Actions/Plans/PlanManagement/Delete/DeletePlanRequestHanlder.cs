using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Delete {
    public class DeletePlanRequestHanlder : IRequestHandler<DeletePlanRequest, IRequestResponse<DeletePlanResponse>> {

        SegurplanContext dbContext;

        public DeletePlanRequestHanlder(SegurplanContext context) {

            dbContext = context;
        }

        public async Task<IRequestResponse<DeletePlanResponse>> Handle(DeletePlanRequest request, CancellationToken cancellationToken) {

            IRequestResponse<DeletePlanResponse> result = null;

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {

                using (var trans = dbContext.Database.BeginTransaction()) {

                    try {

                        var dbPlan = dbContext.SafetyStudyPlan.Where(p => p.Id == request.PlanId).FirstOrDefault();
                        if (dbPlan == null) {
                            throw new Exception($"The plan {request.PlanId} does not exist!");
                        }

                        // Deleting the plan anagram
                        dbContext.SafetyStudyPlanFile.RemoveRange(dbContext.SafetyStudyPlanFile.Where(pf => pf.IdSafetyStudyPlan == request.PlanId));                        

                        // Deleting the plan activities
                        dbContext.PlanActivityVersion.RemoveRange(dbContext.PlanActivityVersion.Where(pav => pav.IdPlan == request.PlanId));

                        // Deleting details
                        dbContext.SafetyStudyPlanDetails.RemoveRange(dbContext.SafetyStudyPlanDetails.Where(pd => pd.IdPlan == request.PlanId));

                        // Deleting planes
                        dbContext.SafetyStudyPlanPlane.RemoveRange(dbContext.SafetyStudyPlanPlane.Where(pp => pp.IdSafetyStudyPlan == request.PlanId));

                        //Deleting budget
                        dbContext.Budget.RemoveRange(dbContext.Budget.Where(x => x.Id == dbPlan.IdBudget));

                        //Deleting budget details
                        dbContext.BudgetDetail.RemoveRange(dbContext.BudgetDetail.Where(x => x.IdBudget == dbPlan.IdBudget));

                        // Deleting the plan
                        dbContext.SafetyStudyPlan.Remove(dbPlan);

                        dbContext.SaveChanges();

                        trans.Commit();

                        result = RequestResponse.Ok(new DeletePlanResponse());

                    } catch (Exception exc) {

                        trans.Rollback();

                        result = RequestResponse.NotOk(new DeletePlanResponse { ErrorMsg = exc.ToString() });
                    }
                }
            });

            return result;
        }
    }
}
