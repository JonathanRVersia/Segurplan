using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Identity.Logout;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Account {
    [Authorize(Roles = "Administrador, Usuario")]
    [ValidateAntiForgeryToken]
    public class LogoutModel : PageModel {
        private readonly ILogger<LogoutModel> logger;
        private readonly IMediator mediator;

        public LogoutModel(ILogger<LogoutModel> logger, IMediator mediator) {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync() {
            var logout = await mediator.Send(new LogoutUserRequest()).ConfigureAwait(true);

            if (logout.Status == RequestStatus.Ok) {
                logger.LogTrace("User logged out.");
                return LocalRedirect("~/Models/Account/Login");
            }
            return Page();
        }
    }
}
