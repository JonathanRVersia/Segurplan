using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Domain.Documents;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.AllDocuments.Documents {
    public class CreateDocumentHandler : IRequestHandler<CreateDocumentRequest, IRequestResponse<CreateDocumentResponse>> {
        private readonly DocumentProcessor documentProcessor;
        protected readonly TemplateDam templateDam;

        public CreateDocumentHandler(DocumentProcessor documentProcessor, TemplateDam templateDam) {
            this.documentProcessor = documentProcessor;
            this.templateDam = templateDam;
        }

        public async Task<IRequestResponse<CreateDocumentResponse>> Handle(CreateDocumentRequest request, CancellationToken cancellationToken) {
#if DEBUG
            var template = File.ReadAllBytes(Path.Combine("Templates\\", request.TemplateName + ".docx"));
            var processedDocument = await documentProcessor.ProcessDocument(request.Content, template, request.TemplateName);
#else
            var templateDetails = templateDam.SelectByTemplateName(request.TemplateName);

            if (templateDetails == null || templateDetails.FileData == null) {
                return RequestResponse.NotFound<CreateDocumentResponse>();
            }

            var template = templateDetails.FileData;
            var processedDocument = await documentProcessor.ProcessDocument(request.Content, template, request.TemplateName);
#endif
            return RequestResponse.Ok(new CreateDocumentResponse(processedDocument));
        }
    }
}
