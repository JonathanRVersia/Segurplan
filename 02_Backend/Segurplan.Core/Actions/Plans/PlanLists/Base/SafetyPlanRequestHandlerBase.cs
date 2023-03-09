using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Segurplan.Core.Actions.Plans.PlansData;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.DataAccessLayer.Database.DataTransferObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Plans.PlanLists {
    public abstract class SafetyPlanRequestHandlerBase {
        protected readonly SafetyStudyPlanDam dam;

        public SafetyPlanRequestHandlerBase(SafetyStudyPlanDam dam) {
            this.dam = dam;
        }

        public async Task<IRequestResponse<SafetyPlanResponseBase>> Handle(SafetyPlanRequestBase request, CancellationToken cancellationToken) {
            var myRoleList = await dam.MyRoles(int.Parse(request.UserID));

            if (!request.TableState.AllPlans || !request.TableState.FirstLoad) {

                IQueryable<SafetyStudyPlan> planQuery = default;

                if (request.Filter != null) {
                    planQuery = dam.SelectByWhere(GetPlanWhere(request.Filter));
                }

                SetCreationUserConstraint(ref planQuery);

                // Count total rows
                var recordQuantity = planQuery.Count();
                var indexPage = request.TableState.IndexPage;
                var planList = SelectFolderToSafetyPlan(planQuery);

                SetOrder(request, ref planList);

                //check if enought rows remaining to fill the response
                var remainingRows = recordQuantity - (request.TableState.PageRows * indexPage);
                var take = request.TableState.PageRows;
                if (remainingRows - request.TableState.PageRows < 0) {
                    take = remainingRows;
                }

                if (remainingRows == 0) {
                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
                    indexPage = indexPage - 1;
                }

                if (planList.Count() > 0) {
                    var response = planList.Skip(request.TableState.PageRows * indexPage)
                   .Take(take).ToList();

                    if (request.TableState.FirstLoad == true && request.TableState.AllPlans == true) {
                        return RequestResponse.Ok(new SafetyPlanResponseBase(null, recordQuantity, myRoleList));
                    }

                    return RequestResponse.Ok(new SafetyPlanResponseBase(response, recordQuantity, myRoleList));
                }
            }

            return RequestResponse.Ok(new SafetyPlanResponseBase(null, 0, myRoleList));
        }


        private static Expression<Func<SafetyStudyPlan, bool>> GetPlanWhere(Filter filter) {
            return plan =>
                (string.IsNullOrEmpty(filter.Activity) || plan.IdGeneralActivityNavigation.Name.Contains(filter.Activity)) &&
                (string.IsNullOrEmpty(filter.Title) || plan.ProjectName.Contains(filter.Title)) &&
                (string.IsNullOrEmpty(filter.ProducedBy) || plan.CreatorName.Contains(filter.ProducedBy)) &&

                // Encontrar con que comparar
                (string.IsNullOrEmpty(filter.CheckedBy) || plan.IdReviewerNavigation.CompleteName.Contains(filter.CheckedBy)) &&
                (string.IsNullOrEmpty(filter.ApprovedBy) || plan.ApproverName.Contains(filter.ApprovedBy)) &&

                (string.IsNullOrEmpty(filter.Organization) || plan.IdDelegationNavigation.Name.Contains(filter.Organization)) &&
                (string.IsNullOrEmpty(filter.ProducedFromDate) || plan.CreateDate >= Convert.ToDateTime(filter.ProducedFromDate)) &&
                (string.IsNullOrEmpty(filter.ProducedToDate) || plan.CreateDate <= Convert.ToDateTime(filter.ProducedToDate));
        }

        public IQueryable<SafetyPlan> SelectFolderToSafetyPlan(IQueryable<SafetyStudyPlan> queryPlan) {
            return from plan in queryPlan
                   select new SafetyPlan {
                       Id = plan.Id,
                       GeneralData = new SafetyPlanGeneralData {
                           IdGeneralActivity = plan.IdGeneralActivity,
                           GeneralActivityName = plan.IdGeneralActivityNavigation.Name,
                           IdCustomer = plan.IdCustomer,
                           CustomerName = plan.IdCustomerNavigation.Name,
                           CustomerDescription = plan.PlanCustomerDescription,
                           Organization = plan.IdDelegationNavigation.Name,
                           PlanTitle = plan.ProjectName,
                           CreatorName = plan.CreatorName,
                           BusinessAddress = plan.IdBusinessAddressNavigation.Name.ToString(),
                           ModifiedByName = plan.ModifiedByNavigation != null
                                ? plan.ModifiedByNavigation.UserName
                                : string.Empty,
                       },
                       CreatedBy = plan.CreatedByNavigation.Id,
                       CreationDate = plan.CreateDate,
                       ModifiedBy = plan.ModifiedBy,
                       ModifiedDate = plan.UpdateDate != null
                        ? plan.UpdateDate
                        : plan.CreateDate
                   };
        }

        protected void SetOrder(SafetyPlanRequestBase request, ref IQueryable<SafetyPlan> consulta) {
            switch (request.TableState.OrderBy) {
                case TableState.OrganizationFilter:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.GeneralData.Organization)
                        : consulta.OrderBy(x => x.GeneralData.Organization);
                    break;
                case TableState.TitleFilter:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.GeneralData.PlanTitle)
                        : consulta.OrderBy(x => x.GeneralData.PlanTitle);
                    break;
                case TableState.CustomerFilter:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.GeneralData.CustomerName)
                        : consulta.OrderBy(x => x.GeneralData.CustomerName);
                    break;
                case TableState.ActivityFilter:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.GeneralData.GeneralActivityName)
                        : consulta.OrderBy(x => x.GeneralData.GeneralActivityName);
                    break;
                case TableState.ProducedByFilter:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.GeneralData.CreatorName)
                        : consulta.OrderBy(x => x.GeneralData.CreatorName);
                    break;
                default:
                    consulta = request.TableState.OrderMode == Helpers.ListOrderMode.Desc
                        ? consulta.OrderByDescending(x => x.ModifiedDate)
                        : consulta.OrderBy(x => x.ModifiedDate);
                    break;
            }
        }

        protected abstract void SetCreationUserConstraint(ref IQueryable<SafetyStudyPlan> consulta);
    }
}
