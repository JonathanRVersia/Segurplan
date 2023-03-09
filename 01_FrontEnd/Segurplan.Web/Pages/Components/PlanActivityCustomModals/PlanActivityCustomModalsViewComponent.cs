using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Segurplan.Core.Actions.Plans.PlansData.Activities;
using Segurplan.Web.Utils;

namespace Segurplan.Web.Pages.Components.PlanActivityCustomModals {
    public class PlanActivityCustomModalsViewComponent : ViewComponent {

        private readonly IMapper mapper;

        public PlanActivityCustomModalsViewComponent(IMapper mapper) {
            this.mapper = mapper;
        }

        public List<PlanChapter> SelectedChapters;

        public PlanActivityCustomModalsModel CustomActivityModel = new PlanActivityCustomModalsModel();

        public ActivityModalDestinationEnum? CurrentDestination => (ActivityModalDestinationEnum)Enum.Parse(typeof(ActivityModalDestinationEnum), CustomActivityModel?.Destination, true);


        public IViewComponentResult Invoke(PlanActivityCustomModalsModel customActivityModel) {

            if (customActivityModel != null)
                CustomActivityModel = customActivityModel;

            switch (CurrentDestination) {
                case ActivityModalDestinationEnum.ChapterModal:
                    return CustomChapterModal();
                case ActivityModalDestinationEnum.SubchaptersModal:
                    return CustomSubChaptersModal();
                case ActivityModalDestinationEnum.ActivitiesModal:
                    return CustomActivityModal();
                default:
                    return null;
            }

        }

        public IViewComponentResult CustomChapterModal() {
            GetSelectedChapters();

            if (CustomActivityModel?.CustomChapter?.SubChapter != null) {
                var customStoredSubChapters = SessionHelper.GetObjectFromJson<List<ViewComponentCustomPlanSubChapter>>(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS);
                SessionHelper.RemoveCustomSubChapters(HttpContext.Session);

                if (customStoredSubChapters != null) {

                    if (!String.IsNullOrEmpty(CustomActivityModel.DeletedSubChapterPosition))
                        RemoveDeletedSubChaptersFromSession(customStoredSubChapters, CustomActivityModel.DeletedSubChapterPosition);

                    foreach (var customSubChapter in customStoredSubChapters) {
                        customSubChapter.Title = CustomActivityModel.CustomChapter.SubChapter.Where(x => x.PreviousPos == customSubChapter.Position).Select(x => x.Title).FirstOrDefault();
                        customSubChapter.Position = CustomActivityModel.CustomChapter.SubChapter.Where(x => x.PreviousPos == customSubChapter.Position).Select(x => x.Position).FirstOrDefault();
                    }

                    if (CustomActivityModel.CustomChapter.SubChapter.Count > customStoredSubChapters.Count)
                        customStoredSubChapters.AddRange(CustomActivityModel.CustomChapter.SubChapter.Where(x => x.PreviousPos == default));

                    CustomActivityModel.EverySubChapterHasActivities = customStoredSubChapters.Any(x => x.Activities != null);

                    SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS, customStoredSubChapters);
                } else {
                    if (CustomActivityModel.CustomChapter.SubChapter.Any(sChap => sChap.Id != 0)) {
                        foreach (var subChapter in CustomActivityModel.CustomChapter.SubChapter) {
                            if (subChapter.Id != 0) {
                                subChapter.Activities = mapper.Map<List<ViewComponentCustomPlanActivity>>(SelectedChapters.SelectMany(x => x.SubChapter.Where(sChap => sChap.Id == subChapter.Id).SelectMany(sChap => sChap.Activities).ToList()));
                            }
                        }
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS, CustomActivityModel.CustomChapter.SubChapter);
                }


            }


            CustomActivityModel.PageHandler = "CustomActivityComponentModal";
            CustomActivityModel.Destination = nameof(ActivityModalDestinationEnum.SubchaptersModal);

            if (CustomActivityModel.CustomChapter == null)
                CustomActivityModel.CustomChapter = new ViewComponentCustomPlanChapter { IsCustomChapter = true };

            CustomActivityModel.CustomChapter.Position = CustomActivityModel.ChapterPosition;

            return View("CustomChapterModal", CustomActivityModel);
        }

