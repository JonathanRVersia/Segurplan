using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Create;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Delete;
using Segurplan.Core.Actions.Administration.SubChapterDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Details;
using Segurplan.Core.Actions.Administration.SubChapterDetails.Save;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;
using Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ChapterDetails;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.SubChapterDetails {
    public class IndexModel : PageModel {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IndexModel(IMediator mediator, IMapper mapper) {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        //[BindProperty(SupportsGet = true)]
        //public string CreateSubChapterTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubChapterVersionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ChapterId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ChapterVersionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsEditMode { get; set; }

        [BindProperty]
        public SubChapterDetailsViewModel SubChapterVersion { get; set; }

        public ChapterActivitiesDeleteCheckModalModel DeleteCheck { get; set; }

        public async Task<IActionResult> OnGet() {
            if (SubChapterVersionId == 0 && !IsEditMode)
                return NotFound();

            //if (CreateSubChapterTitle != null)
            //    IsEditMode = true;

            return await LoadPage().ConfigureAwait(true);
        }

        public async Task<IActionResult> LoadPage() {

            if (SubChapterVersionId != 0) {
                var response = await mediator.Send(new SubChapterDetailsRequest { SubChapterversionId = this.SubChapterVersionId }).ConfigureAwait(true);

                if (response.Status != RequestStatus.Ok)
                    return new NoContentResult();

                SubChapterVersion = mapper.Map(response.Value.SubChapterVersion, SubChapterVersion);

                //if (SubChapterVersion.ProducedBy.Any())
                //    SubChapterVersion.ProducedByIds = SubChapterVersion.ProducedBy.Select(x => x.UserId);
            } else {
                var createResponse = await mediator.Send(new CreateSubChapterRequest { /*Title = CreateSubChapterTitle ,*/ChapterId=this.ChapterId,ChapterVersionId=this.ChapterVersionId}).ConfigureAwait(true);

                if (createResponse.Status != RequestStatus.Ok)
                    return new NoContentResult();

                SubChapterVersion = mapper.Map(createResponse.Value.SubChapterVersion, SubChapterVersion);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteSubChapterAsync(int subChapterToRemoveId) {
            var checkDeleteSubChapterResponse = await mediator.Send(new DeleteCheckSubChapterRequest { SubChapterId = subChapterToRemoveId }).ConfigureAwait(true);
            if (checkDeleteSubChapterResponse.Status != RequestStatus.NoContent) {
                DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                    ActivityHasPlan = checkDeleteSubChapterResponse.Value.ActivityHasPlansOrPreventiveMeasures,
                    SubChapterToRemoveIds=checkDeleteSubChapterResponse.Value.RiskPreventiveSubChapterIds
                };
                SubChapterVersionId = SubChapterVersion.Id;
                return await LoadPage().ConfigureAwait(true);
            }

            await mediator.Send(new DeleteSubChapterRequest { SubChapterId = subChapterToRemoveId }).ConfigureAwait(true);

            return RedirectToPage("/Models/Administration/ChaptersAndActivities/ChapterDetails/Index", new { ChapterVersionId = SubChapterVersion.IdChapterVersion, IsEditMode = true });
        }

        public async Task<IActionResult> OnPostAsync(string saveMode) {
            if (!ModelState.IsValid)
                return await LoadPage().ConfigureAwait(true);

            if (SubChapterVersion.Id != 0 && saveMode == nameof(SaveMode.AddActivity)) {
                return RedirectToPage("/Models/Administration/ChaptersAndActivities/ActivityDetails/Index", new { SubChapterId = SubChapterVersion.IdSubChapterNavigation.Id, SubChapterVersionId = SubChapterVersion.Id, IsEditMode = true });
            }

            if (SubChapterVersion.RemoveActivitiesIds != null) {
                var checkDeleteActivitiesResponse = await mediator.Send(new DeleteCheckActivityRequest { ActivityIds = SubChapterVersion.RemoveActivitiesIds.Split(",").Select(Int32.Parse).ToList() }).ConfigureAwait(true);
                if (checkDeleteActivitiesResponse.Status != RequestStatus.NoContent) {
                    DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                        ActivityHasPlan = checkDeleteActivitiesResponse.Value.ActivityHasPlansOrPreventiveMeasures
                    };
                    SubChapterVersionId = SubChapterVersion.Id;
                    return await LoadPage().ConfigureAwait(true);
                }
            }

            var saveSubChapterResponse = await mediator.Send(mapper.Map<SaveSubChapterRequest>(SubChapterVersion)).ConfigureAwait(true);

            if (saveSubChapterResponse.Status != RequestStatus.Ok) {
                //saveChapterResponse.ValidationResult?.AddToModelState(ModelState, mapper, () => Siniestro);
                //Message?

                return await LoadPage().ConfigureAwait(true);
            }

            if (SubChapterVersion.Id == 0) {
                SubChapterVersion.Id = saveSubChapterResponse.Value.SavedId;
                SubChapterVersion.IdSubChapterNavigation.Id = saveSubChapterResponse.Value.CreatedSubChapterId;
            }

            switch (Enum.Parse(typeof(SaveMode), saveMode)) {
                case SaveMode.Save:
                    SubChapterVersionId = SubChapterVersion.Id;
                    return RedirectToPage("/Models/Administration/ChaptersAndActivities/SubChapterDetails/Index", new { SubChapterversionId= SubChapterVersion.Id, IsEditMode = true });
                case SaveMode.SaveAndClose:
                    return LocalRedirect("~/ActivityList");
                case SaveMode.SaveAndNew:
                    ModelState.Clear();
                    SubChapterVersionId = 0;
                    ChapterId = SubChapterVersion.IdSubChapterNavigation.IdChapterNavigation.Id;
                    ChapterVersionId = SubChapterVersion.IdChapterVersion;
                    IsEditMode = true;
                    break;
                case SaveMode.AddActivity:
                    return RedirectToPage("/Models/Administration/ChaptersAndActivities/ActivityDetails/Index", new { SubChapterId = SubChapterVersion.IdSubChapterNavigation.Id, SubChapterVersionId = SubChapterVersion.Id, IsEditMode = true });
            }

            return await LoadPage().ConfigureAwait(true);
        }
    }
}
