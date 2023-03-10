using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segurplan.Web.Pages {
    public class IndexModel : PageModel {
        public IActionResult OnGet() {
            return new LocalRedirectResult("/Dashboard"); //"/Dashboard");
        }
    }
}
