using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck {
    public class DeleteCheckSubChapterResponse {
        public List<int> RiskPreventiveSubChapterIds { get; set; }

        public bool ActivityHasPlansOrPreventiveMeasures { get; set; }
    }
}
