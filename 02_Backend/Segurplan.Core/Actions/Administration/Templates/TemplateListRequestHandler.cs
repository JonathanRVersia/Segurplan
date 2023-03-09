using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.BusinessManagers;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateListRequestHandler : IRequestHandler<TemplateListRequest, IRequestResponse<TemplateListResponse>> {
        protected readonly TemplateDam templateDam;
        protected readonly UserDam usrDam;

        public TemplateListRequestHandler(TemplateDam templateDam, UserDam usrDam) {
            this.templateDam = templateDam;
            this.usrDam = usrDam;
        }

        public async Task<IRequestResponse<TemplateListResponse>> Handle(TemplateListRequest request, CancellationToken cancellationToken) {
            return await GetTemplateList(request);
        }

        private async Task<IRequestResponse<TemplateListResponse>> GetTemplateList(TemplateListRequest request) {
            try {
                var manager = new TemplateListManager(templateDam, usrDam);
                var templateList = manager.GetTemplateList(request.TableState, request.TableFilter);

                return RequestResponse.Ok(new TemplateListResponse(templateList, manager.FilteredTemplates));
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new TemplateListResponse(null, -1));
            }
        }
    }
}
