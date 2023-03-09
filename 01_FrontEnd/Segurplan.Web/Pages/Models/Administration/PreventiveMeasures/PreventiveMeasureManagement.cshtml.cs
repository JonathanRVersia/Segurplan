using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.PreventiveMeasures;
using Segurplan.Core.Actions.Administration.PreventiveMeasures.GetPreventiveMeasureNewCode;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.PreventiveMeasures {
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class PreventiveMeasureManagementModel : PageModel {

        public IMediator mediator;
        public ILogger<PreventiveMeasureManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationPreventiveMeasure Measure { get; set; }

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public PreventiveMeasureManagementModel(IMediator mediator, ILogger<PreventiveMeasureManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }



        public async Task<IActionResult> OnGetCreateMeasure() {
            CurrentOperation = AdministrationActionType.Read;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            return await MeasureInformation(new ApplicationPreventiveMeasure(), CurrentOperation);
        }

        public async Task<IActionResult> OnGetViewMeasure(int measureId) {

            CurrentOperation = AdministrationActionType.Read;

            return await MeasureInformation(new ApplicationPreventiveMeasure { Id = measureId }, AdministrationActionType.Read);
        }

        public async Task<IActionResult> OnGetEditMeasure(int measureId) {

            CurrentOperation = AdministrationActionType.Update;

            return await MeasureInformation(new ApplicationPreventiveMeasure { Id = measureId }, AdministrationActionType.Read);
        }

        public async Task<IActionResult> OnPostSaveMeasure() {

            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            var response = await mediator.Send(new MeasureManagementRequest(Measure, CurrentOperation, CurrentUserId)).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/PreventiveMeasureList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteMeasureAsync(int deleteMeasureId) {
            var response = await mediator.Send(new MeasureManagementRequest(new ApplicationPreventiveMeasure { Id = deleteMeasureId }, AdministrationActionType.Delete)).ConfigureAwait(true);

            return new LocalRedirectResult($"/PreventiveMeasureList?handler=OnGetAsync&deleteErrors={!response.Value.OperationOk}");
        }

        private async Task<IActionResult> MeasureInformation(ApplicationPreventiveMeasure measure, AdministrationActionType op) {

            var response = await mediator.Send(new MeasureManagementRequest(measure, op)).ConfigureAwait(true);

            Measure = response.Value.MeasureData;
            if (Measure.Id == 0) {
                //creating a measure
                CurrentOperation = AdministrationActionType.Create;

                var codeResponse = await mediator.Send(new GetPreventiveMeasureNewCodeRequest()).ConfigureAwait(true);

                Measure.Code = codeResponse.Value.Code;
            }

            if (response != null) {
                return Page();
            }
            return new LocalRedirectResult("/Error");
        }
    }
}
