using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Identity.Login;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Account {
    [AllowAnonymous]
    public class LoginModel : PageModel {
        private readonly IMediator mediator;
        private readonly ILogger<LoginModel> logger;

        public string ReturnUrl { get; set; }

        [BindProperty]
        public LoginData DatosLogin { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public LoginModel(IMediator mediator, ILogger<LoginModel> logger) {
            this.mediator = mediator;
            this.logger = logger;
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
