using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using Segurplan.Core.Actions.Plans;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Core.Actions.Plans.PlanManagement.Create;
using Segurplan.Core.Actions.Plans.PlanManagement.Delete;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Duplicate;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.Edit;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View;
using Segurplan.Core.Actions.Plans.PlanManagement.Update;
using Segurplan.Core.BusinessObjects;
using Segurplan.DataAccessLayer.Database.Identity;
using Segurplan.FrameworkExtensions.MediatR;
using Segurplan.Web.Pages.Models.SafetyPlans.Models;
//using Segurplan.Web.Pages.Models.SafetyPlans.ActivityPlan;
using Segurplan.Web.Utils;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.Download;
using System.Net.Mime;
using Segurplan.Core.Actions.Plans.PlanManagement.Files.View;
using Segurplan.Core.Actions.Plans.PlanManagement.Create.Plane;
using Segurplan.Core.Actions.Plans.PlanManagement.Read.View.PlanesDropdowns;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using Segurplan.Core.Actions.Plans.PlanManagement.Generate.Plan;
using Segurplan.Core.Actions.Plans.PlanManagement.Dropdowns.UpdateAffiliatedDependantData;
using Segurplan.Core.Actions.Plans.PlanManagement.CreatePlanCustomChapters;
using Segurplan.Core.Actions.Plans.PlanManagement.GetCustomPlanChapters;
using AutoMapper;
using Segurplan.Core.Actions.Plans.PlanManagement.UpdatePlanCustomChapters;
using Segurplan.Web.Pages.Components.PlanActivityCustomModals;
using Segurplan.Core.Actions.Plans.PlanManagement.DeletePlanCustomChapters;
using Segurplan.Core.Actions.Plans.PlanManagement.DefaultValues;
using Segurplan.Core.Actions.Administration.ChapterDetails.CheckChapterVersion;
using Segurplan.Web.Pages.Components.SelectedActivitiesPlanList;
using Segurplan.Web.Pages.Models.Budget;
using Segurplan.Core.Actions.Plans.PlansData;
//using Segurplan.Core.Actions.Plans.PlanManagement.Read.View;

namespace Segurplan.Web.Pages.Models.SafetyPlans {

    [ValidateAntiForgeryToken]
    public class PlanManagementModel : PageModel {

        #region Private members

        private readonly IMediator mediator;
        //private readonly ILogger<PlanManagementModel> logger;
        private readonly IMapper mapper;
        public UserManager<User> userManager;

        #endregion

        #region BindProperties

        [BindProperty]
        public SafetyPlan Plan { get; set; }

        [BindProperty]
        public List<PlanChapter> AvailableActivities { get; set; } = new List<PlanChapter>();

        [BindProperty]
        public List<PlanChapter> SelectedActivities { get; set; } = new List<PlanChapter>();

        [BindProperty]
        public List<PlanChapter> CustomSelectedChapters { get; set; } = new List<PlanChapter>();

        [BindProperty]
        public List<ApplicationArticle> SelectedArticles { get; set; } = new List<ApplicationArticle>();

        [BindProperty]
        public List<ApplicationArticle> SelectedArticlesDB { get; set; } = new List<ApplicationArticle>();

        [BindProperty]
        public ApplicationBudget Budget { get; set; } = new ApplicationBudget();

        [BindProperty]
        public string SelectedActivitiesFilter { get; set; }

        [BindProperty]
        public string AvaiableActivitiesFilter { get; set; }

        [BindProperty]
        public List<ApplicationPlaneFamily> AvailabePlanes { get; set; } = new List<ApplicationPlaneFamily>();
        [BindProperty]
        public List<ApplicationArticleFamily> ArticlesByFamily { get; set; } = new List<ApplicationArticleFamily>();
        [BindProperty]
        public List<ApplicationTask> ArticlesByTask { get; set; } = new List<ApplicationTask>();
        [BindProperty]
        public List<SafetyPlanPlane> SelectedPlanes { get; set; } = new List<SafetyPlanPlane>();

        public List<int> NotCurrentChapterNumbers { get; set; } = new List<int>();

        private PlanTab currentTab;

        [BindProperty]
        public string CurrentTab {
            get { return currentTab.ToString(); }
            set { currentTab = (PlanTab)Enum.Parse(typeof(PlanTab), value, true); }
        }

        [BindProperty]
        public string PrevPage { get; set; }


        #endregion

        #region Properties

        public List<PlanAffiliatedCompany> AffiliatedList { get; set; }

        public List<PlanDelegation> DelegationList { get; set; }

        public List<PlanTemplate> TemplateList { get; set; }

        public List<PlanCustomer> CustomerList { get; set; }

        public List<PlanBusinessAddress> BusinessAddressList { get; set; }

        public List<PlanGeneralActivity> GeneralActivityList { get; set; }

        public List<UserProfile> ProfileList { get; set; }

        public List<ApplicationUser> UserList { get; set; }

        public PlanActionType CurrentOperation { get; private set; }

        public bool editEnabled => CurrentOperation == PlanActionType.Create ||
                        (RightsController.CanEditPlan(Plan.CreatedBy, Convert.ToInt32(userManager.GetUserId(HttpContext.User)))
                        &&
                        CurrentOperation == PlanActionType.Edit);

        public bool IsActivitiesEditEnabled { get; set; }

        public ValidationResult BackValidationResultMsg { get; set; }

        public List<SelectListItem> PlaneFamilies { get; private set; } = new List<SelectListItem>();
        public List<PlanDetailsDefaultValuesDto> PlanDetailsDefaultData { get; set; }

        #endregion

        #region Lifetime

        public PlanManagementModel(IMediator mediator, /*ILogger<PlanManagementModel> logger,*/ UserManager<User> userManager, IMapper mapper) {
            this.mediator = mediator;
            //this.logger = logger;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        #endregion

        #region Get methods

        public async Task<IActionResult> OnGetAsync(int id, PlanActionType op, string title, string prevPage) {

            SessionHelper.RemoveActivities(HttpContext.Session);

            CurrentOperation = op;
            PrevPage = prevPage;

            switch (op) {
                case PlanActionType.View:

                    var response = await mediator.Send(new ViewPlanGeneralDataRequest(id)).ConfigureAwait(true);

                    if (response.Status == RequestStatus.Ok) {
                        Plan = response.Value.PlanInformation;
                        Plan.DuplicatedPlanTitle = "N/A";
                    } else {
                        throw new Exception($"Error en OnGet View de PlanManagement con Id {id}");
                    }
                    break;
                case PlanActionType.Edit:
                    var editResponse = await mediator.Send(new EditPlanGeneralDataRequest(id)).ConfigureAwait(true);

                    if (editResponse.Status == RequestStatus.Ok) {
                        LoadEditPlanInformation(editResponse.Value.PlanInformation, editResponse.Value.DelegationList, editResponse.Value.AffiliatedCompanyList,
                            editResponse.Value.CustomerList, editResponse.Value.TemplateList, editResponse.Value.BusAddList,
                            editResponse.Value.GenActList, editResponse.Value.ProfileList, editResponse.Value.UserList);
                    } else {
                        throw new Exception($"Error en OnGet Edit de PlanManagement con Id {id}");
                    }
                    break;
                case PlanActionType.Duplicate:
                    var userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));
                    var result = await DuplicatePlanAsync(Convert.ToInt32(id), title, userId).ConfigureAwait(true);

                    if (result == RequestStatus.Error)
                        throw new Exception($"Error en OnGet Duplicate de PlanManagement con Id {id}");

                    break;
                default:
                    throw new Exception("Error en OnGet de PlanManagement, no ha entrado por ningún case del switch");
            }

