using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.BusinessManagers;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.DataAccessManagers;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Core.Actions.Administration.Templates {
    public class TemplateManagementRequestHandler : IRequestHandler<TemplateManagementRequest, IRequestResponse<TemplateManagementResponse>> {
        private ApplicationTemplateManager manager;

        public TemplateManagementRequestHandler(TemplateDam templateDam, UserDam userDam) {
            manager = new ApplicationTemplateManager(templateDam, userDam);
        }

        public async Task<IRequestResponse<TemplateManagementResponse>> Handle(TemplateManagementRequest request, CancellationToken cancellationToken) {
            switch (request.CurrentOperation) {
                case AdministrationActionType.Create:
                    return await CreateTemplate(request);
                case AdministrationActionType.Read:
                    return TemplateInformation(request);
                case AdministrationActionType.Update:
                    return UpdateTemplateInformation(request);
                case AdministrationActionType.Delete:
                    return await DeleteTemplate(request);

                default:
                    return RequestResponse.NotOk(new TemplateManagementResponse(null, false));
            }
        }

        private async Task<IRequestResponse<TemplateManagementResponse>> CreateTemplate(TemplateManagementRequest request) {
            try {
                var operationOk = await manager.CreateTemplate(request.Template, request.CurrentUserId) > 0 ? true : false;
                return RequestResponse.Ok(new TemplateManagementResponse(request.Template, operationOk));
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new TemplateManagementResponse(null, false));
            }
        }

        private IRequestResponse<TemplateManagementResponse> TemplateInformation(TemplateManagementRequest request) {
            try {
                var response = new ApplicationTemplate() {
                    Id = 0,
                    Name = string.Empty,
                    Notes = string.Empty,
                    CreationDate = string.Empty,
                    ModifiedDate = string.Empty,
                    CreatedBy = 0
                };

                if (request.Template.Id == 0) {// new!!
                    return RequestResponse.Ok(new TemplateManagementResponse(response, true));
                }

                response = manager.TemplateData(request.Template.Id);
                return RequestResponse.Ok(new TemplateManagementResponse(response, response == null ? false : true));
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new TemplateManagementResponse(null, false));
            }
        }

        private IRequestResponse<TemplateManagementResponse> UpdateTemplateInformation(TemplateManagementRequest request) {
            try {
                var response = manager.UpdateTemplate(request.Template, request.CurrentUserId);
                var operationOk = response != null ? true : false;

                return RequestResponse.Ok(new TemplateManagementResponse(response, operationOk));
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new TemplateManagementResponse(null, false));
            }
        }

        private async Task<IRequestResponse<TemplateManagementResponse>> DeleteTemplate(TemplateManagementRequest request) {
            try {
                var response = await manager.DeleteTemplate(request.Template.Id);
                return RequestResponse.Ok(new TemplateManagementResponse(null, response));

            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return RequestResponse.NotOk(new TemplateManagementResponse(null, false));
            }
        }
    }
}
