using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Seriousness.Delete;
using Segurplan.Core.Actions.Administration.Seriousness.Detail;
using Segurplan.Core.Actions.Administration.Seriousness.MatrixDataList;
using Segurplan.Core.Actions.Administration.Seriousness.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.SeriousnessList {
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class SeriousnessManagementModel : PageModel {

        public IMediator mediator;
        public ILogger<SeriousnessManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        public IRequestResponse<MatrixDataListResponse> MatrixData { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationSeriousness Seriousness { get; set; } = new ApplicationSeriousness();

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public ManagementAction ManagementAction { get; set; }

        public SeriousnessManagementModel(IMediator mediator, ILogger<SeriousnessManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }

        public async Task<IActionResult> OnGetCreateSeriousness() {
            CurrentOperation = AdministrationActionType.Read;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            MatrixData = await mediator.Send(new MatrixDataListRequest()).ConfigureAwait(true);
            return await GetOrCreateSeriousness(default).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnGetManageSeriousness(ManagementAction managementAction, int seriousnessId) {
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

            MatrixData = await mediator.Send(new MatrixDataListRequest()).ConfigureAwait(true);
            return await GetOrCreateSeriousness(seriousnessId).ConfigureAwait(true);
        }

        public async Task<IActionResult> GetOrCreateSeriousness(int seriousnessId) {
            if (seriousnessId == 0) {
                CurrentOperation = AdministrationActionType.Create;
                return Page();
            }

            var response = await mediator.Send(new GetSeriousnessDetailRequest { Id = seriousnessId }).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok)
                return new LocalRedirectResult("/Error");

            Seriousness = response.Value.Seriousness;

            return Page();
        }

        public async Task<IActionResult> OnPostSaveSeriousness() {
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var response = await mediator.Send(new SaveSeriousnessWithMatrixRequest {
                Seriousness = Seriousness,
                UserId = CurrentUserId
            }).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/SeriousnessList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteSeriousnessAsync(int deleteSeriousnessId) {
            var response = await mediator.Send(new DeleteSeriousnessRequest { Id = deleteSeriousnessId }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                return new LocalRedirectResult("/SeriousnessList");
            } else {
                return new LocalRedirectResult($"/SeriousnessList?handler=OnGetAsync&deleteErrors={true}");
                //return new LocalRedirectResult("/Error");
            }
        }
    }
}
