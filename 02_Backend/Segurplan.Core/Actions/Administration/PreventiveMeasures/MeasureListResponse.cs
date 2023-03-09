using System.Collections.Generic;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures {
    public class MeasureListResponse {


        public List<ApplicationPreventiveMeasure> MeasureList { get; set; } = new List<ApplicationPreventiveMeasure>();

        public int TotalRows { get; set; }


        public MeasureListResponse(List<ApplicationPreventiveMeasure> measureList, int totalRows) {

            MeasureList = measureList;
            TotalRows = totalRows;
        }


    }
}
