using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Segurplan.Web.Pages.Components.PlanActivityCustomModals {
    public class PlanActivityCustomModalsModel {
        public string Destination { get; set; }

        public ViewComponentCustomPlanChapter CustomChapter { get; set; }

        //Solo necesario en caso de editar Actividades
        public string ChapterTitle { get; set; }

        //Solo necesario en caso de editar Actividades
        public ViewComponentCustomPlanSubChapter CustomSubchapter { get; set; }

        public int ChapterPosition { get; set; }

        public int? SubchapterPosition { get; set; }

        public int? ActivityPosition { get; set; }

        public string PageHandler { get; set; }

        public bool IsSave { get; set; }

        public bool IsDeleteChapter { get; set; }

        public bool HasActivity { get; set; }

        public string DeletedSubChapterPosition { get; set; }

        public string DeletedActivitiesPosition { get; set; }

        public bool EverySubChapterHasActivities { get; set; }
    }

    public class ViewComponentCustomPlanChapter {
        public int Id { get; set; }

        public int Position { get; set; }

        [Required]
        public string Title { get; set; }

        public List<ViewComponentCustomPlanSubChapter> SubChapter { get; set; }

        public bool IsCustomChapter { get; set; }
    }

    public class ViewComponentCustomPlanSubChapter {
        public int Id { get; set; }

        public int Position { get; set; }


        [Required]
        public string Title { get; set; }

        public List<ViewComponentCustomPlanActivity> Activities { get; set; }

        public bool IsCustomSubChapter { get; set; }

        //Used to store previous position to delete from session removed customChapters
        public int PreviousPos { get; set; }

        public bool IsOnlySubChapterWithoutActivitys { get; set; }
    }

    public class ViewComponentCustomPlanActivity {
        public int Id { get; set; }

        public int Position { get; set; }

        public bool IsCustomActivity { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

    }
}
