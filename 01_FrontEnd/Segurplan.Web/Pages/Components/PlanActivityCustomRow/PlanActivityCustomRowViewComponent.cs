using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.PlanActivityCustomRow {
    public class PlanActivityCustomRowViewComponent : ViewComponent {


        public IViewComponentResult Invoke(PlanActivityCustomRowModel customRowModel) {

            return View(customRowModel);
        }
    }
}
