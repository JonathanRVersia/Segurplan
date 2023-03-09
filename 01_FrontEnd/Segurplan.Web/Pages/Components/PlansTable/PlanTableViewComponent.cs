using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.PlansTable {
    public class PlansTableViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
