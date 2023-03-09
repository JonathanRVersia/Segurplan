using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Web.Pages.Models.SafetyPlans.Models {
    public class PlansViewModels {

        public List<ApplicationPlaneFamily> AvailabePlanes { get; set; } = new List<ApplicationPlaneFamily>();

        public List<SafetyPlanPlane> SelectedPlanes { get; set; } = new List<SafetyPlanPlane>();
    }
}
