using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Actions.Administration.PreventiveMeasures {
    public class MeasureManagementResponse {




        public MeasureManagementResponse(ApplicationPreventiveMeasure measureData, bool operationOk) {
            OperationOk = operationOk;
            MeasureData = measureData;
        }

        public bool OperationOk { get; set; }
        public ApplicationPreventiveMeasure MeasureData { get; set; }
    }
}
