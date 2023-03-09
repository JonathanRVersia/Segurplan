using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;
using System.Linq;
using AutoMapper;

namespace Segurplan.Core.Actions.Plans.PlanManagement.Update {
    public class SavePlanRequestHandler : UpdatePlanBase, IRequestHandler<SavePlanRequest, IRequestResponse<EditPlanGeneralDataResponse>> {

        public SavePlanRequestHandler(SegurplanContext context) : base(context) {
        }

        public async Task<IRequestResponse<EditPlanGeneralDataResponse>> Handle(SavePlanRequest request, CancellationToken cancellationToken) {

            var result = SavePlanInformation(request);

            return result.Exception?.InnerExceptions.Count > 0 ? RequestResponse.Error<EditPlanGeneralDataResponse>() :  RequestResponse.Ok(new EditPlanGeneralDataResponse {
                PlanInformation = request.PlanInformation,
                AffiliatedCompanyList = await GetAffiliatedCompanyList(),
                DelegationList = await GetDelegationList(),
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
