using MediatR;
using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.AllDocuments.Documents {
    public class CreateDocumentRequest : IRequest<IRequestResponse<CreateDocumentResponse>> {
        public CreateDocumentRequest(DocumentContent content, string templateName) {
            Content = content;
            TemplateName = templateName;
        }

        public CreateDocumentRequest(DocumentContent content, string templateName, string documentTitle) {
            Content = content;
            TemplateName = templateName;
            DocumentTitle = documentTitle;
        }

        public DocumentContent Content { get; }
        public string TemplateName { get; }
        public string DocumentTitle { get; set; }
    }
}
