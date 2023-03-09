using Segurplan.Core.BusinessObjects;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateManagementResponse {
        public TemplateManagementResponse(ApplicationTemplate templateData, bool operationOk) {
            OperationOk = operationOk;
            TemplateData = templateData;
        }

        public bool OperationOk { get; set; }
        public ApplicationTemplate TemplateData { get; set; }
    }
}
