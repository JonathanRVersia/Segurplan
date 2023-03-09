using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Segurplan.Web.Pages {

    [AllowAnonymous]
    public class ErrorModel : PageModel {

        public void OnGet() {

        }
    }
}
