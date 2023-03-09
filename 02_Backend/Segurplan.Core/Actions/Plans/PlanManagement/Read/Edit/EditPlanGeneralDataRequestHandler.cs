using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Database;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;
using AutoMapper;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit {
    public class EditPlanGeneralDataRequestHandler : ReadPlanGeneralDataRequestHandlerBase, IRequestHandler<EditPlanGeneralDataRequest, IRequestResponse<EditPlanGeneralDataResponse>> {


        public EditPlanGeneralDataRequestHandler(SegurplanContext context) : base(context) {
        }

        public async Task<IRequestResponse<EditPlanGeneralDataResponse>> Handle(EditPlanGeneralDataRequest request, CancellationToken cancellationToken) {

            var planData = await GetPlanInformation(request.PlanId);

            return planData == default(SafetyPlan) ? RequestResponse.Error<EditPlanGeneralDataResponse>() : RequestResponse.Ok(new EditPlanGeneralDataResponse {
                PlanInformation = planData,
                DelegationList = await GetDelegationList(),
                AffiliatedCompanyList = await GetAffiliatedCompanyList(),
                CustomerList = await GetCustomerList(),
                TemplateList = await GetTemplateList(),
                BusAddList = await GetBusinessAddressList(),
                GenActList = await GetGeneralActivityList(),
                ProfileList = await GetProfileList(),
                UserList = await GetUserList()
            });
        }
    }
}
