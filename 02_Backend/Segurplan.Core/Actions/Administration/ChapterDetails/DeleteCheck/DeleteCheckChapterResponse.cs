using System;
using System.Collections.Generic;
using System.Text;

namespace Segurplan.Core.Actions.Administration.ChapterDetails.DeleteCheck {
    public class DeleteCheckChapterResponse {
        public int? ChapterRiskPreventiveId { get; set; }
        //Cuando solo se puedan elegir los subcapitulos relativos a este capitulo en RiskPreventiveMeasures no hará falta
        public List<int> SubChapterRiskPreventiveIds { get; set; }

        public bool ActivityHasPlansOrPreventiveMeasures { get; set; }
    }
}
