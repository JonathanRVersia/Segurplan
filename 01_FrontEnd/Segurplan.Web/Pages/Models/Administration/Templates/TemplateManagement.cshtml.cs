using System;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Segurplan.Core.Actions.Administration;
using Segurplan.Core.Actions.Administration.Templates;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.Download;
using Segurplan.Core.BusinessObjects;
using Segurplan.FrameworkExtensions.MediatR;

namespace Segurplan.Web.Pages.Models.Administration.Templates {
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class TemplateManagementModel : PageModel {

        private readonly IMediator mediator;

        public int CurrentUserId { get; private set; }
        public bool EditMode { get; set; }

        [BindProperty]
        public ApplicationTemplate Template { get; set; }

        [BindProperty]
        public AdministrationActionType CurrentOperation { get; set; }

        public TemplateManagementModel(IMediator mediator) {
            this.mediator = mediator;
        }

        public async Task<IActionResult> OnGetCreateTemplate() {
            CurrentOperation = AdministrationActionType.Read;
            CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier), CultureInfo.CurrentCulture);
            return await TemplateInformation(new ApplicationTemplate(), CurrentOperation).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnGetViewTemplate(int templateId) {
            CurrentOperation = AdministrationActionType.Read;
            return await TemplateInformation(new ApplicationTemplate { Id = templateId }, AdministrationActionType.Read).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnGetEditTemplate(int templateId) {
            CurrentOperation = AdministrationActionType.Update;
            return await TemplateInformation(new ApplicationTemplate { Id = templateId }, AdministrationActionType.Read).ConfigureAwait(true);
        }

        public async Task<IActionResult> OnPostSaveTemplate() {
            if (Template.DeleteExistingFile) {
                Template.FileData = null;
                Template.FileSize = 0;
            } else {
                if(Template.Archivo != null) {
                    using (var ms = new MemoryStream()) {
                        Template.Archivo.CopyTo(ms);
                        Template.FileData = ms.ToArray();
                    }
                }
            }                       

            CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier), CultureInfo.CurrentCulture);
            var response = await mediator.Send(new TemplateManagementRequest(Template, CurrentOperation, CurrentUserId)).ConfigureAwait(true);
            if (response.Status == RequestStatus.Ok) {
                return LocalRedirect("/TemplateList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteTemplateAsync(int deleteTemplateId) {
            var response = await mediator.Send(new TemplateManagementRequest(new ApplicationTemplate { Id = deleteTemplateId }, AdministrationActionType.Delete)).ConfigureAwait(true);
            if (response.Value.OperationOk) {
                return new LocalRedirectResult("/TemplateList");
            } else {
                return new LocalRedirectResult("/Error");
            }
        }

        private async Task<IActionResult> TemplateInformation(ApplicationTemplate template, AdministrationActionType op) {
            var response = await mediator.Send(new TemplateManagementRequest(template, op)).ConfigureAwait(true);

            Template = response.Value.TemplateData;
            if (Template.Id == 0) {
                //creating a template
                CurrentOperation = AdministrationActionType.Create;
            }

            if(Template.FilePath != null) {
                Template.FileDetails = new PlanFile {
                    Id = Template.Id,
                    Name = Template.FilePath,
                    Data = Template.FileData,
                    DataLength = Template.FileSize,
                    FileSize = GetSize(Template.FileSize)
                };
            }            

            if (response != null) {
                return Page();
            }
            return new LocalRedirectResult("/Error");
        }

        public async Task<IActionResult> OnGetDownloadFileAsync(int fileId, int planId, bool defaultFile) {
            var file = await mediator.Send(new DownloadTemplateRequest() {
                FileId = fileId,
                PlanId = planId,
                DefaultFile = defaultFile

            }).ConfigureAwait(true);

            if (file.Status == RequestStatus.Ok)
                return File(file.Value.File.FileData, MediaTypeNames.Application.Octet, file.Value.File.FilePath);
            else
                return new NoContentResult();
        }

        private string GetSize(decimal dataLength) {
            var fileSize = "0 Byte";
            double bytes = Decimal.ToDouble(dataLength);

            var sizes = new string[5] { "Bytes", "KB", "MB", "GB", "TB" };

            if (bytes != 0) {
                var i = Convert.ToInt32(Math.Floor(Math.Log(bytes)) / Math.Log(1024));

                fileSize = $"{Math.Round(bytes / Math.Pow(1024, i), 0)}  {sizes[i]}";
            }

            return fileSize;
        }
    }
}
