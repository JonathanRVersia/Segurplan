using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Segurplan.Core.Actions.Administration.ActivityDetails.Create;
using Segurplan.Core.Actions.Administration.ActivityDetails.Delete;
using Segurplan.Core.Actions.Administration.ActivityDetails.DeleteCheck;
using Segurplan.Core.Actions.Administration.ActivityDetails.Details;
using Segurplan.Core.Actions.Administration.ActivityDetails.GetRelatedChapSubChapActiv;
using Segurplan.Core.Actions.Administration.ActivityDetails.Models;
using Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.ActivityRelationsData;
using Segurplan.Core.Actions.Administration.ActivityDetails.RelatedActivities.SaveActivityRelations;
using Segurplan.Core.Actions.Administration.ActivityDetails.Save;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Components.ChapterActivitiesDeleteCheckModal;
using Segurplan.Web.Pages.Components.RisksAndPreventiveMeasuresList.Dtos;
using Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ChapterDetails;
using Segurplan.Web.Utils;

namespace Segurplan.Web.Pages.Models.Administration.ChaptersAndActivities.ActivityDetails {
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public IndexModel(IMediator mediator, IMapper mapper) {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int ActivityVersionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubchapterId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubChapterVersionId { get; set; }

        //[BindProperty(SupportsGet = true)]
        //public string CreateActivityTitle { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsEditMode { get; set; }

        [BindProperty]
        public ActivityDetailsViewModel ActivityVersion { get; set; }

        public List<ActivityRelationsModel> FilterData { get; set; } = new List<ActivityRelationsModel>();
        public List<ActivityRelationsModel> SelectedActivitiesData { get; set; } = new List<ActivityRelationsModel>();
        public ChapterActivitiesDeleteCheckModalModel DeleteCheck { get; set; }

        public async Task<IActionResult> OnGetAsync() {

            if (ActivityVersionId == 0 &&!IsEditMode)
                return NotFound();

            //if (CreateActivityTitle != null)
            //    IsEditMode = true;

            return await LoadPage().ConfigureAwait(true);
        }

        public async Task<IActionResult> LoadPage() {
            if (ActivityVersionId != 0) {
                var response = await mediator.Send(new ActivityDetailsRequest { ActivityVersionId = this.ActivityVersionId }).ConfigureAwait(true);

                if (response.Status != RequestStatus.Ok)
                    return new NoContentResult();

                ActivityVersion = mapper.Map(response.Value.ActivityVersion, ActivityVersion);

                var responseRelatedActivities = await mediator.Send(new GetRelatedChapSubChapActRequest { IdRelations = ActivityVersion.RelationsId ?? default(int)}).ConfigureAwait(true);

                FilterData = mapper.Map(responseRelatedActivities.Value.ActivityRealtionsList, FilterData);
            } else {
                //ChapterVersion = new ChapterDetailsViewModel();
                var createResponse = await mediator.Send(new CreateActivityRequest { /*Title = CreateActivityTitle, */SubChapterId = this.SubchapterId, SubChapterVersionId = this.SubChapterVersionId }).ConfigureAwait(true);

                if (createResponse.Status != RequestStatus.Ok)
                    return new NoContentResult();

                ActivityVersion = mapper.Map(createResponse.Value.ActivityVersion, ActivityVersion);
            }

            Parallel.Invoke(
            () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.ACTIVITY_VERSION, ActivityVersion)
            );

            //await FillDropdowns().ConfigureAwait(true);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteActivityAsync(int activityToRemoveId) {
            var checkDeleteActivityResponse = await mediator.Send(new DeleteCheckActivityRequest { ActivityId = ActivityVersion.IdActivity }).ConfigureAwait(true);
            if (checkDeleteActivityResponse.Status != RequestStatus.NoContent) {
                DeleteCheck = new ChapterActivitiesDeleteCheckModalModel {
                    ActivityHasPlan = checkDeleteActivityResponse.Value.ActivityHasPlansOrPreventiveMeasures
                };
                ActivityVersionId = ActivityVersion.Id;
                return await LoadPage().ConfigureAwait(true);
            }

            await mediator.Send(new DeleteActivityRequest { ActivityId = activityToRemoveId }).ConfigureAwait(true);

            return RedirectToPage("/Models/Administration/ChaptersAndActivities/SubChapterDetails/Index", new { SubChapterVersionId = ActivityVersion.IdSubChapterVersion, IsEditMode = true });
        }

