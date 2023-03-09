using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segurplan.Web.Pages {
    [Authorize(Roles = "Administrador, Usuario")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel {
        public IActionResult OnGet() {
            return new LocalRedirectResult("/MyPlans"); //"/Dashboard");
        }
    }
}
