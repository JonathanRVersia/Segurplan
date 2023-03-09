//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Segurplan.Core.Actions.Administration.Activitys;
//using Segurplan.Core.BusinessObjects;
//using Segurplan.DataAccessLayer.DataAccessManagers;

//namespace Segurplan.Core.BusinessManagers {

//    public class ActivityListManager {

//        public const string OrderByChapter = "Chapter";
//        public const string OrderBySubchapter = "Subchapter";
//        public const string OrderByActivityNumber = "Number";
//        public const string OrderByActivityDescription = "Description";
//        public const string OrderByActivityVersion = "Version";


//        public const string FilterByChapter = "Chapter";
//        public const string FilterBySubchapter = "Subchapter";
//        public const string FilterByActivityDescription = "Description";
//        public const string FilterByActivityNumber = "Number";
//        public const string FilterByActivityVersion = "Version";

//        public bool OrderModeDesc = false;

//        private const int RecursiveTrys = 3;
//        private const int SleepTime = 333;

//        public ActivityListTableState TableState { get; set; }
//        public ActivityFilter TableFilter { get; set; }
//        public int FilteredActivitys { get; internal set; }
//        public IQueryable<DataAccessLayer.Database.DataTransferObjects.Activity> Activitys { get; internal set; } = null;
//        public IQueryable<DataAccessLayer.Database.DataTransferObjects.Chapter> Chapters { get; internal set; } = null;
//        public IQueryable<DataAccessLayer.Database.DataTransferObjects.SubChapter> Subchapters { get; internal set; } = null;
      
//        public IQueryable<DataAccessLayer.Database.DataTransferObjects.ChapterVersionInfo> Versions { get; internal set; } = null;

//        private readonly ActivityDam activityDam;


//        public ActivityListManager(ActivityDam activityDam) {
//            this.activityDam = activityDam;
//            GetQuerysAsync();
//        }


//        private async Task GetQuerysAsync() {
//            Activitys = await activityDam.SelectAllActivitys();
//            Chapters = await activityDam.SelectAllChapters();
//            Subchapters = await activityDam.SelectAllSubchapters();
//            Versions = await activityDam.SelectAllChapterVersions();
//        }
//        public async Task<List<ApplicationActivity>> GetActivityList(ActivityListTableState tableState, ActivityFilter tableFilter, int recursive = 0) {
//            try {
//                TableState = tableState;
//                TableFilter = tableFilter;

//                if (Activitys == null || Chapters == null || Subchapters == null || Versions == null) {
//                    recursive++;
//                    Thread.Sleep(SleepTime);//Adjust recursive level and time
//                    return recursive > RecursiveTrys ? null : await GetActivityList(tableState, tableFilter, recursive);
//                }

//                var dbResponse = from activity in Activitys
//                                 from chapter in Chapters
//                                 from subchapter in Subchapters
//                                 from chapterversioninfo in Versions

//                                 where
//                                 activity.SubChapterId == subchapter.Id &&
//                                 subchapter.IdChapter == chapter.Id &&
//                                 chapter.IdVersionInfo == chapterversioninfo.Id

//                                 select new ApplicationActivity {
//                                     Id = activity.Id,
//                                     ChapterTitle = chapter.Title,
//                                     SubchapterTitle = subchapter.Title,
//                                     Description = activity.Description,
//                                     Number = activity.Number.ToString(),
//                                     VersionNumber = chapterversioninfo.VersionNumber.ToString()
//                                 };


