using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Family.Delete;
using Segurplan.Core.Actions.Administration.Family.Detail;
using Segurplan.Core.Actions.Administration.Family.Save;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.FamilyArticle
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]

    public class ArticleFamilyManagementModel : PageModel
    {
        public IMediator mediator;
        public ILogger<ArticleFamilyManagementModel> logger;
        public UserManager<User> userManager;

        public int CurrentUserId { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationFamily Family { get; set; } = new ApplicationFamily();

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public ManagementAction ManagementAction { get; set; }

        public ArticleFamilyManagementModel(IMediator mediator, ILogger<ArticleFamilyManagementModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            this.userManager = userManager;

        }

        public async Task<IActionResult> OnGetCreateFamily() {
            CurrentOperation = AdministrationActionType.Read;
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
            return await GetOrCreateFamily(default).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnGetManageFamily(int familyId, ManagementAction managementAction) {
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
            return await GetOrCreateFamily(familyId).ConfigureAwait(true);
        }

        public async Task<IActionResult> GetOrCreateFamily(int familyId) {
            if (familyId == 0) {
                CurrentOperation = AdministrationActionType.Create;
                Family.CreateDate = DateTime.Now;
                Family.UpdateDate = DateTime.Now;
                return Page();
            }

            var response = await mediator.Send(new FamilyDetailRequest { Id = familyId }).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok)
                return new LocalRedirectResult("/Error");

            Family = response.Value.Family;

            return Page();
        }

        public async Task<IActionResult> OnPostSaveFamily() {
            CurrentUserId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            var response = await mediator.Send(new SaveFamilyRequest {
                family = Family,
                UserId = CurrentUserId
            }).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/ArticleFamilyList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnGetDeleteFamily(int deleteFamilyId) {
            return await OnPostDeleteFamilyAsync(deleteFamilyId);
        }

        public async Task<IActionResult> OnPostDeleteFamilyAsync(int deleteFamilyId) {
            var response = await mediator.Send(new DeleteFamilyRequest { Id = deleteFamilyId }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                return new LocalRedirectResult("/ArticleFamilyList");
            } else {
                return new LocalRedirectResult($"/ArticleFamilyList?handler=OnGetAsync&deleteErrors={true}");
            }
        }




    }
}