            await GetPlanesFamiliesAsync().ConfigureAwait(true);
            await GetPlanDetailsDefaultValuesAsync().ConfigureAwait(true);

            if (id != 0) {
                var checkChapterVersionResponse = await mediator.Send(new CheckChapterVersionRequest { PlanId = id }).ConfigureAwait(true);

                if (checkChapterVersionResponse.Status == RequestStatus.Ok) {
                    NotCurrentChapterNumbers = checkChapterVersionResponse.Value.NotCurrentChapterNumbers;
                }
            }

            return Page();
        }

        private async Task GetPlanDetailsDefaultValuesAsync() {
            var response = await mediator.Send(new GetPlanDetailsDefaultValuesRequest()).ConfigureAwait(true);

            if (response.Status != RequestStatus.Ok) {
                return;
            }

            PlanDetailsDefaultData = response.Value.DefaultValues;

            if (CurrentOperation == PlanActionType.Create) {
                Plan.AdditionalData.AffectedServicesDescription = PlanDetailsDefaultData.Find(x => x.PropName == DefaultValuePropNameList.AffectedServicesFalse.ToString()).Content;
                Plan.AdditionalData.EmergencyPlanDescription = PlanDetailsDefaultData.Find(x => x.PropName == DefaultValuePropNameList.IdEmergencyPlanTypeCorto.ToString()).Content;
            }
        }

        /// <summary>
        /// Function to Get the Activities of plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetActivitiesAsync(int planId, bool isEditEnabled) {

            // Checking whether lists are stored in session variables
            List<SelectedPlanActivity> selList = new List<SelectedPlanActivity>();

            IsActivitiesEditEnabled = isEditEnabled;

            if (AvailableActivities == null || AvailableActivities.Count == 0) {

                var response = await mediator.Send(new ViewPlanActivitiesRequest {

                    PlanId = planId,
                    GetAll = true

                }).ConfigureAwait(true);

                if (response.Status == RequestStatus.Ok) {

                    AvailableActivities = response.Value.ActivityLists.AvailableActivities;
                    selList = response.Value.ActivityLists.PlanActivities;
                }
            }

            if (planId == 0) {

                List<SelectedPlanActivity> selectedIds = new List<SelectedPlanActivity>();
                foreach (var id in AvailableActivities.Where(planChapt => planChapt.DefaultSelectedChapter).SelectMany(planChapt => planChapt.SubChapter.SelectMany(x => x.Activities.Select(y => y.Id))).ToList()) {

                    selectedIds.Add(new SelectedPlanActivity { Id = Convert.ToInt32(id) });
                }
                await AddSelectedActivities(selectedIds).ConfigureAwait(true);
                //GetSelectedActivities(selList, AvailableActivities);
            }

            if (selList != null && selList.Count > 0) {

                await AddSelectedActivities(selList).ConfigureAwait(true);
            }

