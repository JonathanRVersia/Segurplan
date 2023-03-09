using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.SelectedActivitiesPlanList {
    public class SelectedActivitiesPlanListViewComponent : ViewComponent {

        public IViewComponentResult Invoke(SelectedActivitiesPlanListsModel model) {
            return View(model);
        }
    }
}
