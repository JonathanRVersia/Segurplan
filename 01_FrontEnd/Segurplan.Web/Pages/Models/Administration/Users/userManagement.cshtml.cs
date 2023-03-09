using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Users.CreateUserFromAD;
using Segurplan.Core.Actions.Administration.Users.Details;
using Segurplan.Core.Actions.Administration.Users.UpdateUser;
using Segurplan.Web.Localization;
using Segurplan.Web.Pages.Components.UserDetails;

namespace Segurplan.Web.Pages.Models.Administration.Users {

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class UserManagementModel : PageModel {

        private readonly IMediator mediator;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IMapper mapper;

        public bool EditMode { get; set; }

        [BindProperty(SupportsGet = true)]
        public AdministrationActionType CurrentOperation { get; set; }
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }
        [BindProperty]
        public UserDetailsViewComponentModel UserDetails { get; set; } = new UserDetailsViewComponentModel();

        public UserManagementModel(IMediator mediator, IStringLocalizer<SharedResource> localizer, IMapper mapper) {
            this.mediator = mediator;
            this.localizer = localizer;
            this.mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync() {

            UserDetails.Action = CurrentOperation;

            if (UserId > 0) {
                var response = await mediator.Send(new UserDetailsRequest { Id = UserId }).ConfigureAwait(true);

                UserDetails.UserDetailsModel = mapper.Map<UserDetailsModel>(response.Value);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUser() {

            var response = await mediator.Send(mapper.Map<UpdateUserRequest>(UserDetails.UserDetailsModel)).ConfigureAwait(true);
            if (UserDetails.UserDetailsModel.IsSuscribed != response.Value.IsSuscribed) {
                UserDetails.ErrorMsg = localizer["UserDetails.NotInAdError", response.Value.UserName].ToString();
            }
            UserDetails.UserDetailsModel = mapper.Map<UserDetailsModel>(response.Value);
            UserDetails.Action = AdministrationActionType.Update;
            return Page();
        }

        public async Task<IActionResult> OnPostCreateUser(string userName) {

            var response = await mediator.Send(new CreateUserFromADRequest { UserName = userName }).ConfigureAwait(true);

            if (response.Value.ExistsInDB || response.Value.NotExistsInAd) {
                UserDetails.UserDetailsModel.UserName = userName;
                UserDetails.Action = AdministrationActionType.Create;

                if(response.Value.ExistsInDB)
                    UserDetails.ErrorMsg = localizer["UserDetails.IsInDBError", userName].ToString();
                else
                    UserDetails.ErrorMsg = localizer["UserDetails.NotInAdError", userName].ToString();
            } else {
                UserDetails.UserDetailsModel = mapper.Map<UserDetailsModel>(response.Value);
                UserDetails.Action = AdministrationActionType.Update;
            }

            return Page();
        }
    }
}
