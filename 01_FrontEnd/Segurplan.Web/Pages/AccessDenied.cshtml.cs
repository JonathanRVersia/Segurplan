using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segurplan.Web.Pages {
    [Authorize(Roles = "Administrador, Usuario")]
    [AutoValidateAntiforgeryToken]
    public class AccessDeniedModel : PageModel {
        public void OnGet() {

        }
    }
}
