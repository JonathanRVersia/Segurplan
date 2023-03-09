using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Risks.Detail;
using Segurplan.Core.Actions.Administration.Risks.Delete;
using Segurplan.Core.Actions.Administration.Risks.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.Risks
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class RiskManagementModel : PageModel
    {
        public IMediator mediator;
        public ILogger<RiskManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationRisk Risk { get; set; } = new ApplicationRisk();

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public ManagementAction ManagementAction { get; set; }

        public RiskManagementModel(IMediator mediator, ILogger<RiskManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }
        public async Task<IActionResult> OnGetCreateRisk() {
            CurrentOperation = AdministrationActionType.Read;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            return await GetOrCreateRisk(default).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnGetManageRisk(ManagementAction managementAction, int riskId) {
            ManagementAction = managementAction;

            switch (managementAction) {
                case ManagementAction.View:
                    CurrentOperation = AdministrationActionType.Read;
                    break;
                case ManagementAction.Edit:
                    CurrentOperation = AdministrationActionType.Update;
                    break;
                default:
                    CurrentOperation = AdministrationActionType.Read;
                    break;

            }

            return await GetOrCreateRisk(riskId).ConfigureAwait(true);
        }

        public async Task<IActionResult> GetOrCreateRisk(int riskId) {
            if (riskId == 0) {
                CurrentOperation = AdministrationActionType.Create;                
                return Page();
            }

            var response = await mediator.Send(new RiskDetailRequest { Id = riskId }).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok)
                return new LocalRedirectResult("/Error");

            Risk = response.Value.Risk;

            return Page();
        }

        public async Task<IActionResult> OnPostSaveRisk() {
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var response = await mediator.Send(new SaveRiskRequest {
                risk = Risk,
                UserId = CurrentUserId
            }).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/RiskList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }
        public async Task<IActionResult> OnGetDeleteRisk(int deleteRiskId) {
            return await OnPostDeleteRiskAsync(deleteRiskId);
        }
        public async Task<IActionResult> OnPostDeleteRiskAsync(int deleteRiskId) {
            var response = await mediator.Send(new DeleteRiskRequest { Id = deleteRiskId }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                return new LocalRedirectResult("/RiskList");
            } else {
                return new LocalRedirectResult($"/RiskList?handler=OnGetAsync&deleteErrors={true}");                
            }
        }
    }
}
