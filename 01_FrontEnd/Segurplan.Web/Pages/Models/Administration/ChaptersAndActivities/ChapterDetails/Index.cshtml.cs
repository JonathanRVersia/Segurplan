using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Actions.Administration.ChapterDetails.Create;
using Segurplan.Core.Actions.Administration.ChapterDetails.CreateVersion;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteChapterVersionCheck;
using Segurplan.Core.Actions.Administration.ChapterDetails.DeleteVersion;
using Segurplan.Core.Actions.Administration.ChapterDetails.DetailDropdowns;
using Segurplan.Core.Actions.Administration.ChapterDetails.Details;
using Segurplan.Core.Actions.Administration.ChapterDetails.GetChapterVersions;
using Segurplan.Core.Actions.Administration.ChapterDetails.GetLastChapterVersionId;
using Segurplan.Core.Actions.Administration.ChapterDetails.Models;
using Segurplan.Core.Actions.Administration.ChapterDetails.ReorderChapterVersions;
using Segurplan.Core.Actions.Administration.ChapterDetails.Save;
using Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck;
using Segurplan.Core.Database;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;
using Segurplan.Web.Pages.Components.ChapterVersionModalTable;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ChapterDetails {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly SegurplanContext context;

        public IndexModel(IMediator mediator, IMapper mapper, SegurplanContext context) {
            this.mediator = mediator;
            this.mapper = mapper;
            this.context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ChapterVersionId { get; set; }

        //User when new version is created
        [BindProperty(SupportsGet = true)]
        public int ChapterId { get; set; }

        //[BindProperty(SupportsGet = true)]
        //public string CreateChapterTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsEditMode { get; set; }

        [BindProperty]
        public ChapterDetailsViewModel ChapterVersion { get; set; }

        public ChapterDetailsDropdownResponse Dropdowns { get; set; }

        public ChapterActivitiesDeleteCheckModalModel DeleteCheck { get; set; }

        public async Task<IActionResult> OnGetAsync() {

            if (ChapterVersionId == 0 && !IsEditMode && ChapterId == 0)
                return NotFound();

            //if (CreateChapterTitle != null)
            //    IsEditMode = true;

            return await LoadPage().ConfigureAwait(true);
        }

        public async Task<IActionResult> LoadPage() {

            var createChapter = false;
            if (ChapterId != 0) {
                var chapterBorrador = await context.ChapterVersion.Where(x => x.IdChapter == ChapterId && (x.ApprovementDate == null || x.ApprovementDate > DateTime.Now)).FirstOrDefaultAsync();

                if (chapterBorrador == null) {
                    createChapter = true;
                } else {
                    ChapterVersionId = chapterBorrador.Id;
                }
            }

            if (ChapterVersionId != 0) {
                var response = await mediator.Send(new ChapterDetailsRequest { ChapterversionId = this.ChapterVersionId }).ConfigureAwait(true);

                //When version is deleted on List and then is opened we look for last version available
                if (response.Status == RequestStatus.NoContent) {
                    var lastVersionIdResponse = await mediator.Send(new GetLastChapterVersionIdRequest { ChapterId = this.ChapterId }).ConfigureAwait(true);
                    ChapterVersionId = lastVersionIdResponse.Value.ChapterVersionId;
                    return await LoadPage().ConfigureAwait(true);
                }

                if (response.Status != RequestStatus.Ok)
                    return new NoContentResult();

                ChapterVersion = mapper.Map(response.Value.ChapterVersion, ChapterVersion);

                if (ChapterVersion.ProducedBy.Any())
                    ChapterVersion.ProducedByIds = ChapterVersion.ProducedBy.Select(x => x.UserId);

            } else if (createChapter) {
                //Reorder de chapterVersion
                await mediator.Send(new ReorderChapterVersionRequest { ChapterId = this.ChapterId, ChapterVersionId = this.ChapterVersionId });

                var changedEntriesCopy = context.ChangeTracker.Entries().ToList();
                foreach (var entry in changedEntriesCopy)
                    entry.State = EntityState.Detached;

                var response = await mediator.Send(new CreateVersionRequest { ChapterId = this.ChapterId }).ConfigureAwait(true);

                if (response.Status != RequestStatus.Ok)
                    return new NoContentResult();

                ChapterVersion = mapper.Map(response.Value.ChapterVersion, ChapterVersion);

                if (ChapterVersion.ProducedBy.Any())
                    ChapterVersion.ProducedByIds = ChapterVersion.ProducedBy.Select(x => x.UserId);

            } else {
                var createResponse = await mediator.Send(new CreateChapterRequest()).ConfigureAwait(true);

                if (createResponse.Status != RequestStatus.Ok)
                    return new NoContentResult();

                ChapterVersion = mapper.Map(createResponse.Value.ChapterVersion, ChapterVersion);
            }

            await FillDropdowns().ConfigureAwait(true);

            CheckTypeOfVersion();

            return Page();
        }

        private void CheckTypeOfVersion() {
            if (ChapterVersion.ApprovementDate == null)
                ChapterVersion.VersionType = ChapterTypeVersionsEnum.DraftVersion;
            else if (ChapterVersion.EndDate < DateTime.Now || ChapterVersion.ApprovementDate > DateTime.Now)
                ChapterVersion.VersionType = ChapterTypeVersionsEnum.NotCurrentVersion;
            else
                ChapterVersion.VersionType = ChapterTypeVersionsEnum.CurrentVersion;
        }

        public async Task FillDropdowns() {
            var dropdownsResponse = await mediator.Send(new ChapterDetailsDropdownRequest()).ConfigureAwait(true);

            if (dropdownsResponse.Status == RequestStatus.Ok) {
                Dropdowns = mapper.Map(dropdownsResponse.Value, Dropdowns);
            }
        }

        public async Task<IActionResult> OnPostAsync(string saveMode) {
            if (!ModelState.IsValid)
                return await LoadPage().ConfigureAwait(true);

            if (ChapterVersion.Id != 0 && saveMode == nameof(SaveMode.AddSubChapter)) {
                return RedirectToPage("/Models/Administration/ChaptersAndActivities/SubChapterDetails/Index", new { ChapterId = ChapterVersion.IdChapterNavigation.Id, ChapterVersionId = ChapterVersion.Id, IsEditMode = true });
            }

            AddProducedBy();

            if (ChapterVersion.RemoveSubChapterIds != null) {
                var checkDeleteSubChaptersResponse = await mediator.Send(new DeleteCheckSubChapterRequest { SubChapterIds = ChapterVersion.RemoveSubChapterIds.Split(",").Select(Int32.Parse).ToList() }).ConfigureAwait(true);
                if (checkDeleteSubChaptersResponse.Status != RequestStatus.NoContent) {
                    DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                        ActivityHasPlan = checkDeleteSubChaptersResponse.Value.ActivityHasPlansOrPreventiveMeasures,
                        SubChapterToRemoveIds = checkDeleteSubChaptersResponse.Value.RiskPreventiveSubChapterIds
                    };
                    ChapterVersionId = ChapterVersion.Id;
                    return await LoadPage().ConfigureAwait(true);
                }
            }

            var saveChapterResponse = await mediator.Send(mapper.Map<SaveChapterRequest>(ChapterVersion)).ConfigureAwait(true);

            if (saveChapterResponse.Status != RequestStatus.Ok) {
                //saveChapterResponse.ValidationResult?.AddToModelState(ModelState, mapper, () => Siniestro);
                //Message?

                return await LoadPage().ConfigureAwait(true);
            }

            if (ChapterVersion.Id == 0) {
                ChapterVersion.Id = saveChapterResponse.Value.SavedId;
                ChapterVersion.IdChapter = saveChapterResponse.Value.CreatedChapterId;
            }

            switch (Enum.Parse(typeof(SaveMode), saveMode)) {
                case SaveMode.Save:
                    ChapterVersionId = ChapterVersion.Id;
                    return RedirectToPage("/Models/Administration/ChaptersAndActivities/ChapterDetails/Index", new { ChapterVersionId = ChapterVersion.Id, IsEditMode = true });
                case SaveMode.SaveAndClose:
                    return LocalRedirect("~/ActivityList");
                case SaveMode.SaveAndNew:
                    ModelState.Clear();
                    ChapterVersionId = 0;
                    ChapterVersion.ProducedByIds = null;
                    IsEditMode = true;
                    break;
                case SaveMode.AddSubChapter:
                    return RedirectToPage("/Models/Administration/ChaptersAndActivities/SubChapterDetails/Index", new { ChapterId = ChapterVersion.IdChapter, ChapterVersionId = ChapterVersion.Id, IsEditMode = true });
            }

            return await LoadPage().ConfigureAwait(true);
        }

        private void AddProducedBy() {
            if (ChapterVersion.ProducedBy == null)
                ChapterVersion.ProducedBy = new List<UserChapterDetails>();

            if (ChapterVersion.ProducedByIds != null) {
                foreach (var userId in ChapterVersion.ProducedByIds) {
                    ChapterVersion.ProducedBy.Add(new UserChapterDetails { ChapterVersionId = ChapterVersion.Id, UserId = userId });
                }
            }

        }

        public async Task<IActionResult> OnPostDeleteChapterVersionAsync(int versionId, int showingVersionId, int chapterId) {

            var checkResponse = await mediator.Send(new DeleteChapterVersionCheckRequest { ChapterVersionId = versionId }).ConfigureAwait(true);

            if (checkResponse.Status != RequestStatus.NoContent) {
                DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                    ActivityHasPlan = checkResponse.Value.ActivityHasPlansOrPreventiveMeasures,
                    SubChapterToRemoveIds = checkResponse.Value.SubChapterRiskPreventiveIds,
                    ChapterToRemoveId = checkResponse.Value.ChapterRiskPreventiveId
                };
            } else {
                var deleteResponse = await mediator.Send(new DeleteVersionRequest { VersionId = versionId }).ConfigureAwait(true);

                if (deleteResponse.Status != RequestStatus.Ok)
                    return null;
            }

            var getResponse = await mediator.Send(new GetChapterVersionsRequest { ChapterId = chapterId }).ConfigureAwait(true);

            var modalList = new ChapterVersionModalTableModel { ChapterVersions = getResponse.Value.ChapterVersions, DeleteCheck = this.DeleteCheck };

            modalList.ChapterVersions.Remove(modalList.ChapterVersions.Where(x => x.Id == showingVersionId).FirstOrDefault());

            return new ViewComponentResult() {
                ViewComponentName = "ChapterVersionModalTable",
                Arguments = new {
                    modalList
                },
                ViewData = this.ViewData,
                TempData = this.TempData
            };
        }
    }
}
