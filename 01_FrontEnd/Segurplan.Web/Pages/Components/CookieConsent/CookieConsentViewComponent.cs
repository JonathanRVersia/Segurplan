using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.CookieConsent {
    public class CookieConsentViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