        public IViewComponentResult CustomSubChaptersModal() {
            GetSelectedChapters();

            if (CustomActivityModel.CustomSubchapter?.Activities != null) {
                //if(CustomActivityModel.DeletedActivitiesPosition!=null)
                //RemoveDeletedActivitiesFromSession(CustomActivityModel.CustomSubchapter.Activities, CustomActivityModel.DeletedActivitiesPosition);

                var customSubChapters = SessionHelper.GetObjectFromJson<List<ViewComponentCustomPlanSubChapter>>(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS);
                if (customSubChapters == null)
                    customSubChapters = new List<ViewComponentCustomPlanSubChapter>();
                SessionHelper.RemoveCustomSubChapters(HttpContext.Session);

                if (!customSubChapters.Any()) {
                    customSubChapters = mapper.Map<List<ViewComponentCustomPlanSubChapter>>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).FirstOrDefault().SubChapter);
                }

                customSubChapters.Where(x => x.Position == CustomActivityModel.SubchapterPosition).FirstOrDefault().Activities = CustomActivityModel.CustomSubchapter.Activities;

                //if (customSubChapters.Any()) {
                //    if (customSubChapters.Where(x => x.Position == CustomActivityModel.SubchapterPosition).Any())
                //        customSubChapters.Where(x => x.Position == CustomActivityModel.SubchapterPosition).FirstOrDefault().Activities = CustomActivityModel.CustomSubchapter.Activities;
                //} else {
                //    //Checkear que solo entre aqui cuando subimos al entrar a actividades directamente
                //    customSubChapters = mapper.Map<List<ViewComponentCustomPlanSubChapter>>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).FirstOrDefault().SubChapter);
                //    if (CustomActivityModel.SubchapterPosition != default) {
                //        var customUpdatedSubchapter = customSubChapters.Where(x => x.Position == CustomActivityModel.SubchapterPosition).FirstOrDefault();
                //        customUpdatedSubchapter.Activities = CustomActivityModel.CustomSubchapter.Activities;
                //    }

                //    SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS, customSubChapters);
                //}

                SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS, customSubChapters);
            }

            CustomActivityModel.PageHandler = "CustomActivityComponentModal";

            if (CustomActivityModel.CustomChapter == null)
                CustomActivityModel.CustomChapter = mapper.Map<ViewComponentCustomPlanChapter>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).FirstOrDefault());
            else {
                CustomActivityModel.CustomChapter.SubChapter = SessionHelper.GetObjectFromJson<List<ViewComponentCustomPlanSubChapter>>(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS);
            }

            if (CustomActivityModel.CustomChapter.SubChapter != null)
                CustomActivityModel.EverySubChapterHasActivities = CustomActivityModel.CustomChapter.SubChapter.All(x => x.Activities != null);

            return View("CustomSubChaptersModal", CustomActivityModel);
        }

        public IViewComponentResult CustomActivityModal() {
            GetSelectedChapters();

            if (CustomActivityModel.CustomChapter?.SubChapter != null) {

                var customStoredSubChapters = SessionHelper.GetObjectFromJson<List<ViewComponentCustomPlanSubChapter>>(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS);

                if (!String.IsNullOrEmpty(CustomActivityModel.DeletedSubChapterPosition))
                    RemoveDeletedSubChaptersFromSession(customStoredSubChapters, CustomActivityModel.DeletedSubChapterPosition);

                if (customStoredSubChapters == null) {
                    foreach (var subChapter in CustomActivityModel.CustomChapter.SubChapter) {

                        var selectedSubChapter = SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).SelectMany(y => y.SubChapter.Where(z => z.Position == subChapter.Position)).FirstOrDefault();

                        if (selectedSubChapter?.Activities != null) {
                            subChapter.Activities = mapper.Map<List<ViewComponentCustomPlanActivity>>(selectedSubChapter.Activities);
                        }
                        //if (SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).Any(y => y.SubChapter.Any(z => z.Position == subChapter.Position))) {

                        //}

                        //subChapter.Activities = mapper.Map<List<ViewComponentCustomPlanActivity>>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).SelectMany(y => y.SubChapter.Where(z => z.Position == subChapter.Position)).FirstOrDefault().Activities);
                        //if (!subChapter.IsCustomSubChapter || subChapter.Id != 0) {
                        //    subChapter.Activities = mapper.Map<List<ViewComponentCustomPlanActivity>>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).SelectMany(y => y.SubChapter.Where(z => z.Position == subChapter.Position)).FirstOrDefault().Activities);
                        //}
                    }
                } else {

                    if (customStoredSubChapters.Where(x => x.Position == CustomActivityModel.SubchapterPosition).Any()) {
                        var targetElement = CustomActivityModel.CustomChapter.SubChapter.Where(x => x.Position == CustomActivityModel.SubchapterPosition).FirstOrDefault();

                        targetElement.Activities = customStoredSubChapters.Where(x => targetElement.PreviousPos == 0 ? x.Position == targetElement.Position : x.Position == targetElement.PreviousPos).FirstOrDefault().Activities;
                    }

                    foreach (var customSubChapter in customStoredSubChapters) {
                        if (customSubChapter.Activities != null) {
                            CustomActivityModel.CustomChapter.SubChapter.Where(x => x.PreviousPos == 0 ? x.Position == customSubChapter.Position : x.PreviousPos == customSubChapter.Position).FirstOrDefault().Activities = customSubChapter.Activities;
                        }
                    }
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, SessionHelper.CUSTOM_SUBCHAPTERS, CustomActivityModel.CustomChapter.SubChapter.ToList());
                CustomActivityModel.EverySubChapterHasActivities = CustomActivityModel.CustomChapter.SubChapter.All(x => x.Activities != null);
            }

            CustomActivityModel.PageHandler = "CustomActivityComponentModal";

            if (CustomActivityModel.CustomChapter == null)
                CustomActivityModel.CustomChapter = mapper.Map<ViewComponentCustomPlanChapter>(
                                                    SelectedChapters.Where(chap => chap.Position == CustomActivityModel.ChapterPosition)
                                                    .FirstOrDefault());

            if (CustomActivityModel.CustomSubchapter == null && CustomActivityModel.CustomChapter.Id != default)
                CustomActivityModel.CustomSubchapter = CustomActivityModel.CustomChapter.SubChapter
                                                       .Where(sChap => sChap.Position == CustomActivityModel.SubchapterPosition)
                                                       .FirstOrDefault();

            //Cuanto entra por primera vez a actividades directametne no hay datos precargados en el modelo
            if (CustomActivityModel.CustomSubchapter == null && CustomActivityModel.CustomChapter == null)
                CustomActivityModel.CustomSubchapter = mapper.Map<ViewComponentCustomPlanSubChapter>(SelectedChapters.Where(x => x.Position == CustomActivityModel.ChapterPosition).SelectMany(x => x.SubChapter).Where(x => x.Position == CustomActivityModel.SubchapterPosition).FirstOrDefault());

            else
                CustomActivityModel.CustomSubchapter = CustomActivityModel.CustomChapter.SubChapter
                                                       .Where(sChap => sChap.Position == CustomActivityModel.SubchapterPosition)
                                                       .FirstOrDefault();

            if (CustomActivityModel.CustomSubchapter.Activities != null)
                CustomActivityModel.CustomSubchapter.Activities.OrderBy(x => x.Position);

            if (CustomActivityModel.CustomChapter?.SubChapter != null) {

                CustomActivityModel.CustomSubchapter.IsOnlySubChapterWithoutActivitys = true;

                foreach (var subChapter in CustomActivityModel.CustomChapter.SubChapter) {
                    if (subChapter.Position != CustomActivityModel.SubchapterPosition && subChapter.Activities == null) {
                        CustomActivityModel.CustomSubchapter.IsOnlySubChapterWithoutActivitys = false;
                        break;
                    }

                }
            }

            return View("CustomActivitiesModal", CustomActivityModel);
        }

        private void GetSelectedChapters() {
            var selectedChapters = SessionHelper.GetObjectFromJson<List<PlanChapter>>(HttpContext.Session, SessionHelper.SEL_ACTIVITIES_SESSION_ID);

            if (selectedChapters != null) {
                SelectedChapters = selectedChapters;

            }
        }


        private void RemoveDeletedSubChaptersFromSession(List<ViewComponentCustomPlanSubChapter> storedSubchapters, string deletedPositions) {

            var deletedPositionsList = deletedPositions.Split(",").Select(Int32.Parse).ToList();

            storedSubchapters.RemoveAll(x => deletedPositionsList.Contains(x.Position));

        }
    }
}
