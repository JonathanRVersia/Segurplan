using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.CultureSelector {
    public class CultureSelectorViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
