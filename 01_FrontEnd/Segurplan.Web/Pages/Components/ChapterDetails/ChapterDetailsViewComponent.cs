using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.ChapterDetails {
    public class ChapterDetailsViewComponent : ViewComponent {

        public IViewComponentResult Invoke() {
            return View();
        }
    }
}
