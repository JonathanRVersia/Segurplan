using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Segurplan.Web.Localization;

namespace Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal {
    public class ChapterActivitiesDeleteCheckModalViewComponent : ViewComponent {

        private readonly IStringLocalizer<SharedResource> localizer;

        public ChapterActivitiesDeleteCheckModalViewComponent(IStringLocalizer<SharedResource> localizer) {
            this.localizer = localizer;
        }

        public IViewComponentResult Invoke(ChapterActivitiesDeleteCheckModalModel deleteCheck) {

            if(deleteCheck!=null)
                CreateMessage(deleteCheck);

            return View(deleteCheck);
        }

        private void CreateMessage(ChapterActivitiesDeleteCheckModalModel model) {

            if (model.SubChapterToRemoveIds == null)
                model.SubChapterToRemoveIds = new List<int>();

            if (model.ChapterToRemoveId != null)
                model.Message = localizer["DeleteCheckModal.Chapter", model.ChapterToRemoveId]; //Cuando tienen medidas preventivas asignadas
            else if (model.SubChapterToRemoveIds.Any())
                model.Message = localizer["DeleteCheckModal.SubChapter", string.Join(",", model.SubChapterToRemoveIds.Select(n => n.ToString()).ToArray())];//Cuando tienen medidas preventivas asignadas
            else if (model.ActivityHasPlan)
                model.Message = localizer["DeleteCheckModal.Activity"];//Cuando tiene planes asignados
        }
    }
}
