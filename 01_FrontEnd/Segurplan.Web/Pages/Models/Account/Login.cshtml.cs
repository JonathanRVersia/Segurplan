using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Users.CreateUserFromAD;
using Segurplan.Core.Actions.Administration.Users.Details;
using Segurplan.Core.Actions.Identity.Login;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.UserDetails;

namespace Segurplan.Web.Pages.Models.Account {
    [AllowAnonymous]
    public class LoginModel : PageModel {
        private readonly IMediator mediator;
        private readonly ILogger<LoginModel> logger;
        private readonly IMapper mapper;

        public string ReturnUrl { get; set; }

        [BindProperty]
        public LoginData DatosLogin { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(IMediator mediator, ILogger<LoginModel> logger, IMapper mapper) {
            this.mediator = mediator;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task OnGetAsync(string returnUrl = null) {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            ReturnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(true);
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null) {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid && DatosLogin != null) {

                var respuestaLogin = await mediator.Send(new LoginUserRequest() {
                    UserName = DatosLogin.UserName,
                    Password = DatosLogin.Password,
                    RememberMe = DatosLogin.RememberMe
                }).ConfigureAwait(true);

                if (respuestaLogin.Status == RequestStatus.Ok) {
                    logger.LogTrace("User logged in");
                    return LocalRedirect(returnUrl);
                }
                if (respuestaLogin.Value.SignInResult.IsLockedOut) {
                    logger.LogTrace("User account locked out.");
                    return LocalRedirect("~/Lockout");
                } else {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return LocalRedirect("~/Models/Account/LoginAccessDenied");
                }
            }
            return Page();
        }
    }
}