        public async Task<IActionResult> OnPostAsync(string saveMode) {
            if (!ModelState.IsValid)
                return await LoadPage().ConfigureAwait(true);
            SelectedActivitiesData= (List<ActivityRelationsModel>)SessionHelper.GetListFromJson<ActivityRelationsModel>(HttpContext.Session, SessionHelper.ACTIVITY_SELECTED_DATA)?? new List<ActivityRelationsModel>();
            var saveActivityResponse = await mediator.Send(mapper.Map<SaveActivityRequest>(ActivityVersion)).ConfigureAwait(true);

            if (saveActivityResponse.Status != RequestStatus.Ok) {
                //saveChapterResponse.ValidationResult?.AddToModelState(ModelState, mapper, () => Siniestro);
                //Message?

                return await LoadPage().ConfigureAwait(true);
            }

            if (ActivityVersion.Id == 0)
                ActivityVersion.Id = saveActivityResponse.Value.SavedId;

            var response = await mediator.Send(new ActivityRelationsDataRequest { RelationsDataList = SelectedActivitiesData }).ConfigureAwait(true);
            
            if (response.Status == RequestStatus.Ok) {
                var selectedData = response.Value.ActivityRelationsList;
                var responseActivityRelationsSave = await mediator.Send(new SaveActivityRelationsRequest { RelationsDataList = selectedData, ActivityId = ActivityVersion.Id,RelationsId= ActivityVersion.RelationsId}).ConfigureAwait(true);
                if (responseActivityRelationsSave.Status != RequestStatus.Ok)
                    return new NoContentResult();
            }

            switch (Enum.Parse(typeof(SaveMode), saveMode)) {
                case SaveMode.Save:
                    ActivityVersionId = ActivityVersion.Id;
                    break;
                case SaveMode.SaveAndClose:
                    return LocalRedirect("~/ActivityList");
                case SaveMode.SaveAndNew:
                    ModelState.Clear();
                    ActivityVersionId = 0;
                    SubchapterId = ActivityVersion.IdActivityNavigation.SubChapterId;
                    SubChapterVersionId = ActivityVersion.IdSubChapterVersion;
                    IsEditMode = true;
                    break;
            }
            IsEditMode = true;
            return RedirectToAction("Index", new { activityVersionId = ActivityVersion.Id, IsEditMode = true });
        }

        public async Task<JsonResult> OnGetActivitiesAsync() {

            var response = await mediator.Send(new ViewPlanActivitiesRequest {

                PlanId = 0,
                GetAll = true

            }).ConfigureAwait(true);

            ActivityVersion = SessionHelper.GetObjectFromJson<ActivityDetailsViewModel>(HttpContext.Session, SessionHelper.ACTIVITY_VERSION);

            if (response.Status == RequestStatus.Ok) {
                DeleteExtraChapters(response.Value.ActivityLists.AvailableActivities);
                return new JsonResult(response.Value.ActivityLists);

            } else {
                return new JsonResult("error") { StatusCode = 404 };

            }
        }
        
        
        public void DeleteExtraChapters(List<PlanChapter> availableActivities) {
            var actId = ActivityVersion.IdActivity;

            foreach (var chapt in availableActivities) {
                foreach (var subchapt in chapt.SubChapter) {
                    subchapt.Activities.RemoveAll(x => x.Id == actId);
                }
            }

        }
        public async Task OnPostSaveActivityDetails(List<ActivityRelationsModel> selectedData) {
            
            Parallel.Invoke(
            () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.ACTIVITY_SELECTED_DATA, selectedData)
            );
            
        }        
    }
}
