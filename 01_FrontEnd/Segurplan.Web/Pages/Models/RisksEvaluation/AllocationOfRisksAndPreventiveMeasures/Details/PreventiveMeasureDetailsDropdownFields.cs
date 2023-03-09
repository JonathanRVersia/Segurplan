using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Segurplan.Web.Pages.Models.RisksEvaluation.AllocationOfRisksAndPreventiveMeasures.Details {
    public class PreventiveMeasureDetailsDropdownFields {
        public List<SelectListItem> RiskOrderDropdownItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SubShapterDropdownItems { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ActivityDropdownItems { get; set; } = new List<SelectListItem>();
    }
}
