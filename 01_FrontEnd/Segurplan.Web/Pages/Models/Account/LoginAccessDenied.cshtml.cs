using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segurplan.Web.Pages.Models.Account {
    [AllowAnonymous]
    public class LoginAccessDenied : PageModel {
        public void OnGet() {

        }
    }
}
