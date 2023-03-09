using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Segurplan.Web.Pages.Components.Activities {
    public class ActivitiesViewComponent : ViewComponent {

        public async Task<IViewComponentResult> InvokeAsync(ActivitiesModel activitiesModel) {

            return View(activitiesModel);
        }
    }
}
