using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.Warning {
    public class WarningViewComponent : ViewComponent {
        public IViewComponentResult Invoke(WarningDTO WarningDTO) {
            return View(WarningDTO);
        }
    }
}