            return Page();
        }

        public async Task<JsonResult> OnGetPlanesAsync(int planId) {

            var response = await mediator.Send(new ViewPlanPlanesRequest { PlanId = planId }).ConfigureAwait(true);

            AvailabePlanes = response.Value.PlaneList;
            SelectedPlanes = response.Value.PlanPlaneList;

            SelectedPlanes = SelectedPlanes.OrderBy(x => x.Position).ToList();

            return new JsonResult(new PlansViewModels { AvailabePlanes = this.AvailabePlanes, SelectedPlanes = this.SelectedPlanes });
        }
        public async Task<JsonResult> OnGetBudgetsAsync(int planId, int durationWork, int numberWorkers, int budgetId) {

            var response = await mediator.Send(new ViewPlanBudgetRequest { PlanId = planId, DurationWork = durationWork, NumberWorkers = numberWorkers, BudgetId = budgetId }).ConfigureAwait(true);

            ArticlesByFamily = response.Value.ArticlesFamily;
            ArticlesByTask = response.Value.ArticlesByTask;
            SelectedArticlesDB = response.Value.SelectedArticlesDB;

            SelectedPlanes = SelectedPlanes.OrderBy(x => x.Position).ToList();

            var applicablePercentage = response.Value.ApplicablePercentage;

            return new JsonResult(new BudgetViewModel { ArticlesByFamily = this.ArticlesByFamily, ArticlesByTask = this.ArticlesByTask, SelectedArticlesDB = this.SelectedArticlesDB, ApplicablePercentage = applicablePercentage });
        }

        private async Task GetPlanesFamiliesAsync() {

            var response = await mediator.Send(new ViewFamiliesPlanesDropdownsRequest()).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                foreach (var item in response.Value.families) {
                    PlaneFamilies.Add(
                        new SelectListItem { Value = item.Family, Text = item.Family }
                        );
                }
            }

        }

        public async Task<IActionResult> OnGetDownloadFileAsync(int fileId, int planId, bool defaultFile) {

            var files = await mediator.Send(new DownloadAnagramRequest() {
                FilesId = new List<int> { fileId },
                PlanId = planId,
                DefaultFile = defaultFile

            }).ConfigureAwait(true);

            if (files.Status == RequestStatus.Ok) {
                if (files.Value.Files.Count == 1) {
                    return File(files.Value.Files.First().FileData, MediaTypeNames.Application.Octet, files.Value.Files.First().FileName);
                }

                return new NoContentResult();

            } else {
                return new NoContentResult();
            }
        }


        public async Task<JsonResult> OnGetGetBlueprintAsync(int planeId, bool isFromGenericBlueprint) {

            IRequestResponse<ViewPlanPlaneFileResponse> result = await mediator.Send(new ViewPlanPlaneFileRequest {
                GenericPlaneId = planeId,
                IsFromGenericBlueprint = isFromGenericBlueprint
            }).ConfigureAwait(true);


            return result.Status == RequestStatus.Ok ?
                new JsonResult(result.Value.files)
                : new JsonResult(new NotFoundResult());

        }


        public async Task<JsonResult> OnGetDependantData(int businessAddressId) {

            var filteredData = await mediator.Send(new UpdateAffiliatedDependantDataRequest {
                BusinessAddressId = businessAddressId
            }).ConfigureAwait(true);


            if (filteredData.Status == RequestStatus.Ok) {

                return new JsonResult(filteredData.Value);
            }

            return new JsonResult("error");

        }

        #endregion

        #region Post methods

        ///// <summary>
        ///// Creates custom Chapter/Subchapter/Activity and returns the new row to insert in Activity List
        ///// </summary>
        ///// <param name="chapterName"></param>
        ///// <param name="subchapterName"></param>
        ///// <param name="activityName"></param>
        ///// <returns></returns>
        //public IActionResult OnPostCustomChapter(string chapterName, string subchapterName, string activityName, int rowIndex) {

        //    var customChap = new CustomPlanChapter { Title = chapterName };
        //    var customSubchap = new CustomPlanSubchapter { Title = subchapterName };
        //    var customAct = new CustomPlanActivity { Title = activityName };

        //    customSubchap.Activities = new List<CustomPlanActivity>();
        //    customChap.Subchapters = new List<CustomPlanSubchapter>();

        //    customSubchap.Activities.Add(customAct);
        //    customChap.Subchapters.Add(customSubchap);

        //    var customRowModel = new PlanActivityCustomRowModel { CustomChapter = mapper.Map<PlanChapter>(customChap), RowIndex = rowIndex };

        //    return new ViewComponentResult() {
        //        ViewComponentName = "PlanActivityCustomRow",
        //        Arguments = new {
        //            customRowModel
        //        },
        //        ViewData = this.ViewData,
        //        TempData = this.TempData
        //    };
        //}

        /// <summary>
        /// Saves created/edited chapter/sub/act and refresh the list o selected Activities
        /// </summary>
        /// <param name="customActivityModel"></param>
        /// <returns></returns>
        private IActionResult SaveActivityAndUpdateSelectedActivityList(PlanActivityCustomModalsModel customActivityModel) {

            var oldSelectedActivities = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);
            var customSubChapters = SessionHelper.GetObjectFromJson<List<ViewComponentCustomPlanSubChapter>>(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS);

            var destination = (ActivityModalDestinationEnum)Enum.Parse(typeof(ActivityModalDestinationEnum), customActivityModel?.Destination, true);

            SessionHelper.RemoveSelectedActivities(HttpContext.Session);
            SessionHelper.RemoveCustomSubChapters(HttpContext.Session);
            PlanSubChapter newSubChapter;
            PlanChapter newChapter;

            //Buscamos en los captulos que había en el listado (almacenados en session) el capítulo. 
            //Si es null, es que es nuevo
            //Si no es null y no es customChapter no se actualiza
            //Si no es null y es CustomChapter se actualiza
            var editedChapter = oldSelectedActivities.Where(x => x.Position == customActivityModel.ChapterPosition).FirstOrDefault();

            if (editedChapter == null) {
                editedChapter = new PlanChapter {
                    Position = customActivityModel.ChapterPosition,
                    Title = customActivityModel.CustomChapter.Title,
                    IsCustomChapter = true
                };
            } else if (editedChapter.IsCustomChapter) {
                editedChapter.Title = customActivityModel.CustomChapter.Title;
            }

            //List usado cuando solo se ha editado/borrado desde el modal de sub sin entrar a ningun otro para sustituir anterior
            var newSubchaperList = new List<PlanSubChapter>();

            switch (destination) {
                case ActivityModalDestinationEnum.SaveChapterModal:

                    if (customSubChapters != null) {
                        foreach (var customSubChapter in customSubChapters) {
                            //Si no son custom hacer mapeo mapper.Map(source,destination) para que mantenga datos
                            if (editedChapter.SubChapter == null)
                                editedChapter.SubChapter = new List<PlanSubChapter>();

                            editedChapter.SubChapter.Add(mapper.Map<PlanSubChapter>(customSubChapter));
                        }
                    }

                    break;
                case ActivityModalDestinationEnum.SaveSubChapterModal:
                    //Limpiamos list de Subchapters para añadirle los editados, si solo hubieran editado/borrado los subchapter (sería null) sin cambiar de modal hay que mantenerlos
                    if (customSubChapters != null)
                        editedChapter.SubChapter = new List<PlanSubChapter>();

                    //Por cada uno de los que suben del modal
                    foreach (var postedSubchapter in customActivityModel.CustomChapter.SubChapter) {
                        //Si solo se hubiera entrado a editar el nombre de un customSubchapter y se le da a guardar estaría vacio session
                        if (customSubChapters != null) {
                            //Cogemos de los almacenados por la posicion previa de los que se han subido (si se hubiera borrado alguno la posicion habría cambiado)
                            var customSubChapterToUpdate = customSubChapters.Where(x => x.Position == postedSubchapter.PreviousPos).FirstOrDefault();

                            var test = mapper.Map<PlanSubChapter>(customSubChapterToUpdate);

                            editedChapter.SubChapter.Add(test);

                            ////Si es custom hay que añadirlo/editarlo, subchapter nuevos siempre estarán en session porque al entrar a crearle actividades se almacenan
                            //if (customSubChapterToUpdate.IsCustomSubChapter) {
                            //    //Creo que comprobacion no hace falta ya que se actualiza todo
                            //    //Si id 0 hay que crear, si no actualizar
                            //    if (customSubChapterToUpdate.Id == 0) {
                            //        //Si no son custom hacer mapeo mapper.Map(source,destination) para que mantenga datos
                            //        var test = mapper.Map<PlanSubChapter>(customSubChapterToUpdate);

                            //        editedChapter.SubChapter.Add(test);
                            //    } else {
                            //        //Si no son custom hacer mapeo mapper.Map(source,destination) para que mantenga datos
                            //        var test = mapper.Map<PlanSubChapter>(customSubChapterToUpdate);

                            //        editedChapter.SubChapter.Add(test);
                            //    }
                            //}
                        } else {

                            //caso de solo actualizar el titulo y posicion, ha entrado directamente a editar subs.
                            //COMPROBAR funcionamiento si entramos a editar y eliminamos una custom y guardamos, posted y edited
                            //EL FOREACH HACERLO SOBRE CUSTOMACTIVITYMODEL.CUSTOMCHAPTER.SUBCHAPTER Y CON EL PREVIOUSPOS TENEMOS EL QUE QUEREMOS ACTUALIZAR, SI NO ESTÁ ELIMINAR

                            var subACambiar = editedChapter.SubChapter.Where(x => x.Position == postedSubchapter.PreviousPos).FirstOrDefault();

                            if (subACambiar != null) {

                                if (subACambiar.IsCustomSubChapter)
                                    subACambiar.Title = postedSubchapter.Title;

                                subACambiar.Position = postedSubchapter.Position;

                                newSubchaperList.Add(subACambiar);
                            }
                        }
                    }

                    if (newSubchaperList.Any())
                        editedChapter.SubChapter = newSubchaperList;

                    break;
                case ActivityModalDestinationEnum.SaveActivityModal:

                    if (customSubChapters != null) {

                        editedChapter.SubChapter = new List<PlanSubChapter>();

                        foreach (var customSubChapter in customSubChapters) {

                            if (customSubChapter.Position == customActivityModel.SubchapterPosition)
                                customSubChapter.Activities = customActivityModel.CustomSubchapter.Activities;
                            //Si no son custom hacer mapeo mapper.Map(source,destination) para que mantenga datos
                            editedChapter.SubChapter.Add(mapper.Map<PlanSubChapter>(customSubChapter));
                        }

                    } else {
                        var oldSelectedSubChapter = oldSelectedActivities.Where(x => x.Position == customActivityModel.ChapterPosition).SelectMany(x => x.SubChapter).Where(x => x.Position == customActivityModel.SubchapterPosition).FirstOrDefault();

                        oldSelectedSubChapter.Activities = new List<PlanActivity>();

                        foreach (var postedActivity in customActivityModel.CustomSubchapter.Activities) {
                            //Si no son custom hacer mapeo mapper.Map(source,destination) para que mantenga datos
                            oldSelectedSubChapter.Activities.Add(mapper.Map<PlanActivity>(postedActivity));
                        }
                    }

                    break;
                default:
                    break;
            }

            oldSelectedActivities.RemoveAll(x => x.Position == editedChapter.Position);
            oldSelectedActivities.Add(editedChapter);

            var chapterList = new List<PlanChapter>();

            chapterList.AddRange(oldSelectedActivities);

            chapterList = chapterList.OrderBy(x => x.Position).ToList();

            SessionHelper.RemoveSelectedActivities(HttpContext.Session);
            SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID, chapterList);

            var model = new SelectedActivitiesPlanListsModel { SelectedChapters = chapterList, IsEditEnabled = true };

            return new ViewComponentResult() {
                ViewComponentName = "SelectedActivitiesPlanList",
                Arguments = new {
                    model
                },
                ViewData = this.ViewData,
                TempData = this.TempData
            };
        }

        private IActionResult DeleteChapterAndUpdateSelectedActivitiesList(PlanActivityCustomModalsModel customActivityModel) {

            SessionHelper.RemoveCustomSubChapters(HttpContext.Session);
            var chapterList = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);

            chapterList.RemoveAll(x => x.Position == customActivityModel.ChapterPosition);
            if (chapterList.Any()) {
                chapterList.OrderBy(x => x.Position);

                int chapterPosition = 1;//Always stars at second position because first Chapter is always added
                foreach (var chapter in chapterList) {
                    chapter.Position = chapterPosition;
                    chapterPosition++;
                }
            }

            SessionHelper.RemoveSelectedActivities(HttpContext.Session);
            SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID, chapterList);

            var model = new SelectedActivitiesPlanListsModel { SelectedChapters = chapterList, IsEditEnabled = true };

            return new ViewComponentResult() {
                ViewComponentName = "SelectedActivitiesPlanList",
                Arguments = new {
                    model
                },
                ViewData = this.ViewData,
                TempData = this.TempData
            };
        }

        /// <summary>
        /// Receives POST on PlanActivityCustomModalsViewComponent
        /// </summary>
        /// <param name="customActivityModel"></param>
        /// <returns></returns>
        public IActionResult OnPostCustomActivityComponentModal(PlanActivityCustomModalsModel customActivityModel) {

            if (customActivityModel.IsDeleteChapter)
                return DeleteChapterAndUpdateSelectedActivitiesList(customActivityModel);

            if (customActivityModel.IsSave)
                return SaveActivityAndUpdateSelectedActivityList(customActivityModel);

            return GetPlanActivityCustomModalsViewComponent(customActivityModel);
        }

        /// <summary>
        /// Receives the Post from Activity tab when edit/create Chapter, Subchater or Activity
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="chapterPositionModal">The row index of new Chapter or Chapter to edit to search in Session</param>
        /// <param name="subchapterPositionModal">The row index of new SubChapter or SubChapter to edit to search in Session</param>
        /// <param name="activityPositionModal">The row index of new Activity or Activity to edit to search in Session</param>
        /// <param name="selectedActivities"></param>
        /// <returns></returns>
        public IActionResult OnPostCustomChapterModal(string destination, int chapterPositionModal, int? subchapterPositionModal, int? activityPositionModal, List<PlanChapter> selectedActivities) {

            SessionHelper.RemoveCustomSubChapters(HttpContext.Session);

            UpdateSelectedActivitiesSession(selectedActivities);

            return GetPlanActivityCustomModalsViewComponent(new PlanActivityCustomModalsModel {
                Destination = destination,
                ChapterPosition = chapterPositionModal,
                SubchapterPosition = subchapterPositionModal,
                ActivityPosition = activityPositionModal
            });
        }

        private IActionResult GetPlanActivityCustomModalsViewComponent(PlanActivityCustomModalsModel customActivityModel) => new ViewComponentResult() {
            ViewComponentName = "PlanActivityCustomModals",
            Arguments = new {
                customActivityModel
            },
            ViewData = this.ViewData,
            TempData = this.TempData
        };

        private void UpdateSelectedActivitiesSession(List<PlanChapter> selectedActivities) {

            SessionHelper.RemoveSelectedActivities(HttpContext.Session);

            SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID, selectedActivities);
        }



        public async Task<IActionResult> OnPostNewPlan(string prevPage) {
            this.PrevPage = prevPage;
            var res = await mediator.Send(new CreatePlanRequest()).ConfigureAwait(true);

            if (res.Status == RequestStatus.Ok) {

                CurrentOperation = PlanActionType.Create;
                LoadEditPlanInformation(res.Value.PlanInformation, res.Value.DelegationList, res.Value.AffiliatedCompanyList,
                            res.Value.CustomerList, res.Value.TemplateList, res.Value.BusAddList,
                            res.Value.GenActList, res.Value.ProfileList, res.Value.UserList);
            } else {
                throw new Exception("Error en OnPostNewPlan de PLanManagement");
            }

            await GetPlanDetailsDefaultValuesAsync().ConfigureAwait(true);
            return Page();
        }

        public async Task<IActionResult> OnGetNewPlan(string prevPage) {
            return await OnPostNewPlan(prevPage).ConfigureAwait(true);
        }

        private async Task DeleteCustomChapters(List<PlanChapter> chapters, SafetyPlan plan) {

            //En plan activityList estan los que se mantienen (ha pasdo ya por update, create va luego no nos interesan su SelectedPlanActivity)
            /*  var response =*/
            await mediator.Send(new DeletePlanCustomChaptersRequest { Plan = plan }).ConfigureAwait(true);

        }

        private async Task UpdateCustomChapters(List<PlanChapter> chapters, SafetyPlan plan) {

            var customChapters = chapters.Where(chap =>
                            chap.SubChapter.Any(sChap => sChap.Activities.Any(act => act.IsCustomActivity && act.Id != 0))).ToList();

            if (customChapters.Any()) {

                var response = await mediator.Send(new UpdatePlanCustomChaptersRequest { Chapters = customChapters }).ConfigureAwait(true);

                plan.ActivityLists.PlanActivities.AddRange(response.Value.SelectedPlanActivities);
            }

        }

        private async Task CreateCustomChapters(List<PlanChapter> chapters, SafetyPlan plan) {
            var newCustomChapters = chapters.Where(chap =>
                            chap.SubChapter.Any(sChap => sChap.Activities.Any(act => act.IsCustomActivity && act.Id == 0))).ToList();

            if (newCustomChapters.Any()) {

                var response = await mediator.Send(new CreatePlanCustomChaptersRequest { Chapters = chapters }).ConfigureAwait(true);

                plan.ActivityLists.PlanActivities.AddRange(response.Value.SelectedPlanActivities);
            }

        }

        public async Task<IActionResult> OnPostSave(string saveType, int idBudget, SafetyPlan plan, List<PlanChapter> availableActivities, List<PlanChapter> selectedActivities) {
            IRequestResponse<EditPlanGeneralDataResponse> res;

            AvailableActivities = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.AVA_ACTIVITIES_SESSION_ID);
            SelectedActivities = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);
            SelectedArticles = SessionHelper.GetObjectFromJson<List<ApplicationArticle>>(HttpContext.Session, SessionHelper.SEL_ARTICLES_SESSION_ID);
            Budget = SessionHelper.GetObjectFromJson<ApplicationBudget>(HttpContext.Session, SessionHelper.BUDGET_INFO_SESSION_ID);

            if (selectedActivities != null && plan != null) {
                if (plan.Id != 0) {
                    await UpdateCustomChapters(selectedActivities, plan).ConfigureAwait(true);

                    await DeleteCustomChapters(selectedActivities, plan).ConfigureAwait(true);
                }

                await CreateCustomChapters(selectedActivities, plan).ConfigureAwait(true);
            }

            // Cleaning selected activities list
            SessionHelper.RemoveActivities(HttpContext.Session);

            var userId = LoadPlanInformation(plan, selectedActivities);

            LoadPlanBudgetInfo(plan, idBudget);

            FillPlaneInfo(plan);
            if (plan.GeneralData.IdReviewer == null)
                plan.GeneralData.IdReviewer = userId;
            switch (saveType) {
                case "Save":
                    // Cleaning selected activities list
                    SessionHelper.RemoveActivities(HttpContext.Session);
                    res = await SaveAndSaveAndNewCommonBehaviour(plan, userId).ConfigureAwait(true);

                    if (res.Status != RequestStatus.Ok)
                        throw new Exception($"Error en OnPostSave de PlanManagement con Id {plan?.Id}");

                    return RedirectToPage("./PlanManagement", new { id = plan.Id, op = PlanActionType.Edit, PrevPage = PrevPage });

                case "SaveAndNew":
                    res = await SaveAndSaveAndNewCommonBehaviour(plan, userId).ConfigureAwait(true);

                    if (res.Status != RequestStatus.Ok)
                        throw new Exception($"Error en SaveAndNew de PlanManagement con Id {plan?.Id}");

                    return RedirectToPage("./PlanManagement", "NewPlan", new { PrevPage = PrevPage });

                case "SaveAndClose":
                    if (plan.Id > 0)
                        await mediator.Send(new UpdatePlanRequest { PlanInformation = plan, UserId = userId }).ConfigureAwait(true);
                    else
                        await mediator.Send(new CreatePlanRequest { PlanInformation = plan, UserId = userId }).ConfigureAwait(true);

                    return RedirectToPage("./MyPlans");

                default:
                    throw new Exception("Error en OnPostSave de PlanManagement, no ha entrado por ningún caso del switch");

            }

        }

        private void LoadPlanBudgetInfo(SafetyPlan plan, int idBudget) {
            if (Budget == null) {
                Budget = new ApplicationBudget {
                    ApplicabePercentage = 100,
                    StudyBudget = 0,
                    Difference = 0,
                };
            }
            Budget.Id = idBudget;
            plan.Budget = Budget;

            plan.Budget.SelectedArticles = SelectedArticles;
            decimal total = 0;
            if (plan.Budget.SelectedArticles != null) {
                if (plan.Budget.SelectedArticles.Any()) {
                    foreach (var article in plan.Budget.SelectedArticles) {
                        total += article.TotalPrice;
                    }
                }
            }
            decimal difference = Convert.ToDecimal(plan.AdditionalData.PSSBudget) - total;
            plan.Budget.StudyBudget = total;
            plan.Budget.Difference = difference;
        }

        private async Task<IRequestResponse<EditPlanGeneralDataResponse>> SaveAndSaveAndNewCommonBehaviour(SafetyPlan plan, int userId) {
            return plan.Id > 0
                      ? await mediator.Send(new SavePlanRequest { PlanInformation = plan, UserId = userId }).ConfigureAwait(true)
                      : await mediator.Send(new CreatePlanRequest { PlanInformation = plan, UserId = userId }).ConfigureAwait(true);
        }



        private void FillPlaneInfo(SafetyPlan plan) {
            SelectedPlanes.Select(x => { x.IdPlan = plan.Id; return x; }).ToList();
            plan.SelectedPlanes = this.SelectedPlanes;
        }

        private async Task FillUserPlanDataAsync(SafetyPlan oldPlan) {

            // new plan with user fields loaded 
            var newPlan = await mediator.Send(new CreatePlanRequest()).ConfigureAwait(true);
            var tempPlan = oldPlan;

            if (newPlan.Status == RequestStatus.Ok) {

                CurrentOperation = PlanActionType.Create;
                LoadEditPlanInformation(newPlan.Value.PlanInformation, newPlan.Value.DelegationList, newPlan.Value.AffiliatedCompanyList,
                            newPlan.Value.CustomerList, newPlan.Value.TemplateList, newPlan.Value.BusAddList,
                            newPlan.Value.GenActList, newPlan.Value.ProfileList, newPlan.Value.UserList);
            }
            Plan = tempPlan;
        }


        public async Task<IActionResult> OnPostDuplicateAsync(string planId, string planName) {

            var userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            await DuplicatePlanAsync(Convert.ToInt32(planId), planName, userId).ConfigureAwait(true);

            return Page();
        }
        public async Task<IActionResult> OnPostDuplicatePlanAsync(SafetyPlan plan) {

            var userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            await DuplicatePlanAsync(plan.Id, plan.DuplicatedPlanTitle, userId).ConfigureAwait(true);

            return Page();
        }

        public async Task<IActionResult> OnPostDeletePlanAsync(SafetyPlan plan) {

            var userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            await mediator.Send(new DeletePlanRequest {
                PlanId = plan.Id,
                UserId = userId
            }).ConfigureAwait(true);

            var allPlans = SessionHelper.GetObjectFromJson<bool>(HttpContext.Session, SessionHelper.ALL_PLANS);
            return LocalRedirect(allPlans ? "/AllPlans" : "/MyPlans");
        }

        /// <summary>
        /// Function to Select the Activities of plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="newSelectedActivities"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostManageActivitiesAsync(int planId, bool isEditEnabled, string activities, int direction) {

            IsActivitiesEditEnabled = isEditEnabled;

            if (!string.IsNullOrWhiteSpace(activities)) {

                // Las actividades a controlar llegan como un string separado por comas. Se convierten a lista de enteros.
                List<SelectedPlanActivity> selectedActivities = new List<SelectedPlanActivity>();
                if (!string.IsNullOrWhiteSpace(activities)) {

                    var idList = activities.Split(',');
                    foreach (var id in idList) {

                        selectedActivities.Add(new SelectedPlanActivity { Id = Convert.ToInt32(id) });
                    }
                }


                if (selectedActivities != null && selectedActivities.Count > 0) {

                    AvailableActivities = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.AVA_ACTIVITIES_SESSION_ID);
                    SelectedActivities = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);

                    if (AvailableActivities == null || AvailableActivities.Count == 0) {

                        var response = await mediator.Send(new ViewPlanActivitiesRequest { PlanId = 0, GetAll = true }).ConfigureAwait(true);
                        if (!(response.Status == RequestStatus.Ok))
                            return Page();

                        AvailableActivities = response.Value.ActivityLists.AvailableActivities;
                    }

                    if (direction > 0) {
                        await AddSelectedActivities(selectedActivities).ConfigureAwait(true);
                    } else {
                        RemoveSelectedActivities(selectedActivities);
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostManageArticlesAsync(int planId, List<PlanArticle> selectedArticles, int percentage) {

            var budget = new ApplicationBudget {
                ApplicabePercentage = percentage
            };
            Parallel.Invoke(
            () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.BUDGET_INFO_SESSION_ID, budget)
            );

            var newSelectedArticles = new List<ApplicationArticle>();

            if (selectedArticles != null && selectedArticles.Count > 0) {
                foreach (var article in selectedArticles) {
                    var planArticle = new ApplicationArticle() {
                        AmortizationPrice = Convert.ToDecimal(article.AmortizationPrice.Replace('.', ',')),
                        Price = Convert.ToDecimal(article.Price.Replace('.', ',')),
                        PriceDurationWork = Convert.ToDecimal(article.PriceDurationWork.Replace('.', ',')),
                        TotalPrice = Convert.ToDecimal(article.TotalPrice.Replace('.', ',')),
                        AmortizationTime = Convert.ToDecimal(article.AmortizationTime.Replace('.', ',')),
                        TimeOfWork = Convert.ToDecimal(article.TimeOfWork.Replace('.', ',')),
                        Id = article.Id,
                        IdArticleFamily = article.IdArticleFamily,
                        MinimumUnit = article.MinimumUnit,
                        Name = article.Name,
                        Percentage = article.Percentage,
                        Unit = article.Unit,
                    };
                    newSelectedArticles.Add(planArticle);
                }
            }
            Parallel.Invoke(
                () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.SEL_ARTICLES_SESSION_ID, newSelectedArticles)
                );

            return Page();
        }


        public async Task<IActionResult> OnPostCreateDocument(string templateName = "PLAN DE SEGURIDAD") {
            var response = await mediator.Send(new GeneratePlanRequest() { PlanId = Plan.Id, TemplateName = Plan.GeneralData.TemplateName != null ? Plan.GeneralData.TemplateName : templateName, IsEvaluation = Plan.GeneralData.IsEvaluation, SelectedActivities = SelectedActivities }).ConfigureAwait(false);

            if (response.Status == RequestStatus.Ok) {
                HttpContext.Response.Cookies.Append("downloadToken", "downloaded", new Microsoft.AspNetCore.Http.CookieOptions {
                    Expires = DateTime.Now.AddMinutes(4),
                    IsEssential = true,
                    HttpOnly = false
                });
                return new FileStreamResult(response.Value.Document.ResponseStream, new MediaTypeHeaderValue(response.Value.Document.MediaType));
            } else {
                HttpContext.Response.Cookies.Append("downloadToken", "downloaded", new Microsoft.AspNetCore.Http.CookieOptions {
                    Expires = DateTime.Now.AddMinutes(4),
                    IsEssential = true,
                    HttpOnly = false
                });
                return new NoContentResult();
            }
        }

        public async Task<IActionResult> OnPostClosePlan() {
            return LocalRedirect(PrevPage == "/AllPlans" ? "/AllPlans" : "/MyPlans");
        }

        public async Task<JsonResult> OnPostAddBlueprintAsync(IFormFileCollection files, int planId, string description, string family, int position) {
            int userId = Convert.ToInt32(userManager.GetUserId(HttpContext.User));

            List<AddPlanPlaneRequestFiles> ParsedFiles = new List<AddPlanPlaneRequestFiles>();

            foreach (var file in files) {
                using (var ms = new MemoryStream()) {

                    file.CopyTo(ms);

                    ParsedFiles.Add(
                        new AddPlanPlaneRequestFiles {
                            Name = file.FileName.Split(file.FileName.Substring(file.FileName.LastIndexOf('.'))).First(),
                            FileName = file.FileName,
                            //as byte[]
                            Data = ms.ToArray(),
                            //as base 64 stream
                            DataBaseString = Convert.ToBase64String(ms.ToArray())
                        });
                }
            }

            IRequestResponse<AddPlanPlaneResponse> result = await mediator.Send(new AddPlanPlaneRequest {
                Files = ParsedFiles,
                IdSafetyStudyPlan = planId,
                Description = description,
                FamilyName = family,
                Position = position,
                ModifiedBy = userId,
                CreatedBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            }).ConfigureAwait(true);

            return result.Status == RequestStatus.Ok ?
               new JsonResult(result.Value.safetyPlanPlane)
               : new JsonResult(new NotFoundResult());
        }


        #endregion

        #region Private helpers

        private int LoadPlanInformation(SafetyPlan plan, List<PlanChapter> selectedActivities) {

            if (plan != null && SelectedActivities != null) {
                plan.ActivityLists.PlanActivities.AddRange(from c in selectedActivities
                                                           from s in c.SubChapter
                                                           from a in s.Activities
                                                           where !a.IsCustomActivity
                                                           select new SelectedPlanActivity {
                                                               Id = a.Id,
                                                               ActivityPosition = a.Position,
                                                               WordDescription = a.WordDescription,
                                                               ChapterPosition = c.Position,
                                                               ChapterDescription = c.WordDescription,
                                                               SubChapterPosition = s.Position,
                                                               SubChapterDescription = s.WordDescription,
                                                               AvailableActivitiId = c.Id
                                                           });

            }

            plan.GeneralData.AnagramUploadFiles = plan.GeneralData.AnagramUploadFiles is null ?
                new List<IFormFile>() : plan.GeneralData.AnagramUploadFiles;

            foreach (var file in plan.GeneralData.AnagramUploadFiles) {

                if (CheckAnagramUploadFile(file)) {

                    using (var ms = new MemoryStream()) {

                        file.CopyTo(ms);

                        if (ms != null) {
                            plan.GeneralData.Anagrams.Add(new PlanFile {
                                Name = file.FileName,
                                Data = ms.ToArray(),
                                DataLength = Convert.ToInt32(ms.Length)
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(plan.GeneralData.DeleteExistingFileIdsCsv)) {
                plan.GeneralData.DeleteExistingFileIds = plan.GeneralData.DeleteExistingFileIdsCsv.Split(',').Select(int.Parse).ToList();
            }

            return Convert.ToInt32(userManager.GetUserId(HttpContext.User));

        }

        private static bool CheckAnagramUploadFile(IFormFile file) {
            string[] validImageTypes = { "image/gif", "image/jpeg", "image/jpg", "image/png", "image/bmp", "image/tif", "image/tiff" };
            bool isCorrect = false;

            if (file != null) {
                if (validImageTypes.Contains(file.ContentType))
                    isCorrect = true;
            }

            return isCorrect;
        }

        private void LoadEditPlanInformation(SafetyPlan planInformation, List<PlanDelegation> delegationList, List<PlanAffiliatedCompany> companyList,
            List<PlanCustomer> customerList, List<PlanTemplate> templateList, List<PlanBusinessAddress> busAddList,
            List<PlanGeneralActivity> genActList, List<UserProfile> profileList, List<ApplicationUser> userList) {

            ModelState.Clear();

            Plan = planInformation ?? new SafetyPlan();

            Plan.DuplicatedPlanTitle = "N/A";

            DelegationList = delegationList;
            AffiliatedList = companyList;
            CustomerList = customerList;
            TemplateList = templateList;
            BusinessAddressList = busAddList;
            GeneralActivityList = genActList;
            ProfileList = profileList;
            UserList = userList;
        }

        private async Task<RequestStatus> DuplicatePlanAsync(int planId, string planName, int userId) {

            var res = await mediator.Send(new DuplicatePlanRequest {
                OriginalPlanId = planId,
                PlanTitle = planName,
                UserId = userId
            }).ConfigureAwait(true);

            if (res.Status == RequestStatus.Ok) {

                LoadEditPlanInformation(res.Value.PlanInformation, res.Value.DelegationList, res.Value.AffiliatedCompanyList,
                            res.Value.CustomerList, res.Value.TemplateList, res.Value.BusAddList,
                            res.Value.GenActList, res.Value.ProfileList, res.Value.UserList);

                // Setting operation Edit to show the duplicated plan in "Edition mode"
                CurrentOperation = PlanActionType.Edit;
                //load dropdowns
                await GetPlanesFamiliesAsync().ConfigureAwait(true);
                await GetPlanDetailsDefaultValuesAsync().ConfigureAwait(true);

                return RequestStatus.Ok;
            } else {
                //throw new Exception($"Error duplicating plan {planId}");
                return RequestStatus.Error;
            }
        }

        private async Task AddSelectedActivities(List<SelectedPlanActivity> selectedIds) {

            var selectedActs = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);

            if (selectedActs != null) {
                if (selectedActs.Where(x => x.IsCustomChapter).Any())
                    CustomSelectedChapters = selectedActs.Where(x => x.IsCustomChapter).ToList();
                selectedActs.RemoveAll(x => x.IsCustomChapter);
            }

            SelectedActivities = selectedActs ?? new List<PlanChapter>();

            var notRepeatedActivities = selectedIds.Where(sId => !SelectedActivities.Any(c => c.SubChapter.Any(s => s.Activities.Any(a => a.Id == sId.Id)))).ToList();

            var newActivitites = GetSelectedActivities(notRepeatedActivities, AvailableActivities);

            OrderSelectedActivitiesByPosition(newActivitites, notRepeatedActivities);

            AddActivitiesToList(newActivitites, SelectedActivities);
            RemoveEmpties(selectedIds, AvailableActivities);

            if (CustomSelectedChapters.Any())
                SelectedActivities.AddRange(CustomSelectedChapters);

            if (selectedIds.Where(x => x.IsCustomActivity).Any())
                await GetCustomSelectedActivities(selectedIds.Where(x => x.IsCustomActivity).ToList()).ConfigureAwait(true);

            //SelectedActivities.AddRange(CustomSelectedChapters);

            SortLists();
        }

        private void OrderSelectedActivitiesByPosition(List<PlanChapter> newActivitites, List<SelectedPlanActivity> notRepeatedActivities) {

            newActivitites.Select(chap => {
                chap.Position = notRepeatedActivities.Find(selectedPlanActivity => selectedPlanActivity.Id == chap.SubChapter.First().Activities.First().Id).ChapterPosition;

                chap.SubChapter.Select(sub => {
                    sub.Position = notRepeatedActivities.Find(selectedPlanActivity => selectedPlanActivity.Id == sub.Activities.First().Id).SubChapterPosition;
                    return sub;
                }).ToList();

                chap.SubChapter = chap.SubChapter.OrderBy(planSubChapter => planSubChapter.Position).ToList();

                return chap;
            }).ToList();

            newActivitites = newActivitites.OrderBy(planChapter => planChapter.Position).ToList();

        }

        private void RemoveSelectedActivities(List<SelectedPlanActivity> selectedActivities) {
            // Getting the activities to be removed from the selected ones



            var removeActivities = GetSelectedActivities(selectedActivities, SelectedActivities);

            removeActivities.RemoveAll(x => x.DefaultSelectedChapter || x.IsCustomChapter);

            foreach (var chap in removeActivities) {

                chap.IsSelected = false;


                foreach (var s in chap.SubChapter) {

                    s.IsSelected = false;

                    var subAct = (from sa in SelectedActivities
                                  from su in sa.SubChapter
                                  where su.Id == s.Id
                                  && !su.IsCustomSubChapter
                                  select su).FirstOrDefault();

                    // Setting the activities as "not selected" to be displayed as available
                    s.Activities.Select(act => { act.IsSelected = false; return act; }).ToList();
                    //foreach (var sAc in s.Activities) {

                    //    sAc.IsSelected = false;
                    //}

                    subAct.Activities.RemoveAll(selA => removeActivities.Any(c => c.SubChapter.Any(selSub => selSub.Activities.Any(a => a.Id == selA.Id || a.IsCustomActivity))));
                }
            }

            RemoveEmpties(selectedActivities, SelectedActivities);
            AddActivitiesToList(removeActivities, AvailableActivities);
            SortLists();
        }

        private void RemoveEmpties(List<SelectedPlanActivity> selectedIds, List<PlanChapter> activityList) {

            List<int> idList = selectedIds.Select(a => a.Id).ToList();

            foreach (var chapter in activityList.Where(chap => chap.SubChapter.Any(s => s.Activities.Any(a => idList.Contains(a.Id))))) {

                foreach (var subChap in chapter.SubChapter) {

                    foreach (var act in subChap.Activities.Where(a => idList.Contains(a.Id))) {
                        act.IsSelected = true;
                    }

                    if (!subChap.Activities.Any(a => !a.IsSelected)) {
                        subChap.IsSelected = true;
                    }
                }

                if (!chapter.SubChapter.Any(s => !s.IsSelected)) {
                    chapter.IsSelected = true;
                }
            }

            // Removing selected activities from available list
            foreach (var subChap in activityList.SelectMany(chap => chap.SubChapter)) {
                var actList = subChap.Activities.Where(act => selectedIds.Any(a => a.Id == act.Id)).ToList();
                foreach (var act in actList) {
                    act.IsSelected = true;
                }

                if (actList.Count == subChap.Activities.Count)
                    subChap.IsSelected = true;
            }

            // Removing empty subchapters
            foreach (var act in activityList) {
                act.SubChapter.RemoveAll(sub => sub.Activities.Count == 0);
            }

            // Removing empty chapters
            activityList.RemoveAll(chap => chap.SubChapter.Count == 0);
        }

        private void AddActivitiesToList(List<PlanChapter> newActivitites, List<PlanChapter> destinationList) {

            foreach (var chap in newActivitites) {

                var avChap = destinationList.FirstOrDefault(a => a.Id == chap.Id);
                if (avChap != null) {
                    // The chapter is already in available activities

                    avChap.IsSelected = false;
                    foreach (var sub in chap.SubChapter) {

                        var avSub = avChap.SubChapter.FirstOrDefault(s => s.Id == sub.Id);

                        if (avSub != null) {

                            avSub.IsSelected = false;
                            foreach (var act in sub.Activities) {

                                var avAct = avSub.Activities.FirstOrDefault(a => a.Id == act.Id);

                                if (avAct != null) {

                                    avAct.IsSelected = false;
                                } else {

                                    avSub.Activities.Add(act);
                                }
                            }
                        } else {

                            // Adding the subchapter to the available activity list
                            if (!sub.IsCustomSubChapter)
                                avChap.SubChapter.Add(sub);

                        }
                    }
                } else {

                    // Adding the chapter to the available activity list
                    //chap.Position = destinationList.Any() ? destinationList.Last().Position + 1 : 1;
                    destinationList.Add(chap);
                }
            }
        }

        private async Task GetCustomSelectedActivities(List<SelectedPlanActivity> selectecCustomActs) {

            var response = await mediator.Send(new GetCustomPlanChaptersRequest { SelectedActivities = selectecCustomActs }).ConfigureAwait(true);

            if (response.Status == RequestStatus.Ok) {
                if (response.Value.CustomChapters.Any()) {
                    var customSelectedChapters = mapper.Map<List<PlanChapter>>(response.Value.CustomChapters);

                    SelectedActivities.AddRange(customSelectedChapters);
                }
                if (response.Value.CustomSubchapters.Any()) {

                    var customSelectedSubChapters = mapper.Map<List<PlanSubChapter>>(response.Value.CustomSubchapters);

                    foreach (var customSubChapter in customSelectedSubChapters) {
                        //If Chapter is not in SelectedActivities is because dont have system Subchapters. If have custom we need to add to list
                        if (customSubChapter.ChapterId != default && !SelectedActivities.Any(x => x.Id == customSubChapter.ChapterId)) {
                            var originalChapter = AvailableActivities.Where(chap => chap.Id == customSubChapter.ChapterId).FirstOrDefault();

                            var chapterToAdd = new PlanChapter {
                                Id = originalChapter.Id,
                                DefaultSelectedChapter = originalChapter.DefaultSelectedChapter,
                                IsSelected = originalChapter.IsSelected,
                                Number = originalChapter.Number,
                                Position = customSubChapter.ChapterPosition,
                                SubChapter = new List<PlanSubChapter>(),
                                Title = originalChapter.Title,
                                WordDescription = originalChapter.WordDescription
                            };
                            //chapterToAdd = mapper.Map<PlanChapter>(originalChapter);

                            chapterToAdd.SubChapter = new List<PlanSubChapter>();

                            SelectedActivities.Add(chapterToAdd);
                        }

                        SelectedActivities.Where(x => x.Position == customSubChapter.ChapterPosition).FirstOrDefault().SubChapter.Add(customSubChapter);
                    }
                }
                if (response.Value.CustomActivities.Any()) {
                    var customSelectedActivities = mapper.Map<List<PlanActivity>>(response.Value.CustomActivities);

                    foreach (var customActivity in customSelectedActivities) {
                        //If Chapter is not in SelectedActivities is because dont have system Activities. If have custom we need to add to list
                        if (customActivity.SubChapterId != default && !SelectedActivities.Any(chap => chap.SubChapter.Any(sChap => sChap.Id == customActivity.SubChapterId))) {
                            var originalChapter = AvailableActivities.Where(chap => chap.SubChapter.Any(sChap => sChap.Id == customActivity.SubChapterId)).FirstOrDefault();

                            var chapterToAdd = new PlanChapter {
                                Id = originalChapter.Id,
                                DefaultSelectedChapter = originalChapter.DefaultSelectedChapter,
                                IsSelected = originalChapter.IsSelected,
                                Number = originalChapter.Number,
                                Position = customActivity.ChapterPosition,
                                SubChapter = new List<PlanSubChapter>(),
                                Title = originalChapter.Title,
                                WordDescription = originalChapter.WordDescription
                            };

                            var originalSubChapter = AvailableActivities.Where(chap => chap.Id == originalChapter.Id).SelectMany(x => x.SubChapter).Where(z => z.Id == customActivity.SubChapterId).FirstOrDefault();

                            var subChapterToAdd = new PlanSubChapter {
                                Id = originalSubChapter.Id,
                                IsSelected = originalSubChapter.IsSelected,
                                Number = originalSubChapter.Number,
                                Position = customActivity.SubChapterPosition,
                                Activities = new List<PlanActivity>(),
                                Title = originalSubChapter.Title,
                                WordDescription = originalSubChapter.WordDescription
                            };
                            //chapterToAdd = mapper.Map<PlanChapter>(originalChapter);

                            //chapterToAdd.SubChapter.RemoveAll(sChap => sChap.Id != customActivity.SubChapterId);
                            //chapterToAdd.SubChapter.FirstOrDefault().Activities = new List<PlanActivity>();
                            if (!SelectedActivities.Any(d => d.Id == chapterToAdd.Id)) {
                                SelectedActivities.Add(chapterToAdd);
                            }
                            var chapterSelect = SelectedActivities.Where(chap => chap.Position == customActivity.ChapterPosition).FirstOrDefault();
                            if (!chapterSelect.SubChapter.Any(d => d.Id == subChapterToAdd.Id)) {
                                chapterSelect.SubChapter.Add(subChapterToAdd);
                            }
                        }

                        var chapter = SelectedActivities.Where(chap => chap.Position == customActivity.ChapterPosition).FirstOrDefault();
                        chapter.SubChapter.Where(sChap => sChap.Position == customActivity.SubChapterPosition).FirstOrDefault().Activities.Add(customActivity);
                    }
                }
            }



            //if (response.Status == RequestStatus.Ok)
            //    return mapper.Map<List<PlanChapter>>(response.Value.CustomChapters);

            //return new List<PlanChapter>();
        }

        private List<PlanChapter> GetSelectedActivities(List<SelectedPlanActivity> selectedActs, List<PlanChapter> sourceList) {

            var selectedIds = selectedActs.Select(a => a.Id).ToList();
            var plChapter = sourceList.Where(chap => chap.SubChapter.Any(sub => sub.Activities.Any(act => selectedIds.Contains(act.Id)))).ToList();
            List<PlanChapter> planChapters = new List<PlanChapter>();
            List<PlanSubChapter> planSubChapters = new List<PlanSubChapter>();
            List<PlanActivity> planActivities = new List<PlanActivity>();

            foreach (var chap in plChapter) {
                foreach (var sub in chap.SubChapter.Where(sub => sub.Activities.Any(act => selectedIds.Contains(act.Id)))) {
                    foreach (var act in sub.Activities.Where(act => selectedIds.Contains(act.Id))) {
                        planActivities.Add(new PlanActivity {
                            Id = act.Id,
                            Description = act.Description,
                            WordDescription = selectedActs.FirstOrDefault(selA => selA.Id == act.Id).WordDescription,
                            Number = act.Number,
                            Position = selectedActs.FirstOrDefault(selA => selA.Id == act.Id).ActivityPosition,
                            IsCustomActivity = act.IsCustomActivity
                        });
                    }
                    planSubChapters.Add(new PlanSubChapter {
                        Id = sub.Id,
                        Description = sub.Description,
                        //Position = selectedActs.FirstOrDefault(selSub => selSub.AvailableActivitiId == chap.Id
                        //    && selSub.SubChapterPosition == sub.Position)?.SubChapterPosition ?? int.MaxValue,
                        Position = selectedActs.FirstOrDefault(selA => selA.SubChaptId == sub.Id)?.SubChapterPosition ?? int.MaxValue,
                        Number = sub.Number,
                        Title = sub.Title,
                        //selectedActs.FirstOrDefault(selA => selA.Id == sub.Id)
                        WordDescription = selectedActs.FirstOrDefault(selA => selA.SubChaptId == sub.Id)?.SubChapterDescription ?? "",
                        //WordDescription = selectedActs.FirstOrDefault(selSub =>
                        //   selSub.AvailableActivitiId == chap.Id
                        //   && selSub.SubChapterPosition == sub.Position)?.SubChapterDescription ?? "",
                        Activities = planActivities
                    });
                    planActivities = new List<PlanActivity>();
                }
                planChapters.Add(new PlanChapter {
                    Id = chap.Id,
                    Title = chap.Title,
                    Position = selectedActs.FirstOrDefault(selC => selC.AvailableActivitiId == chap.Id)?.ChapterPosition ?? int.MaxValue,
                    Number = chap.Number,
                    WordDescription = selectedActs.FirstOrDefault(selC => selC.AvailableActivitiId == chap.Id)?.ChapterDescription ?? "",
                    DefaultSelectedChapter = chap.DefaultSelectedChapter,
                    IsCustomChapter = chap.IsCustomChapter,
                    SubChapter = planSubChapters
                });
                planSubChapters = new List<PlanSubChapter>();
            }
            return planChapters.OrderBy(x => x.Position).ToList();
        }

        private void SortLists() {
            SortAvailableActivities();
            SortSelectedActivities();

            Parallel.Invoke(
                () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID, SelectedActivities),
                () => SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.AVA_ACTIVITIES_SESSION_ID, AvailableActivities)
            );
        }

        /// <summary>
        /// Se ordenan las actividades seleccionadas
        /// </summary>
        /// <param name="activityList">Todo el arbol de actividades seleccionadas por el usu</param>
        /// <returns></returns>
        private void SortSelectedActivities() {

            var sortedList = SelectedActivities.OrderBy(c => c.Position == 0).ThenBy(c => c.Position).ToList();

            int chapIndex = 1;
            foreach (var chap in sortedList) {

                //if (chap.Position == 0)
                chap.Position = chapIndex;

                chapIndex++;

                chap.SubChapter = chap.SubChapter.OrderBy(c => c.Position == 0).OrderBy(s => s.Position).ToList();

                int subIndex = 1;
                foreach (var sub in chap.SubChapter) {

                    sub.Position = subIndex;
                    subIndex++;

                    sub.Activities = sub.Activities.OrderBy(c => c.Position == 0).OrderBy(a => a.Position).ToList();

                    int actIndex = 1;
                    foreach (var act in sub.Activities) {
                        act.Position = actIndex;
                        actIndex++;
                    }
                }
            }

            SelectedActivities = sortedList;
        }

        private void SortAvailableActivities() {

            var sortedList = AvailableActivities.OrderBy(c => c.Number).ToList();
            foreach (var chap in sortedList) {

                chap.SubChapter = chap.SubChapter.OrderBy(s => s.Number).ToList();
                foreach (var sub in chap.SubChapter) {

                    sub.Activities = sub.Activities.OrderBy(a => a.Number).ToList();

                }
            }

            AvailableActivities = sortedList;
        }




        #endregion
    }
}