//                dbResponse = string.IsNullOrEmpty(TableFilter.Chapter) ? dbResponse : await ApplyFilters(dbResponse, FilterByChapter, TableFilter.Chapter);
//                dbResponse = string.IsNullOrEmpty(TableFilter.Subchapter) ? dbResponse : await ApplyFilters(dbResponse, FilterBySubchapter, TableFilter.Subchapter);
//                dbResponse = string.IsNullOrEmpty(TableFilter.Description) ? dbResponse : await ApplyFilters(dbResponse, FilterByActivityDescription, TableFilter.Description);
//                dbResponse = string.IsNullOrEmpty(TableFilter.Number) ? dbResponse : await ApplyFilters(dbResponse, FilterByActivityNumber, TableFilter.Number);
//                dbResponse = string.IsNullOrEmpty(TableFilter.Version) ? dbResponse : await ApplyFilters(dbResponse, FilterByActivityVersion, TableFilter.Version);
//                //After filtering we need to count results
//                FilteredActivitys = dbResponse.Count();
//                var IndexPage = tableState.IndexPage;
//                if (tableState.OrderModeDesc == Helpers.ListOrderMode.Desc) { OrderModeDesc = true; }
//                dbResponse = await OrderList(dbResponse, tableState.OrderBy, OrderModeDesc);

//                var RemainingRows = FilteredActivitys - (tableState.PageRows * IndexPage);
//                var Takep = tableState.PageRows;
//                if (RemainingRows - tableState.PageRows < 0) {
//                    Takep = RemainingRows;
//                }
//                if (RemainingRows == 0) {
//                    //not enough rows to fill this page, lets see if enough row to fill indexPage - 1 because we will check planlist.count() later
//                    IndexPage = IndexPage - 1;
//                }
//                if (dbResponse.Count() > 0) {
//                    dbResponse = await GetListOffset(dbResponse, IndexPage, tableState.PageRows, Takep);
//                }

//                return dbResponse == null || FilteredActivitys <= 0 ? null : dbResponse.ToList();

//            } catch (Exception e) {
//                Debug.WriteLine(e.ToString());
//                return null;
//            }

//        }

//        private async Task<IQueryable<ApplicationActivity>> ApplyFilters(IQueryable<ApplicationActivity> query, string filterName, string filterValue) {
//            switch (filterName) {
//                case FilterByChapter:
//                    return (from x in query where x.ChapterTitle.Contains(filterValue) select x);

//                case FilterBySubchapter:
//                    return (from x in query where x.SubchapterTitle.Contains(filterValue) select x);

//                case FilterByActivityDescription:
//                    return (from x in query where x.Description.Contains(filterValue) select x);

               
//                case FilterByActivityNumber:
//                    return (from x in query where (Convert.ToInt32(x.Number))  == (Convert.ToInt32(filterValue)) select x);


//                case FilterByActivityVersion:
//                    return (from x in query where (Convert.ToInt32(x.VersionNumber)) == (Convert.ToInt32(filterValue)) select x);

//                default:
//                    return null;
//            }



//        }
//        private async Task<IQueryable<ApplicationActivity>> GetListOffset(IQueryable<ApplicationActivity> query, int IndexPage, int PageRows, int Takep) {
//            return (from x in query select x).Skip(IndexPage * PageRows).Take(Takep);
//        }
//        private async Task<IQueryable<ApplicationActivity>> OrderList(IQueryable<ApplicationActivity> query, string orderBy, bool orderModeDesc) {

//            switch (orderBy) {

//                default:
//                case OrderByChapter:
//                    return orderModeDesc
//                       ? (from x in query select x).OrderByDescending(x => x.ChapterTitle)
//                       : (from x in query select x).OrderBy(x => x.ChapterTitle);


//                case OrderBySubchapter:
//                    return orderModeDesc
//                         ? (from x in query select x).OrderByDescending(x => x.SubchapterTitle)
//                         : (from x in query select x).OrderBy(x => x.SubchapterTitle);

//                case OrderByActivityDescription:
//                    return orderModeDesc
//                         ? (from x in query select x).OrderByDescending(x => x.Description)
//                         : (from x in query select x).OrderBy(x => x.Description);

//                case OrderByActivityNumber:
//                    return orderModeDesc
//                         ? (from x in query select x).OrderByDescending(x => (Convert.ToInt32(x.Number)))
//                         : (from x in query select x).OrderBy(x => (Convert.ToInt32(x.Number)));
//                case OrderByActivityVersion:
//                    return orderModeDesc
//                         ? (from x in query select x).OrderByDescending(x => x.VersionNumber)
//                         : (from x in query select x).OrderBy(x => x.VersionNumber); 

//            }
//        }
//    }
//}
