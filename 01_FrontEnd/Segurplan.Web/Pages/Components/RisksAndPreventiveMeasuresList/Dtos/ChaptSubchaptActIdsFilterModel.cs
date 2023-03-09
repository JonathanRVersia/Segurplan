using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos {
    public class ChaptSubchaptActIdsFilterModel {
        public int ChapterId { get; set; }
        public int SubChapterId { get; set; }
        public int ActivityId { get; set; }
    }
}
