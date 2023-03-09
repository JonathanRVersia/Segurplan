using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Segurplan.Core.Actions.Plans.PlanAditionalContent;
using Segurplan.Core.Extensions;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.Web {
    [Authorize(Roles = "Administrador, Usuario")]
    [ValidateAntiForgeryToken]
    public class TabDetallesModel : PageModel {
        public IMediator mediator;
        public ILogger logger;
        public string Response { get; set; }
        private UserManager<User> UserManager { get; set; }
        public TabDetallesModel(IMediator mediator, ILogger<TabDetallesModel> logger, UserManager<User> userManager) {
            this.mediator = mediator;
            this.logger = logger;
            UserManager = userManager;
        }
        [Ajax(HttpVerb = "GET")]
        [HttpGet("PlanID")]
        public async Task<IActionResult> OnGetAsync(string planID) {
            var h = HttpContext.Request;
            if (!h.IsAjax()) {
                return new LocalRedirectResult("~/Error");

            }

            var result = await mediator.Send(new PlanAditionalContentRequest() {
                PlanID = planID
            }).ConfigureAwait(true);
            Response = result.Value.Response;


            return Page();
        }
    }
}
