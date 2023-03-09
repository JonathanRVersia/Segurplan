using Segurplan.Core.Domain.Documents;

namespace Segurplan.Core.Actions.AllDocuments.Documents {
    public class CreateDocumentResponse {
        public CreateDocumentResponse(ProcesedDocument document) {
            Document = document;
        }

        public ProcesedDocument Document { get; }
    }
}
