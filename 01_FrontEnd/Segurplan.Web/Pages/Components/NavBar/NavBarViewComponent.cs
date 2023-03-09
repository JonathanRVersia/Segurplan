using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.NavBar {
    public class NavBarViewComponent : ViewComponent {

        private NavBarViewModel model = new NavBarViewModel();

        public IViewComponentResult Invoke(NavBarViewModel navBarViewModel) {
            if(navBarViewModel != null)                
                model = navBarViewModel;                    
            return View(model);
        }
    }
}
