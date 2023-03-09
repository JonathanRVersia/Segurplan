//using System.Collections.Generic;
//using Segurplan.Core.Helpers;

//namespace Segurplan.Core.Actions.Administration.Activitys {
//    public class ActivityListTableState {
//        public const string ChapterFilter = "Chapter";
//        public const string SubchapterFilter = "Subchapter";
//        public const string DescriptionFilter = "Description";
//        public const string NumberFilter = "Number";
//        public const string VersionFilter = "Version";

//        public ActivityListTableState() : this(0, 15, ListOrderMode.Asc, "Chapter") {

//        }

//        public ActivityListTableState(int indexPage, int pageRows, ListOrderMode oderMode, string orderBy) {
//            IndexPage = indexPage;
//            PageRows = pageRows;
//            OrderModeDesc = oderMode;
//            OrderBy = orderBy;
//            PageRowList = new List<int>(3) { 15, 25, 50, 100 };
//        }


//        public int IndexPage { get; set; }
//        public int PageRows { get; set; }
//        public List<int> PageRowList { get; private set; }
//        public ListOrderMode OrderModeDesc { get; set; }
//        public string OrderBy { get; set; }
//    }
//}
