using Microsoft.AspNetCore.Mvc;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;

namespace Segurplan.Web.Pages.Components.ChapterVersionModalTable {
    public class ChapterVersionModalTableViewComponent : ViewComponent {

        public IViewComponentResult Invoke(ChapterVersionModalTableModel modalList) {

            return View(modalList);
        }
    }
}
