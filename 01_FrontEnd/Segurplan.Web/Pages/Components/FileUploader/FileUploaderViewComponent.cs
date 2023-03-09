using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Segurplan.Core.BusinessObjects;

namespace Segurplan.Web.Pages.Components.FileUploader {

    [Authorize]
    public class FileUploaderViewComponent : ViewComponent {

        public FileUploaderModel FileDetails = new FileUploaderModel();

        public IViewComponentResult Invoke(List<PlanFile> files, string inputId, bool enabled, int planId, string destination) {

            files = files ?? new List<PlanFile>();

            FileDetails.Files = files;
            FileDetails.InputId = inputId;
            FileDetails.EditEnabled = enabled;
            FileDetails.PlanId = planId;
            FileDetails.Destination = destination;

            return View(FileDetails);
        }
    }
}
