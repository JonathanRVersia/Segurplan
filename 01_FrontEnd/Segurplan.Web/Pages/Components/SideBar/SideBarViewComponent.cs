using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.SideBar {
    public class SideBarViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
